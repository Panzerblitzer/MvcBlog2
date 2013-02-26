using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcBlog.Models
{
    public class Category
    {
        
        [HiddenInput(DisplayValue = false)]
        public int CategoryId { get; set; }

        [Display(Name="Tag Name")]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public Category()
        {
            Articles = new HashSet<Article>();
        }
    }
}