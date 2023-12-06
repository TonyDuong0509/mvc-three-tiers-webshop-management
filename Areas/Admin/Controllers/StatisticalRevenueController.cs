using BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class StatisticalRevenueController : Controller
    {
        private IStatisticalRevenueService _statisticalRevenueService = new StatisticalRevenueService();

        public ActionResult Index()
        {
            var item = _statisticalRevenueService.CalculateMonthlyRevenues();
            return View(item);
        }

        public ActionResult PartialMonthlyRevenue()
        {
            var item = _statisticalRevenueService.CalculateMonthlyRevenues();
            return PartialView("_PartialMonthlyRevenue", item);
        }
    }
}