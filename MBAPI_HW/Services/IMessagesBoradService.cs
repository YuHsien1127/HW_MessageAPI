using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using MBAPI_HW.Models;

namespace MBAPI_HW.Services
{
    public interface IMessagesBoradService
    {
        public MessagesBoradResponse GetAllMessagesBorad(int page, int pageSize);
        public MessagesBoradResponse GetMessagesBoradById(int id);
        public MessagesBoradResponse AddMessagesBorad(MessagesBoradRequest messagesBoradRequest);
        public MessagesBoradResponse UpdateMessagesBorad(MessagesBoradRequest messageBoradRequest);
        public MessagesBoradResponse DeleteMessagesBorad(int id);
    }
}
