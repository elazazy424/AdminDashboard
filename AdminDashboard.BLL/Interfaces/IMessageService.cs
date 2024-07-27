using AdminDashboard.Dtos.General;
using AdminDashboard.Dtos.Message;
using System.Security.Claims;

namespace AdminDashboard.BLL.Interfaces
{	
	public interface IMessageService
	{
		Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto);
		Task<IEnumerable<GetMessageDto>> GetMessagesAsync();
		Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User);

	}
}
