
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;

namespace ArcGISProSDKExtensionsModule
{
    class ArcGISProSDKExtensions
    {
        /*
        取得路徑資料Featureclass
        可取得gdb,sde,shp資料featureclass
        若路徑無效,或是檔案有問題,回傳false,否則回傳true
        若路徑無效,或是檔案有問題,featureclass為null,否則取得featureclass
        使用完畢後,記得要釋放featureclass, 與從featureclass中取得其他物件
        此方法只能在以下情況使用
        1.Stand alone開發: TaskUtils.StartSTATask內部使用
        2.Pro Addin開發:   QueuedTask.Run內部使用
        */
        public static bool GetFeatureClass(string datapath,out FeatureClass featureclass)
        {
            featureclass = null;
            try
            {
                //判定是否為gdb或是sde路徑
                var dbextension = new List<string> { ".gdb", ".sde" };
                if (!string.IsNullOrWhiteSpace(datapath) && Directory.Exists(Path.GetDirectoryName(datapath)) && dbextension.Any(Path.GetDirectoryName(datapath).Contains))
                {
                    var GDBfileConnection = new FileGeodatabaseConnectionPath(new Uri(Path.GetDirectoryName(datapath), UriKind.Absolute));
                    Geodatabase geodatabase = new Geodatabase(GDBfileConnection);
                    featureclass = geodatabase.OpenDataset<FeatureClass>(Path.GetFileName(datapath));
                    geodatabase.Dispose();
                }
                //是否為shp
                else if (!string.IsNullOrWhiteSpace(datapath) && Directory.Exists(Path.GetDirectoryName(datapath)) && Path.GetFileName(datapath).Contains(".shp"))
                {
                    var srcfileConnection = new FileSystemConnectionPath(new Uri(Path.GetDirectoryName(datapath)), FileSystemDatastoreType.Shapefile);
                    FileSystemDatastore srcshapefile = new FileSystemDatastore(srcfileConnection);
                    featureclass = srcshapefile.OpenDataset<FeatureClass>(Path.GetFileName(datapath));
                    srcshapefile.Dispose();
                }
                else
                {
                    return false;
                }

                //取得fc成功,回傳true
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                //開啟資料發生例外,回傳null
                return false;
            }
        }

        public static bool GetFeatureClass(string datapath, out FeatureClass featureclass, out string errmsg)
        {
            featureclass = null;
            errmsg = "";
            try
            {
                //判定是否為gdb或是sde路徑
                var dbextension = new List<string> { ".gdb", ".sde" };
                if (!string.IsNullOrWhiteSpace(datapath) && Directory.Exists(Path.GetDirectoryName(datapath)) && dbextension.Any(Path.GetDirectoryName(datapath).Contains))
                {
                    var GDBfileConnection = new FileGeodatabaseConnectionPath(new Uri(Path.GetDirectoryName(datapath), UriKind.Absolute));
                    Geodatabase geodatabase = new Geodatabase(GDBfileConnection);
                    featureclass = geodatabase.OpenDataset<FeatureClass>(Path.GetFileName(datapath));
                    geodatabase.Dispose();
                }
                //是否為shp
                else if (!string.IsNullOrWhiteSpace(datapath) && Directory.Exists(Path.GetDirectoryName(datapath)) && Path.GetFileName(datapath).Contains(".shp"))
                {
                    var srcfileConnection = new FileSystemConnectionPath(new Uri(Path.GetDirectoryName(datapath)), FileSystemDatastoreType.Shapefile);
                    FileSystemDatastore srcshapefile = new FileSystemDatastore(srcfileConnection);
                    featureclass = srcshapefile.OpenDataset<FeatureClass>(Path.GetFileName(datapath));
                    srcshapefile.Dispose();
                }
                else
                {
                    errmsg = "輸入檔案並非shp,gdb,sde其中一種,無法取得featureclass";
                    return false;
                }

                //取得fc成功,回傳true
                return true;
            }
            catch (Exception ex)
            {
                errmsg=ex.Message;
                //開啟資料發生例外,回傳null
                return false;
            }
        }
        
        //檢查資料與對欄位性是否存在
        //路徑無效或是資料有問題,回傳false
        //若指定欄位不存在,回傳false
        public static async Task<Tuple<bool, string>> CheckFeatureClassFields(string datapath, List<string> chkfieldnames)
        {
            return await TaskUtils.StartSTATask<Tuple<bool, string>>(() =>
            {
                string errmsg = "";
                FeatureClass featureclass;
                if (!GetFeatureClass(datapath, out featureclass, out errmsg))
                {
                    errmsg = "檢查欄位失敗\n"+errmsg;
                    return new Tuple<bool, string>(false, errmsg);
                }
                
                FeatureClassDefinition featureclassDefinition = featureclass.GetDefinition();
                var fields = featureclassDefinition.GetFields();
                List<string> fieldnames = fields.Select(o => o.Name).ToList();
                
                featureclassDefinition.Dispose();
                featureclass.Dispose();

                if (chkfieldnames.Count > fieldnames.Count)
                {
                    errmsg = "檢查欄位失敗\n輸入欄位數量大於圖資欄位數量";
                    return new Tuple<bool, string>(false, errmsg);
                }

                bool fieldisexist = chkfieldnames.All(fieldnames.Contains);
                if (!fieldisexist)
                {
                    errmsg = "檢查欄位失敗\n輸入欄位不存在於圖資欄位中";
                    return new Tuple<bool, string>(false, errmsg);
                }

                errmsg = "檢查欄位成功\n所有輸入欄位存在於圖資欄位中";
                return new Tuple<bool, string>(true, errmsg);
            });
        }

        //檢查FeatureClass是否有指定座標系統
        //cheskspecsr: 設定是否只檢查有無座標系統
        public static async Task<bool> CheckFeatureClassHasSpatialReference(string datapath, bool cheskspecsr, int wkid = 0)
        {
            bool checksrexist = false;
            return await TaskUtils.StartSTATask<bool>(() =>
            {
                FeatureClass featureclass;
                if (GetFeatureClass(datapath, out featureclass))
                {
                    FeatureClassDefinition featureclassDefinition = featureclass.GetDefinition();
                    //檢查座標系統
                    //若wkid給0,表示不用檢查
                    var sr = featureclassDefinition.GetSpatialReference();

                    if (cheskspecsr)
                    {
                        if (sr != null && sr.Wkid == wkid)
                        {
                            checksrexist = true;
                        }
                    }
                    else 
                    {
                        if (sr!=null)
                        {
                            checksrexist = true;
                        }
                    }

                    featureclassDefinition.Dispose();
                    featureclass.Dispose();
                }

                return checksrexist;
            });
        }

        //建立座標系統
        //此方法只能在TaskUtils.StartSTATask底下使用
        public static SpatialReference CreateSpatialReference(int epsg)
        {
            SpatialReference sr;
            try
            {
                //建立座標系統物件
                sr = SpatialReferenceBuilder.CreateSpatialReference(epsg);
            }
            catch (Exception ex)
            {
                //建立undefined座標系統物件
                sr = SpatialReferenceBuilder.CreateSpatialReference(0);
            }
            return sr;
        }

    }
    public static class ArcGISProSDKLinq
    {
        public static IEnumerable<ArcGIS.Core.Data.Feature> AsFeatureEnumerable(this ArcGIS.Core.Data.RowCursor source)
        {
            Func<ArcGIS.Core.Data.Feature> next = () =>
            {
                if (source.MoveNext())
                {
                    return (ArcGIS.Core.Data.Feature)source.Current;
                }
                return null;
            };
            ArcGIS.Core.Data.Feature current = next();
            while (current != null)
            {
                yield return current;
                current = next();
            }
        }

        public static IEnumerable<ArcGIS.Core.Data.Row> AsRowEnumerable(this ArcGIS.Core.Data.RowCursor source)
        {
            Func<ArcGIS.Core.Data.Row> next = () =>
            {
                if (source.MoveNext())
                {
                    return (ArcGIS.Core.Data.Row)source.Current;
                }
                return null;
            };
            ArcGIS.Core.Data.Row current = next();
            while (current != null)
            {
                yield return current;
                current = next();
            }
        }


        public static void DisposeRowEnumerableList(List<Row> listrow)
        {
            foreach (var row in listrow)
            {
                row.Dispose();
            }
        }


        /*
        public static IEnumerable<long> AsEnumerable(this ILongArray longArray)
        {
            for (int i = 0; i < longArray.Count; i++)
            {
                yield return longArray.get_Element(i);
            }
        }

        public static IEnumerable<IRow> AsEnumerable(this ICursor source)
        {
            Func<IRow> next = () => source.NextRow();
            IRow current = next();
            while (current != null)
            {
                yield return current;
                current = next();
            }
        }

        public static IEnumerable<IMapLayerInfo> AsEnumerable(this IMapLayerInfos layerInfos)
        {
            for (int i = 0; i < layerInfos.Count; i++)
            {
                yield return layerInfos.get_Element(i);
            }
        }
        */
    }
    class TaskUtils
    {
        //http://stackoverflow.com/questions/16720496/set-apartmentstate-on-a-task
        /// <summary>
        /// The problem is that Core.Data must be STA. An STA thread can't be a threadpool thread 
        /// and must pump a message loop
        /// </summary>
        /// <remarks>Note: there is no non-generic equivalent for TaskCompletionSource</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns>Task{T}</returns>
        public static Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread(() => {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }
}
