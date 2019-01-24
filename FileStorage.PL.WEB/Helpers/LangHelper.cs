using System.Web.Mvc;
using System.Web.Routing;

namespace FileStorage.PL.WEB.Helpers
{
    public static class LangHelper
    {
        public static MvcHtmlString LangSwitcher(this UrlHelper url, string name, RouteData routeData, string lang)
        {
            var optionTagBuilder = new TagBuilder("option");
            var aTagBuilder = new TagBuilder("span");
            var routeValueDictionary = new RouteValueDictionary(routeData.Values);
            if (routeValueDictionary.ContainsKey("lang"))
            {
                if (routeData.Values["lang"] as string == lang)
                {
                    //liTagBuilder.AddCssClass("active");
                }
                else
                {
                    routeValueDictionary["lang"] = lang;
                }
            }
            aTagBuilder.AddCssClass($"flag-icon flag-icon-us");
            optionTagBuilder.MergeAttribute("value", url.RouteUrl(routeValueDictionary));
            optionTagBuilder.MergeAttribute("data-content", "<span class=\"flag-icon flag-icon-us\" ></span>");
            optionTagBuilder.SetInnerText(name);
            return new MvcHtmlString(optionTagBuilder.ToString());
        }
    }
}