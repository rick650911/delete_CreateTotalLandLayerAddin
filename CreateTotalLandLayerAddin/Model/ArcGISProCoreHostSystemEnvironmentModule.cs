using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArcGISProCoreHostSystemEnvironmentModule
{
    class ArcGISProCoreHostSystemEnvironment
    {
        public static bool CheckProCoreHostSystemDll(string ArcGISSysEnvironmentPath, out string errormsg)
        {
            errormsg = "";
            //取得本應用程式編譯時,專案所設定的參考元件版本
            AssemblyName[] assemblies = Assembly.GetCallingAssembly().GetReferencedAssemblies();
            assemblies = assemblies.Where(x => x.Name == "ArcGIS.Core" || x.Name == "ArcGIS.CoreHost").ToArray();
            if (assemblies.Length < 2)
            {
                errormsg = "程式未加入ArcGIS.Core及ArcGIS.CoreHost參考,無法檢測相關dll版本是否與系統環境相符\n程式自動結束";
                return false;
            }

            //專案編譯時參考的元件版本,可能會和config重新導向的元件版本不同
            //檢查config導向的元件版本,是否和主機所安裝的元件版本不同即可
            int check = 0;
            string configpath = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".config";
            XDocument xmlDocument = XDocument.Load(configpath);
            foreach (AssemblyName assembly in assemblies)
            {
                //首先檢查config中,是否有設定特定版本
                //若有,檢查系統環境dll版本是否符合config特定版本
                //若沒有,檢查系統環境dll版本是否與編譯時版本相同
                string bindingRedirectversion = GetbindingRedirectVersion(xmlDocument, assembly.FullName);

                //若版本不同,提示需要執行安裝程式
                //若版本相同,但程式目錄dll版本不同,也提醒需要執行安裝程式
                string srccomponentpath = ArcGISSysEnvironmentPath + @"\" + assembly.Name + ".dll"; ;
                string destcomponentpath = AppDomain.CurrentDomain.BaseDirectory + assembly.Name + ".dll";
                string dllname = assembly.Name + ".dll";
                //若系統檔案不存在
                if (!File.Exists(srccomponentpath))
                {
                    check = 1;
                    errormsg = dllname + "不在系統目錄中";
                }
                else if (!File.Exists(destcomponentpath))
                {
                    check = 2;
                    errormsg = "找不到" + dllname + "(不在執行檔所在目錄)";

                }
                //比較bindingRedirectversion,目錄所在dll版本,是否和主機環境相同
                else if (bindingRedirectversion != GetVersionfComponent(srccomponentpath) || GetVersionfComponent(destcomponentpath) != GetVersionfComponent(srccomponentpath))
                {
                    check = 2;
                    errormsg = "執行檔所在目錄" + dllname + "版本與主機環境(系統目錄)版本不同\n或是程式預設使用的" + dllname + "版本與主機環境(系統目錄)版本不同";
                }
            }

            if (check == 1)
            {
                errormsg = "請確認主機有無安裝ArcGIS Pro,系統目錄是否正確\n程式自動結束";
                return false;
            }
            else if (check == 2)
            {
                errormsg = "請先執行安裝設定程式,完成程式執行環境設定\n程式自動結束";
                return false;
            }

            return true;
        }

        //檢測config和程式導向的dll版本資訊
        //若config有導向版本資訊,回傳config中紀錄的版本資訊
        //若沒有,回傳程式本身使用
        private static string GetbindingRedirectVersion(XDocument xmlDocument, string assemblyfullname)
        {
            XNamespace ns = "urn:schemas-microsoft-com:asm.v1";
            assemblyfullname = assemblyfullname.Replace(", ", ",");
            List<string> fullnamelist = assemblyfullname.Split(',').ToList();
            string version = fullnamelist[1].Replace("Version=", "");
            var xeassemblyBindingList = xmlDocument.Descendants("runtime").Elements().Where(x => x.Name.LocalName == "assemblyBinding").ToList();
            if (xeassemblyBindingList.Count == 1)
            {
                XElement assemblyBinding = xeassemblyBindingList[0];
                var dependentAssembly = assemblyBinding.Elements(ns + "dependentAssembly").Where(x => x.Element(ns + "assemblyIdentity").Attribute("name")?.Value == fullnamelist[0]).ToList();
                if (dependentAssembly.Count == 1)
                {
                    XElement bindingRedirect = dependentAssembly[0].Element(ns + "bindingRedirect");
                    if (bindingRedirect != null)
                    {
                        string configversion = bindingRedirect.Attribute("newVersion")?.Value;
                        if (configversion != null)
                        {
                            version = configversion;
                        }
                    }

                }
            }
            return version;
        }

        private static string GetVersionfComponent(string ComponentPath)
        {
            //用不載入dll方式,取得dll資訊
            string fullname = AssemblyName.GetAssemblyName(ComponentPath).ToString();
            fullname = fullname.Replace(", ", ",");
            List<string> fullnamelist = fullname.Split(',').ToList();
            return fullnamelist[1].Replace("Version=", "");
        }

    }
}
