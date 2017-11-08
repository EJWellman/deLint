using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class TransactionCatagory {
		public int id { get; set; }
		public int? householdId { get; set; }
		public string name { get; set; }

		//Virtuals
		public virtual Household household { get; set; }
	}
}