using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Service
{
	public interface IDirectoryService
	{
		bool UploadFile();
		bool DownloadFile();
		dynamic GetFolderDetail();
	}

	public class DirectoryService : IDirectoryService
	{
		public bool DownloadFile()
		{
			return true;
		}

		public dynamic GetFolderDetail()
		{
			return "From service";
		}

		public bool UploadFile()
		{
			return true;
		}
	}
}
