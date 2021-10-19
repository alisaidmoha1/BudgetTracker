using Budget.Data;
using Budget.Models;
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
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            var model = service.GetExpense();
            return View(model);
        }

        public ActionResult Create()
        {
            ExpenseCreate model = new ExpenseCreate();
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseCategoryService(userId);
            var cat = service.GetExpenseCategories();
            model.ExpenseCategories = new SelectList(cat, "ExpenseCategoryId", "ExpenseCategoryName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseCreate model)
        {
            if (!ModelState.IsValid)
            {
                ExpenseCreate mdl = new ExpenseCreate();
                var userId = Guid.Parse(User.Identity.GetUserId());
                var svc = new ExpenseCategoryService(userId);
                var cat = svc.GetExpenseCategories();
                model.ExpenseCategories = new SelectList(cat, "ExpenseCategoryId", "ExpenseCategoryName");
                return View(model);
            }

            var service = CreateExpenseService();

            if (service.CreateExpense(model))
            {
                TempData["SaveResult"] = "Your Expense was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Expense could not be created.");

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateExpenseService();
            var detail = service.GetExpenseById(id);
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new ExpenseCategoryService(userId);
            var cat = svc.GetExpenseCategories();
            var model = new ExpenseEdit
            {
                ExpenseCategories = new SelectList(cat, "ExpenseCategoryId", "ExpenseCategoryName"),
                //ExpenseCategoryId = detail.ExpenseCategoryId,
                ExpenseId = detail.ExpenseId,
                //Category = detail.Category,
                CreatedUtc = detail.CreatedUtc,
                Amount = detail.Amount,
                IsRepeat = detail.IsRepeat,
                Note = detail.Note
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExpenseEdit model)
        {
            ViewData["Edit"] = model;

            if (!ModelState.IsValid) return View(model);

            if (model.ExpenseId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateExpenseService();

            if (service.UpdateExpense(model))
            {
                TempData["SaveResult"] = "You Expense was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your Expense could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateExpenseService();
            var model = svc.GetExpenseById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeletePost(int id)
        {
            var service = CreateExpenseService();
            service.DeleteExpense(id);
            TempData["SaveResult"] = "You Expense was deleted";
            return RedirectToAction("Index");
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        public JsonResult GetMonthlyExpense()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            Dictionary<string, decimal> monthlyExpense = service.CalculateMonthlyExpense();
            return Json(monthlyExpense, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWeeklyExpense()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            Dictionary<string, decimal> weeklyExpense = service.CalculateWeeklyExpense();
            return Json(weeklyExpense, JsonRequestBehavior.AllowGet);

        }

        private ExpenseService CreateExpenseService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            return service;
        }
    }
}