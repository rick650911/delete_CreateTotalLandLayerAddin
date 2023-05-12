using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTotalLandLayerAddin
{
    public class CreateTotalLandModule
    {
        public static bool LandToTotal_one_layer(FeatureClass fc_land_to_add, FeatureClass fc_land_base, string aStrVer_to_add, Dictionary<string, mdlLand> dicLand_base, Dictionary<string, mdlLand> dicLand_base2, Dictionary<string, mdlLand> dicLand_base3, StreamWriter sr_out_add, StreamWriter sr_out_exist)
        {
            bool blOk = false;

            try
            {
                int intBatchSize = 20000;// 10000;

                QueryFilter pQf = new ArcGIS.Core.Data.QueryFilter();
                pQf.WhereClause = "";

                if (aStrVer_to_add != null && aStrVer_to_add.Length > 0)
                {
                    pQf.WhereClause = "版次=N'" + aStrVer_to_add + "'";
                }

                //pQf.PostfixClause = "ORDER BY " + "段號";

                RowCursor rowCursor = fc_land_to_add.Search(pQf, false);

                Dictionary<string, mdlLand> dicLand_base_tmp = new Dictionary<string, mdlLand>();
                Dictionary<string, mdlLand> dicLand_base2_tmp = new Dictionary<string, mdlLand>();
                Dictionary<string, mdlLand> dicLand_base3_tmp = new Dictionary<string, mdlLand>();

                List<long> listOID_to_add = new List<long>();
                List<long> listOID_exist = new List<long>();

                int int_csv_index = 0;

                int int_csv_index2 = 0;

                int intDicSize = 40000000;

                using (rowCursor)
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Feature pFeature_now = (Feature)rowCursor.Current)
                        {
                            mdlLand pmdlLand = new mdlLand(pFeature_now);

                            if (
                                (!dicLand_base.ContainsKey(pmdlLand.Key)) &&
                                (!dicLand_base2.ContainsKey(pmdlLand.Key)) &&
                                (!dicLand_base3.ContainsKey(pmdlLand.Key))
                                )// || !dicLand_base.ContainsValue(pmdlLand))//目前總表中沒有的地籍(無該段號，或有段號但地籍屬性有異動)
                            {
                                /*int intDicSize = 40000000;*/// 40;// 50000000;

                                //先加入temp總表字典，因同一版次的地籍有重複的段號，所以同一版次不作重複資料的判斷
                                if (
                               (!dicLand_base_tmp.ContainsKey(pmdlLand.Key)) &&
                               (!dicLand_base2_tmp.ContainsKey(pmdlLand.Key)) &&
                               (!dicLand_base3_tmp.ContainsKey(pmdlLand.Key))
                               )
                                {
                                    if (dicLand_base_tmp.Count <= intDicSize)
                                    {
                                        try
                                        {
                                            dicLand_base_tmp.Add(pmdlLand.Key, pmdlLand);//加入總表
                                        }
                                        catch (Exception ex)
                                        {
                                            // ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  dicLand_base_tmp.Add(pmdlLand.Key, pmdlLand);//總表size:" + dicLand_base_tmp.Count+" "+ ex.Message, "Info");
                                            // return false;
                                        }


                                    }
                                    else if (dicLand_base2_tmp.Count <= intDicSize)
                                    {
                                        try
                                        {
                                            dicLand_base2_tmp.Add(pmdlLand.Key, pmdlLand);//加入總表
                                        }
                                        catch (Exception ex)
                                        {
                                            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  dicLand_base2_tmp.Add(pmdlLand.Key, pmdlLand);//總表size:" + dicLand_base2_tmp.Count + " " + ex.Message, "Info");
                                            //return false;
                                        }

                                    }
                                    else
                                    {
                                        try
                                        {
                                            dicLand_base3_tmp.Add(pmdlLand.Key, pmdlLand);//加入總表
                                        }
                                        catch (Exception ex)
                                        {
                                            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  dicLand_base3_tmp.Add(pmdlLand.Key, pmdlLand);//總表size:" + dicLand_base3_tmp.Count + " " + ex.Message, "Info");
                                            //return false;
                                        }

                                    }
                                }


                                //if (fc_land_base != null)//fc_land_to_add 不是總表
                                //{
                                //記錄objectid，之後整批新增到總表圖層
                                listOID_to_add.Add(pmdlLand.ObjectID);

                                if (listOID_to_add.Count >= intBatchSize)
                                {
                                    bool blOkAdd = AddListToFeatureClass(listOID_to_add, fc_land_to_add, fc_land_base);

                                    //write exist log
                                    int_csv_index2++;
                                    //    StreamWriter sr_out = new StreamWriter(@"D:\temp" + @"\add_" + fc_land_to_add.GetName() + "_" + (int_csv_index2).ToString() + ".csv", false, Encoding.Unicode);

                                    //if (int_csv_index2 == 1)
                                    //{
                                    //    sr_out.WriteLine(fc_land_to_add.GetName());
                                    //}

                                    foreach (long strOID_add in listOID_to_add)
                                    {
                                        sr_out_add.WriteLine(strOID_add);
                                    }

                                    listOID_to_add.Clear();

                                    if (!blOkAdd)
                                    {
                                        return false;
                                    }

                                }

                                //}
                            }
                            else  //總表已有的地籍
                            {
                                if (fc_land_base != null)//fc_land_to_add 不是總表
                                {
                                    //記錄objectid
                                    listOID_exist.Add(pmdlLand.ObjectID);

                                    if (listOID_exist.Count >= intBatchSize)
                                    {
                                        //write exist log
                                        int_csv_index++;
                                        //  StreamWriter sr_out = new StreamWriter(@"D:\temp" + @"\exist_" + fc_land_to_add.GetName()+"_"+ (int_csv_index).ToString() + ".csv", false, Encoding.Unicode);

                                        //if (int_csv_index==1)
                                        //{ 
                                        // sr_out.WriteLine(fc_land_to_add.GetName());
                                        //}

                                        foreach (long strOID_exist in listOID_exist)
                                        {
                                            sr_out_exist.WriteLine(strOID_exist);
                                        }

                                        listOID_exist.Clear();
                                        //sr_out.Close();
                                    }
                                    ////////
                                }
                                /////////////
                            }
                        }
                    }

                }//  using (rowCursor)

                if (listOID_to_add.Count >= 0)
                {
                    bool blOkAdd = AddListToFeatureClass(listOID_to_add, fc_land_to_add, fc_land_base);

                    //write exist log
                    int_csv_index2++;
                    //StreamWriter sr_out = new StreamWriter(@"D:\temp" + @"\add_" + fc_land_to_add.GetName() + "_" + (int_csv_index2).ToString() + ".csv", false, Encoding.Unicode);

                    foreach (long strOID_add in listOID_to_add)
                    {
                        sr_out_add.WriteLine(strOID_add);
                    }

                    listOID_to_add.Clear();

                    if (!blOkAdd)
                    {
                        return false;
                    }

                }

                //write exist log
                int_csv_index++;
                // StreamWriter sr_out2 = new StreamWriter(@"D:\temp" + @"\exist_" + fc_land_to_add.GetName() + "_" + (int_csv_index++).ToString() + ".csv", false, Encoding.Unicode);

                // sr_out2.WriteLine(fc_land_to_add.GetName());

                foreach (long strOID_exist in listOID_exist)
                {
                    sr_out_exist.WriteLine(strOID_exist);
                }

                listOID_exist.Clear();

                //temp字典加入總表字典

                foreach (KeyValuePair<string, mdlLand> item in dicLand_base_tmp)
                {
                    blOk = AddToDictionary_總表(dicLand_base, dicLand_base2, dicLand_base3, item.Value, intDicSize);

                    //if (!blOk)            //允許錯誤，因為有重複段號 ，當以段號為key 時，會發生錯誤
                    //{
                    //    return false;
                    //}
                }

                foreach (KeyValuePair<string, mdlLand> item in dicLand_base2_tmp)
                {
                    blOk = AddToDictionary_總表(dicLand_base, dicLand_base2, dicLand_base3, item.Value, intDicSize);

                    //if (!blOk)
                    //{
                    //    return false;
                    //}
                }

                foreach (KeyValuePair<string, mdlLand> item in dicLand_base3_tmp)
                {
                    blOk = AddToDictionary_總表(dicLand_base, dicLand_base2, dicLand_base3, item.Value, intDicSize);

                    //if (!blOk)
                    //{
                    //    return false;
                    //}
                }


                //sr_out_exist.Close();

                ////append to base
                //if (fc_land_base != null)//fc_land_to_add 不是總表
                //{
                //    //分批處理
                //    int intBatchSize = 10000;
                //    int intIdx = 0;

                //    while (intIdx < listOID_to_add.Count)
                //    {
                //string strOIDs = string.Empty;

                //int intBatchCount=0;

                ////  foreach (long lngOID in listOID_to_add)
                ////for (; intIdx < listOID_to_add.Count; )
                //while (intIdx < listOID_to_add.Count)
                //{
                //    long lngOID = listOID_to_add[intIdx];

                //    strOIDs += lngOID.ToString() + ",";

                //    intIdx++;

                //    intBatchCount++;

                //    if (intBatchCount>= intBatchSize)
                //    {
                //        break;
                //    }
                //}

                //strOIDs = strOIDs.TrimEnd(",".ToCharArray());

                //if (strOIDs.Length > 0)
                //{
                //    string strWhere = fc_land_to_add.GetDefinition().GetObjectIDField() + " in (" + strOIDs + ")";

                //    List<IGPResult> list_output_GpResult = new List<IGPResult>();
                //    List<string> list_output_ErrorMsg = new List<string>();

                //    //選取fc_land_to_add要新增到base的OID
                //    GpTools.MakeFeatureLayer(fc_land_to_add,
                //                     "fc_land_to_add_lyr",
                //                    strWhere,
                //                      list_output_GpResult,
                //                     list_output_ErrorMsg);

                //    if (list_output_GpResult.Count == 1)
                //    {
                //        IGPResult gpResult = list_output_GpResult[0];

                //        if (gpResult.IsFailed == true)
                //        {
                //            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + fieldName + "]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
                //            return false;
                //        }
                //    }


                //    List<object> list_inputs = new List<object>();
                //    list_inputs.Add("fc_land_to_add_lyr");

                //    GpTools.Append(list_inputs,
                //                        fc_land_base,
                //                      list_output_GpResult,
                //                     list_output_ErrorMsg);

                //    if (list_output_GpResult.Count == 1)
                //    {
                //        IGPResult gpResult = list_output_GpResult[0];

                //        if (gpResult.IsFailed == true)
                //        {
                //            //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + fieldName + "]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
                //            return false;
                //        }
                //    }
                //}
                //}



                //  }//if (fc_land_base != null)//fc_land_to_add 不是總表
            }//try
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error on LandToTotal_one_layer:" + ex.Message);
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  LandToTotal_one_layer:" + ex.Message, "Info");
                return false;
            }


            blOk = true;

            return blOk;

        }

        public static bool AddToDictionary_總表(Dictionary<string, mdlLand> dicLand_base, Dictionary<string, mdlLand> dicLand_base2, Dictionary<string, mdlLand> dicLand_base3, mdlLand pmdlLand, int intDicSize)
        {
            bool blOk = false;

            if (dicLand_base.Count <= intDicSize)
            {
                try
                {
                    dicLand_base.Add(pmdlLand.Key, pmdlLand);//加入總表
                }
                catch (Exception ex)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  dicLand_base.Add(pmdlLand.Key, pmdlLand);//總表size:" + dicLand_base.Count + " " + ex.Message, "Info");
                    return false;
                }


            }
            else if (dicLand_base2.Count <= intDicSize)
            {
                try
                {
                    dicLand_base2.Add(pmdlLand.Key, pmdlLand);//加入總表
                }
                catch (Exception ex)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  dicLand_base2.Add(pmdlLand.Key, pmdlLand);//總表size:" + dicLand_base2.Count + " " + ex.Message, "Info");
                    return false;
                }

            }
            else
            {
                try
                {
                    dicLand_base3.Add(pmdlLand.Key, pmdlLand);//加入總表
                }
                catch (Exception ex)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on  dicLand_base3.Add(pmdlLand.Key, pmdlLand);//總表size:" + dicLand_base3.Count + " " + ex.Message, "Info");
                    return false;
                }

            }

            blOk = true;

            return blOk;
        }


        public static bool AddListToFeatureClass(List<long> listOID_to_add, FeatureClass fc_land_to_add, FeatureClass fc_land_base)
        {
            bool blOk = false;

            string strOIDs = string.Empty;

            //       int intBatchCount = 0;
            int intIdx = 0;
            //  foreach (long lngOID in listOID_to_add)
            //for (; intIdx < listOID_to_add.Count; )
            while (intIdx < listOID_to_add.Count)
            {
                long lngOID = listOID_to_add[intIdx];

                strOIDs += lngOID.ToString() + ",";

                intIdx++;

                //   intBatchCount++;

                //if (intBatchCount >= intBatchSize)
                //{
                //    break;
                //}
            }

            strOIDs = strOIDs.TrimEnd(",".ToCharArray());

            if (strOIDs.Length > 0)
            {
                string strWhere = fc_land_to_add.GetDefinition().GetObjectIDField() + " in (" + strOIDs + ")";

                List<IGPResult> list_output_GpResult = new List<IGPResult>();
                List<string> list_output_ErrorMsg = new List<string>();

                //選取fc_land_to_add要新增到base的OID
                GpTools.MakeFeatureLayer(fc_land_to_add,
                                 "fc_land_to_add_lyr",
                                strWhere,
                                  list_output_GpResult,
                                 list_output_ErrorMsg);

                if (list_output_GpResult.Count == 1)
                {
                    IGPResult gpResult = list_output_GpResult[0];

                    if (gpResult.IsFailed == true)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + fieldName + "]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
                        return false;
                    }
                }


                List<object> list_inputs = new List<object>();
                list_inputs.Add("fc_land_to_add_lyr");

                GpTools.Append(list_inputs,
                                    fc_land_base,
                                  list_output_GpResult,
                                 list_output_ErrorMsg);

                if (list_output_GpResult.Count == 1)
                {
                    IGPResult gpResult = list_output_GpResult[0];

                    if (gpResult.IsFailed == true)
                    {
                        //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("新增[" + fieldName + "]欄位發生錯誤: " + gpResult.ErrorCode.ToString());
                        return false;
                    }
                }
            }

            blOk = true;

            return blOk;

        }


    }
}
