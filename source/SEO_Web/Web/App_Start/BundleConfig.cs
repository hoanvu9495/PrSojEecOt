using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Style bundles

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                       "~/Content/themes/base/jquery.ui.all.css",
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

            bundles.Add(new StyleBundle("~/bundles/Custom/css").Include(
                        "~/Content/Custom/css/dropdown.css",
                        "~/Content/Custom/css/jquery.treetable.css",
                        "~/Content/Custom/css/jquery.treetable.theme.default.css",
                        "~/Content/panel.css",
                        "~/Content/Control.css"));

            //Hungnd modify 17/02/2014 - Add treeview css
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                "~/Content/jquery.treeview.css"));

            #endregion

            #region Script bundles

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            //Hungnd Modify 11/03/2014 Add anotation validate
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                         "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Custom config here
            bundles.Add(new ScriptBundle("~/bundles/Custom/js").Include(
                        "~/Content/Custom/js/dropdown.js",
                        "~/Content/Custom/js/form.js",
                        "~/Content/Custom/js/jquery.treetable.js"));

            //Hungnd modify 17/02/2014 - Add treeview js
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                       "~/Scripts/jquery.treeview.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jQueryForm")
                .Include("~/Scripts/jquery.form.js"));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete")
                .Include("~/Scripts/jquery.autocomplete.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/CommonJS")
                .Include("~/Content/Custom/Common.js", "~/Content/Custom/FormulaExcel-{version}.js", "~/Content/Custom/EditableTable-{version}.js"));

            #endregion
        }
    }
}