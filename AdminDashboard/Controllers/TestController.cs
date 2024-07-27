using AdminDashboard.BLL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpGet("getPublic")]
		public IActionResult GetPublic()
		{
			return Ok("This is a public endpoint");
		}

		[HttpGet("getUserRole")]
		[Authorize(Roles =StaticUserRoles.User)]
		public IActionResult GetUserData()
		{
			return Ok("User Role Data");
		}

		[HttpGet("getManagerRole")]
		[Authorize(Roles = StaticUserRoles.Manager)]
		public IActionResult GetManagerData()
		{
			return Ok("Manager Role Data");
		}

		[HttpGet("getAdminRole")]
		[Authorize(Roles = StaticUserRoles.Admin)]
		public IActionResult GetAdminData()
		{
			return Ok("Admin Role Data");
		}

		[HttpGet("getOwnerRole")]
		[Authorize(Roles = StaticUserRoles.Owner)]
		public IActionResult GetOwnerData()
		{
			return Ok("Owner Role Data");
		}
	}
}
