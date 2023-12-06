using BusinessLogic.Repositories;
using DataAccess.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository = new ProductRepository();
        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page.Value) : 1;
            products = products.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(products);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategory = _productRepository.AddCategoryForProduct();
            return View();
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                _productRepository.AddProduct(model, Images, rDefault);
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = _productRepository.AddCategoryForProduct();
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = _productRepository.AddCategoryForProduct();
            Product product = _productRepository.GetById(id);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Edit(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                bool result = _productRepository.Delete(id);
                if (result == true)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult IsActive(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _productRepository.UpdateIsActive(id);
                if (result != null)
                {
                    return Json(new { success = true, isActive = result.IsActive });
                }
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult IsHome(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _productRepository.UpdateIsHome(id);
                if (result != null)
                {
                    return Json(new { success = true, isHome = result.IsHome });
                }
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult IsSale(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _productRepository.UpdateIsSale(id);
                if (result != null)
                {
                    return Json(new { success = true, isSale = result.IsSale });
                }
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    var result = _productRepository.DeleteAll(ids);
                    if (result != null)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return Json(new { success = false });
        }
    }
}