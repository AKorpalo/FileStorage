using System.Web.Mvc;
using System.Web.Routing;

namespace FileStorage.PL.WEB.Helpers
{
    public static class LangHelper
    {
        public static MvcHtmlString LangSwitcher(this UrlHelper url, string name, RouteData routeData, string lang)
        {
            var optionTagBuilder = new TagBuilder("option");
            var routeValueDictionary = new RouteValueDictionary(routeData.Values);
            if (routeValueDictionary.ContainsKey("lang"))
            {
                if (routeData.Values["lang"] as string == lang)
                {
                    optionTagBuilder.MergeAttribute("selected", "selected");
                }
                else
                {
                    routeValueDictionary["lang"] = lang;
                }
            }
            optionTagBuilder.MergeAttribute("value", url.RouteUrl(routeValueDictionary));
            optionTagBuilder.SetInnerText(name);
            return new MvcHtmlString(optionTagBuilder.ToString());
        }
    }
}