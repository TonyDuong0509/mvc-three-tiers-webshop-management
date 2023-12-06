using BusinessLogic.Repositories;
using BusinessLogic.Services;
using DataAccess.Models;
using DataAccess.Models.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ISubscribeService _subscribeService = new SubscribeService();
        private readonly IProductRepository _productRepository = new ProductRepository();
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        private IMailService _mailService = new EmailServices();
        private ApplicationUserManager _userManager;
        public ShoppingCartController()
        {

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
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        public ActionResult CheckOutSuccess()
        {
            return View();
        }

        public ActionResult Partial_Item_Payment()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.items.Any())
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.items.Any())
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                return Json(new { Count = cart.items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { success = false, msg = "", code = -1, Count = 0 };
            var dbContext = new ApplicationDbContext();
            var checkProduct = _productRepository.CheckProduct(id);
            if (checkProduct != null)
            {
                ShoppingCart cart = Session["Cart"] as ShoppingCart;
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.productCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = checkProduct.PriceSale;
                    item.TotalPrice = item.Quantity * checkProduct.Price;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { success = true, msg = $"Added {quantity} {checkProduct.Title} in cart !!!", code = 1, Count = cart.items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { success = false, code = -1, Count = 0 };
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                ShoppingCartItem checkProduct = cart.items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { success = true, code = 1, Count = cart.items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel model)
        {
            var code = new { success = false, code = 0 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = Session["cart"] as ShoppingCart;
                if (cart != null)
                {
                    var userId = User.Identity.GetUserId();
                    var user = UserManager.FindById(userId);
                    string userEmail = user.Email;
                    if (user != null && !user.FirstLoginDate.HasValue)
                    {
                        foreach (var item in cart.items)
                        {
                            item.Price = item.Price * (decimal)0.9;
                        }
                        user.FirstLoginDate = DateTime.Now;
                        UserManager.Update(user);
                    }
                    var emailToCheck = _subscribeService.CheckEmailForSubscribe(userEmail);
                    if (emailToCheck != null && !emailToCheck.FirstLoginDate.HasValue)
                    {
                        foreach (var item in cart.items)
                        {
                            item.Price = item.Price * (decimal)0.9;
                        }
                        emailToCheck.FirstLoginDate = DateTime.Now;
                    }

                    // Create Order

                    Order order = new Order();
                    order.CustomerName = model.CustomerName;
                    order.Phone = model.Phone;
                    order.Address = model.Address;
                    order.Email = model.Email;
                    cart.items.ForEach(x => order.orderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = model.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifierDate = DateTime.Now;
                    order.CreatedBy = model.Phone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();

                    // SendMail for customer
                    _mailService.SendConfirmationEmails(cart, order, model.Email);
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }
    }
}