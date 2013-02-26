using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcBlog.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<ArticleEntities>
    {
        protected override void Seed(ArticleEntities context)
        {
            var category0 = new Category { Name = "Wargames" };
            var category1 = new Category { Name = "Code" };
            var category2 = new Category { Name = "MVC" };
            var category3 = new Category { Name = "Miscellaneous" };
            var category4 = new Category { Name = "Books" };

            var categories = new List<Category>() { category0, category1, category2, category3 };

            categories.ForEach(c => context.Categories.Add(c));

            var articles = new List<Article>();

            articles.Add(new Article{ ArticleId=1, Title = "My First Post", Author = "Mr. Grinch", DateCreated = DateTime.Parse("01/02/2012 06:30"), LastEdit = DateTime.Parse("01/03/2012 08:30"), IsPublished = true, IsDeleted = false, Content = "<p>My First post in all it's glory.</p>", Categories = new List<Category>() {category1, category2}});
            articles.Add(new Article{ ArticleId=2, Title = "My Second Post", Author = "Mr. Grinch", DateCreated = DateTime.Parse("01/04/2012 04:30"), LastEdit = DateTime.Parse("01/03/2012 12:30"), IsPublished = true, IsDeleted = false, Content = "<p>Another post? How does he do that?!</p>", Categories = new List<Category>() {category2, category3}});
            articles.Add(new Article{ ArticleId=3, Title = "Post the Third", Author = "Mr. Grinch", DateCreated = DateTime.Parse("02/02/2012 14:30"), LastEdit = DateTime.Parse("02/03/2012 08:30"), IsPublished = false, IsDeleted = false, Content = "<p>This post shouldn't show since it is not published</p>", Categories = new List<Category>() {category3}});
            articles.Add(new Article{ ArticleId=4, Title = "And another...", Author = "Mr. Grinch", DateCreated = DateTime.Parse("04/04/2012 06:30"), LastEdit = DateTime.Parse("04/22/2012 08:30"), IsPublished = true, IsDeleted = true, Content = "<p>This one should not show because it was deleted.</p>", Categories = new List<Category>() {category0}});
            articles.Add(new Article { ArticleId = 5, Title = "5 for 5", Author = "Mr. Grinch", DateCreated = DateTime.Parse("06/15/2012 06:30"), LastEdit = DateTime.Parse("06/22/2012 09:30"), IsPublished = true, IsDeleted = false, Content = "<p>I want to see if the HTML is coming through <font color='red'>THIS IS <b>RED</b></font>.</p>", Categories = new List<Category>() { category3 } });
            articles.Add(new Article { ArticleId = 6, Title = "Half a dozen?", Author = "Mr. Grinch", DateCreated = DateTime.Parse("07/4/2012 06:30"), LastEdit = DateTime.Parse("07/5/2012 08:30"), IsPublished = true, IsDeleted = false, Content = "<p>One more article to make sure only 5 appear on the home index page.</p>", Categories = new List<Category>() { category0 } });
            articles.Add(new Article { ArticleId = 7, Title = "And one more?", Author = "Mr. Grinch", DateCreated = DateTime.Parse("07/4/2012 07:30"), LastEdit = DateTime.Parse("07/5/2012 09:30"), IsPublished = true, IsDeleted = false, Content = "<p>The last in a series of posts to get more than five to show</p>", Categories = new List<Category>() { category0 } });
            articles.Add(new Article { ArticleId = 8, Title = "I Lied", Author = "Mr. Grinch", DateCreated = DateTime.Parse("07/4/2012 10:30"), LastEdit = DateTime.Parse("07/5/2012 11:30"), IsPublished = true, IsDeleted = false, Content = "<p>OK...so I neeed one more.....</p>", Categories = new List<Category>() { category0 } });

            articles.ForEach(a => context.Articles.Add(a));


        }
    }
}