using Budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Budget.WebMVC.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult Index()
        {
            var model = new ExpenseListItem[0];
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}