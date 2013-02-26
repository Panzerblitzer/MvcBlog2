using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcBlog.Models
{
    public class ArticleEntities : DbContext
    {
        protected override bool ShouldValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry)
        {
            if (entityEntry.Entity is Article)
            {
                return false;
            }
            return base.ShouldValidateEntity(entityEntry);
        }
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }


    }
}