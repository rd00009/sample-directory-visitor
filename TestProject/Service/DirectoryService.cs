using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Models;

namespace TestProject.Service
{
    public interface IDirectoryService
    {
        bool UploadFile();
        bool DownloadFile();
        List<Files> GetFolderDetail(string folderPath);
        DirectoryModel GetCurrentDirectory(string directoryPath);
    }

    public class DirectoryService : IDirectoryService
    {
        private const string DefaultServerDirectory = @"D:\";
        private static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        public bool DownloadFile()
        {
            return true;
        }

        public DirectoryModel GetCurrentDirectory(string directoryPath)
        {
            DirectoryModel model = new DirectoryModel();
            model.CurrentDirectory = new CurrentDirectory();
            model.Files = new List<Files>();
            model.Folders = new List<Folder>();
            long dirSize = 0;
            if (string.IsNullOrEmpty(directoryPath))
            {
                directoryPath = DefaultServerDirectory;
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


            //var fileNames = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            //foreach (var file in fileNames)
            //{
            //    var fileInfo = new FileInfo(file);
            //    dirSize += fileInfo.Length;
            //}

            //model.CurrentDirectory.DirectorySize = ConvertFileSize(dirSize);

            var directories = GetAllDirectories(directory);
            var files = GetAllFiles(directory);

            foreach (var item in directories)
            {
                if (!item.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    model.CurrentDirectory.FolderCount += 1;
                    model.Folders.Add(new Folder { Name = item.Name, FullPath = item.FullName });
                }
            }

            foreach (var item in files)
            {
                if (!item.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    model.CurrentDirectory.FileCount += 1;
                    model.Files.Add(new Files { FileName = item.Name, FileSize = ConvertFileSize(item.Length), FileType = item.Extension, DownloadPath = item.DirectoryName });
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


        private FileInfo[] GetAllFiles(DirectoryInfo directoryInfo)
        {
            return directoryInfo.GetFiles();
        }

        private DirectoryInfo[] GetAllDirectories(DirectoryInfo directoryInfo)
        {
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

    }
}
