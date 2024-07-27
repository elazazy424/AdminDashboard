using AdminDashboard.BLL.Constants;
using AdminDashboard.BLL.Interfaces;
using AdminDashboard.Dtos.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly IMessageService _messageService;

		public MessageController(IMessageService messageService)
		{
			_messageService = messageService;
		}
		#region create a new message to send to another user
		[HttpPost("Create")]
		[Authorize]
		public async Task<ActionResult<CreateMessageDto>> CreateMessage([FromBody]CreateMessageDto createMessageDto)
		{
			var message = await _messageService.CreateNewMessageAsync(User, createMessageDto);
			if (message.IsSuccess)
			{
				return Ok(message);
			}
			return StatusCode(message.StatusCode, message.Message);
		}
		#endregion
		#region get all messages for the current user, either sent or received
		[HttpGet("mine")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMyMessages()
		{
			var messages = await _messageService.GetMyMessagesAsync(User);
			return Ok(messages);
		}
		#endregion
		#region get all msgs with owner access and admin access
		[HttpGet]
		[Authorize(Roles = StaticUserRoles.OwnerAdmin)]
		public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMessages()
		{
			var messages = await _messageService.GetMessagesAsync();
			return Ok(messages);
		}
		#endregion
	}
}
