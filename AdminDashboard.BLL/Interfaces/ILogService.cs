using AdminDashboard.Dtos.Log;
using System.Security.Claims;

namespace AdminDashboard.BLL.Interfaces
{
	public interface ILogService
	{
		Task SaveNewLog(string UserName, string Description);
		Task<IEnumerable<GetLogDto>> GetLogsAsync();
		Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User);
	}
}
