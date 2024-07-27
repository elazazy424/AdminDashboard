using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDashboard.DAL.Entity
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[NotMapped]
		public IList<string> Roles { get; set; }

	}
}
