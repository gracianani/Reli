using System.Web;
using System.Web.Optimization;

namespace ReliDemo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.9.1.js", 
                        "~/Scripts/jquery-migrate-1.0.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.10.3.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap*"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/simpliq/css/*.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                        "~/Scripts/flot/jquery.flot.*", 
                        "~/Scripts/flot/excanvas.js",
                        "~/Scripts/flot.jquery.colorhelpers.js"));

            bundles.Add(new ScriptBundle("~/bundles/simpliq" ).Include(
                        "~/Scripts/simpliq/bootstrap-datepicker.js",
                        "~/Scripts/simpliq/moment.js",
                        "~/Scripts/simpliq/daterangepicker.js",
                        "~/Scripts/simpliq/bootstrap.js",
                        "~/Scripts/simpliq/jquery.flot.js",
                        "~/Scripts/simpliq/jquery.*",
                        "~/Scripts/simpliq/fullcalendar.js",
                        "~/Scripts/simpliq/excanvas.js",
                        "~/Scripts/simpliq/gauge.js",
                        "~/Scripts/simpliq/counter.js",
                        "~/Scripts/simpliq/raphael.{version}.js",
                        "~/Scripts/simpliq/justgage.{version}.js",
                        "~/Scripts/simpliq/retina.js",
                        "~/Scripts/simpliq/wizard.js",
                        "~/Scripts/simpliq/core.js",
                        "~/Scripts/simpliq/charts.js",
                        "~/Scripts/simpliq/custom.js",
                        "~/Scripts/simpliq/dashboard.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}