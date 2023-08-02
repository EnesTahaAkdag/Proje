using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Proje.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-1.9.1.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap.min.js",
                "~/Scripts/custom.js",
                "~/Scripts/bootbox.min.js",
                "~/Scripts/toastr.min.js",
                "~/Scripts/toastr.js"
                ));
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/sitee.css",
                "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                "~/Content/toastr.min.css",
                "~/Content/toastr.css"
                ));
        }
    }
}