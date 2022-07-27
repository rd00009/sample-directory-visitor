using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Service;

namespace TestProject.Controllers
{
	public class DefaultController : Controller
	{		
        public DefaultController()
        {            
        }      
        public ActionResult Index()
		{			
			return View();
		}
	}
}