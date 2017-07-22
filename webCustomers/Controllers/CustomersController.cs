using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webCustomers.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            var ct = new libData.NORTHEntities();

            var res = (from x in ct.Customers
                       select x).ToList();

            return View(res);
        }
    }
}