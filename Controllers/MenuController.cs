using BusinessLogic.Repositories;
using DataAccess.Models.EF;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebShopOnline.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuRepository _menuRepository = new MenuRepository();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
        {
            List<Category> categories = _menuRepository.GetMenuTop();
            return PartialView("_MenuTop", categories);
        }


        public ActionResult MenuProductCategory()
        {
            List<ProductCategory> productCategories = _menuRepository.GetMenuProductCategory();
            return PartialView("_MenuProductCategory", productCategories);
        }


        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            List<ProductCategory> productCategories = _menuRepository.GetMenuLeft();
            return PartialView("_MenuLeft", productCategories);
        }


        public ActionResult MenuArrivals()
        {
            List<ProductCategory> productCategories = _menuRepository.GetMenuArrivals();
            return PartialView("_MenuArrivals", productCategories);
        }
    }
}