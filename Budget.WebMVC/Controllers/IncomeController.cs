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
    public class IncomeController : Controller
    {
        // GET: Income
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            var model = service.GetIncome().ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            IncomeCreate model = new IncomeCreate();
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeCategoryService(userId);
            var cat = service.GetIncomeCategories();
            ViewBag.CategoryList = new SelectList(cat, "IncomeCategoryId", "IncomeCategoryName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeCreate model)
        {

            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new IncomeCategoryService(userId);
            var cat = svc.GetIncomeCategories();
            ViewBag.CategoryList = new SelectList(cat, "IncomeCategoryId", "IncomeCategoryName");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = CreateIncomeService();

            if (service.CreateIncome(model))
            {
                TempData["SaveResult"] = "Your category was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Category could not be created.");

            return View(model);
        }

        //public ActionResult AddEditIncome(int IncomeId)
        //{
        //    var service = CreateIncomeService();
        //    var userId = Guid.Parse(User.Identity.GetUserId());
        //    var svc = new IncomeCategoryService(userId);
        //    var cat = svc.GetIncomeCategories();
        //    ViewBag.CategoryList = new SelectList(cat, "IncomeCategoryId", "IncomeCategoryName");
        //    var model = new IncomeEdit();
        //    if (IncomeId > 0)
        //    {
        //        var detail = service.GetIncomeById(IncomeId);

        //        model.IncomeCategoryId = detail.IncomeCategoryId;
        //        model.Amount = detail.Amount;
        //        //model.Categories = detail.Categories;
        //        model.IsRepeat = detail.IsRepeat;
        //        model.Note = detail.Note;
        //    }

        //    return PartialView("_addedit", model);

        //}

        public ActionResult Edit(int id)
        {
            var service = CreateIncomeService();
            var detail = service.GetIncomeById(id);
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new IncomeCategoryService(userId);
            var cat = svc.GetIncomeCategories();
            var model = new IncomeEdit
            {
                Categories = new SelectList(cat, "ExpenseCategoryId", "ExpenseCategoryName"),
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
        public ActionResult Edit(int id, IncomeEdit model)
        {


            if (!ModelState.IsValid) {
                var userId = Guid.Parse(User.Identity.GetUserId());
                var svc = new IncomeCategoryService(userId);
                var cat = svc.GetIncomeCategories();
                model.Categories = new SelectList(cat, "ExpenseCategoryId", "ExpenseCategoryName");
                return View(model);
            }
            return View(model);

            if (model.IncomeId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateIncomeService();

            

            if (service.UpdateIncome(model))
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
            var svc = CreateIncomeService();
            var model = svc.GetIncomeById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeletePost(int id)
        {
            var service = CreateIncomeService();
            service.DeleteIncome(id);
            TempData["SaveResult"] = "You category was deleted";
            return RedirectToAction("Index");
        }

        private IncomeService CreateIncomeService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            return service;
        }
    }
}