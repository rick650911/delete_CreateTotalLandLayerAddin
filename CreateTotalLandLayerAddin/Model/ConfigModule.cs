//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ConfigModule
//{
//    class Config
//    {
//        private static Configuration configFile;
//        private static KeyValueConfigurationCollection configsettings;
//        private static ConnectionStringsSection connectionStrings;
//        private static bool configisopen = false;

//        //開啟config檔案
//        //只有要更新config檔案時,才需用到此方法
//        public static bool Open(ref string errmsg)
//        {
//            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//            if (configFile.HasFile)
//            {
//                configsettings = configFile.AppSettings.Settings;
//                connectionStrings = configFile.ConnectionStrings;
//                configisopen = true;
//                return true;
//            }
//            else
//            {
//                errmsg = "config檔案不存在";
//                configisopen = false;
//                return false;
//            }

//        }

//        public static string GetAppSettingsValue(string index)
//        {
//            return ConfigurationManager.AppSettings[index];
//        }

//        public static string GetConnectionStringsValue(string index)
//        {
//            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[index];
//            if (css == null)
//            {
//                return null;
//            }

//            return css.ConnectionString;
//        }

//        //必須先open才可以使用
//        public static void SetAppSettingsValue(string index, string value)
//        {
//            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//            connectionStrings.ConnectionStrings[""].ConnectionString = "";

//            if (configisopen)
//            {
//                try 
//                {
//                    configsettings[index].Value = value;
//                    configFile.Save(ConfigurationSaveMode.Modified);
//                }
//                catch (Exception ex)
//                { 
                
//                }
//            }
//        }

//        public static void SetConnectionStringsValue(string index, string value)
//        {
//            if (configisopen)
//            {
//                try
//                {
//                    connectionStrings.ConnectionStrings[index].ConnectionString = value;
//                    configFile.Save(ConfigurationSaveMode.Modified);
//                }
//                catch (Exception ex)
//                {

//                }
//            }
//        }

//        public static bool CheckAppSettingsDigitalParm(List<string> configindex,ref string errmsg)
//        {
//            bool check = true;
//            foreach (string index in configindex)
//            {
//                if (ConfigurationManager.AppSettings[index] != null)
//                {
//                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
//                    if (!reg.IsMatch(ConfigurationManager.AppSettings[index]))
//                    {
//                        errmsg += index+"參數非數字\n";
//                        check = false;
//                    }
//                }
//                else
//                {
//                    errmsg += index + "參數不存在config中\n";
//                    check = false;
//                }
//            }
//            return check;
//        }

//        public static bool CheckAppSettingsStringParm(List<string> configindex, ref string errmsg)
//        {
//            bool check = true;
//            foreach (string index in configindex)
//            {
//                if (ConfigurationManager.AppSettings[index] == null)
//                {
//                    errmsg += index+ "參數不存在config中\n";
//                    check = false;
//                }
//            }
//            return check;
//        }

//        //檢查檔案目錄是否有效
//        //(並未檢查檔案是否存在)
//        public static bool CheckAppSettingsFileFolderParm(List<string> configindex, bool checkexist, ref string errmsg)
//        {
//            bool check = true;
//            foreach (string index in configindex)
//            {
//                string folderpath = ConfigurationManager.AppSettings[index];
//                if (folderpath == null)
//                {
//                    errmsg += index + "檔案目錄參數不存在config中\n";
//                    check = false;
//                }
//                else if (string.IsNullOrEmpty(folderpath) || folderpath.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
//                {
//                    errmsg += index + "檔案目錄參無效(目錄是空字串)\n";
//                    check = false;
//                }
//                else if (folderpath.Substring(0, 1) == " " || folderpath.Substring(folderpath.Length - 1, 1) == " ")
//                {
//                    errmsg += index + "檔案目錄參無效(目錄頭尾是空白)\n";
//                    check = false;
//                }
//                else if (checkexist && !Directory.Exists(folderpath))
//                {
//                    errmsg += index + "檔案目錄不存在\n";
//                    check = false;
//                }
//            }
//            return check;
//        }

//        //檢查檔案路徑是否有效
//        //(並未檢查檔案是否存在)
//        public static bool CheckAppSettingsFilePathParm(List<string> configindex, bool checkexist, ref string errmsg)
//        {
//            bool check = true;
//            foreach (string index in configindex)
//            {
//                string filepath = ConfigurationManager.AppSettings[index];
//                if (filepath == null)
//                {
//                    errmsg += index + "參數不存在config中\n";
//                    check = false;
//                }
//                else if (string.IsNullOrEmpty(filepath))
//                {
//                    errmsg += index + "檔案路徑參無效(路徑是空字串)\n";
//                    check = false;
//                }
//                else if (filepath.Substring(0, 1) == " " || filepath.Substring(filepath.Length - 1, 1) == " ")
//                {
//                    errmsg += index + "檔案路徑參無效(路徑頭尾是空白)\n";
//                    check = false;
//                }
//                else
//                {
//                    string folder = Path.GetDirectoryName(filepath);
//                    string filename = Path.GetFileName(filepath);

//                    if (folder.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0 || filename.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
//                    {
//                        errmsg += index + "檔案路徑參無效(含非法字元)\n";
//                        check = false;
//                    }
//                    else if(checkexist && !File.Exists(filepath))
//                    {
//                        errmsg += index + "檔案不存在\n";
//                        check = false;
//                    }
  
//                }
//            }
            
//            return check;
//        }

       

//    }
//}
