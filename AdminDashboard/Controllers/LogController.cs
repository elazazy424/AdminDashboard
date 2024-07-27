using AdminDashboard.BLL.Constants;
using AdminDashboard.BLL.Interfaces;
using AdminDashboard.Dtos.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LogController : ControllerBase
	{
		private readonly ILogService _logService;

		public LogController(ILogService logService)
		{
			_logService = logService;
		}

		[HttpGet]
		[Authorize(Roles = StaticUserRoles.OwnerAdmin)]
		public async Task<ActionResult<IEnumerable<GetLogDto>>> GetLogs()
		{
			var logs = await _logService.GetLogsAsync();
			return Ok(logs);
		}

		[HttpGet("mine")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<GetLogDto>>> GetMyLogs()
		{
			var logs = await _logService.GetMyLogsAsync(User);
			return Ok(logs);
		}
	}
}
