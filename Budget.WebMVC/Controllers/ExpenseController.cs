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
            var service = new CategoryService(userId);
            var cat = service.GetCategories();
            model.Categories = new SelectList(cat, "CategoryId", "CategoryName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseCreate model)
        {
            if (!ModelState.IsValid)
            {
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

        private ExpenseService CreateExpenseService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            return service;
        }
    }
}