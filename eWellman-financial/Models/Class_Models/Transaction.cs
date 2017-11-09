using eWellman_financial.Models.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class Transaction {
		public int id { get; set; }
		public string description { get; set; }
		public decimal amount { get; set; }
		public decimal reconciledAmount { get; set; }
		public DateTimeOffset date { get; set; }
		public DateTimeOffset? reconciledDate { get; set; }
		public bool reconciled { get; set; }
		public bool recurring { get; set; }
		public bool voided { get; set; }
		public int? frequency { get; set; }
		public string receiptUrl { get; set; }
		public string receiptName { get; set; }
		public string receiptType { get; set; }
		public string creatorId { get; set; }
		public string updaterId { get; set; }
		public int? budgetId { get; set; }
		public int bankAcctId { get; set; }
		public int transactionTypeId { get; set; }
		//public int? transactionCatagoryId { get; set; }

		//Virtuals
		public virtual Budget budget { get; set; }
		public virtual ApplicationUser creator { get; set; }
		public virtual ApplicationUser updater { get; set; }
		public virtual BankAcct bankAcct { get; set; }
		public virtual TransactionType type { get; set; }
		//public virtual TransactionCatagory catagory {get;set;}
	}
}