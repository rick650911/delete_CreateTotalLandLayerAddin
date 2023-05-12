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
using ArcPyModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityClassLibrary;

namespace CreateTotalLandLayerAddin
{
    internal class btnCreateTotalLand : Button
    {
        protected override async void OnClick()
        {
            await QueuedTask.Run(async () =>
            {
                List<string> listName_gdb = null;

                try
                {
                    listName_gdb = EnvSettings._srVersions.Split(",".ToCharArray()).ToList();
                }
                catch (Exception ex)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 取得版次清單：", "Info");
                    return;
                }


                FeatureClass fc_land_本島_base = null;
                FeatureClass fc_land_外島_base = null;

                //建立總表
                Dictionary<string, mdlLand> dicLand_base = new Dictionary<string, mdlLand>();
                Dictionary<string, mdlLand> dicLand_base2 = new Dictionary<string, mdlLand>();
                Dictionary<string, mdlLand> dicLand_base3 = new Dictionary<string, mdlLand>();

                string strName_gdb = listName_gdb[0];

                Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(EnvSettings._srRootPath + @"\COA_" + strName_gdb + ".gdb")));

                fc_land_本島_base = geodatabase.OpenDataset<FeatureClass>("Land_Taiwan_97_" + strName_gdb);
                fc_land_外島_base = geodatabase.OpenDataset<FeatureClass>("Land_Penghu_97_" + strName_gdb);

                //for (int i=1; i<listVersion.Count;i++)
                //{
                strName_gdb = listName_gdb[1];

                FeatureClass fc_land_本島 = null;
                FeatureClass fc_land_外島 = null;

                try
                {
                    geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(EnvSettings._srRootPath + @"\COA_" + strName_gdb + ".gdb")));

                    fc_land_本島 = geodatabase.OpenDataset<FeatureClass>("Land_Taiwan_97_" + strName_gdb);
                    fc_land_外島 = geodatabase.OpenDataset<FeatureClass>("Land_Penghu_97_" + strName_gdb);

                    StreamWriter sr_out_add = new StreamWriter(EnvSettings._srRootPath + @"" + @"\add_" + fc_land_本島.GetName() + "_" + ".csv", false, Encoding.Unicode);
                    StreamWriter sr_out_exist = new StreamWriter(EnvSettings._srRootPath + @"" + @"\exist_" + fc_land_本島.GetName() + "_" + ".csv", false, Encoding.Unicode);

                    StreamWriter sr_out_add_外島 = new StreamWriter(EnvSettings._srRootPath + @"" + @"\add_" + fc_land_外島.GetName() + "_" + ".csv", false, Encoding.Unicode);
                    StreamWriter sr_out_exist_外島 = new StreamWriter(EnvSettings._srRootPath + @"" + @"\exist_" + fc_land_外島.GetName() + "_" + ".csv", false, Encoding.Unicode);

                    string strVersionsInLayer = "111Jun,111Apr,111Feb,110Dec,110Oct,110Aug,110Jun,110Apr,110Feb,109Dec,109Oct,109Aug,109Jun,109Apr,109Feb,108Dec,108Oct,108Aug,108Jun,108Apr,108Feb,107Dec,107Oct,107Aug,107Jun,107Apr,107Feb,106Q4,106Q3,106Q2,106Q1,105Q4,105Q3,105Q2";

                    string[] strArrayVersion_InLayer = strVersionsInLayer.Split(",".ToCharArray());

                    for (int j = 0; j < strArrayVersion_InLayer.Length; j++)
                    {
                        string strVersionInLayer = strArrayVersion_InLayer[j];

                        bool blOk = CreateTotalLandModule.LandToTotal_one_layer(fc_land_本島, fc_land_本島_base, strVersionInLayer, dicLand_base, dicLand_base2, dicLand_base3, sr_out_add, sr_out_exist);

                        if (!blOk)
                        {
                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立總表：" + fc_land_本島.GetName(), "Info");
                            return;
                        }

                        //if (i==0)//以最新版為基礎
                        //{
                        //    fc_land_本島_base = fc_land_本島;
                        //}

                        blOk = CreateTotalLandModule.LandToTotal_one_layer(fc_land_外島, fc_land_外島_base, strVersionInLayer, dicLand_base, dicLand_base2, dicLand_base3, sr_out_add_外島, sr_out_exist_外島);



                        if (!blOk)
                        {
                            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立總表：" + fc_land_外島.GetName(), "Info");
                            return;
                        }
                    }

                    sr_out_add.Close();
                    sr_out_exist.Close();


                    sr_out_add_外島.Close();
                    sr_out_exist_外島.Close();

                }
                catch (Exception ex)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立總表：" + ex.Message, "Info");
                    return;
                }

                //}


            }/*, RasterlizeManager._CancelableProgressorSource.Progressor*/);
        }
    }
}
