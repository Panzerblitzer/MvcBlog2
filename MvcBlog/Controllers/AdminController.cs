using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using System.Data;
using System.Data.Entity;
using System.Web.Security;
using System.IO;

namespace MvcBlog.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        ArticleEntities blogDb = new ArticleEntities();
        

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            var articles = blogDb.Articles.Where(a => a.IsDeleted == false).OrderByDescending(a => a.DateCreated);
            return View(articles);
        }

        // GET: /Admin/Tags/
        public ActionResult Tags()
        {
            var tags = blogDb.Categories.ToList();
            return View(tags);
        }

        // GET: /Admin/Edit/{articleId}
        public ActionResult Edit(int articleId)
        {
            if (articleId == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Article article = blogDb.Articles.FirstOrDefault(a => a.ArticleId == articleId);
                if (article == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Categories = blogDb.Categories.ToList();
                    return View(article);
                }
            }
        }

        // POST: /Admin/Edit/{articleId}
        [HttpPost]
        public ActionResult Edit(Article article, IEnumerable<int> ArticleTagId)
        {
            if (ModelState.IsValid)
            {
                article.LastEdit = DateTime.Now;
                using (var ctx = blogDb)
                {
                    //Load original article from DB including its Categories
                    var articleinDb = ctx.Articles.Include(a => a.Categories).Single(a => a.ArticleId == article.ArticleId);

                    // Update scalar properties of the article
                    ctx.Entry(articleinDb).CurrentValues.SetValues(article);

                    //Remove categories that are not in the id list anymore
                    foreach (var categoryInDb in articleinDb.Categories.ToList())
                    {
                        if (!ArticleTagId.Contains(categoryInDb.CategoryId))
                            articleinDb.Categories.Remove(categoryInDb);
                    }

                    //Add categories that are not int he DB list but in the id list
                    foreach (var categoryId in ArticleTagId)
                    {
                        if (!articleinDb.Categories.Any(c => c.CategoryId == categoryId))
                        {
                            var category = new Category { CategoryId = categoryId };
                            ctx.Categories.Attach(category); // this avoids duplicate categories
                            articleinDb.Categories.Add(category);
                        }
                    }
                    ctx.SaveChanges();
                }

                //blogDb.Entry(article).State = EntityState.Modified;
                //blogDb.SaveChanges();
                TempData["Message"] = "Article " + article.Title + " is saved.";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Error!";
            return View("article");
        }

        // GET: /Admin/Create
        public ActionResult Create()
        {
            ViewBag.Categories = blogDb.Categories.ToList();
            Article article = new Article();
            article.DateCreated = DateTime.Now;
            article.Author = User.Identity.Name;
            article.LastEdit = DateTime.Now;
            article.IsDeleted = false;
            return View(article);
        }

        // POST: /Admin/Create
        [HttpPost]
        public ActionResult Create(Article article, IEnumerable<int> ArticleTagId)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = blogDb)
                {
                    foreach (var categoryId in ArticleTagId)
                    {
                        var category = new Category { CategoryId = categoryId };
                        ctx.Categories.Attach(category); //avoids duplicate categories
                        article.Categories.Add(category);
                    }
                    ctx.Articles.Add(article);
                    ctx.SaveChanges();
                }
                
                //blogDb.Articles.Add(article);
                //blogDb.SaveChanges();
                TempData["Message"] = article.Title.ToString() + " is saved.";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Error!";
            return View("Edit", article);
        }

        // GET: /Admin/Delete/{id}
        public ActionResult Delete(int articleId)
        {

            if (articleId == 0)
            {
                //no article selected - redirect to index
                return RedirectToAction("Index");
            }
            else
            {
                //process
                var article = blogDb.Articles.Find(articleId);

                if (article != null && article.IsDeleted == false)
                {
                    TempData["Message"] = "Confirm deletion of article";
                    return View(article);
                }
                else
                {
                    //Bad Article ID, Article Deleted or unpublished
                    TempData["Message"] = "Invalid article";
                    return RedirectToAction("Index");
                }
            }
        }

        // POST:
        [HttpPost]
        public ActionResult Delete(Article article)
        {
            if (ModelState.IsValid)
            {
                var articleInDb = blogDb.Articles.Single(a => a.ArticleId == article.ArticleId);

                articleInDb.IsDeleted = true;

                blogDb.SaveChanges();
                TempData["Message"] = "Article was deleted.";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Error!";
            return RedirectToAction("Index");
        }

        //GET: /Admin/EditTag/{categoryId}
        public ActionResult EditTag(int categoryId)
        {
            if (categoryId == 0)
            {
                TempData["Message"] = "Invalid Tag";
                return RedirectToAction("Index");
            }
            else
            {
                Category category = blogDb.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                if (category == null)
                {
                    TempData["Message"] = "Invalid tag";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
        }

        //POST: /Admin/EditTag/
        [HttpPost]
        public ActionResult EditTag(Category category)
        {
            if (ModelState.IsValid)
            {
                Category oldCategory = blogDb.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
                oldCategory.Name = category.Name;
                blogDb.SaveChanges();
                TempData["Message"] = "Tag " + category.Name + " is saved.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Error!";
                return View("EditTag", category);
            }
        }

        //GET: Admin/CreateTag/
        public ActionResult CreateTag()
        {
            Category category = new Category();

            return View(category);
        }

        //POST: Admin/CreateTag/
        [HttpPost]
        public ActionResult CreateTag(Category category)
        {
            if (ModelState.IsValid)
            {
                blogDb.Categories.Add(category);
                blogDb.SaveChanges();
                TempData["Message"] = "Tag " + category.Name + " is saved.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Error!";
                return View("EditTag", category);
            }
        }

        /// <summary>
        /// This method handles the uploaded file in the controller.
        /// This placement of method may not be ideal as it is restricted this controller only.
        /// </summary>
        [HttpPost]
        public void ControllerUploadHandler()
        {
            // Set the response return data type
            this.Response.ContentType = "text/html";

            try
            {
                // First check this header (cross browser support)
                string uploadFileName = this.Request.Headers["X-File-Name"];

                if (string.IsNullOrEmpty(uploadFileName) == false || this.Request.Files.Count > 0)
                {
                    // Get the uploads physical directory on the server
                    string directory = this.Server.MapPath("~/Content/Images");

                    // get just the original filename
                    //string filename = System.IO.Path.GetFileName(this.Request.Files[0].FileName);

                    // create full server path
                    string file = string.Format("{0}\\{1}", directory, uploadFileName);

                    // If file exists already, delete it (optional)
                    if (System.IO.File.Exists(file) == true) System.IO.File.Delete(file);

                    if (string.IsNullOrEmpty(uploadFileName) == true) // IE Browsers
                    {
                        // Save file to server
                        this.Request.Files[0].SaveAs(file);
                    }
                    else // Other Browsers
                    {
                        // Save file to server
                        using (System.IO.FileStream fileStream = new System.IO.FileStream(file, System.IO.FileMode.OpenOrCreate))
                        {
                            this.Request.InputStream.CopyTo(fileStream);
                            fileStream.Close();
                        }
                    }

                    // return the json object as successful
                    this.Response.Write("{ 'success': true }");
                    this.Response.End();
                    return;
                }

                // return the json object as unsuccessful
                this.Response.Write("{ 'success': false }");
            }
            catch (Exception)
            {
                // return the json object as unsuccessful
                this.Response.Write("{ 'success': false }");
            }

            this.Response.End();
        }  
    
    
    
    
    }
}
