using AdminDashboard.BLL.Interfaces;
using AdminDashboard.DAL.Data;
using AdminDashboard.DAL.Entity;
using AdminDashboard.Dtos.General;
using AdminDashboard.Dtos.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AdminDashboard.BLL.Services
{
	public class MessageService : IMessageService
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogService _logService;
		private readonly UserManager<ApplicationUser> _userManager;

		public MessageService(ApplicationDbContext context,
			ILogService logService,
			UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_logService = logService;
			_userManager = userManager;
		}
		#region create new message
		public async Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
		{
			if (User.Identity.Name == createMessageDto.ReceiverUserName)
			{
				return new GeneralServiceResponseDto
				{
					IsSuccess = false,
					StatusCode = 400,
					Message = "You can't send message to yourself"
				};
			}
			var isReceiverUserNameValied =  _userManager.Users.Any(x => x.UserName == createMessageDto.ReceiverUserName);
			if (!isReceiverUserNameValied)
			{
				return new GeneralServiceResponseDto
				{
					IsSuccess = false,
					StatusCode = 400,
					Message = "Receiver User Name is not valid"
				};
			}
			Message message = new Message
			{
				ReceiverUserName = createMessageDto.ReceiverUserName,
				SenderUserName = User.Identity.Name,
				Text = createMessageDto.Text,
			};
			await _context.Messages.AddAsync(message);
			await _context.SaveChangesAsync();
			await _logService.SaveNewLog(User.Identity.Name, "Send Message");
			return new GeneralServiceResponseDto
			{
				IsSuccess = true,
				StatusCode = 200,
				Message = "Message saved successfully"
			};
		}
		#endregion
		#region get messages 
		public async Task<IEnumerable<GetMessageDto>> GetMessagesAsync()
		{
			var messages = await _context.Messages
				.Select(q => new GetMessageDto()
				{
					Id = q.Id,
					ReceiverUserName = q.ReceiverUserName,
					SenderUserName = q.SenderUserName,
					Text = q.Text,
					CreatedAt = q.CreatedAt
				}).OrderByDescending(q => q.CreatedAt).ToListAsync();
			return messages;
		}
		#endregion
		#region get my messages
		public async Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User)
		{
			var loggedInUser = User.Identity.Name;
			var messages = await _context.Messages
				.Where(q => q.ReceiverUserName == loggedInUser || q.SenderUserName == loggedInUser)
				.Select(q => new GetMessageDto()
				{
					Id = q.Id,
					ReceiverUserName = q.ReceiverUserName,
					SenderUserName = q.SenderUserName,
					Text = q.Text,
					CreatedAt = q.CreatedAt
				}).OrderByDescending(q => q.CreatedAt).ToListAsync();
			return messages;
		}
		#endregion
	}
}
