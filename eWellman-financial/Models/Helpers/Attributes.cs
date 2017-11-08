using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eWellman_financial.Models.Helpers {
	public class InAHouse : AuthorizeAttribute {
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			bool isAuthorized = base.AuthorizeCore(httpContext);
			if (!isAuthorized) {
				return false;
			}

			return httpContext.User.Identity.IsInAHousehold();
		}
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary(
						new {
							controller = "Household",
							action = "Create"
						}));
			}
			else {
				base.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}