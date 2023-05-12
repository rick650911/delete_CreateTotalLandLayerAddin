
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace UtilityClassLibrary
{
    public class IoUtility
    {
        public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            bool blSuccess = false;

            try
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();

                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                // If the destination directory doesn't exist, create it. 
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, true);
                }

                // If copying subdirectories, copy them and their contents to new location. 
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }

                blSuccess = true;
            }
            catch (Exception ex)
            {
                //Logger.WriteError("IoUtility", "DirectoryCopy", string.Empty, ex);
            }

            return blSuccess;
        }

        /// <summary>
        /// 檢查是否在存在資料夾，有的話刪除
        /// </summary>
        /// <param name="argStrCaseNo"></param>
        /// <returns>是否成功執行</returns>
        public static bool Check_Delete_IfExistFolder(string strFolder)
        {
            ////1. 檢查是否在存在案件編號之shapefile資料夾，有的話先刪除
            //string strCaseRootFolder = EnvSettings.Path_Part1_Case + @"\" + argStrCaseNo;// "Case2015A0002";

            bool blSuccess = false;

            try
            {
                bool blDirectoryExists = Directory.Exists(strFolder);

                if (blDirectoryExists)
                {
                    Directory.Delete(strFolder, true);
                }

                blSuccess = true;
            }
            catch (Exception ex)
            {
                //Logger.WriteError("IoUtility", "CheckIfExistShapefileFolder", "folder:" + strFolder, ex);
            }

            return blSuccess;
        }

    }
}
