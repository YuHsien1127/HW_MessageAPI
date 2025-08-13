using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using MBAPI_HW.Models;
using MBAPI_HW.Repositorys;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using X.PagedList.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MBAPI_HW.Services
{
    public class MessagesBoradService : IMessagesBoradService
    {
        private readonly MessageSQLContext _messageSQLContext;
        private readonly ILogger<MessagesBoradService> _logger;
        private readonly IMessagesBoradRepository _messagesBoradRepository;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public MessagesBoradService(MessageSQLContext messageSQLContext, ILogger<MessagesBoradService> logger, IMessagesBoradRepository messagesBoradRepository, IHttpContextAccessor HttpContextAccessor)
        {
            _messageSQLContext = messageSQLContext;
            _logger = logger;
            _messagesBoradRepository = messagesBoradRepository;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public MessagesBoradResponse GetAllMessagesBorad(int page, int pageSize)
        {
            _logger.LogTrace("【Trace】進入GetAllMessagesBorad");
            MessagesBoradResponse response = new MessagesBoradResponse();

            var messagesBorad = _messagesBoradRepository.GetAllMessagesBorad();
            _logger.LogDebug("【Debug】取得MessagesBorad數量：{Count}", messagesBorad.Count());
            // 從 Claims 取出 UserId
            // HttpContext.User 是一個 ClaimsPrincipal 物件，存放登入使用者的身分資訊（Claims）
            var userId = _HttpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            var m = messagesBorad.Select(x => new MessagesBoradDto
            {
                Id = x.Id,
                Subject = x.Subject,
                Decription = x.Decription,
                CreateDate = x.CreateDate,
                CreateUserId = x.CreateUserId,
                ModifyDate = x.ModifyDate,
                ModifyUserId = x.ModifyUserId,
                MessageHistories = x.MessagesHistories
                    .Select(y => new MessageHistoryDTO
                    {
                        Id = y.Id,
                        Mbid = y.Mbid,
                        Message = (y.IsMark && y.CreateUserId != userId) ? "無權限觀看此流言"  : (y.IsDel ? "已刪除" : y.Message),
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
            MessagesBoradResponse response = new MessagesBoradResponse();
            var messagesBorad = _messagesBoradRepository.GetMessagesBoradById(id);
            if (messagesBorad == null)
            {
                _logger.LogWarning("【Warning】無此{id}留言", id);
                response.Success = false;
                response.Message = "無此Id留言";
                return response;
            }
            // 從 Claims 取出 UserId
            // HttpContext.User 是一個 ClaimsPrincipal 物件，存放登入使用者的身分資訊（Claims）
            var userId = _HttpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            var m = new MessagesBoradDto
            {
                Id = messagesBorad.Id,
                Subject = messagesBorad.Subject,
                Decription = messagesBorad.Decription,
                CreateDate = messagesBorad.CreateDate,
                CreateUserId = messagesBorad.CreateUserId,
                ModifyDate = messagesBorad.ModifyDate,
                ModifyUserId = messagesBorad.ModifyUserId,
                MessageHistories = messagesBorad.MessagesHistories
                    .Select(x => new MessageHistoryDTO
                    {
                        Id = x.Id,
                        Mbid = x.Mbid,
                        Message = (x.IsMark && x.CreateUserId != userId) ? "無權限觀看此流言" : (x.IsDel ? "已刪除" : x.Message),
                        IsDel = x.IsDel,
                        IsMark = x.IsMark,
                        CreateDate = x.CreateDate,
                        CreateUserId = x.CreateUserId
                    }).ToList()
            };

            response.MessagesBorad = new List<MessagesBoradDto> { m };
            response.Success = true;
            response.Message = "查詢成功";
            _logger.LogInformation("【Info】成功查詢留言板Id({id})及其 {Count} 筆留言", id, messagesBorad.MessagesHistories.Count);
            _logger.LogTrace("【Trace】離開GetMessagesBoradById");
            return response;

        }

        // MessagesBorad
        public MessagesBoradResponse AddMessagesBorad(MessagesBoradRequest messagesBoradRequest)
        {
            _logger.LogTrace("【Trace】進入AddMessagesBorad");
            MessagesBoradResponse response = new MessagesBoradResponse();
            try
            {
                if (messagesBoradRequest == null)
                {
                    _logger.LogWarning("新增項目為空");
                    response.Success = false;
                    response.Message = "新增項目為空";
                    return response;
                }
                // 從 Claims 取出 UserId
                // HttpContext.User 是一個 ClaimsPrincipal 物件，存放登入使用者的身分資訊（Claims）
                var userId = _HttpContextAccessor.HttpContext?.User?.FindFirst("User")?.Value;
                _logger.LogTrace("【Trace】開始建立留言板");
                var messagesBorad = new MessagesBorad
                {
                    Subject = messagesBoradRequest.Subject,
                    Decription = messagesBoradRequest.Decription,
                    CreateDate = DateTime.Now,
                    CreateUserId = userId,
                    ModifyDate = DateTime.Now,
                    ModifyUserId = userId
                };
                _logger.LogDebug("【Debug】準備新增留言板：{Id}", messagesBorad.Id);
                _logger.LogTrace("【Trace】建立留言板完成，Id：{Id}", messagesBorad.Id);
                _messagesBoradRepository.AddMessagesBorad(messagesBorad);

                int count = _messageSQLContext.SaveChanges(); // 寫入資料庫
                _logger.LogTrace("【Trace】呼叫SaveChanges完成，影響筆數：{Count}", count);
                if (count > 0)
                {
                    _logger.LogInformation("【Info】新增成功，留言板Id：{Id}", messagesBorad.Id); // log
                    response.Success = true;
                    response.Message = "新增成功";
                }
                else
                {
                    _logger.LogWarning("【Warning】新增失敗"); // log
                    response.Success = false;
                    response.Message = "新增失敗";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【Error】新增留言板發生錯誤"); //log 嚴重錯誤
                response.Success = false;
                response.Message = "新增留言板發生錯誤";
            }
            _logger.LogTrace("【Trace】離開AddMessagesBorad");
            return response;
        }
        public MessagesBoradResponse UpdateMessagesBorad(MessagesBoradRequest messageBoradRequest)
        {
            _logger.LogTrace("【Trace】進入UpdateMessagesBorad");
            MessagesBoradResponse response = new MessagesBoradResponse();

            try
            {
                if (messageBoradRequest == null)
                {
                    _logger.LogWarning("更新資料空");
                    response.Success = false;
                    response.Message = "更新資料為空";
                    return response;
                }
                var existMessagesBorad = _messagesBoradRepository.GetMessagesBoradById(messageBoradRequest.Id);
                if (existMessagesBorad == null)
                {
                    _logger.LogWarning("更新資料Id錯誤");
                    response.Success = false;
                    response.Message = "更新資料Id錯誤";
                    return response;
                }
                // 從 Claims 取出 UserId
                // HttpContext.User 是一個 ClaimsPrincipal 物件，存放登入使用者的身分資訊（Claims）
                var userId = _HttpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                existMessagesBorad.Subject = messageBoradRequest.Subject == "" ? existMessagesBorad.Subject : messageBoradRequest.Subject;
                existMessagesBorad.Decription = messageBoradRequest.Decription == "" ? existMessagesBorad.Decription : messageBoradRequest.Decription;
                existMessagesBorad.ModifyDate = DateTime.Now;
                existMessagesBorad.ModifyUserId = userId;
                _messagesBoradRepository.UpdateMessagesBorad(existMessagesBorad);

                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】留言板Id{Id}，更新成功", messageBoradRequest.Id); // log
                    response.Success = true;
                    response.Message = "更新成功";
                }
                else
                {
                    _logger.LogWarning("【Warning】留言板Id{Id}，更新失敗", messageBoradRequest.Id); // log
                    response.Success = false;
                    response.Message = "更新失敗";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【Error】更新留言板發生錯誤，Id: {Id}", messageBoradRequest.Id);
                response.Success = false;
                response.Message = "更新留言板發生錯誤";
            }
            _logger.LogTrace("【Trace】離開UpdateMessagesBorad");
            return response;
        }
        public MessagesBoradResponse DeleteMessagesBorad(int id)
        {
            _logger.LogTrace("【Trace】進入DeleteMessagesBorad");
            MessagesBoradResponse response = new MessagesBoradResponse();

            try
            {
                var deleteMessageBorad = _messagesBoradRepository.GetMessagesBoradById(id);
                if (deleteMessageBorad == null)
                {
                    _logger.LogWarning("【Warning】找不到此留言板（{id}）", id); // log
                    response.Success = false;
                    response.Message = "查無此留言板";
                    return response;
                }
                _messagesBoradRepository.DeleteMessagesBorad(deleteMessageBorad);

                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】刪除成功，留言板Id：{id}", id);
                    response.Success = true;
                    response.Message = "刪除成功";
                }
                else
                {
                    _logger.LogWarning($"【Warning】刪除失敗");
                    response.Success = false;
                    response.Message = "刪除失敗";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"【Error】刪除留言板發生錯誤");
                response.Success = false;
                response.Message = "刪除留言板發生錯誤";
            }
            _logger.LogTrace("【Trace】離開DeleteMessagesBorad");
            return response;
        }
        
        // MessagesHietory
        public MessagesBoradResponse AddMessagesHistory(MessageHistoryRequest messagesHistoryRequest)
        {
            _logger.LogTrace("【Trace】進入AddMessagesHistory");
            MessagesBoradResponse response = new MessagesBoradResponse();

            try
            {
                if (messagesHistoryRequest == null)
                {
                    _logger.LogWarning("新增項目為空");
                    response.Success = false;
                    response.Message = "新增項目為空";
                    return response;
                }
                // 從 Claims 取出 UserId
                // HttpContext.User 是一個 ClaimsPrincipal 物件，存放登入使用者的身分資訊（Claims）
                var userId = _HttpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                // 沒有登入
                if(string.IsNullOrEmpty(userId))
                {
                    userId = $"{messagesHistoryRequest.CreateUserId}_{Guid.NewGuid().ToString("N")}";
                    _logger.LogDebug("【Debug】匿名留言使用代號：{userId}", userId);
                }
                _logger.LogTrace("【Trace】開始建立留言");
                var messagesHistory = new MessagesHistory
                {
                    Mbid = messagesHistoryRequest.Mbid,
                    Message = messagesHistoryRequest.Message,
                    IsMark = messagesHistoryRequest.IsMark,
                    CreateDate = DateTime.Now,
                    CreateUserId = userId
                };
                _logger.LogDebug("【Debug】準備新增留言：{Id}", messagesHistory.Id);
                _logger.LogTrace("【Trace】建立留言完成，Id：{Id}", messagesHistory.Id);
                _messagesBoradRepository.AddMessagesHistory(messagesHistory);

                int count = _messageSQLContext.SaveChanges(); // 寫入資料庫
                _logger.LogTrace("【Trace】呼叫SaveChanges完成，影響筆數：{Count}", count);
                if (count > 0)
                {
                    _logger.LogInformation("【Info】新增成功，留言板Id：{Id}", messagesHistory.Id); // log
                    response.Success = true;
                    response.Message = "新增成功";
                }
                else
                {
                    _logger.LogWarning("【Warning】新增失敗"); // log
                    response.Success = false;
                    response.Message = "新增失敗";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【Error】新增留言發生錯誤"); //log 嚴重錯誤
                response.Success = false;
                response.Message = "新增留言發生錯誤";
            }
            _logger.LogTrace("【Trace】離開AddMessagesHistory");
            return response;
        }
        public MessagesBoradResponse UpdateMessagesHistory(MessageHistoryRequest messagesHistoryRequest)
        {
            _logger.LogTrace("【Trace】進入UpdateMessagesHistory");
            MessagesBoradResponse response = new MessagesBoradResponse();

            try
            {
                if (messagesHistoryRequest == null)
                {
                    _logger.LogWarning("更新資料空");
                    response.Success = false;
                    response.Message = "更新資料為空";
                    return response;
                }
                var existMessagesHistory = _messagesBoradRepository.GetMessagesHistoryById(messagesHistoryRequest.Id);
                if (existMessagesHistory == null)
                {
                    _logger.LogWarning("更新資料Id錯誤");
                    response.Success = false;
                    response.Message = "更新資料Id錯誤";
                    return response;
                }
                existMessagesHistory.Message = messagesHistoryRequest.Message == "" ? existMessagesHistory.Message : messagesHistoryRequest.Message;
                existMessagesHistory.IsMark = messagesHistoryRequest.IsMark;
                _messagesBoradRepository.UpdateMessagesHistory(existMessagesHistory);

                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】留言Id{Id}，更新成功", messagesHistoryRequest.Id); // log
                    response.Success = true;
                    response.Message = "更新成功";
                }
                else
                {
                    _logger.LogWarning("【Warning】留言Id{Id}，更新失敗", messagesHistoryRequest.Id); // log
                    response.Success = false;
                    response.Message = "更新失敗";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【Error】更新留言發生錯誤，Id: {Id}", messagesHistoryRequest.Id);
                response.Success = false;
                response.Message = "更新留言發生錯誤";
            }
            _logger.LogTrace("【Trace】離開UpdateMessagesHistory");
            return response;
        }
        public MessagesBoradResponse DeleteMessagesHistory(int id)
        {
            _logger.LogTrace("【Trace】進入DeleteMessagesHistory");
            MessagesBoradResponse response = new MessagesBoradResponse();

            try
            {
                var deleteMessagesHistory = _messagesBoradRepository.GetMessagesHistoryById(id);
                if (deleteMessagesHistory == null)
                {
                    _logger.LogWarning("【Warning】找不到此留言（{id}）", id); // log
                    response.Success = false;
                    response.Message = "查無此留言";
                    return response;
                }
                deleteMessagesHistory.IsDel = true;
                _messagesBoradRepository.UpdateMessagesHistory(deleteMessagesHistory);

                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】刪除成功，留言Id：{id}", id);
                    response.Success = true;
                    response.Message = "刪除成功";
                }
                else
                {
                    _logger.LogWarning($"【Warning】刪除失敗");
                    response.Success = false;
                    response.Message = "刪除失敗";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"【Error】刪除留言發生錯誤");
                response.Success = false;
                response.Message = "刪除留言發生錯誤";
            }
            _logger.LogTrace("【Trace】離開DeleteMessagesHistory");
            return response;
        }
    }
}
