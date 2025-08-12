using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using MBAPI_HW.Models;
using MBAPI_HW.Repositorys;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace MBAPI_HW.Services
{
    public class MessagesBoradService : IMessagesBoradService
    {
        private MessageSQLContext _messageSQLContext;
        private ILogger<MessagesBoradService> _logger;
        private IMessagesBoradRepository _messagesBoradRepository;
        private IGuestRepository _guestRepository;
        public MessagesBoradService(MessageSQLContext messageSQLContext, ILogger<MessagesBoradService> logger, IMessagesBoradRepository messagesBoradRepository, IGuestRepository guestRepository)
        {
            _messageSQLContext = messageSQLContext;
            _logger = logger;
            _messagesBoradRepository = messagesBoradRepository;
            _guestRepository = guestRepository;
        }

        public MessagesBoradResponse GetAllMessagesBorad(int page, int pageSize)
        {
            _logger.LogTrace("【Trace】進入GetAllMessagesBorad");
            MessagesBoradResponse response = new MessagesBoradResponse();

            var messagesBorad = _messagesBoradRepository.GetAllMessagesBorad();
            _logger.LogDebug("【Debug】取得MessagesBorad數量：{messagesBorad.Count()}", messagesBorad.Count());
            var m = messagesBorad.Select(x => new MessagesBoradDto
            {
                Id = x.Id,
                Subject = x.Subject,
                Decription = x.Decription,
                CreateDate = x.CreateDate,
                CreateUserId = x.CreateUserId,
                ModifyDate = x.ModifyDate,
                ModifyUserId = x.ModifyUserId,
                MessageHistories = x.MessagesHistories.Select(y => new MessageHistoryDTO
                {
                    Id = y.Id,
                    Mbid = y.Mbid,
                    Message = y.Message,
                    IsDel = y.IsDel,
                    IsMark = y.IsMark,
                    CreateDate = y.CreateDate,
                    CreateUserId = y.CreateUserId
                }).ToList()
            });
            var pagedList = m.ToPagedList(page, pageSize);

            response.MessagesBorad = pagedList.ToList();
            response.PageCount = pagedList.PageCount;
            response.TotalCount = pagedList.TotalItemCount;
            response.Success = true;
            response.Message = $"取得第{page}頁，{pageSize}筆資料";
            _logger.LogTrace("【Trace】離開GetAllMessagesBorad");
            return response;
        }
        public MessagesBoradResponse GetMessagesBoradById(int id)
        {
            _logger.LogTrace("【Trace】進入GetMessagesBoradById");
            _logger.LogTrace("【Trace】離開GetMessagesBoradById");
            throw new NotImplementedException();
        }

        public MessagesBoradResponse AddMessagesBorad(MessagesBoradRequest messagesBoradRequest)
        {
            _logger.LogTrace("【Trace】進入AddMessagesBorad");
            _logger.LogTrace("【Trace】離開AddMessagesBorad");
            throw new NotImplementedException();
        }

        public MessagesBoradResponse DeleteMessagesBorad(int id)
        {
            _logger.LogTrace("【Trace】進入DeleteMessagesBorad");
            _logger.LogTrace("【Trace】離開DeleteMessagesBorad");
            throw new NotImplementedException();
        }        

        public MessagesBoradResponse UpdateMessagesBorad(MessagesBoradRequest messageBoradRequest)
        {
            _logger.LogTrace("【Trace】進入GetAllMessagesBorad");
            _logger.LogTrace("【Trace】離開UpdateMessagesBorad");
            throw new NotImplementedException();
        }
    }
}
