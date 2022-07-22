using System;
using System.Collections.Generic;
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
		public IHttpActionResult GetCurrentDirectory(string DirectoryPath="")
		{
            try
            {
				return Ok(_directoryService.GetCurrentDirectory(DirectoryPath));
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
		public void Post([FromBody] string value)
		{
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