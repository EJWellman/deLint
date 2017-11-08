using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class Notification {
		public int id { get; set; }
		public string title { get; set; }
		public DateTimeOffset date { get; set; }
		public string body { get; set; }
		public DateTimeOffset notificationDate { get; set; }
		public int? userId { get; set; }
		public int? householdId { get; set; }

		//Virtuals
		public virtual Household household { get; set; }
		public virtual ApplicationUser user { get; set; }
	}
}