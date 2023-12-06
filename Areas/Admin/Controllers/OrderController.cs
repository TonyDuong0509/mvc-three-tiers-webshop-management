using BusinessLogic.Repositories;
using DataAccess.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            List<Order> orders = _orderRepository.GetAll();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 5;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult View(int id)
        {
            Order order = _orderRepository.GetById(id);
            return View(order);
        }

        public ActionResult Partial_Product(int id)
        {
            if (ModelState.IsValid)
            {
                List<OrderDetail> orderDetails = _orderRepository.GetDetailById(id);
                return PartialView(orderDetails);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            if (ModelState.IsValid)
            {
                var checkResult = _orderRepository.UpdateStatus(id, status);
                if (checkResult != null)
                {
                    return Json(new { message = "Success", success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { message = "Fail", success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}