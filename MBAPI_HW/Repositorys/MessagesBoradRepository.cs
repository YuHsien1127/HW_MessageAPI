using Microsoft.EntityFrameworkCore;
using MBAPI_HW.Models;

namespace MBAPI_HW.Repositorys
{
    public class MessagesBoradRepository : IMessagesBoradRepository
    {
        private readonly MessageSQLContext _messageSQLContext;
        public MessagesBoradRepository(MessageSQLContext messageSQLContext)
        {
            _messageSQLContext = messageSQLContext;
        }

        // MessagesBorad
        public IQueryable<MessagesBorad> GetAllMessagesBorad()
        {
            return _messageSQLContext.MessagesBorads.Include(h => h.MessagesHistories);
        }
        public MessagesBorad GetMessagesBoradById(int id)
        {
            return _messageSQLContext.MessagesBorads.Include(h => h.MessagesHistories).FirstOrDefault(i => i.Id == id); ;
        }

        public void AddMessagesBorad(MessagesBorad messagesBorad)
        {
            _messageSQLContext.Add(messagesBorad);
        }

        public void DeleteMessagesBoradById(MessagesBorad messageBorad)
        {
            _messageSQLContext.Remove(messageBorad);
        }        

        public void UpdateMessagesBorad(MessagesBorad messagesBorad)
        {
            _messageSQLContext.Update(messagesBorad);
        }

        // MessagesHistory
        public IQueryable<MessagesHistory> GetAllMessagesHistory()
        {
            return _messageSQLContext.MessagesHistories;
        }

        public MessagesHistory GetMessagesHistoryById(int id)
        {
            return _messageSQLContext.MessagesHistories.Find(id);
        }

        public void AddMessagesHistory(MessagesHistory messagesHistory)
        {
            _messageSQLContext.Add(messagesHistory);
        }

        public void UpdateMessagesHistory(MessagesHistory messagesHistory)
        {
            _messageSQLContext.Update(messagesHistory);
        }

        public void DeleteMessagesHistoryById(MessagesHistory messagesHistory)
        {
            throw new NotImplementedException();
        }
    }
}
