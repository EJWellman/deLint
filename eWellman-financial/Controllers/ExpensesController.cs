//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using eWellman_financial.Models.Class_Models;

//namespace eWellman_financial.Controllers
//{
//    public class ExpensesController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Expenses
//        public ActionResult Index()
//        {
//            var expenses = db.Expenses.Include(e => e.budget).Include(e => e.frequency);
//            return View(expenses.ToList());
//        }

//        // GET: Expenses/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Expense expense = db.Expenses.Find(id);
//            if (expense == null)
//            {
//                return HttpNotFound();
//            }
//            return View(expense);
//        }

//        // GET: Expenses/Create
//        public ActionResult _Create(){
//			return PartialView();
//		}

//        // POST: Expenses/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "id,title,date,amount,frequencyId")] Expense expense, string budgetId2){
//            if (ModelState.IsValid){
//				expense.budgetId = int.Parse(budgetId2);
//                db.Expenses.Add(expense);
//                db.SaveChanges();
//				return Json(new { success = true, text = "Expense Created!", step = 3, redirectUrl = Url.Action("Details", "Budget", new { id = expense.budgetId }) }, JsonRequestBehavior.AllowGet);
//			}
//			return Json(new { success = false, text = "Something is missing, try again please." }, JsonRequestBehavior.AllowGet);
//		}

//        // GET: Expenses/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Expense expense = db.Expenses.Find(id);
//            if (expense == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.budgetId = new SelectList(db.Budgets, "id", "title", expense.budgetId);
//            ViewBag.frequencyId = new SelectList(db.Frequencies, "id", "name", expense.frequencyId);
//            return View(expense);
//        }

//        // POST: Expenses/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "id,title,date,amount,frequencyId,budgetId")] Expense expense)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(expense).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.budgetId = new SelectList(db.Budgets, "id", "title", expense.budgetId);
//            ViewBag.frequencyId = new SelectList(db.Frequencies, "id", "name", expense.frequencyId);
//            return View(expense);
//        }

//        // GET: Expenses/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Expense expense = db.Expenses.Find(id);
//            if (expense == null)
//            {
//                return HttpNotFound();
//            }
//            return View(expense);
//        }

//        // POST: Expenses/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Expense expense = db.Expenses.Find(id);
//            db.Expenses.Remove(expense);
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
//    }
//}
