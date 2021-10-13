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
    public class IncomeCategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeCategoryService(userId);
            var model = service.GetIncomeCategories();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeCategoryCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = CreateIncomeCategoryService();

            if (service.CreateIncomeCategroy(model))
            {
                TempData["SaveResult"] = "Your category was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Category could not be created.");

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateIncomeCategoryService();
            var detail = service.GetIncomeCategoryById(id);
            var model = new IncomeCategoryEdit
            {
                IncomeCategoryId = detail.IncomeCategoryId,
                IncomeCategoryName = detail.IncomeCategoryName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IncomeCategoryEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.IncomeCategoryId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateIncomeCategoryService();

            if (service.UpdateIncomeCategory(model))
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
            var svc = CreateIncomeCategoryService();
            var model = svc.GetIncomeCategoryById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeletePost(int id)
        {
            var service = CreateIncomeCategoryService();
            service.DeleteIncomeCategory(id);
            TempData["SaveResult"] = "You category was deleted";
            return RedirectToAction("Index");
        }

        private IncomeCategoryService CreateIncomeCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeCategoryService(userId);
            return service;
        }
    }
}