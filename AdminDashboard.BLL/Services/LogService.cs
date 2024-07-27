using AdminDashboard.BLL.Interfaces;
using AdminDashboard.DAL.Data;
using AdminDashboard.DAL.Entity;
using AdminDashboard.Dtos.Log;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdminDashboard.BLL.Services
{
	public class LogService : ILogService
	{
		private readonly ApplicationDbContext _context;
		public LogService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task SaveNewLog(string UserName, string Description)
		{
			var log = new Log
			{
				UserName = UserName,
				Description = Description,
			};
			_context.Logs.AddAsync(log);
			await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<GetLogDto>> GetLogsAsync()
		{
			var logs =await _context.Logs.Select(x => new GetLogDto
			{
				UserName = x.UserName,
				Description = x.Description,
				CreatedAt = x.CreatedAt
			}).OrderByDescending(q => q.CreatedAt)
				.ToListAsync();
			return logs;
		}

		public async Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
		{
			var logs = await _context.Logs
				.Where(q => q.UserName == User.Identity.Name)
				.Select(x => new GetLogDto
			{
				UserName = x.UserName,
				Description = x.Description,
				CreatedAt = x.CreatedAt
			}).OrderByDescending(q => q.CreatedAt)
				.ToListAsync();
			return logs;
		}
	}
}
