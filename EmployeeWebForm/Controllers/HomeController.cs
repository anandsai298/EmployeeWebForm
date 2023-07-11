using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeWebForm.Controllers
{
    public class HomeController : Controller
    {
        EmployeeManagementEntities1 db=new EmployeeManagementEntities1();
        public ActionResult Index()
        {
            return View();
        }   
    }
}