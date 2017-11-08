//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using eWellman_financial.Models.Class_Models;
//using eWellman_financial.Models.Helpers;

//namespace eWellman_financial.Controllers
//{
//    public class IncomeController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Income
//        public ActionResult Index()
//        {
//            var incomes = db.Incomes.Include(i => i.budget).Include(i => i.frequency);
//            return View(incomes.ToList());
//        }

//        // GET: Income/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Income income = db.Incomes.Find(id);
//            if (income == null)
//            {
//                return HttpNotFound();
//            }
//            return View(income);
//        }

//        // GET: Income/Create
//        public ActionResult _Create()
//        {
//			return PartialView();
//        }

//        // POST: Income/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "title,date,amount,frequencyId,budgetId")] Income income){
//            if (ModelState.IsValid){
//                db.Incomes.Add(income);
//                db.SaveChanges();
//				return Json(new { success = true, text = "Income Created!", step = 2 }, JsonRequestBehavior.AllowGet);
//			}
//			return Json(new { success = false, text = "Something is missing, try again please." }, JsonRequestBehavior.AllowGet);
//		}

//        // GET: Income/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Income income = db.Incomes.Find(id);
//            if (income == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.budgetId = new SelectList(db.Budgets, "id", "title", income.budgetId);
//            ViewBag.frequencyId = new SelectList(db.Frequencies, "id", "name", income.frequencyId);
//            return View(income);
//        }

//        // POST: Income/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "id,title,date,amount,frequencyId,budgetId")] Income income)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(income).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.budgetId = new SelectList(db.Budgets, "id", "title", income.budgetId);
//            ViewBag.frequencyId = new SelectList(db.Frequencies, "id", "name", income.frequencyId);
//            return View(income);
//        }

//        // GET: Income/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Income income = db.Incomes.Find(id);
//            if (income == null)
//            {
//                return HttpNotFound();
//            }
//            return View(income);
//        }

//        // POST: Income/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Income income = db.Incomes.Find(id);
//            db.Incomes.Remove(income);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//		//GET: Transaction/Json Select Lists
//		public ActionResult JsonSelect() {
//			int houseId = User.Identity.GetHouseId();
//			var budgets = db.Budgets.Where(b => b.householdId == houseId).ToList();

//			return Json(budgets, JsonRequestBehavior.AllowGet);
//		}
//	}
//}
