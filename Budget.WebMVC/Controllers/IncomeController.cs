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
            var model = service.GetIncome();
            return View(model);
        }

        public ActionResult Create()
        {
            IncomeCreate model = new IncomeCreate();
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeCategoryService(userId);
            var cat = service.GetIncomeCategories();
            model.IncomeCategories =  new SelectList(cat, "IncomeCategoryId", "IncomeCategoryName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeCreate model)
        {

            if (!ModelState.IsValid)
            {
                var userId = Guid.Parse(User.Identity.GetUserId());
                var svc = new IncomeCategoryService(userId);
                var cat = svc.GetIncomeCategories();
                model.IncomeCategories = new SelectList(cat, "IncomeCategoryId", "IncomeCategoryName");
                return View(model);
            }

            var service = CreateIncomeService();

            if (service.CreateIncome(model))
            {
                TempData["SaveResult"] = "Your Income was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Income could not be created.");

            return View(model);
        }

        public ActionResult CashFlow()
        {
            return PartialView("_cashFlow");
        }

        public ActionResult Edit(int id)
        {
            var service = CreateIncomeService();
            var detail = service.GetIncomeById(id);
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new IncomeCategoryService(userId);
            var cat = svc.GetIncomeCategories();
            var model = new IncomeEdit
            {
                IncomeCategories = new SelectList(cat, "IncomeCategoryId", "IncomeCategoryName"),
                IncomeId = detail.IncomeId,
                //CategoryId = detail.CategoryId,
                CreatedUtc = detail.CreatedUtc,
                Amount = detail.Amount,
                Note = detail.Note
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IncomeEdit model)
        {
            ViewData["Edit"] = model;

            if (!ModelState.IsValid) return View(model);

            if (model.IncomeId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateIncomeService();

            

            if (service.UpdateIncome(model))
            {
                TempData["SaveResult"] = "You Income was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your Income could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateIncomeService();
            var model = svc.GetIncomeById(id);
            return View(model);
        }

        public ActionResult IncomeSummary()
        {
            return PartialView("_IncomeReport");
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeletePost(int id)
        {
            var service = CreateIncomeService();
            service.DeleteIncome(id);
            TempData["SaveResult"] = "You Income was deleted";
            return RedirectToAction("Index");
        }

        public JsonResult GetYearlyIncome()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            Dictionary<string, decimal?> yearlyIncome = service.CalculateYearlyIncome();
            return Json(yearlyIncome, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthlyIncome()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            Dictionary<string, decimal?> monthlyIncome = service.CalculateMonthlyIncome();
            return Json(monthlyIncome, JsonRequestBehavior.AllowGet);

        }

        private IncomeService CreateIncomeService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            return service;
        }
    }
}