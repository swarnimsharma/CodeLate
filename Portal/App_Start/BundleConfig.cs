using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Portal.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Areas/Vendor/Scripts/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "https://code.jquery.com/ui/1.12.1/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/Scripts/SaleInvoice").Include(
                      "~/Scripts/SaleInvoice/AddSaleInvoice.js"));
            BundleTable.EnableOptimizations = true;
        }
    }
}