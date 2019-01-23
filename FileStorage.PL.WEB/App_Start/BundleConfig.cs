using System.Web;
using System.Web.Optimization;

namespace FileStorage.PL.WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            //js  
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/myJavaScript").Include(
                        "~/Scripts/MyDatePicker.js",
                        "~/Scripts/MyModal.js"));
            //css  
            bundles.Add(new StyleBundle("~/Content/cssjqryUi").Include(
                "~/Content/themes/base/jquery-ui.css"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/themes/base/all.css",
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
