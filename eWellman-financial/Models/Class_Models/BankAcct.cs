using eWellman_financial.Models.View_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class BankAcct {
		public BankAcct() {
			transactions = new HashSet<Transaction>();
		}
		public int id { get; set; }
		[RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]+[0-9]*$", ErrorMessage = "Use A-Z, numbers, spaces, underscores, and hyphens only, please")]
		[MinLength(4, ErrorMessage = "Please enter a house name at least 4 characters long.")]
		public string title { get; set; }
		public decimal balance { get {
				decimal total = new decimal();
				foreach (var i in transactions.Where(t => t.type.name == "Debit" && t.date <= DateTimeOffset.UtcNow && !t.voided)) {
					if (i.reconciled) {
						total -= i.reconciledAmount;
					}
					else {
						total -= i.amount;
					}
				}
				foreach (var i in transactions.Where(t => t.type.name == "Credit" && t.date <= DateTimeOffset.UtcNow && !t.voided)) {
					if (i.reconciled) {
						total += i.reconciledAmount;
					}
					else {
						total += i.amount;
					}
				}
				return total;
			}
		}
		//public decimal balance { get; set; }
		public DateTimeOffset added { get; set; }
		public DateTimeOffset? updated { get; set; }
		public int householdId { get; set; }

		//Virtuals
		public virtual ICollection<Transaction> transactions { get; set; }
		public virtual Household household { get; set; }

		
	}
}