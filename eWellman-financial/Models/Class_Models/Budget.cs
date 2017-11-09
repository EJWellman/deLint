using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class Budget {
		public Budget() {
			transactions = new HashSet<Transaction>();
		}
		public int id { get; set; }
		[Display(Name = "Budget Name")]
		public string title { get; set; }
		[Display(Name = "Goal Expenses")]
		public decimal goalExpense { get; set; }
		[Display(Name = "Actual Expenses")]
		public decimal actualExpense {
			get {
				decimal total = new decimal();
				foreach (var i in transactions.Where(t => t.type.name == "Debit" && !t.voided)) {
					total += i.amount;
				}
				return total;
			}
		}
		[Display(Name = "Budget Duration")]
		public int? durationId { get; set; }
		public int householdId { get; set; }

		//Virtuals
		public virtual Frequency duration { get; set; }
		public virtual Household household { get; set; }
		public virtual ICollection<Transaction> transactions { get; set; }
	}
}