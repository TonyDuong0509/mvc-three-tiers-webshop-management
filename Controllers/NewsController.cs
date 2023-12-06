using BusinessLogic.Repositories;
using DataAccess.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository = new NewsRepository();
        // GET: News
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> news = _newsRepository.GetAllUser();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            news = news.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(news);
        }

        public ActionResult Partial_News_Home()
        {
            List<News> news = _newsRepository.GetNewsUser();
            return PartialView(news);
        }

        public ActionResult NewsDetail(int id)
        {
            News mews = _newsRepository.GetById(id);
            return View(mews);
        }
    }
}