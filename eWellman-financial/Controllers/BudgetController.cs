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
using eWellman_financial.Models.View_Models;

namespace eWellman_financial.Controllers{
    public class BudgetController : Controller{
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budget
        public ActionResult Index(){
			int houseId = User.Identity.GetHouseId();
			var budgets = db.Budgets.Where(b => b.householdId == houseId).ToList();
            return View(budgets);
        }

        // GET: Budget/Details/5
        public ActionResult Details(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
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
        public ActionResult Create([Bind(Include = "title,goalIncome,goalExpense,durationId")] Budget budget){
            if (ModelState.IsValid){
				budget.householdId = User.Identity.GetHouseId();
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budget);
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
        public ActionResult Edit([Bind(Include = "id,title,goalIncome,goalExpense,durationId,householdId")] Budget budget){
            if (ModelState.IsValid){
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budget);
        }

        // GET: Budget/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
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

		public ActionResult JsonSelect() {
			var model = new FrequencyListViewModel();
			model.frequencies = db.Frequencies.Select(f => new SelectListItem {
				Value = f.id.ToString(),
				Text = f.name
			});

			return Json(model, JsonRequestBehavior.AllowGet);
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
