using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;

namespace MvcBlog.Controllers
{
    public class LeftNavController : Controller
    {
        ArticleEntities blogDb = new ArticleEntities();      
        //
        // GET: /LeftNav/

        public ViewResult NavList()
        {
            var tags = blogDb.Categories.ToList();
            return View(tags);
        }


    }
}
