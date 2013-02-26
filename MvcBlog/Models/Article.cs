using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcBlog.Models
{
    public class Article
    {
        [HiddenInput(DisplayValue=false)]
        public int ArticleId { get; set; }
        
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = true)]
        public string Author { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name="Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        
        [HiddenInput(DisplayValue=true)]
        [Display(Name="Last Edited")]
        [DataType(DataType.DateTime)]
        public DateTime LastEdit { get; set; }
 
        [Display(Name="Publish")]
        public bool IsPublished { get; set; }

        [HiddenInput(DisplayValue=false)]
        public bool IsDeleted { get; set; }

        [AllowHtml]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public Article()
        {
            Categories = new HashSet<Category>();
        }

    }
}