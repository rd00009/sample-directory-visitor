using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class DirectoryModel
    {
        public CurrentDirectory CurrentDirectory { get; set; }
        public List<Folder> SiteMap { get; set; }
        public List<Files> Files { get; set; }
        public List<Folder> Folders { get; set; }
        public string ResponseText { get; set; }=string.Empty;
    }

    public class CurrentDirectory
    {        
        public int FileCount { get; set; }
        public int FolderCount { get; set; }
        public string Path { get; set; }
        public string DirectorySize { get; set; }
        public string Parent { get; set; }
    }
    public class Files
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string DownloadPath { get; set; }
    }

    public class Folder
    {
        public string Name { get; set; }
        public string FullPath { get; set; }        
    }
}
