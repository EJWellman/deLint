using eWellman_financial.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eWellman_financial.Controllers {
	[Authorize]
	public class HomeController : Controller {
		public ActionResult Index() {
			if (User.Identity.IsInAHousehold()) {
				return RedirectToAction("Details", "Household", new { id = User.Identity.GetHouseId() });
			}
			return View();
		}

		public ActionResult About() {
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}