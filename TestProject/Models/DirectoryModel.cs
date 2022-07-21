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
        public List<Files> Files { get; set; }
    }

    public class CurrentDirectory
    {
        public string Size { get; set; }
        public string FileCount { get; set; }
        public string Path { get; set; }
    }
    public class Files
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
    }
}
