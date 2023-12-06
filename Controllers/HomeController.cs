using BusinessLogic.Services;
using DataAccess.Models.EF;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubscribeService _subscribeService = new SubscribeService();
        private ApplicationUserManager _userManager;
        public HomeController()
        {

        }
        public HomeController(ApplicationUserManager userManager)
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Subscribe()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Subscribe(Subscribe model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                if (userId == null)
                {
                    return Json(new { Success = false, ErrorMessage = "You have not log in, please log in before subscribe." });
                }
                else
                {
                    var userEmail = UserManager.GetEmail(userId);
                    if (userEmail == model.Email)
                    {
                        _subscribeService.Subscribe(model);
                        BusinessLogic.Services.EmailServices.SendMailInBackground("Shine Shop", "Thank you !", "Shine Shop thanks you for your subscribe to support. Your order will be discount 10% for the first purchase.", model.Email);
                        return Json(new { Success = true });
                    }
                    else if (userEmail != model.Email)
                    {
                        return Json(new { Success = false, ErrorMessage = "Please input your main email which you registered." });
                    }
                }
            }
            return View("Partial_Subscribe", model);
        }
    }
}