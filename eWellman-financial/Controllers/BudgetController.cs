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

namespace eWellman_financial.Controllers {
	[InAHouse]
	public class BudgetController : Controller{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Budget
		public ActionResult Index(){
			List<Budget> budgets = new List<Budget>();
			foreach(var budget in db.Budgets) {
				if (budget.householdId == User.Identity.GetHouseId()) {
					//foreach (var inc in budget.incomes) {
					//	budget.totalIncome += inc.amount;
					//}
					//foreach (var exp in budget.expenses) {
					//	budget.totalExpense += exp.amount;
					//}
					//budget.difference = budget.totalIncome - budget.totalExpense;
					budgets.Add(budget);
				}
			}
			return View(budgets);
		}

		// GET: Budget/Details/5
		public ActionResult Details(int? id){
			if (id == null){
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Budget budget = db.Budgets.Find(id);
			//foreach (var inc in budget.incomes) {
			//	budget.totalIncome += inc.amount;
			//}
			//foreach (var exp in budget.expenses) {
			//	budget.totalExpense += exp.amount;
			//}
			//budget.difference = budget.totalIncome - budget.totalExpense;
			if (budget == null){
				return HttpNotFound();
			}
			return View(budget);
		}

		// GET: Budget/Create
		public ActionResult Create(){
			return View();
		}

		// POST: Budget/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "title,durationId")] Budget budget){
			if (ModelState.IsValid){
				budget.householdId = User.Identity.GetHouseId();
				db.Budgets.Add(budget);
				db.SaveChanges();
				return Json(new { success = true, text = "Budget Created!", id = budget.id, title = budget.title, step = 1 }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, text = "Somethings missing, please try again." }, JsonRequestBehavior.AllowGet);
		}

		// GET: Budget/Edit/5
		public ActionResult Edit(int? id){
			if (id == null){
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Budget budget = db.Budgets.Find(id);
			if (budget == null){
				return HttpNotFound();
			}
			return View(budget);
		}

		// POST: Budget/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "id,title,totalIncome,totalExpense,difference,durationId,householdId")] Budget budget){
			if (ModelState.IsValid){
				db.Entry(budget).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.durationId = new SelectList(db.Frequencies, "id", "name", budget.durationId);
			ViewBag.householdId = new SelectList(db.Households, "id", "title", budget.householdId);
			return View(budget);
		}

		// GET: Budget/Delete/5
		public ActionResult Delete(int? id){
			if (id == null){
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Budget budget = db.Budgets.Find(id);
			if (budget == null){
				return HttpNotFound();
			}
			return View(budget);
		}

		// POST: Budget/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id){
			Budget budget = db.Budgets.Find(id);
			db.Budgets.Remove(budget);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing){
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		//GET: Transaction/Json Select Lists
		public ActionResult JsonSelect() {
			int houseId = (int)User.Identity.GetHouseholdId();
			var durations = db.Frequencies.ToList();

			return Json(durations, JsonRequestBehavior.AllowGet);
		}
	}
}