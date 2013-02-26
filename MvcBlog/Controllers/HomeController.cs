using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        ArticleEntities blogDb = new ArticleEntities();

        public ActionResult Index()
        {
            //Return 5 newest published blog posts
            var articles = blogDb.Articles.Where(a => a.IsPublished == true && a.IsDeleted == false).OrderByDescending(a => a.DateCreated).Take(5).ToList();
            return View(articles);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
