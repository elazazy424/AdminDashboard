using AdminDashboard.BLL.Constants;
using AdminDashboard.BLL.Interfaces;
using AdminDashboard.DAL.Entity;
using AdminDashboard.Dtos.Auth;
using AdminDashboard.Dtos.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

namespace AdminDashboard.BLL.Repository
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ILogService _logService;
		private readonly IConfiguration _configuration;

		public AuthService(RoleManager<IdentityRole> roleManager,
						   UserManager<ApplicationUser> userManager,
						   ILogService logService,
						   IConfiguration configuration)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_logService = logService;
		    _configuration = configuration;
		}
		#region SeedRoles
		public async Task<GeneralServiceResponseDto> SeedRolesAsync()
		{
			bool isOwnerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Owner);
			bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Admin);
			bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.User);
			bool isManagerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Manager);

			if (isOwnerRoleExist && isAdminRoleExist && isManagerRoleExist && isManagerRoleExist)
			{
				return new GeneralServiceResponseDto
				{
					IsSuccess = true,
					StatusCode = 200,
					Message = "Roles already exist"
				};
			}
			else
			{
				await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Owner));
				await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Admin));
				await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Manager));
				await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.User));

				return new GeneralServiceResponseDto
				{
					IsSuccess = true,
					StatusCode = 201,
					Message = "Roles created successfully"
				};
			}
		}
		#endregion
		#region Register
		public async Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto)
		{
			var isExistUser = await _userManager.FindByNameAsync(registerDto.UserName);
			if (isExistUser != null)
			{
				return new GeneralServiceResponseDto
				{
					IsSuccess = false,
					StatusCode = 409,
					Message = "User already exists"
				};
			}

			ApplicationUser newUser = new ApplicationUser
			{
				Email = registerDto.Email,
				UserName = registerDto.UserName,
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName,
				Address = registerDto.Address,
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var result = await _userManager.CreateAsync(newUser, registerDto.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(newUser, StaticUserRoles.User);
				await _logService.SaveNewLog(newUser.UserName, "Registered to Website");
				return new GeneralServiceResponseDto
				{
					IsSuccess = true,
					StatusCode = 201,
					Message = "User created successfully"
				};
			}
			else
			{
				var errorString = "User creation failed because";
				foreach (var error in result.Errors)
				{
					errorString += " #" + error.Description;
				}

				return new GeneralServiceResponseDto
				{
					IsSuccess = false,
					StatusCode = 400,
					Message = errorString
				};
			}
		}

		#endregion
		#region Login
		public async Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto)
		{
			// Find user by username
			var user = await _userManager.FindByNameAsync(loginDto.UserName);
			if (user is null)
				return null;

			// Check password of user
			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			if (!isPasswordCorrect)
				return null;

			// Create token
			var newToken = await GenerateJWTTokenAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var userInfo = GenerateUserInfoObject(user, roles);

			await _logService.SaveNewLog(user.UserName, "New Login");

			return new LoginServiceResponseDto
			{
				NewToken = newToken,
				UserInfo = userInfo
			};
		}


		#endregion
		#region updateRole 
		public async Task<GeneralServiceResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user is null)
			{
				return new GeneralServiceResponseDto
				{
					IsSuccess = false,
					StatusCode = 404,
					Message = "User not found"
				};
			}

			var userRoles = await _userManager.GetRolesAsync(user);

			//JUST THE OWNER and admin CAN CHANGE THE ROLE
			if (User.IsInRole(StaticUserRoles.Admin))
			{
				//user is admin
				if (updateRoleDto.Role == RoleType.User || updateRoleDto.Role == RoleType.Manager)
				{
					//adim can change role to user or manager only
					if (userRoles.Any(q => q.Equals(StaticUserRoles.Owner) || q.Equals(StaticUserRoles.Admin)))
					{
						return new GeneralServiceResponseDto
						{
							IsSuccess = false,
							StatusCode = 404,
							Message = "You are not allowed to change role of this user"
						};
					}
					else
					{
						await _userManager.RemoveFromRolesAsync(user, userRoles);
						await _userManager.AddToRoleAsync(user, updateRoleDto.Role.ToString());
						await _logService.SaveNewLog(user.UserName, "Role changed to " + updateRoleDto.Role.ToString());
						return new GeneralServiceResponseDto
						{
							IsSuccess = true,
							StatusCode = 200,
							Message = "Role changed successfully"
						};
					}
				}
				else
				{
					return new GeneralServiceResponseDto
					{
						IsSuccess = false,
						StatusCode = 404,
						Message = "Role not found"
					};
				}
	
			}
			else
			{
				//user is owner
				if (userRoles.Any(q => q.Equals(StaticUserRoles.Owner)))
				{
					return new GeneralServiceResponseDto
					{
						IsSuccess = false,
						StatusCode = 404,
						Message = "Role not found"
					};
				}
				else
				{
					await _userManager.RemoveFromRolesAsync(user, userRoles);
					await _userManager.AddToRoleAsync(user, updateRoleDto.Role.ToString());
					await _logService.SaveNewLog(user.UserName, "Role changed to " + updateRoleDto.Role.ToString());
					return new GeneralServiceResponseDto
					{
						IsSuccess = true,
						StatusCode = 200,
						Message = "Role changed successfully"
					};
				}
			}

		}
		#endregion
		#region  to return JWT token
		public async Task<LoginServiceResponseDto?> MeAsync(MeDto meDto)
		{
			ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = _configuration["Jwt:ValidIssuer"],
				ValidAudience = _configuration["Jwt:ValidAudience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
			}, out SecurityToken validatedToken);
			string decodedUserName = handler.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
			if (decodedUserName is null)
			{
				return null;
			}
			var user = await _userManager.FindByNameAsync(decodedUserName);
			if (user is null)
			{
				return null;
			}
			var newToken = await GenerateJWTTokenAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var userInfo = GenerateUserInfoObject(user, roles);
			await _logService.SaveNewLog(user.UserName, "New Token Generated");
			return new LoginServiceResponseDto
			{
				NewToken = newToken,
				UserInfo = userInfo
			};
		}
		#endregion
		#region function to return UserInfoResultDto
		public async Task<IEnumerable<UserInfoResultDto>> GetUserListAcync()
		{
			var users = await _userManager.Users.ToListAsync();
			List<UserInfoResultDto> userInfoResultDtos = new List<UserInfoResultDto>();
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userInfoResultDtos.Add(GenerateUserInfoObject(user, roles));
			}
			return userInfoResultDtos;
		}
		#endregion
		#region function to return UserInfoResultDto
		public async Task<UserInfoResultDto?> GetUserDetailsByUserName(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
			{
				return null;
			}
			var roles = await _userManager.GetRolesAsync(user);
			return GenerateUserInfoObject(user, roles);
		}
		#endregion

		public async Task<IEnumerable<string>> GetUserNameListAcync()
		{
			return await _userManager.Users
				.Select(q => q.UserName)
				.ToListAsync();
		}

		#region function to return JWT token
		private async Task<string> GenerateJWTTokenAsync(ApplicationUser applicationUser)
		{
			var userRoles = await _userManager.GetRolesAsync(applicationUser);
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
				new Claim(ClaimTypes.Name, applicationUser.UserName),
				new Claim("FirstName", applicationUser.FirstName),
				new Claim("LastName", applicationUser.LastName)
			};

			foreach (var userRole in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var authSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var signingCredentials = new SigningCredentials(authSecretKey, SecurityAlgorithms.HmacSha256);

			var tokenObject = new JwtSecurityToken(
				issuer: _configuration["Jwt:ValidIssuer"],
				audience: _configuration["Jwt:ValidAudience"],
				claims: claims,
				notBefore: DateTime.Now,
				expires: DateTime.Now.AddHours(3),
				signingCredentials: signingCredentials
			);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
			return token;
		}

		#endregion
		#region function to return UserInfoResultDto
		private UserInfoResultDto GenerateUserInfoObject(ApplicationUser user, IEnumerable<string> roles)
		{
			return new UserInfoResultDto
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName,
				Email = user.Email,
				CreatedAt = user.CreatedAt,
				Roles = roles
			};
		}
		#endregion

	}
}
