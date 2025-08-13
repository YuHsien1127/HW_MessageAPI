using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using MBAPI_HW.Models;
using MBAPI_HW.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Principal;
using X.PagedList.Extensions;

namespace MBAPI_HW.Services
{
    public class GuestService : IGuestService
    {
        private readonly MessageSQLContext _messageSQLContext;
        private readonly ILogger<GuestService> _logger;
        private readonly IGuestRepository _guestRepository;
        public GuestService(MessageSQLContext messageSQLContext, ILogger<GuestService> logger, IGuestRepository guestRepository)
        {
            _messageSQLContext = messageSQLContext;
            _logger = logger;
            _guestRepository = guestRepository;
        }

        public GuestResponse GetAllGuests(int page, int pageSize)
        {
            _logger.LogTrace("【Trace】進入GetAllGuests");
            GuestResponse response = new GuestResponse();

            var guests = _guestRepository.GetAllGuests();
            _logger.LogDebug("【Debug】取得Guest數量：{guests.Count()}", guests.Count());

            var g = guests.Select(x => new GuestDto
            {
                UserId = x.UserId,
                Role = x.Role
            });
            var pagedList = g.ToPagedList(page, pageSize);
            response.Guest = pagedList.ToList();
            response.PageCount = pagedList.PageCount;
            response.TotalCount = pagedList.TotalItemCount;
            response.Message = "查詢成功";
            _logger.LogTrace("【Trace】離開GetAllGuests");
            return response;
        }
        public GuestResponse GetGuestByUserId(string userId)
        {
            _logger.LogTrace("【Trace】進入GetGuestByUserId");
            GuestResponse response = new GuestResponse();

            if(userId == "")
            {
                _logger.LogWarning("【Warning】UserId為空");
                response.Success = false;
                response.Message = $"UserId為空";
                return response;
            }
            var guests = _guestRepository.GetGuestByUserId(userId);
            if(guests == null)
            {
                _logger.LogWarning("【Warning】無此UserId用戶");
                response.Success = false;
                response.Message = $"無此UserId({userId})用戶";
                return response;
            }
            var g = new GuestDto
            {
                UserId = guests.UserId,
                Role = guests.Role
            };
            
            response.Guest = new List<GuestDto> { g };
            response.Success = true;
            response.Message = "查詢成功";
            _logger.LogInformation("【Info】成功查詢Guest資訊");
            _logger.LogTrace("【Trace】離開GetGuestByUserId");
            return response;
        }

        public GuestResponse AddGuest(GuestRequest guestRequest)
        {
            _logger.LogTrace("【Trace】進入AddGuest");
            GuestResponse response = new GuestResponse();

            try 
            {
                if(guestRequest == null)
                {
                    _logger.LogWarning("【Warning】新增Guest資料為空"); //log
                    response.Success = false;
                    response.Message = "新增Guest資料為空";
                    return response;
                }
                _logger.LogDebug("【Debug】接收到新增顧客資料（UserId：{guestRequest.UserId},Role：{guestRequest.Role}", guestRequest.UserId, guestRequest.Role);
                var guest = new Guest
                {                    
                    Password = guestRequest.Password,
                    Role = guestRequest.Role,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,

                };
                _guestRepository.AddGuest(guest);
                var q = new GuestDto
                {
                    UserId = guestRequest.UserId,
                    Role = guestRequest.Role
                };

                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】新增成功（UserId：{guest.UserId}）", guest.UserId); // log
                    response.Guest = new List<GuestDto> { q };
                    response.Success = true;
                    response.Message = "新增成功";
                }
                else
                {
                    _logger.LogWarning("【Warning】新增失敗");
                    response.Success = false;
                    response.Message = "新增失敗";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "【Error】新增發生錯誤（ID：{guestRequest.Id}）", guestRequest.Id); //log 嚴重錯誤
                response.Success = false;
                response.Message = "新增發生錯誤";
            }
            _logger.LogTrace("【Trace】離開AddGuest");
            return response;
        }

        public GuestResponse DeleteGuest(string userId)
        {
            _logger.LogTrace("【Trace】進入DeleteGuest");
            GuestResponse response = new GuestResponse();

            try
            {
                if(userId == "")
                {
                    _logger.LogWarning("【Warning】UserId為空");
                    response.Success = false;
                    response.Message = $"UserId為空";
                    return response;
                }
                var guest = _guestRepository.GetGuestByUserId(userId);
                if (guest == null)
                {
                    _logger.LogWarning("【Warning】無此UserId({userId})的用戶", userId);
                    response.Success = false;
                    response.Message = "Guest資料為空";
                    return response;
                }
                _logger.LogDebug("【Debug】準備刪除Guest資料（UserId ：{guest.UserId}）", guest.UserId);
                _guestRepository.DeleteGuest(guest);

                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】刪除成功(UserId：{userId})", userId);
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "【Error】刪除發生錯誤"); //log 嚴重錯誤
                response.Success = false;
                response.Message = "刪除發生錯誤";
            }
            _logger.LogTrace("【Trace】離開DeleteGuest");
            return response;
        }
        
        public GuestResponse UpdateGuest(GuestRequest guestRequest) 
        {
            _logger.LogTrace("【Trace】進入UpdateGuest");
            GuestResponse response = new GuestResponse();

            try
            {
                _logger.LogDebug("【Debug】傳入更新資料（UserId：{request.Account},Password：{request.Name}）", guestRequest.UserId, guestRequest.Password);
                if(guestRequest == null)
                {
                    _logger.LogWarning("【Warning】更新項目為空"); //log
                    response.Success = false;
                    response.Message = "更新項目為空";
                    return response;
                }
                var existGuest = _guestRepository.GetGuestById(guestRequest.Id);
                if (existGuest == null)
                {
                    _logger.LogWarning("【Warning】此Id（{guestRequest.Id}）Guest資料為空", guestRequest.Id); //log
                    response.Success = false;
                    response.Message = "Guest資料為空";
                    return response;
                }
                existGuest.UserId = guestRequest.UserId == "" ? existGuest.UserId : guestRequest.UserId;
                existGuest.Password = guestRequest.Password == "" ? existGuest.Password : guestRequest.Password;
                existGuest.ModifyDate = DateTime.Now;
                _guestRepository.UpdateGuest(existGuest);
                _logger.LogDebug("【Debug】更新後的資料（UserId：{request.Account},Password：{request.Name}）", guestRequest.UserId, guestRequest.Password);
                int count = _messageSQLContext.SaveChanges();
                if (count > 0)
                {
                    _logger.LogInformation("【Info】Guest's Id{guestRequest.Id}（UserId：{existGuest.UserId}）更新成功", guestRequest.Id, existGuest.UserId); // log
                    response.Success = true;
                    response.Message = "更新成功";
                }
                else
                {
                    _logger.LogWarning("【Warning】Guest's Id{guestRequest.Id}（UserId：{existGuest.UserId}）更新失敗", guestRequest.Id, existGuest.UserId); // log
                    response.Success = false;
                    response.Message = "更新失敗";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "【Error】更新發生錯誤（Id：{guestRequest.Id}）", guestRequest.Id);
                response.Success = false;
                response.Message = "更新發生錯誤";
            }
            _logger.LogTrace("【Trace】離開UpdateGuest");
            return response;
        }
    }
}