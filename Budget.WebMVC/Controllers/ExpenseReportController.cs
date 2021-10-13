using Budget.Data;
using Budget.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Budget.WebMVC.Controllers
{
    [Authorize]
    public class ExpenseReportController : Controller
    {
        // GET: ExpenseReport
        //public ActionResult Index()
        //{
        //    var userId = Guid.Parse(User.Identity.GetUserId());
        //    var service = new ExpenseReportService(userId);
        //    var model = service.GetAllExpenses().ToList();
        //    return View(model);
        //}

        public ActionResult Index(string searchString)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseReportService(userId);
            var model = service.GetAllExpenses().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                model = service.GetSearchResults(searchString).ToList();
            }
            return View(model);
        }

        public ActionResult AddEditExpense(int itemId)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseReportService(userId);
            var model = new ExpenseReport();

            if (itemId > 0)
            {
                model = service.GetExpenseData(itemId);
            }

            return PartialView("_expenseForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseReport newExpense)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseReportService(userId);
            if (ModelState.IsValid)
            {
                if (newExpense.ItemId > 0)
                {
                    service.UpdateExpense(newExpense);
                }
                else
                {
                    service.AddExpense(newExpense);
                }
            }

                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseReportService(userId);
            service.DeleteExpense(id);
            TempData["SaveResult"] = "You category was deleted";
            return RedirectToAction("Index");
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        public JsonResult GetMonthlyExpense()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseReportService(userId);
            Dictionary<string, decimal> monthlyExpense = service.CalculateMonthlyExpense();
            return Json(monthlyExpense, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWeeklyExpense()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseReportService(userId);
            Dictionary<string, decimal> weeklyExpense = service.CalculateWeeklyExpense();
            return Json(weeklyExpense, JsonRequestBehavior.AllowGet);
            
        }
    }
}
