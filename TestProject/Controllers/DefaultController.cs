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
        //private readonly IDirectoryService _directoryService;
        public DefaultController()
        {
            //_directoryService = new DirectoryService();
        }
        //public DefaultController(IDirectoryService directoryService)
        //{
        //    _directoryService = directoryService;
        //}
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}