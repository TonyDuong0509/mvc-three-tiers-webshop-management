using BusinessLogic.Repositories;
using DataAccess.Models;
using DataAccess.Models.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository = new ProductRepository();
        private readonly IProductCategoryRepository _productCategoryRepository = new ProductCategoryRepository();
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ProductsController()
        {
        }

        public ProductsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Products
        public ActionResult Index(string searching, int? page)
        {
            IEnumerable<Product> products = _productRepository.GetAllProductForUser(searching, page);
            var pageSize = 8;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page.Value) : 1;
            products = products.ToPagedList(pageIndex, pageSize);
            return View(products);
        }

        public ActionResult Detail(int id)
        {
            Product product = _productRepository.GetDetail(id);
            return View(product);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            List<Product> products = _productRepository.GetProductCategory(alias, id);
            ProductCategory productCategoryName = _productCategoryRepository.GetById(id);
            if (productCategoryName != null)
            {
                ViewBag.ProductCategoryName = productCategoryName.Title;
            }
            ViewBag.CateId = id;
            return View(products);
        }


        public ActionResult DisplayItemsByCateId()
        {
            List<Product> products = _productRepository.DisplayItemsByCateId();
            return PartialView(products);
        }


        public ActionResult DisplayItemsIsSale()
        {
            List<Product> products = _productRepository.DisplayItemsIsSale();
            return PartialView(products);
        }

        public ActionResult PartialAddReview(int productId)
        {
            Product product = _productRepository.GetDetail(productId);
            if (product != null)
            {
                ProductReviewViewModel productReviewViewModel = new ProductReviewViewModel();
                productReviewViewModel.ProductId = product.Id;
                return PartialView(productReviewViewModel);
            }
            return PartialView(product);
        }


        [HttpPost]
        public ActionResult AddReview(ProductReviewViewModel productReviewViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductReview productReview = new ProductReview();
                productReview.ProductId = productReviewViewModel.ProductId;
                productReview.UserName = User.Identity.Name;
                productReview.Rating = productReviewViewModel.Rating;
                string comment = productReviewViewModel.Comment;
                if (comment == null)
                {
                    comment = string.Empty;
                }
                else if (comment != null)
                {
                    productReview.Comment = comment;
                }
                productReview.CreatedDate = DateTime.Now;
                _dbContext.ProductReviews.Add(productReview);
                _dbContext.SaveChanges();
                return Json(new { Success = true });
            }
            return View("PartialAddReview", productReviewViewModel);
        }

        public ActionResult PartialUserReview(int productId)
        {
            List<ProductReview> productReviews = _dbContext.ProductReviews.Where(x => x.ProductId == productId).ToList();
            if (productReviews.Any())
            {
                double averageRating = productReviews.Average(x => x.Rating);
                Product product = _dbContext.Products.Find(productId);
                if (product != null)
                {
                    product.AverageRating = (float)averageRating;
                    _dbContext.Entry(product).Property(x => x.AverageRating).IsModified = true;
                    _dbContext.SaveChanges();
                }
                ViewBag.ShowCount = productReviews.Count;
                return PartialView(productReviews);
            }
            return PartialView();
        }
    }
}