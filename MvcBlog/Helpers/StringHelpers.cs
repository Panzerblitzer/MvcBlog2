using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace MvcBlog.Helpers
{
    public static class StringHelpers
    {
            public static string Shorten(this string input, int length)
            {
                //rip out images
                string cleanedInput = Regex.Replace(input, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);

                if (input.Length <= length)
                {
                    return cleanedInput;
                }
                else
                {
                    int firstPara = cleanedInput.IndexOf("</p>");
                    int padding = 4;

                    if (!cleanedInput.StartsWith("<p"))
                    {
                        //does not start with a <p>
                        //find out what it starts with
                        if (cleanedInput.StartsWith("<ol"))
                        {
                            firstPara = cleanedInput.IndexOf("</ol>");
                            padding = 5;

                        }
                        else if (cleanedInput.StartsWith("<div"))
                        {
                            firstPara = cleanedInput.IndexOf("</div>");
                            padding = 6;
                        }
                        else if (cleanedInput.StartsWith("<ul"))
                        {
                            firstPara = cleanedInput.IndexOf("</ul>");
                            padding = 5;
                        }
                    }

                    string result = cleanedInput.Substring(0, firstPara + padding);

                    string returnString = Regex.Replace(result, @"<[^>]*>", "", RegexOptions.IgnoreCase);
                    

                    if (firstPara < length)
                    {
                        return returnString;
                    }
                    else
                    {
                        int newLastIndex = returnString.LastIndexOf(" ");
                        return returnString.Substring(0, newLastIndex) + " ...";
                    }
                
                }
            }

            public static string ToSeoUrl(this string url)
            {
                // make the url lowercase
                string encodedUrl = (url ?? "").ToLower();

                // replace & with and
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

                // remove characters
                encodedUrl = encodedUrl.Replace("'", "");

                // remove invalid characters
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

                // remove duplicates
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

                // trim leading & trailing characters
                encodedUrl = encodedUrl.Trim('-');

                return encodedUrl;
            }
    }
}