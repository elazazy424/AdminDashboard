using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Dtos.Auth
{
	public class RegisterDto
	{
		[Required(ErrorMessage ="First name is required")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "UserName is required")]
		public string UserName { get; set; }
		[EmailAddress]
		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }
		public string Password { get; set; }
		//data annotation for password confirmation
		[Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
		public string ConfirmPassword { get; set; }
        public  string? Address { get; set; }
    }
}
