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
                TempData["SaveResult"] = "Your category was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Category could not be created.");

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
                //CategoryId = detail.CategoryId,
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
            
            if (!ModelState.IsValid) return View(model);

            if (model.ExpenseId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateExpenseService();

            if (service.UpdateExpense(model))
            {
                TempData["SaveResult"] = "You category was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your Category could not be updated.");
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
            TempData["SaveResult"] = "You category was deleted";
            return RedirectToAction("Index");
        }

        private ExpenseService CreateExpenseService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            return service;
        }
    }
}