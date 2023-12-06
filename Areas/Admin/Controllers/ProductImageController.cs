using BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IProductImageRepository _productImageRepository = new ProductImageRepository();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _productImageRepository.GetAll(id);
            return View(items);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool checkResult = _productImageRepository.Delete(id);
            if (checkResult == true)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            _productImageRepository.AddImage(productId, url);
            return Json(new { success = true });
        }
    }
}