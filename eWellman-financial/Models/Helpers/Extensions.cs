using eWellman_financial.Models.Class_Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eWellman_financial.Models.Helpers {
	public static class Extensions {
		public static int? GetHouseholdId(this IIdentity user) {
			var claimsId = (ClaimsIdentity)user;
			var householdClaim = claimsId.Claims.FirstOrDefault(c => c.Type == "HouseholdId");

			if (householdClaim != null) {
				return Int32.Parse(householdClaim.Value);
			}
			else {
				return null;
			}
		}
		public static string GetFullName(this IPrincipal user) {
			var fullNameClaim = ((ClaimsIdentity)user.Identity).FindFirst("Name");
			
			if (fullNameClaim != null) {
				return fullNameClaim.Value;
			}
			else {
				return "";
			}
		}
		public static string GetUserTimeZone(this IIdentity user) {
			var claimsId = (ClaimsIdentity)user;
			var TimeZoneClaim = claimsId.Claims.FirstOrDefault(c => c.Type == "Time Zone");

			if (TimeZoneClaim != null) {
				return TimeZoneClaim.Value;
			}
			else {
				return null;
			}
		}

		public static bool IsInAHousehold(this IIdentity user) {
			var cUser = (ClaimsIdentity)user;
			var householdId = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
			return (householdId != null && !String.IsNullOrWhiteSpace(householdId.Value));
		}
		public static bool IsInThisHouse(this IIdentity user, int houseId) {
			var cUser = (ClaimsIdentity)user;
			var cHouse = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
			return (cHouse != null && !String.IsNullOrWhiteSpace(cHouse.Value) && cHouse.Value == houseId.ToString());
		}
		public static int GetHouseId(this IIdentity user) {
			var cUser = (ClaimsIdentity)user;
			var cHouseId = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
			return (int.Parse(cHouseId.Value));
		}
	}
	public static class RefreshAuthentication {
		public static async Task RefreshCookies(this HttpContextBase context, ApplicationUser user, int houseId) {
			var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
			identity.RemoveClaim(identity.FindFirst("HouseholdId"));
			identity.AddClaim(new Claim("HouseholdId", user.householdId.ToString()));
			context.GetOwinContext().Authentication.SignOut();
			await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent: false, rememberBrowser: false);
		}
	}
}

namespace HtmlHelpers {
	public static class HTMLHelpers {
		public static IHtmlString ToUserTime(this HtmlHelper helper, DateTimeOffset servTime, string timeZone) {
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
			DateTimeOffset userTime = TimeZoneInfo.ConvertTime(servTime, timeZoneInfo);
			string htmlString = userTime.ToString();

			return new HtmlString(htmlString);
		}

		public static IHtmlString ToUserTime(this HtmlHelper helper, DateTimeOffset servTime, string timeZone, string toStringFormat) {
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
			DateTimeOffset userTime = TimeZoneInfo.ConvertTime(servTime, timeZoneInfo);
			string htmlString = userTime.ToString(toStringFormat);

			return new HtmlString(htmlString);
		}
	}
}