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
	}

	public class DirectoryService : IDirectoryService
	{
		private const string DefaultServerDirectory = @"D:\";
		public bool DownloadFile()
		{
			return true;
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
	}
}
