using BusinessLogic.Repositories;
using DataAccess.Models.EF;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryCategory = new CategoryRepository();
        // GET: Admin/Category
        public ActionResult Index()
        {
            IEnumerable<Category> categories = _categoryCategory.GetAll();
            return View(categories);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                _categoryCategory.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Category category = _categoryCategory.GetById(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _categoryCategory.Edit(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool checkResult = _categoryCategory.Delete(id);
            if (checkResult == true)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}