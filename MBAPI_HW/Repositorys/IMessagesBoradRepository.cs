using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using MBAPI_HW.Models;

namespace MBAPI_HW.Repositorys
{
    public interface IMessagesBoradRepository
    {
        // MessagesBorad
        public IQueryable<MessagesBorad> GetAllMessagesBorad();
        public MessagesBorad GetMessagesBoradById(int id);
        public void AddMessagesBorad(MessagesBorad messagesBorad);
        public void UpdateMessagesBorad(MessagesBorad messageBorad);
        public void DeleteMessagesBoradById(MessagesBorad messageBorad);

        // MessagesHistory
        public IQueryable<MessagesHistory> GetAllMessagesHistory();
        public MessagesHistory GetMessagesHistoryById(int id);
        public void AddMessagesHistory(MessagesHistory messagesHistory);
        public void UpdateMessagesHistory(MessagesHistory messagesHistory);
        public void DeleteMessagesHistoryById(MessagesHistory messagesHistory);
    }
}
