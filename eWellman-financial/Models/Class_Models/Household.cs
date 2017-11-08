using eWellman_financial.Models.View_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace eWellman_financial.Models.Class_Models {
	public class Household {
		public Household() {
			members = new HashSet<ApplicationUser>();
			accounts = new HashSet<BankAcct>();
			budgets = new HashSet<Budget>();
		}
		public int id { get; set; }
		[RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use A-Z, spaces, underscores, and hyphens only, please")]
		[MinLength(6, ErrorMessage = "Please enter a house name at least 6 characters long.")]
		public string title { get; set; }
		public string description { get; set; }
		[Required(AllowEmptyStrings = false)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		[DataType(DataType.Password)]
		public string housePassword { get; set; }

		//Virtuals
		public virtual ICollection<ApplicationUser> members { get; set; }
		public virtual ICollection<BankAcct> accounts { get; set; }
		public virtual ICollection<Budget> budgets { get; set; }

		//Methods
		public bool IsUnique (string houseName) {
			ApplicationDbContext db = new ApplicationDbContext();
			foreach (Household house in db.Households.ToList()) {
				if (house.title == houseName) {
					return false;
				}
			}
			return true;
		}

		public ApplicationUser AddToHouse(int houseId, string userId) {
			ApplicationDbContext db = new ApplicationDbContext();
			ApplicationUser user = db.Users.Find(userId);
			user.householdId = houseId;
			db.SaveChanges();
			return (user);
		}
		public void RemoveFromHouse (string userEmail) {
			ApplicationDbContext db = new ApplicationDbContext();
			db.Users.Where(u => u.Email == userEmail).FirstOrDefault().householdId = null;
			db.SaveChanges();
		}

		public async Task InviteToHouse(int houseId, string userEmail) {
			ApplicationDbContext db = new ApplicationDbContext();
			var dbHouse = db.Households.Find(houseId);

			try {
				string body = "<p>You've been invited to join a household in DeLint!<br/>" +
					"DeLint is a budgeting application for your entire household,<br/>" +
					"you can have as many people as you have in your house, in your DeLint household.<br/>" +
					"Click {0} if you have an account already.<br/>" +
					"Click {1} if you need to make an account.<br/>" +
					"In order to join, you'll need the house name, and password.<br/>" +
					"House Name: {2}<br/>" +
					"Password: {3}<br/>" +
					"Note: You'll need to type the house name, and password EXACTLY as they appear here.</p><br/><br/>" +
					"<p>This message is automated, please do not respond to it.</p>";
				string from = "Notification Wizard <eWellmanEServ@Gmail.com>";
				var host = HttpContext.Current.Request.Url.Host;
				string loginLink = "<a href=\"" + host + "/Account/Login\">Here </a>";
				string registerLink = "<a href=\"" + host + "/Account/Register\">Here </a>";
				MailMessage email = new MailMessage(from, userEmail) {
					Subject = "Household Invitation - DeLint",
					Body = string.Format(body, loginLink, registerLink, dbHouse.title, dbHouse.housePassword),
					IsBodyHtml = true
				};
				var svc = new PersonalEmail();
				await svc.SendAsync(email);
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
				await Task.FromResult(0);
			}
		}
	}
}