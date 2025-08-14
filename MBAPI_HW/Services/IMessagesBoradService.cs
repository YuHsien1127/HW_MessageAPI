using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;

namespace MBAPI_HW.Services
{
    public interface IMessagesBoradService
    {
        public MessagesBoradResponse GetAllMessagesBorad(int page, int pageSize);
        public MessagesBoradResponse GetMessagesBoradById(int id);
        // 用 userId 依 startDate 來查詢留言
        public MessagesBoradResponse GetMessagesByUserId(string userId, DateTime startDate, DateTime endDate, int page, int pageSize);
        
        // MessagesBorad add/update/delete
        public MessagesBoradResponse AddMessagesBorad(MessagesBoradRequest messagesBoradRequest);
        public MessagesBoradResponse UpdateMessagesBorad(MessagesBoradRequest messageBoradRequest);
        public MessagesBoradResponse DeleteMessagesBorad(int id);

        // MessagesHistory add/update/delete
        public MessagesBoradResponse AddMessagesHistory(MessageHistoryRequest messagesHistoryRequest);
        public MessagesBoradResponse UpdateMessagesHistory(MessageHistoryRequest messagesHistoryRequest);
        public MessagesBoradResponse DeleteMessagesHistory(int id);
    }
}
