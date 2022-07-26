using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestProject.Models;

namespace TestProject.Service
{
    public interface IDirectoryService
    {
        bool UploadFile();
        byte[] DownloadFile(string FilePath);
        List<Files> GetFolderDetail(string folderPath);
        DirectoryModel GetCurrentDirectory(string directoryPath, string sText = "");
        string UploadFile(HttpRequest httpRequest);
    }

    public class DirectoryService : IDirectoryService
    {
        private string DefaultServerDirectory = ConfigurationManager.AppSettings["RootDirectory"];
        private static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };


        public DirectoryModel GetCurrentDirectory(string directoryPath, string sText = "")
        {
            DirectoryModel model = new DirectoryModel();
            model.CurrentDirectory = new CurrentDirectory();
            model.Files = new List<Files>();
            model.Folders = new List<Folder>();

            if (string.IsNullOrEmpty(directoryPath))
            {
                directoryPath = DefaultServerDirectory;
            }

            if (!Directory.Exists(directoryPath))
            {
                model.ResponseText = "Failed: Directory does not exist!";
                return model;
            }
            var directory = new DirectoryInfo(directoryPath);

            model.CurrentDirectory.Path = directoryPath;
            if (directoryPath.Length == DefaultServerDirectory.Length)
            {
                model.CurrentDirectory.Parent = "";
            }
            else
            {
                model.CurrentDirectory.Parent = directory.Parent.FullName;
            }


            long totalSize = directory.GetFiles("*" + sText + "*.*", SearchOption.AllDirectories).Sum(file => file.Length);
            model.CurrentDirectory.DirectorySize = ConvertFileSize(totalSize);

            //only add dictory object to model when there is no search
            //if (string.IsNullOrEmpty(sText))
            //{
            var directories = GetAllDirectories(directory, sText);
            foreach (var item in directories)
            {
                if (!item.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    model.CurrentDirectory.FolderCount += 1;
                    model.Folders.Add(new Folder { Name = item.Name, FullPath = item.FullName });
                }
            }
            //}




            var files = GetAllFiles(directory, sText);
            foreach (var item in files)
            {
                if (!item.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    model.CurrentDirectory.FileCount += 1;
                    model.Files.Add(new Files { FileName = item.Name, FileSize = ConvertFileSize(item.Length), FileType = item.Extension, DownloadPath = item.FullName });
                }
            }

            return model;
        }

        public List<Files> GetFolderDetail(string folderPath)
        {
            List<Files> files = new List<Files>();
            var list = Directory.GetFiles(folderPath);
            foreach (var item in list)
            {
                files.Add(new Files { FileName = item });
            }

            return files;
        }

        public bool UploadFile()
        {
            return true;
        }


        private FileInfo[] GetAllFiles(DirectoryInfo directoryInfo, string sText = "")
        {
            if (!string.IsNullOrEmpty(sText))
            {
                return directoryInfo.GetFiles("*" + sText + "*.*", SearchOption.AllDirectories);
            }
            return directoryInfo.GetFiles();
        }

        private DirectoryInfo[] GetAllDirectories(DirectoryInfo directoryInfo, string sText = "")
        {
            if (!string.IsNullOrEmpty(sText))
            {
                return directoryInfo.GetDirectories("*" + sText + "*.*", SearchOption.AllDirectories);
            }
            return directoryInfo.GetDirectories();
        }

        private static string ConvertFileSize(Int64 fileSize)
        {
            int counter = 0;
            decimal number = (decimal)fileSize;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, suffixes[counter]);
        }


        public string UploadFile(HttpRequest httpRequest)
        {
            var dirPath = httpRequest["directoryPath"];
            if (string.IsNullOrEmpty(dirPath))
            {
                dirPath = DefaultServerDirectory;
            }
            var docfiles = new List<string>();
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                string fileToUpload = dirPath + "/" + postedFile.FileName;
                //if (!File.Exists(fileToUpload))
                //{
                postedFile.SaveAs(fileToUpload);
                docfiles.Add(fileToUpload);
                //}                               
            }
            return string.Join(",", docfiles);
        }


        public byte[] DownloadFile(string FilePath)
        {
            var stream = new MemoryStream();
            using (var fileStream = File.OpenRead(FilePath))
            {
                stream.CopyTo(fileStream);
                return stream.ToArray();
            }

            return null;            
        }

    }
}
