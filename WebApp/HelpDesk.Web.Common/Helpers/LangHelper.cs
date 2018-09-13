using System.Web.Mvc;
using System.Web.Routing;

namespace HelpDesk.Web.Common.Helpers
{
    public static class LangHelper
    {
        public static MvcHtmlString LangSwitcher(this UrlHelper url, string Name, RouteData routeData, string lang)
        {
            var liTagBuilder = new TagBuilder("li");
            var aTagBuilder = new TagBuilder("a");
            var routeValueDictionary = new RouteValueDictionary(routeData.Values);
            if (routeValueDictionary.ContainsKey("lang"))
            {
                if (routeData.Values["lang"] as string == lang)
                {
                    liTagBuilder.AddCssClass("active");
                }
                else
                {
                    routeValueDictionary["lang"] = lang;
                }
            }
            string href = url.RouteUrl(routeValueDictionary);
            if (href.Contains("AngularTemplate"))
            {
                href = href.Replace("Header", "Index");
            }

            aTagBuilder.MergeAttribute("href", href);
            aTagBuilder.SetInnerText(Name);
            liTagBuilder.InnerHtml = aTagBuilder.ToString();
            return new MvcHtmlString(liTagBuilder.ToString());
        }
    }
}