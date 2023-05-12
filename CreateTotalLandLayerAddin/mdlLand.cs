using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace CreateTotalLandLayerAddin
{
    public class mdlLand
    {
        private long _ObjectID;
        //private string _段號;
        //private string _登記日期;
        //private string _登記原因碼;
        //private string _地目碼;
        //private string _等則碼;
        //private double _登記面積_平方公尺;
        //private string _使用分區碼;
        //private string _使用類別碼;
        //private string _公告現值_NTD;
        //private string _公告地價_NTD;

        //private string _段延伸碼;
        //private string _轉檔日期;

        public mdlLand()
        { }

        public mdlLand(Feature aFeature)
        {
            ObjectID = aFeature.GetObjectID();

            object obj_段號 = aFeature["段號"];
            string str_段號 = "";

            if (obj_段號 != null)
            {
                str_段號 = obj_段號.ToString().Trim();
                //段號 = str_段號;
            }

            object obj_登記日期 = aFeature["登記日期"];
            string str_登記日期 = "";

            if (obj_登記日期 != null)
            {
                str_登記日期 = obj_登記日期.ToString().Trim();
              //  登記日期 = str_登記日期;
            }

            object obj_登記原因碼 = aFeature["登記原因碼"];
            string str_登記原因碼 = "";

            if (obj_登記原因碼 != null)
            {
                str_登記原因碼 = obj_登記原因碼.ToString().Trim();
              //  登記原因碼 = str_登記原因碼;
            }

            object obj_地目碼 = aFeature["地目碼"];
            string str_地目碼 = "";

            if (obj_地目碼 != null)
            {
                str_地目碼 = obj_地目碼.ToString().Trim();
              //  地目碼 = str_地目碼;
            }

            object obj_等則碼 = aFeature["等則碼"];
            string str_等則碼 = "";

            if (obj_等則碼 != null)
            {
                str_等則碼 = obj_等則碼.ToString().Trim();
          //      等則碼 = str_等則碼;
            }

            object obj_登記面積_平方公尺 = aFeature["登記面積_平方公尺"];
            string str_登記面積_平方公尺 = "";
            double dbl_登記面積_平方公尺=0;

            if (obj_登記面積_平方公尺 != null)
            {
                str_登記面積_平方公尺 = obj_登記面積_平方公尺.ToString().Trim();

                bool blParse = double.TryParse(str_登記面積_平方公尺, out dbl_登記面積_平方公尺);

                if (blParse)
                {
          //          登記面積_平方公尺 = dbl_登記面積_平方公尺;
                }
            }

            object obj_使用分區碼 = aFeature["使用分區碼"];
            string str_使用分區碼 = "";

            if (obj_使用分區碼 != null)
            {
                str_使用分區碼 = obj_使用分區碼.ToString().Trim();
     //           使用分區碼 = str_使用分區碼;
            }

            object obj_使用類別碼 = aFeature["使用類別碼"];
            string str_使用類別碼 = "";

            if (obj_使用類別碼 != null)
            {
                str_使用類別碼 = obj_使用類別碼.ToString().Trim();
       //         使用類別碼 = str_使用類別碼;
            }

            object obj_公告現值_NTD = aFeature["公告現值_NTD"];
            string str_公告現值_NTD = "";

            if (obj_公告現值_NTD != null)
            {
                str_公告現值_NTD = obj_公告現值_NTD.ToString().Trim();
   //             公告現值_NTD = str_公告現值_NTD;
            }

            object obj_公告地價_NTD = aFeature["公告地價_NTD"];
            string str_公告地價_NTD = "";

            if (obj_公告地價_NTD != null)
            {
                str_公告地價_NTD = obj_公告地價_NTD.ToString().Trim();
       //       公告地價_NTD = str_公告地價_NTD;
            }

            object obj_段延伸碼 = aFeature["段延伸碼"];
            string str_段延伸碼 = "";

            if (obj_段延伸碼 != null)
            {
                str_段延伸碼 = obj_段延伸碼.ToString().Trim();
        //        段延伸碼 = str_段延伸碼;
            }

            object obj_轉檔日期 = aFeature["轉檔日期"];
            string str_轉檔日期 = "";

            if (obj_轉檔日期 != null)
            {
                str_轉檔日期 = obj_轉檔日期.ToString().Trim();
      //          轉檔日期 = str_轉檔日期;
            }

            object obj_版次 = aFeature["版次"];
            string str_版次 = "";

            if (obj_版次 != null)
            {
                str_版次 = obj_版次.ToString().Trim();
                //          版次 = str_版次;
            }

            //_Key = str_段號 +
            //             str_登記日期 +
            //             str_登記原因碼 +
            //             str_地目碼 +
            //             str_等則碼 +
            //             dbl_登記面積_平方公尺 +
            //             str_使用分區碼 +
            //             str_使用類別碼 +
            //             str_段延伸碼 +
            //             str_轉檔日期 +
            //             str_公告現值_NTD +
            //             str_公告地價_NTD;

            _Key = str_段號 ;

            //_段號 = str_段號;

            _版次 = str_版次;
        }


        public long ObjectID
        {
            get 
            {
                return _ObjectID;
            }
            set
            {
                _ObjectID = value;
            }
        }

        //public string 段號
        //{
        //    get
        //    {
        //        if (_段號 == null)
        //        {
        //            _段號 = string.Empty;
        //        }

        //        return _段號;
        //    }
        //    set 
        //    {
        //        if (value != null) 
        //        {
        //            _段號 = value.Trim();
        //        }                
        //    }
        //}

        //public string 登記日期
        //{
        //    get
        //    {
        //        if (_登記日期 == null)
        //        {
        //            _登記日期 = string.Empty;
        //        }

        //        return _登記日期;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _登記日期 = value.Trim();
        //        }
        //    }
        //}

        //public string 登記原因碼
        //{
        //    get
        //    {
        //        if (_登記原因碼 == null)
        //        {
        //            _登記原因碼 = string.Empty;
        //        }

        //        return _登記原因碼;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _登記原因碼 = value.Trim();
        //        }
        //    }
        //}

        //public string 地目碼
        //{
        //    get
        //    {
        //        if (_地目碼 == null)
        //        {
        //            _地目碼 = string.Empty;
        //        }

        //        return _地目碼;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _地目碼 = value.Trim();
        //        }
        //    }
        //}

        //public string 等則碼
        //{
        //    get
        //    {
        //        if (_等則碼 == null)
        //        {
        //            _等則碼 = string.Empty;
        //        }

        //        return _等則碼;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _等則碼 = value.Trim();
        //        }
        //    }
        //}

        //public double 登記面積_平方公尺
        //{
        //    get
        //    {
        //        return _登記面積_平方公尺;
        //    }
        //    set
        //    {
        //        //if (value != null)
        //        //{
        //            _登記面積_平方公尺 = value;
        //        //}
        //    }
        //}

        //public string 使用分區碼
        //{
        //    get
        //    {
        //        if (_使用分區碼 == null)
        //        {
        //            _使用分區碼 = string.Empty;
        //        }

        //        return _使用分區碼;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _使用分區碼 = value.Trim();
        //        }
        //    }
        //}

        //public string 使用類別碼
        //{
        //    get
        //    {
        //        if (_使用類別碼 == null)
        //        {
        //            _使用類別碼 = string.Empty;
        //        }

        //        return _使用類別碼;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _使用類別碼 = value.Trim();
        //        }
        //    }
        //}

        //public string 公告現值_NTD
        //{
        //    get
        //    {
        //        if (_公告現值_NTD == null)
        //        {
        //            _公告現值_NTD = string.Empty;
        //        }

        //        return _公告現值_NTD;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _公告現值_NTD = value.Trim();
        //        }
        //    }
        //}

        //public string 公告地價_NTD
        //{
        //    get
        //    {
        //        if (_公告地價_NTD == null)
        //        {
        //            _公告地價_NTD = string.Empty;
        //        }

        //        return _公告地價_NTD;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _公告地價_NTD = value.Trim();
        //        }
        //    }
        //}

        //public string 段延伸碼
        //{
        //    get
        //    {
        //        if (_段延伸碼 == null)
        //        {
        //            _段延伸碼 = string.Empty;
        //        }

        //        return _段延伸碼;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _段延伸碼 = value.Trim();
        //        }
        //    }
        //}

        //public string 轉檔日期
        //{
        //    get
        //    {
        //        if (_轉檔日期 == null)
        //        {
        //            _轉檔日期 = string.Empty;
        //        }

        //        return _轉檔日期;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _轉檔日期 = value.Trim();
        //        }
        //    }
        //}

        private string _Key;
        public string Key
        {
            get
            {
                if (_Key == null)
                {
                    //_Key = 段號+
                    //      登記日期 +
                    //      登記原因碼 +
                    //      地目碼 +
                    //      等則碼 +
                    //      登記面積_平方公尺 +
                    //      使用分區碼 +
                    //      使用類別碼 +
                    //      段延伸碼 +
                    //      轉檔日期 +
                    //      公告現值_NTD +
                    //      公告地價_NTD;
                    _Key = string.Empty;
                }

                return _Key;
            }
        }

        //private string _段號;

        //public string 段號
        //{
        //    get
        //    {
        //        if (_段號 == null)
        //        {

        //            _段號 = string.Empty;
        //        }

        //        return _段號;
        //    }
        //}

        private string _版次;

        public string 版次
        {
            get
            {
                if (_版次 == null)
                {

                    _版次 = string.Empty;
                }

                return _版次;
            }
        }

        //public override bool Equals(object obj)
        //{
        //    bool blEqual = false;

        //    if (obj is mdlLand)
        //    {
        //        mdlLand mdlLand_obj = (mdlLand)obj;

        //        if (
        //          段號.Equals(mdlLand_obj.段號) &&
        //          登記日期.Equals(mdlLand_obj.登記日期) &&
        //          登記原因碼.Equals(mdlLand_obj.登記原因碼) &&
        //          地目碼.Equals(mdlLand_obj.地目碼) &&
        //          等則碼.Equals(mdlLand_obj.等則碼) &&
        //          登記面積_平方公尺.Equals(mdlLand_obj.登記面積_平方公尺) &&
        //          使用分區碼.Equals(mdlLand_obj.使用分區碼) &&
        //          使用類別碼.Equals(mdlLand_obj.使用類別碼) &&

        //          段延伸碼.Equals(mdlLand_obj.段延伸碼) &&
        //          轉檔日期.Equals(mdlLand_obj.轉檔日期) &&

        //          公告現值_NTD.Equals(mdlLand_obj.公告現值_NTD) &&
        //          公告地價_NTD.Equals(mdlLand_obj.公告地價_NTD)
        //          )
        //        {
        //            blEqual = true;
        //        }

        //        //if (
        //        //    ( (_段號 == null && mdlLand_obj._段號 == null) || (_段號!=null && mdlLand_obj._段號!=null && _段號.Equals(mdlLand_obj._段號)) ) &&
        //        //    ( (_登記日期 == null && mdlLand_obj._登記日期 == null) || (_登記日期 != null && mdlLand_obj._登記日期 != null && _登記日期.Equals(mdlLand_obj._登記日期)) ) &&
        //        //    ( (_登記原因碼 == null && mdlLand_obj._登記原因碼 == null) || (_登記原因碼 != null && mdlLand_obj._登記原因碼 != null && _登記原因碼.Equals(mdlLand_obj._登記原因碼)) ) &&
        //        //    ( (_地目碼 == null && mdlLand_obj._地目碼 == null) || (_地目碼 != null && mdlLand_obj._地目碼 != null && _地目碼.Equals(mdlLand_obj._地目碼)) ) &&
        //        //    ( (_等則碼 == null && mdlLand_obj._等則碼 == null) || (_等則碼 != null && mdlLand_obj._等則碼 != null && _等則碼.Equals(mdlLand_obj._等則碼)) ) &&
        //        //    _登記面積_平方公尺.Equals(mdlLand_obj._登記面積_平方公尺) &&
        //        //    ( (_使用分區碼 == null && mdlLand_obj._使用分區碼 == null) || (_使用分區碼 != null && mdlLand_obj._使用分區碼 != null && _使用分區碼.Equals(mdlLand_obj._使用分區碼)) ) &&
        //        //    ( (_使用類別碼 == null && mdlLand_obj._使用類別碼 == null) || (_使用類別碼 != null && mdlLand_obj._使用類別碼 != null && _使用類別碼.Equals(mdlLand_obj._使用類別碼)) ) &&

        //        //    ((_段延伸碼 == null && mdlLand_obj._段延伸碼 == null) || (_段延伸碼!= null && mdlLand_obj._段延伸碼 != null && _段延伸碼.Equals(mdlLand_obj._段延伸碼))) &&
        //        //    ((_轉檔日期 == null && mdlLand_obj._轉檔日期 == null) || (_轉檔日期 != null && mdlLand_obj._轉檔日期 != null && _轉檔日期.Equals(mdlLand_obj._轉檔日期))) &&

        //        //    ( (_公告現值_NTD == null && mdlLand_obj._公告現值_NTD == null) || (_公告現值_NTD != null && mdlLand_obj._公告現值_NTD != null && _公告現值_NTD.Equals(mdlLand_obj._公告現值_NTD)) ) &&
        //        //    ( (_公告地價_NTD == null && mdlLand_obj._公告地價_NTD == null) || (_公告地價_NTD != null && mdlLand_obj._公告地價_NTD != null && _公告地價_NTD.Equals(mdlLand_obj._公告地價_NTD)))
        //        //    )
        //        //{
        //        //    blEqual = true;
        //        //}
        //    }

        //    return blEqual;
        //}


    }
}
