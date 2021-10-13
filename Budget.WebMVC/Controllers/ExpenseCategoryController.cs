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
    public class ExpenseCategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseCategoryService(userId);
            var model = service.GetExpenseCategories();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseCategoryCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = CreateCategoryService();

            if(service.CreateExpenseCategroy(model))
            {
                TempData["SaveResult"] = "Your category was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Category could not be created.");

            return View(model);
        }

        public ActionResult Edit (int id)
        {
            var service = CreateCategoryService();
            var detail = service.GetExpenseCategoryById(id);
            var model = new ExpenseCategoryEdit
            {
                ExpenseCategoryId = detail.ExpenseCategoryId,
                ExpenseCategoryName = detail.ExpenseCategoryName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (int id, ExpenseCategoryEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if(model.ExpenseCategoryId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateCategoryService();

            if(service.UpdateExpenseCategory(model))
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
            var svc = CreateCategoryService();
            var model = svc.GetExpenseCategoryById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeletePost(int id)
        {
            var service = CreateCategoryService();
            service.DeleteExpenseCategory(id);
            TempData["SaveResult"] = "You category was deleted";
            return RedirectToAction("Index");
        }

        private ExpenseCategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseCategoryService(userId);
            return service;
        }
    }
}