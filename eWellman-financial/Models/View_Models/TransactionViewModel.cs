using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eWellman_financial.Models.View_Models {
	public class TransactionCreateViewModel {
		public IEnumerable<SelectListItem> budget { get; set; }
		public IEnumerable<SelectListItem> bankAccount { get; set; }
		public IEnumerable<SelectListItem> transactionCatagory { get; set; }
		public IEnumerable<SelectListItem> transactionType { get; set; }
		public IEnumerable<SelectListItem> frequency { get; set; }
	}
}