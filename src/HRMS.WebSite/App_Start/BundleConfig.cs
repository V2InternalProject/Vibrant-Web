using System.Web.Optimization;

namespace HRMS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryForm").Include("~/Scripts/jquery.form.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.validate*"));

            bundles.Add(
                new ScriptBundle("~/bundles/jqueryValidation").Include(
                    "~/Scripts/jquery.validate.js"
                // "~/Scripts/jquery.validate.unobtrusive.js",
                //"~/Scripts/jquery.unobtrusive-ajax.js"
                    ));

            bundles.Add(
                new ScriptBundle("~/bundles/tabjquery").Include(
                    "~/Scripts/jquery-1.8.3.js",
                    "~/Scripts/jquery-ui-1.8.23.js",
                    "~/Scripts/jquery.ui.core.js",
                    "~/Scripts/jquery.ui.widget.js",
                    "~/Scripts/jquery.ui.tabs.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/GridViewJquery").Include(
                    "~/Scripts/GridView/grid.locale-en.js", "~/Scripts/GridView/jquery.jqGrid.src.js"));

            bundles.Add(new ScriptBundle("~/bundle/CommonFile").Include("~/Scripts/Common.js", "~/Scripts/Global.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(
                new StyleBundle("~/Content/themes/base/css").Include(
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

            bundles.Add(
                new StyleBundle("~/Content/themes/base/DrpCheklist").Include(
                    "~/Content/themes/base/ui.dropdownchecklist.standalone.css",
                    "~/Content/themes/base/ui.dropdownchecklist.themeroller.css"));

            bundles.Add(
                new StyleBundle("~/Content/themes/base/tabcss").Include(
                    "~/Content/themes/base/jquery.ui.all.css",
                    "~/Content/TabHeader.css",
                    "~/Content/themes/GridView/ui.jqgrid.css",
                    "~/Content/PersonalProfile.css"));

            bundles.Add(
                new StyleBundle("~/Content/themes/base/sitecss").Include(
                    "~/Content/Default.css"
                // ,"~/Content/themes/Menu/style-menu.css"
                    ));

            bundles.Add(new ScriptBundle("~/Appriasal/js").Include(

                        "~/Scripts/Appraisal/global.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/Appraisal/app.js",
                        "~/Scripts/Appraisal/Factories/*.js",
                        "~/Scripts/Appraisal/Values/*.js",
                        "~/Scripts/Appraisal/Directives/*.js",
                        "~/Scripts/Appraisal/Filters/*.js",
                        "~/Scripts/Appraisal/Controllers/*.js"
                        ));
        }
    }
}