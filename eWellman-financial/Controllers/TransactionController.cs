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
using Microsoft.AspNet.Identity;
using System.IO;

namespace eWellman_financial.Controllers {
	[InAHouse]
	public class TransactionController : Controller{
		private ApplicationDbContext db = new ApplicationDbContext();

		//GET: Transaction
		public ActionResult Index()
		{
			var transactions = db.Transactions.Include(t => t.bankAcct).Include(t => t.catagory).Include(t => t.type);
			return View(transactions.ToList());
		}

		//GET: Transaction/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Transaction transaction = db.Transactions.Find(id);
			if (transaction == null)
			{
				return HttpNotFound();
			}
			return View(transaction);
		}

		//GET: Transaction/Create
		public ActionResult Create()
		{
			return View();
		}

		//POST: Transaction/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(string blah) {
			if (Request.Form.Count >= 8) {
				ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
				Household house = db.Households.Find(user.householdId);
				Transaction newTransaction = new Transaction() {
					description = Request.Form[0],
					amount = decimal.Parse(Request.Form[1]),
					date = DateTimeOffset.Parse(Request.Form[2]),
					frequency = int.Parse(Request.Form[4]),
					bankAcctId = int.Parse(Request.Form[5]),
					budgetId = int.Parse(Request.Form[6]),
					transactionCatagoryId = int.Parse(Request.Form[7]),
					transactionTypeId = int.Parse(Request.Form[8]),
					creatorId = user.Id
				};
				if (Request.Form[3] == "on") {
					newTransaction.recurring = true;
				}
				else if(Request.Form[3] == "off") {
					newTransaction.recurring = false;
				}
				BankAcct acct = db.BankAccts.Find(newTransaction.bankAcctId);
				if (newTransaction.transactionTypeId == 1) {
					//acct.balance -= newTransaction.amount;
				}
				else if (newTransaction.transactionTypeId == 2) {
					//acct.balance += newTransaction.amount;
				}

				if (Request.Files.Count > 0) {
					HttpPostedFileBase file = Request.Files[0];
					var ext = Path.GetExtension(file.FileName).ToLower();
					if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif") {
						try {
							var imgPath = "/content/attachments/" + house.title;
							var dirPath = HttpContext.Server.MapPath("~" + imgPath);
							Directory.CreateDirectory(dirPath);
							newTransaction.receiptType = ext;
							newTransaction.receiptUrl = Path.Combine(imgPath, file.FileName);
							newTransaction.receiptName = file.FileName;
							file.SaveAs(Path.Combine(dirPath, file.FileName));


							db.Transactions.Add(newTransaction);
							db.SaveChanges();
							return Json(new { success = true, text = "Transaction Added!" }, JsonRequestBehavior.AllowGet);
						}
						catch (Exception ex) {
							return Json(new { success = false, rtext = "Transaction failed! Please try again." }, JsonRequestBehavior.AllowGet);
						}
					}
				}

				db.Transactions.Add(newTransaction);
				db.SaveChanges();
				return Json(new { success = true, text = "Transaction Added!" }, JsonRequestBehavior.AllowGet);


				//[Bind(Include = "description,amount,date,bankAcctId,transactionCatagoryId,transactionTypeId")] Transaction transaction, HttpPostedFileBase fileInput
				//if (ModelState.IsValid){
			}
			return Json(new { success = false, text = "Transaction failed! Please try again." }, JsonRequestBehavior.AllowGet);
		}

		//GET: Transaction/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Transaction transaction = db.Transactions.Find(id);
			if (transaction == null)
			{
				return HttpNotFound();
			}
			return View(transaction);
		}

		//POST: Transaction/Edit/5
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "id,description,amount,reconciledAmount,date,reconciledDate,recurring,reconciled,receiptUrl,receiptName,receiptType,creatorId,updaterId,bankAcctId,transactionCatagoryId,transactionTypeId,voided")] Transaction transaction, HttpPostedFileBase file)
		{
			if (transaction.voided) {
				var dbTrans = db.Transactions.Find(transaction.id);
				dbTrans.voided = transaction.voided;
				db.SaveChanges();
				return RedirectToAction("Details", new { id = transaction.id });
			}
			if (ModelState.IsValid) {
				Household house = db.Households.Find(User.Identity.GetHouseholdId());
				if (file != null) {
					var ext = Path.GetExtension(file.FileName).ToLower();
					if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif") {
						var imgPath = "/content/attachments/" + house.title;
						var dirPath = HttpContext.Server.MapPath("~" + imgPath);
						Directory.CreateDirectory(dirPath);
						transaction.receiptType = ext;
						transaction.receiptUrl = Path.Combine(imgPath, file.FileName);
						transaction.receiptName = file.FileName;
						file.SaveAs(Path.Combine(dirPath, file.FileName));


						db.Entry(transaction).State = EntityState.Modified;
						db.SaveChanges();
						return RedirectToAction("Details", new { id = transaction.id });
					}
				}

				db.Entry(transaction).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(transaction);
		}

		//GET: Transaction/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Transaction transaction = db.Transactions.Find(id);
			if (transaction == null)
			{
				return HttpNotFound();
			}
			return View(transaction);
		}

		//POST: Transaction/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Transaction transaction = db.Transactions.Find(id);
			db.Transactions.Remove(transaction);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		//GET: Transaction/Partial
		public ActionResult _TransactionPartial() {

			return PartialView();
		}

		//GET: Transaction/Json Select Lists
		public ActionResult JsonSelect() {
			int houseId = (int)User.Identity.GetHouseholdId();
			var bankAccts = db.BankAccts.Where(b => b.householdId == houseId).ToList();
			var catagories = db.TransactionCatagory.Where(c => c.householdId == houseId || c.householdId == null).ToList();
			var budgets = db.Budgets.Where(b => b.householdId == houseId).ToList();
			var model = new TransactionCreateViewModel();
			model.bankAccount = bankAccts.Select(b => new SelectListItem {
				Value = b.id.ToString(),
				Text = b.title
			});
			model.transactionCatagory = catagories.Select(b => new SelectListItem {
				Value = b.id.ToString(),
				Text = b.name
			});
			model.transactionType = db.TransactionType.Select(b => new SelectListItem {
				Value = b.id.ToString(),
				Text = b.name
			});
			model.frequency = db.Frequencies.Select(f => new SelectListItem {
				Value = f.id.ToString(),
				Text = f.name
			});
			model.budget = budgets.Select(b => new SelectListItem {
				Value = b.id.ToString(),
				Text = b.title
			});

			return Json(model, JsonRequestBehavior.AllowGet);
		}

		public ActionResult OverDraftCheck() {
			decimal numIn = decimal.Parse(Request.Form[0]);
			int bankAcctId = int.Parse(Request.Form[1]);
			BankAcct acct = db.BankAccts.Find(bankAcctId);
			decimal newBal = acct.balance - numIn;
			if (newBal > 0) {
				return Json(new {overdraft = false}, JsonRequestBehavior.AllowGet);
			}
			else{
				return Json(new {overdraft = true}, JsonRequestBehavior.AllowGet);
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
