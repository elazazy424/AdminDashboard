using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Dtos.Auth
{
	public class LoginDto
	{
		[Required(ErrorMessage = "UserName is required")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "UserName is required")]
		public string Password { get; set; }
	}
}
