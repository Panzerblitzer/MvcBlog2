using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcBlog
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ViewArticle",
                "Blog/Article/{id}/{seotext}",
                new { controller = "Blog", action = "Article", id = @"\d+", seotext = UrlParameter.Optional}
            );

            routes.MapRoute(
                "EditTags",
                "Admin/EditTag/{id}",
                new { controller = "Admin", action = "EditTag", categoryId = 0 }
            );

            routes.MapRoute(
                "Tagged",
                "Blog/Tags/{id}",
                new { controller = "Blog", action = "Tags", id = 0 }
            );

            routes.MapRoute(
                "Delete",
                "Admin/Delete/{articleId}",
                new { controller = "Admin", action = "Delete", articleId = 0 }
            );

            routes.MapRoute(
                "Edit",
                "Admin/Edit/{articleId}",
                new { controller = "Admin", action = "Edit", articleId = 0 }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new MvcBlog.Models.SampleData());
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}