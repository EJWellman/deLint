using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class Budget {
		public int id { get; set; }
		public string title { get; set; }
		public decimal goalIncome { get; set; }
		public decimal goalExpense { get; set; }
		public decimal totalIncome {
			get {
				decimal total = new decimal();
				foreach (var i in transactions.Where(t =>  t.type.name == "Credit")) {
					total += i.amount;
				}
				return total;
			}
		}
		public decimal totalExpense {
			get {
				decimal total = new decimal();
				foreach (var i in transactions.Where(t => t.type.name == "Debit")) {
					total += i.amount;
				}
				return total;
			}
		}
		public decimal difference {
			get {
				return totalIncome - totalExpense;
			}
		}
		public int? durationId { get; set; }
		public int householdId { get; set; }
		public Budget() {
			transactions = new HashSet<Transaction>();
		}

		//Virtuals
		public virtual Frequency duration { get; set; }
		public virtual Household household { get; set; }
		public virtual ICollection<Transaction> transactions { get; set; }
	}
}