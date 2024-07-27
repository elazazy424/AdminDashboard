using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Dtos.Auth
{
	public class UpdateRoleDto
	{
		[Required(ErrorMessage ="UserName is required")]
		public string UserName { get; set; }
		[Required(ErrorMessage ="Role is required")]
		public RoleType Role { get; set; }
	}

	public enum RoleType
	{
		Admin,
		Manager,
		User
	}
}
