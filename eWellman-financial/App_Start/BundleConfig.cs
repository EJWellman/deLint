using System.Web;
using System.Web.Optimization;

namespace eWellman_financial {
	public class BundleConfig {
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles) {
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-3.2.1.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/jquery.validate*"
			));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"
			));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/umd/popper.js",
				"~/Scripts/umd/popper-utils.js",
				"~/Scripts/respond.js",
				"~/Scripts/bootstrap.js",
				"~/Scripts/sb-admin.js",
				"~/Scripts/jquery.toast.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/templatescripts").Include(
				"~/Scripts/sb-admin-charts.js",
				"~/Scripts/sb-admin-datatables.js"
			));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/font-awesome.css",
				"~/Content/bootstrap.css",
				"~/Content/jquery.toast.css",
				"~/Content/sb-admin.css",
				"~/Content/site.css"
			));
		}
	}
}
