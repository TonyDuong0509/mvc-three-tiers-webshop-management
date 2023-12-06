using BusinessLogic.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository = new RoleRepository();
        // GET: Admin/Role
        public ActionResult Index()
        {
            List<IdentityRole> items = _roleRepository.GetAll();
            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                _roleRepository.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}