using Microsoft.AspNetCore.Mvc;
using MBAPI_HW.Services;
using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBAPI_HW.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "SuperUser, User")]
    public class MessagesBoradController : ControllerBase
    {
        private readonly IMessagesBoradService _messagesBoradService;
        public MessagesBoradController(IMessagesBoradService messagesBoradService)
        {
            _messagesBoradService = messagesBoradService;
        }

        [HttpGet]
        public MessagesBoradResponse GetAllMessagesBorad(int page, int pageSize)
        {
            return _messagesBoradService.GetAllMessagesBorad(page, pageSize);
        }
        [HttpGet]
        public MessagesBoradResponse GetMessagesBoradById(int id)
        {
            return _messagesBoradService.GetMessagesBoradById(id);
        }
        [HttpGet]
        public MessagesBoradResponse GetMessagesByUserId(string userId, DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            return _messagesBoradService.GetMessagesByUserId(userId, startDate, endDate, page, pageSize);
        }

        // MessagesBorad
        [HttpPost]        
        public MessagesBoradResponse AddMessagesBorad([FromBody] MessagesBoradRequest messagesBoradRequest)
        {
            return _messagesBoradService.AddMessagesBorad(messagesBoradRequest);
        }
        [HttpPut]
        public MessagesBoradResponse UpdateMessagesBorad([FromBody] MessagesBoradRequest messagesBoradRequest)
        {
            return _messagesBoradService.UpdateMessagesBorad(messagesBoradRequest);
        }
        [HttpDelete]
        public MessagesBoradResponse DeleteMessagesBorad(int id)
        {
            return _messagesBoradService.DeleteMessagesBorad(id);
        }

        // MessagesHistory
        [HttpPost]
        [AllowAnonymous]
        public MessagesBoradResponse AddMessagesHistory([FromBody] MessageHistoryRequest messageHistoryRequest)
        {
            return _messagesBoradService.AddMessagesHistory(messageHistoryRequest);
        }
        [HttpPut]
        public MessagesBoradResponse UpdateMessagesHistory([FromBody] MessageHistoryRequest messageHistoryRequest)
        {
            return _messagesBoradService.UpdateMessagesHistory(messageHistoryRequest);
        }
        [HttpDelete]
        public MessagesBoradResponse DeleteMessagesHistory(int id)
        {
            return _messagesBoradService.DeleteMessagesHistory(id);
        }
    }
}
