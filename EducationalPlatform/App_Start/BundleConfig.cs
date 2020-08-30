using System.Web;
using System.Web.Optimization;

namespace EducationalPlatform
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery-3.3.1.min.js",
                      "~/Scripts/js/main.js",
                      "~/Scripts/js/vendor/jquery-2.2.4.min.js",
                      "~/Scripts/js/vendor/bootstrap.min.js",
                      "~/Scripts/js/easing.min.js",
                      "~/Scripts/js/hoverIntent.js",
                      "~/Scripts/js/superfish.min.js",
                      "~/Scripts/js/jquery.ajaxchimp.min.js",
                      "~/Scripts/js/jquery.magnific-popup.min.js",
                      "~/Scripts/js/owl.carousel.min.js",
                      "~/Scripts/js/owl-carousel-thumb.min.js",
                      "~/Scripts/js/jquery.sticky.js",
                      "~/Scripts/js/jquery.nice-select.min.js",
                      "~/Scripts/js/parallax.min.js",
                      "~/Scripts/js/waypoints.min.js",
                      "~/Scripts/js/wow.min.js",
                      "~/Scripts/js/jquery.counterup.min.js",
                      "~/Scripts/js/mail-script.js",
                      "~/Scripts/js/jquery.tagsinput.js",
                      "~/Scripts/js/jquery.plugin.js",
                      "~/Scripts/js/jquery.countdown.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/css/bootstrap.css",
                     "~/Content/main.css",
                     "~/Content/Site.css",
                     "~/css/linearicons.css",
                     "~/css/font-awesome.min.css",
                     "~/css/magnific-popup.css",
                     "~/css/nice-select.css",
                     "~/css/animate.min.css",
                     "~/css/owl.carousel.css",
                     "~/css/jquery.tagsinput.css",
                     "~/Content/assets/css/fontawsom-all.min.css",
                     "~/css/jquery.countdown.css"
                     ));
        }
    }
}
