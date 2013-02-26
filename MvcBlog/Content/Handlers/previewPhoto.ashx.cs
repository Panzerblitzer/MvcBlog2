using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlog.Content.Handlers
{
    /// <summary>
    /// Summary description for previewPhoto
    /// </summary>
    public class previewPhoto : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.QueryString["filename"];
            string extension = System.IO.Path.GetExtension(filename).Trim('.');

            string directory = context.Server.MapPath("~/Content/Images");
            string file = string.Format("{0}\\{1}", directory, filename);

            context.Response.ContentType = string.Format("image/{0}", extension);
            context.Response.WriteFile(file);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}