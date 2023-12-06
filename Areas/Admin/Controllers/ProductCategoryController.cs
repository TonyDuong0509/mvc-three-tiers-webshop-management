using BusinessLogic.Repositories;
using DataAccess.Models.EF;
using System.Data.Entity;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository = new ProductCategoryRepository();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var items = _productCategoryRepository.GetAll();
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                _productCategoryRepository.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}