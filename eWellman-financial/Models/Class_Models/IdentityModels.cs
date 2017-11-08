using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace eWellman_financial.Models.Class_Models{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser {
		[Display(Name = "First Name")]
		public string firstName { get; set; }
		[Display(Name = "Last Name")]
		public string lastName { get; set; }
		[Display(Name = "Name")]
		public string fullName {
			get {
				return firstName + " " + lastName;
			}
		}
		[Display(Name = "Time Zone")]
		public string timeZone { get; set; }
		public int? householdId { get; set; }

		public ApplicationUser() {
			transactions = new HashSet<Transaction>();
		}
		//Virtuals
		public virtual ICollection<Transaction> transactions { get; set; }
		public virtual Household household { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager){
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			userIdentity.AddClaim(new Claim("HouseholdId", householdId.ToString()));
			userIdentity.AddClaim(new Claim("Name", fullName));
			userIdentity.AddClaim(new Claim("Time Zone", timeZone));
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>{
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false){
		}

		public static ApplicationDbContext Create(){
			return new ApplicationDbContext();
		}

		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<Household> Households { get; set; }
		public DbSet<BankAcct> BankAccts { get; set; }
		public DbSet<TransactionCatagory> TransactionCatagory { get; set; }
		public DbSet<TransactionType> TransactionType { get; set; }
		public DbSet<Budget> Budgets { get; set; }
		public DbSet<Frequency> Frequencies { get; set; }
		//public System.Data.Entity.DbSet<eWellman_financial.Models.Class_Models.Income> Incomes { get; set; }
		//public System.Data.Entity.DbSet<eWellman_financial.Models.Class_Models.Expense> Expenses { get; set; }
	}
}