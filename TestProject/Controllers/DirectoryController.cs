using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        // GET api/<controller>
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

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //	return "value";
        //}

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult PostFile()
        {
            var httpRequest = HttpContext.Current.Request;
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string fileResponse = _directoryService.UploadFile(httpRequest);
                    return Ok("File has been uploaded");
                }
                return BadRequest("Please upload file!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}