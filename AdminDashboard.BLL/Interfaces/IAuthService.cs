using AdminDashboard.Dtos.Auth;
using AdminDashboard.Dtos.General;
using System.Security.Claims;

namespace AdminDashboard.BLL.Interfaces
{
	public interface IAuthService
	{
		Task<GeneralServiceResponseDto> SeedRolesAsync();
		Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto);
		Task<GeneralServiceResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);
		Task<LoginServiceResponseDto?> LoginAsync(LoginDto LoginDto);
		Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
		Task<UserInfoResultDto?> GetUserDetailsByUserName(string userName);
		Task<IEnumerable<UserInfoResultDto>> GetUserListAcync();
		Task<IEnumerable<string>> GetUserNameListAcync();
	}
}
