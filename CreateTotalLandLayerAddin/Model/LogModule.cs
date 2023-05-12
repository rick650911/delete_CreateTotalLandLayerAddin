using System;
using System.IO;


namespace LogModule
{

    class Log
    {
        private static FileStream ufs;
        private static StreamWriter usw;
        private static bool processlogisopen=false;
        //取得執行程式本身的路徑
        //即使由別處啟動,call A程式, 也能取得A程式所在目錄
        private static string logfloder = AppDomain.CurrentDomain.BaseDirectory + "//log";
        private static string processlogtitle = "processlog_";


        public static string GetLogFloder()
        {
            return logfloder;
        }


        public static void WriteErrorLog(string errormsg)
        {
            string logpath = logfloder + "//errorlog.txt";

            //建立存放log資料夾
            if (!System.IO.Directory.Exists(logfloder))
            {
                System.IO.Directory.CreateDirectory(logfloder);
            }

            if (!System.IO.File.Exists(logpath))
            {
                //沒有則創建這個文件
                FileStream fs = new FileStream(logpath, FileMode.Create, FileAccess.Write);//創建寫入文件                                                                                                                                                                       //設置文件屬性為隱藏
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(errormsg + "發生時間: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(logpath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(errormsg + "發生時間: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Close();
                fs.Close();
            }
        }


        public static void CreateaorOpenProcessLog()
        {
            string logpath = logfloder + "//"+ processlogtitle + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            //建立存放log資料夾
            if (!System.IO.Directory.Exists(logfloder))
            {
                System.IO.Directory.CreateDirectory(logfloder);
            }

            if (!System.IO.File.Exists(logpath))
            {
                //沒有則創建這個文件
                ufs = new FileStream(logpath, FileMode.Create, FileAccess.Write);//創建寫入文件                                                                                                                                                                       //設置文件屬性為隱藏
                usw = new StreamWriter(ufs);   
            }
            else
            {
                ufs = new FileStream(logpath, FileMode.Append);
                usw = new StreamWriter(ufs);
            }
            processlogisopen = true;
        }

        public static void CloseProcessLog()
        {
            usw.Close();
            ufs.Close();
            usw = null;
            ufs = null;
            processlogisopen = false;
        }
       
        //close給false,可關閉輸出流 (用於最後寫入)
        public static void WriteProcessLog(string msg,bool close=true)
        {
            if (!processlogisopen)
            {
                CreateaorOpenProcessLog();
            }

            usw.WriteLine(msg);
            usw.Flush();

            if (!close)
            {
                CloseProcessLog();
            }
        }

        //不換行
        public static void WriteProcessLog2(string msg, bool newline = false, bool close = true)
        {
            if (!processlogisopen)
            {
                CreateaorOpenProcessLog();
            }

            usw.Write(msg);
            if(newline)
                usw.WriteLine("");
            usw.Flush();

            if (!close)
            {
                CloseProcessLog();
            }
        }

    }
}
