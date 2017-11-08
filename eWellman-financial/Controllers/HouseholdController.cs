using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eWellman_financial.Models.Class_Models;
using eWellman_financial.Models.Helpers;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eWellman_financial.Controllers{
	[Authorize]
	public class HouseholdController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Household
		//[InAHouse]
		//public ActionResult Index()
		//{
		//	return View(db.Households.ToList());
		//}

		// GET: Household/Details/5
		[InAHouse]
		//[Route("Household/{id}/{title}")]
		public ActionResult Details(int? id){
			if (User.Identity.IsInThisHouse((int)id)) {
				if (id == null) {
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Household household = db.Households.Find(id);
				if (household == null) {
					return HttpNotFound();
				}
				return View(household);
			}
			else {
				return RedirectToAction("Details", new { id = User.Identity.GetHouseId(), title = db.Households.Find(User.Identity.GetHouseId()).title });
			}
		}

		// GET: Household/Create
		[Authorize]
		public ActionResult Create()
		{
			return View();
		}

		// POST: Household/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<ActionResult> Create([Bind(Include = "title,description,housePassword")] Household household){
			if (!User.Identity.IsInAHousehold()) {
				if (ModelState.IsValid) {
					if (household.IsUnique(household.title)) {
						ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
						db.Households.Add(household);
						db.SaveChanges();
						foreach (var h in db.Households) {
							if (h.title == household.title) {
								household.id = h.id;
							}
						}
						await ControllerContext.HttpContext.RefreshCookies(user, household.id);
						return Json(new { success = true, redirectUrl = Url.Action("Details", new { id = household.id }), isRedirect = true, errorMsg = "" });
					}
					else {
						return Json(new { success = false, redirectUrl = "", isRedirect = false, errorMsg = "That house Name is not unique." });
					}
				}
				else {
					return Json(new { success = false, redirectUrl = "", isRedirect = false, errorMsg = "Something's missing, would you kindly check that again?" });
				}
			}
			else {
				return Json(new { success = false, redirectUrl = "", isRedirect = false, errorMsg = "You're already in a household, how did you even get here?" });
			}
		}

		// GET: Household/Edit/5
		[InAHouse]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Household household = db.Households.Find(id);
			if (household == null)
			{
				return HttpNotFound();
			}
			if (household.id != User.Identity.GetHouseId()) {
				return RedirectToAction("Edit", new { id = User.Identity.GetHouseId() });
			}
			return View(household);
		}

		// POST: Household/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[InAHouse]
		public ActionResult Edit([Bind(Include = "id,title,description")] Household household)
		{
			if (ModelState.IsValid)
			{
				db.Entry(household).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(household);
		}

		// GET: Household/Delete/5
		[InAHouse]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Household household = db.Households.Find(id);
			if (household == null)
			{
				return HttpNotFound();
			}
			if (household.id != User.Identity.GetHouseId()) {
				return RedirectToAction("Delete", new { id = User.Identity.GetHouseId() });
			}
			return View(household);
		}

		// POST: Household/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[InAHouse]
		public ActionResult DeleteConfirmed(int id)
		{
			Household household = db.Households.Find(id);
			db.Households.Remove(household);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		//Custom
		public async Task<ActionResult> LeaveHousehold() {
			ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
			int houseId = user.householdId.Value;
			user.householdId = null;
			db.SaveChanges();
			await ControllerContext.HttpContext.RefreshCookies(user, houseId);
			return View();
		}
		//GET: Join House
		public ActionResult Join() {
			return View();
		}
		//POST: Join House
		[HttpPost]
		public async Task<ActionResult> Join([Bind(Include = "title, housePassword")] Household household) {
			ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
			if (string.IsNullOrWhiteSpace(household.title) || string.IsNullOrWhiteSpace(household.housePassword)) {
				goto failed;
			}
			else {
				foreach (var house in db.Households.ToList()) {
					if (house.title == household.title) {
						if(house.housePassword == household.housePassword) {
							user = house.AddToHouse(house.id, user.Id);
							db.SaveChanges();
							await ControllerContext.HttpContext.RefreshCookies(user, house.id);
							goto found;
						}
					}
					else {
						goto failed;
					}
				}
			}
			found:
				db.SaveChanges();
				return RedirectToAction("Details", "Household", new { id = user.householdId, title = household.title });

			failed:
				return View();
		}
		//await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

		//POST: Check for unique household name
		public ActionResult NameCheck(string title) {
			Household house = new Household();
			if (house.IsUnique(title)) {
				return Json(new { success = true, responseText = "That house name is unavailable." }, JsonRequestBehavior.AllowGet);
			}
			else {
					return Json(new { success = false, responseText = "That house name is available!" }, JsonRequestBehavior.AllowGet);
			}
		}

		//POST: House Invite
		public async Task<ActionResult> HouseInvite(string userEmail) {
			if (userEmail != null) {
				Household house = db.Households.Find(User.Identity.GetHouseId());
				await house.InviteToHouse((int)house.id, userEmail);
				return Json(new { success = true, text = "Invite Sent!" }, JsonRequestBehavior.AllowGet);
			}
			else {
				return Json(new { success = false, text = "Invite failed to send, please check the email address." }, JsonRequestBehavior.AllowGet);
			}
		}
		


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
