using eWellman_financial.Models.Class_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eWellman_financial.Models.View_Models {
	public class FrequencyListViewModel {
		public IEnumerable<SelectListItem> frequencies { get; set; }
	}
}