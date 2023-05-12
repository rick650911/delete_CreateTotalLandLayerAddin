using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using ArcGIS.Desktop.Core.Geoprocessing;
using CreateTotalLandLayerAddin;

namespace ArcPyModule
{
    public class ArcPyProcess
    {
        //private static Process CurArcpyProcess;
        private static string pyhonexe;
        private static string syspyfile;
        private static string arcpyscrtiptitle= "arcpy_";
        private static string pyext= ".py";
        private static KeyValuePair<string, string> curgpresult;

        public static void ClearGPResult()
        {
            curgpresult = new KeyValuePair<string, string>(null, null);
        }

        public static string GetGPMessage()
        {
            return curgpresult.Value;
        }

        public static string GetGPStatus()
        {
            return curgpresult.Key;
        }

        private static void SetGPResult(KeyValuePair<string, string> gpresult)
        {
            curgpresult = gpresult;
        }

        public static void SetPyhonexePath(string path)
        {
            pyhonexe = path;
        }

        public static void SetSysPyFilePath(string path)
        {
            syspyfile = path;
        }

        //執行arcpy (顯示視窗)

        public static bool Run_ArcPy_Task(string pythonFile, List<string> args)
        {
            try
            {
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = pyhonexe;

                string sArguments = "\"" + syspyfile + "\\" + pythonFile + "\"";
                foreach (string sigstr in args)
                {
                    sArguments += " " + "\"" + sigstr + "\"";
                }

                startinfo.Arguments = sArguments;
                startinfo.UseShellExecute = false;
                startinfo.RedirectStandardOutput = true;
                //python執行視窗要設置為開啟
                startinfo.CreateNoWindow = false;

                Process process = new Process();
                process.StartInfo = startinfo;
                //測試C#呼叫python 輸出錯誤訊息
                //process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                //process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
                process.Start();
                //process.BeginOutputReadLine();
                process.WaitForExit();
                //取得輸出結果
                int exitcode = process.ExitCode;
                string outputstr = process.StandardOutput.ReadToEnd();
                process.Close();
                SetGPResult(ParsingArcpyStandardOutput(outputstr, exitcode));
                if (GetGPStatus() == "ERROR" || GetGPStatus() == "Exception")
                {
                    //中止執行
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error on Run_ArcPy_Task:" + ex.Message);
                return false;
            }

            return true;

        }


        private static KeyValuePair<string, string> ParsingArcpyStandardOutput(string outputstr,int exitcode)
        {
            //異常終止,回傳例外訊息
            if (exitcode!=0)
            {
                return new KeyValuePair<string, string>("Exception", "An exception occurs,cause program terminate");
            }

            //處理失敗訊息
            if (outputstr.Contains("ERROR"))
            {
                string[] result = outputstr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string s in result)
                {
                    if (s.Contains("ERROR"))
                    {
                        int midindex = s.IndexOf(":");
                        string errmessage = s.Substring(midindex + 2, s.Length - midindex - 2);
                        return new KeyValuePair<string, string>("ERROR", errmessage);
                    }
                }
                return new KeyValuePair<string, string>("ERROR", "");
            }
            //處理警告訊息
            else if (outputstr.Contains("WARNING"))
            {
                int midindex = outputstr.IndexOf(":");
                string warnmessage = outputstr.Substring(midindex + 2, outputstr.Length - midindex - 2);
                warnmessage = Regex.Replace(warnmessage, @"\t|\n|\r", "");
                return new KeyValuePair<string, string>("WARNING", warnmessage);
            }

            //處理成功訊息
            outputstr = Regex.Replace(outputstr, @"\t|\n|\r", "");
            return new KeyValuePair<string, string>("SUCCESS", outputstr);
        }


        public void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }

        //在非同步方法,取得目前運行方法名稱
        public static string GetActualAsyncMethodName([CallerMemberName]string name = null) => name;
        

        public static bool GetCount(List<string> para)
        {
            string funcname = GetActualAsyncMethodName();
            if (!Run_ArcPy_Task(arcpyscrtiptitle + funcname + pyext, para))
            {
                //回傳處理錯誤或異常
                return false;
            }
            return true;
        }

        public static bool FeatureClassToFeatureClass(List<string> para)
        {
            try
            {

                string funcname = GetActualAsyncMethodName();
                if (!Run_ArcPy_Task(arcpyscrtiptitle + funcname + pyext, para))
                {
                    //回傳處理錯誤或異常
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error on FeatureClassToFeatureClass:" + ex.Message);
                return false;
            }

            return true;
        }

        public static bool AddField(List<string> para)
        {
            string funcname = GetActualAsyncMethodName();
            if (!Run_ArcPy_Task(arcpyscrtiptitle + funcname + pyext, para))
            {
                //回傳處理錯誤或異常
                return false;
            }
            return true;
        }

        public static bool AddField(object layerName, string fieldName, string strType, string strLength)
        {
            List<IGPResult> list_output_GpResult = new List<IGPResult>();
            List<string> list_output_ErrorMsg = new List<string>();

            GpTools.AddField(layerName, fieldName, strType,
                            "", "", strLength,
                            fieldName, "NULLABLE",
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

            return true;
        }

        public static bool CalculateField(object inFeatures, string fieldName, string expression, string strExpType, string codeblick)
        {
            List<IGPResult> list_output_GpResult = new List<IGPResult>();
            List<string> list_output_ErrorMsg = new List<string>();

            GpTools.CalculateField( inFeatures,  fieldName,  expression,  strExpType,  codeblick,
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

            return true;
        }

        public static bool GeoTaggedPhotosToPoints(List<string> para)
        {
            try
            {
                string funcname = GetActualAsyncMethodName();
                if (!Run_ArcPy_Task(arcpyscrtiptitle + funcname + pyext, para))
                {
                    //回傳處理錯誤或異常
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error on GeoTaggedPhotosToPoints:" + ex.Message);
                return false;
            }

            return true;
        }

        public static bool CalculateField(List<string> para)
        {
            string funcname = GetActualAsyncMethodName();
            if (!Run_ArcPy_Task(arcpyscrtiptitle + funcname + pyext, para))
            {
                //回傳處理錯誤或異常
                return false;
            }
            return true;
        }

        public static bool Append(List<string> para)
        {
            try
            {

                string funcname = GetActualAsyncMethodName();
                if (!Run_ArcPy_Task(arcpyscrtiptitle + funcname + pyext, para))
                {
                    //回傳處理錯誤或異常
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error on Append:" + ex.Message);
                return false;
            }

            return true;
        }

    }
}
