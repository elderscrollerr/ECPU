using ECPU.Exceptions;
using ECPU.LoadOrderUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
    public static class FileManager
    {

        public static string IsExist(string file)
        {
            string not_find = "";               
                if (!File.Exists(file))
                {
                   not_find = file;
                   return not_find;
                }          
            return not_find;
        }

        public static bool IsNotEmpty(params string[] filePaths)
        {
            bool isNotEmpty = true;
            foreach (string file in filePaths)
            {
                isNotEmpty = (new FileInfo(file).Length > 0);
                if (!isNotEmpty)
                {
                    return isNotEmpty;
                }
            }

            return isNotEmpty;
        }


        public static bool isDirectoryContainFiles(string path)
        {
            if (!Directory.Exists(path)) return false;
            return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any();
        }

        public static ArrayList GetContentAsLines(string filePath)
        {

            string[] content = File.ReadAllLines(filePath);
            ArrayList listOfLines = new ArrayList();

            foreach (string line in content)
            {
                listOfLines.Add(line);
            }

            return listOfLines;
        }

        public static void WriteAllLines(string filePath, ArrayList content)
        {

            string[] wl = (string[])content.ToArray(typeof(string));


            File.WriteAllLines(filePath, wl);
        }

        



        public static void remove(params string[] paths)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch
                    {

                        MessageBox.Show("Не удалось удалить файл: " + path);
                    }
                }
                else if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path, true);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось удалить папку: " + path);
                    }
                }
            }



        }

        public static void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    MessageBox.Show("Не удалось создать папку: " + path);
                }
            }
        }


        public static void CopyAllDirectoryContent(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
          
            foreach (FileInfo fi in source.GetFiles())
            {
             
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

          
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAllDirectoryContent(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static bool copyFiles(string source, string target)
        {
            bool result = true;
            if (File.Exists(source))
            {
                try
                {
                    File.Copy(source, target, true);
                    return result;
                }
                catch
                {
                    result = false;
                    MessageBox.Show("Не удалось скопировать: " + source);
                    return result;
                }
            }
            else
            {
                result = false;
                return result;
            }
        }

       

        public static void downloadBig(string httpLink, string target)
        {
            const int BUFFER_SIZE = 16 * 1024;
            using (var outputFileStream = File.Create(target, BUFFER_SIZE))
            {
                var req = WebRequest.Create(httpLink);
                using (var response = req.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        var buffer = new byte[BUFFER_SIZE];
                        int bytesRead;
                        do
                        {
                            bytesRead = responseStream.Read(buffer, 0, BUFFER_SIZE);
                            outputFileStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
            }
        }


        public static string[] getOnlyFileNamesFromDirectory(string directoryPath)
        {
            List<string> filesNames = new List<string>();
            string[] fileWithPaths = Directory.GetFiles(directoryPath);
            string[] dirsWithPaths = Directory.GetDirectories(directoryPath);
            try
            {
                for (int i = 0; i < fileWithPaths.Length; i++)
                {
                    filesNames.Add(Path.GetFileName(fileWithPaths[i]));
                }
                for (int i = 0; i < dirsWithPaths.Length; i++)
                {
                    filesNames.Add(Path.GetFileName(dirsWithPaths[i]));
                }
            }
            catch
            {

            }

            return filesNames.ToArray();
        }

        public static void MoveWithReplace(string sourceFileName, string destFileName)
        {

          
            if (File.Exists(destFileName))
            {
                File.Delete(destFileName);
            }

            File.Move(sourceFileName, destFileName);

            if (File.Exists(sourceFileName))
            {
                File.Delete(sourceFileName);
            }
        }

    }
}
