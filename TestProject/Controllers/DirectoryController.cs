using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestProject.Models;
using TestProject.Service;

namespace TestProject.Controllers
{
    public class DirectoryController : ApiController
    {
        private readonly IDirectoryService _directoryService;
        public DirectoryController(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }
        /// <summary>
        /// Get the current directory or search a keyword in the current directory
        /// </summary>
        /// <param name="DirectoryPath">physical path of directory</param>
        /// <param name="sText">keyword to search</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCurrentDirectory(string DirectoryPath = "", string sText = "")
        {
            try
            {
                var model = _directoryService.GetCurrentDirectory(DirectoryPath, sText);
                if (model.ResponseText.Contains("Failed"))
                {
                    return BadRequest(model.ResponseText);
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To upload file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostFile()
        {
            var httpRequest = HttpContext.Current.Request;
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string fileResponse = _directoryService.UploadFile(httpRequest);
                    if (fileResponse.Length > 0)
                    {
                        return Ok("File has been uploaded");
                    }
                    return BadRequest("No file has been uploaded!");
                }
                return BadRequest("Please upload file!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        /// <summary>
        /// Donwload file by path
        /// </summary>
        /// <param name="filePath">absolute path of file</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DownloadFile(string filePath)
        {
            HttpResponseMessage result = null;
            var documentDownload = _directoryService.ReadFileBytes(filePath);

            if (documentDownload != null)
            {
                FileInfo fileInfo = new FileInfo(filePath);

                var stream = new MemoryStream(documentDownload);
                result = Request.CreateResponse(HttpStatusCode.OK);

                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileInfo.Name));
                result.Headers.AcceptRanges.Add("bytes");
                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = string.Concat(fileInfo.Name)
                    };
                result.Content.Headers.ContentDisposition.FileName = fileInfo.Name;
                result.Content.Headers.ContentLength = stream.Length;
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }

            return result;
        }
    }

}