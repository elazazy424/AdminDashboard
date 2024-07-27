using AdminDashboard.BLL.Constants;
using AdminDashboard.BLL.Interfaces;
using AdminDashboard.Dtos.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		#region send roles to db
		[HttpGet("send-roles")]
		public async Task<IActionResult> SendRoles()
		{
			var seedResult = await _authService.SeedRolesAsync();
			return StatusCode(seedResult.StatusCode, seedResult);
		}
		#endregion
		#region register
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			var registerResult = await _authService.RegisterAsync(registerDto);
			return StatusCode(registerResult.StatusCode, registerResult.Message);
		}
		#endregion
		#region login
		[HttpPost("login")]
		public async Task<ActionResult<LoginServiceResponseDto>> Login([FromBody] LoginDto loginDto)
		{
			var loginResult = await _authService.LoginAsync(loginDto);
			if (loginResult == null)
			{
				return Unauthorized("Your credentials are invalid please contact to an admin");
			}
			return Ok(loginResult);
		}
		#endregion
		#region update user role
		[HttpPost("update-role")]
		[Authorize(Roles =StaticUserRoles.OwnerAdmin)]
		public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
		{
			var updateRoleResult = await _authService.UpdateRoleAsync(User, updateRoleDto);
			if (updateRoleResult.IsSuccess)
			{
				return Ok(updateRoleResult.Message);
			}
			return StatusCode(updateRoleResult.StatusCode, updateRoleResult.Message);
		}
		#endregion
		#region geeting data of a user from jwt
		[HttpGet("me")]
		public async Task<ActionResult<LoginServiceResponseDto>> Me([FromBody]MeDto token)
		{
			try
			{
				var me = await _authService.MeAsync(token);
				if (me == null)
				{
					return Unauthorized("Invalid Token");
				}
				else
				{
					return Ok(me);
				}
			}
			catch (Exception)
			{
				return Unauthorized("Invalid Token");
			}

		}
		#endregion
		#region list of all users with details
		[HttpGet("users")]
		public async Task<ActionResult<IEnumerable<UserInfoResultDto>>> GetUserList()
		{
			var users = await _authService.GetUserListAcync();
			return Ok(users);
		}
		#endregion
		#region get user by username
		[HttpGet("user/{userName}")]
		public async Task<ActionResult<UserInfoResultDto>> GetUserByUserName([FromRoute]string userName)
		{
			var user = await _authService.GetUserDetailsByUserName(userName);
			if (user == null)
			{
				return NotFound("User not found");
			}
			return Ok(user);
		}
		#endregion
		#region get list of all usernames for send msgs
		[HttpGet("usernames")]
		public async Task<ActionResult<IEnumerable<string>>> GetUserNames()
		{
			var userNames = await _authService.GetUserNameListAcync();
			return Ok(userNames);
		}
		#endregion
	}
}
