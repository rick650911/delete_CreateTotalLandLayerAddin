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
    internal class btnAddVersionField : Button
    {
        protected override async void OnClick()
        {
            await QueuedTask.Run(async () =>
            {
                List<string> listVersion = null;

                try
                {
                    listVersion = EnvSettings._srVersions.Split(",".ToCharArray()).ToList();
                }
                catch (Exception ex)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 取得版次清單：", "Info");
                    return;
                }

                //listVersion.Add("111Jun");//base
                //listVersion.Add("111Apr");

                //listVersion.Add("110Dec");//base
                //listVersion.Add("110Oct");
                //listVersion.Add("110Aug");
                //listVersion.Add("110Jun");
                //listVersion.Add("110Apr");
                //listVersion.Add("110Feb");

                //所有地籍加版次欄位
                for (int i = 0; i < listVersion.Count; i++)
                {
                    string strVer = listVersion[i];
                    string str_land_本島 = EnvSettings._srRootPath + @"\COA_" + strVer + @".gdb\taiwan_97\Land_Taiwan_97_" + strVer;
                    string str_land_外島 = EnvSettings._srRootPath + @"\COA_" + strVer + @".gdb\penghu_97\Land_Penghu_97_" + strVer;




                    bool blOk = ArcPyProcess.AddField(str_land_本島, "版次", "TEXT", "10");

                    if (!blOk)
                    {
                        System.Diagnostics.Debug.WriteLine("error");
                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立版次欄位：", "Info");

                        return;
                    }

                    blOk = ArcPyProcess.AddField(str_land_外島, "版次", "TEXT", "10");
                    if (!blOk)
                    {
                        System.Diagnostics.Debug.WriteLine("error");
                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立版次欄位：", "Info");

                        return;
                    }

                    blOk = ArcPyProcess.CalculateField(str_land_本島, "版次", "'" + strVer + "'", "Python 3", string.Empty);

                    if (!blOk)
                    {
                        System.Diagnostics.Debug.WriteLine("error");
                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立版次欄位：", "Info");

                        return;
                    }

                    blOk = ArcPyProcess.CalculateField(str_land_外島, "版次", "'" + strVer + "'", "Python 3", string.Empty);

                    if (!blOk)
                    {
                        System.Diagnostics.Debug.WriteLine("error");
                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("error on 建立版次欄位：", "Info");

                        return;
                    }



                }



            }/*, RasterlizeManager._CancelableProgressorSource.Progressor*/);
        }
    }
}
