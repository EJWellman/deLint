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

namespace eWellman_financial.Controllers
{
	[InAHouse]
    public class BankAcctsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BankAccts
        public ActionResult Index()
        {
            var bankAccts = db.BankAccts.Include(b => b.household);
            return View(bankAccts.ToList());
        }

        // GET: BankAccts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAcct bankAcct = db.BankAccts.Find(id);
            if (bankAcct == null)
            {
                return HttpNotFound();
            }
            return View(bankAcct);
        }

        // GET: BankAccts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankAccts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string title, decimal balance) {
			//[Bind(Include = "title,balance")] BankAcct bankAcct){
			if (title != "" || title != null) {
				BankAcct bankAcct = new BankAcct() {
					title = title,
					householdId = User.Identity.GetHouseId(),
					added = DateTimeOffset.UtcNow
				};
				db.BankAccts.Add(bankAcct);
				db.SaveChanges();
				Transaction transaction = new Transaction() {
					description = "Account Seed",
					amount = balance,
					bankAcctId = bankAcct.id,
					creatorId = User.Identity.GetUserId(),
					date = DateTimeOffset.UtcNow,
					transactionTypeId = 2
				};
				db.Transactions.Add(transaction);
				db.SaveChanges();
				return RedirectToAction("Details", new { id = bankAcct.id });
			}
		return View();



			//if (ModelState.IsValid){
				//bankAcct.added = DateTimeOffset.UtcNow;
				//bankAcct.householdId = (int)User.Identity.GetHouseholdId();
				//db.BankAccts.Add(bankAcct);
				//db.SaveChanges();
				//return RedirectToAction("Index");
			//}
            //return View(bankAcct);
        }

        // GET: BankAccts/Edit/5
        public ActionResult Edit(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAcct bankAcct = db.BankAccts.Find(id);
            if (bankAcct == null){
                return HttpNotFound();
            }
            return View(bankAcct);
        }

        // POST: BankAccts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,added,updated,householdId")] BankAcct updatedBankAcct){
            if (ModelState.IsValid){
				updatedBankAcct.updated = DateTimeOffset.UtcNow;
                db.Entry(updatedBankAcct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updatedBankAcct);
        }

        // GET: BankAccts/Delete/5
        public ActionResult Delete(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAcct bankAcct = db.BankAccts.Find(id);
            if (bankAcct == null){
                return HttpNotFound();
            }
            return View(bankAcct);
        }

        // POST: BankAccts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAcct bankAcct = db.BankAccts.Find(id);
            db.BankAccts.Remove(bankAcct);
            db.SaveChanges();
            return RedirectToAction("Index");
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
