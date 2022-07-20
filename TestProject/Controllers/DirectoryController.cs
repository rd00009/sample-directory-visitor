﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2", _directoryService.GetFolderDetail() };
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

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