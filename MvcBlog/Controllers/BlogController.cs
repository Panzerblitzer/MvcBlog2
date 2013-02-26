using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using MvcBlog.Helpers;

namespace MvcBlog.Controllers
{
    public class BlogController : Controller
    {
        ArticleEntities blogDb = new ArticleEntities();

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            //All articles.
            var articles = blogDb.Articles.Where(a => a.IsPublished == true && a.IsDeleted == false).OrderByDescending(a => a.DateCreated).ToList();
            return View(articles);
        }

        public ActionResult Tags(int id)
        {
            if (id == 0)
            {
                //No Tag selected - redirect to index
                return RedirectToAction("Index");
            }
            else
            {
                //Tag Selected
                var tag = blogDb.Categories.Find(id);

                if (tag != null)
                {
                    ViewBag.Name = tag.Name;

                    var articleDB = blogDb.Articles.ToList();
                    List<Article> articles = new List<Article>();

                    foreach (Article article in articleDB)
                    {
                        var categories = article.Categories.ToList();

                        foreach (Category category in categories)
                        {
                            if (category.CategoryId == tag.CategoryId && article.IsDeleted == false && article.IsPublished == true)
                            {
                                //add the article
                                articles.Add(article);
                            }
                        }
                    }

                    var sortedArticles = articles.OrderByDescending(a => a.DateCreated);
                    return View(sortedArticles);
                }
                else
                {
                    //Invalid tag - redirect to index
                    TempData["error"] = "Invalid Tag";
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Article(int id, string seotext)
        {
            if (id == 0)
            {
                //no article selected - redirect to index
                return RedirectToAction("Index");
            }
            else
            {
                //process
                var article = blogDb.Articles.Single(a => a.ArticleId == id);

                if (article != null && article.IsDeleted == false && article.IsPublished == true)
                {
                    //make sure the articleTitle for the route matches the encoded seotext
                    string expectedTitle = article.Title.ToSeoUrl();
                    string actualTitle = (seotext ?? "").ToLower();

                    //permanently redirect to the correct URL
                    if (expectedTitle != actualTitle)
                    {
                        return RedirectToActionPermanent("Article", "Blog", new { id = article.ArticleId, seotext = expectedTitle });
                    }

                    return View(article);
                }
                else
                {
                    //Bad Article ID, Article Deleted or unpublished
                    TempData["error"] = "Invalid Article";
                    return RedirectToAction("Index");
                }
            }


        }

    }
}
