using Microsoft.AspNetCore.Mvc;
using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;
using MBAPI_HW.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBAPI_HW.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "SuperUser, User")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet]
        public GuestResponse GetAllGuests(int page = 1, int pageSize = 10)
        {
            return _guestService.GetAllGuests(page, pageSize);
        }
        [HttpGet]
        public GuestResponse GetGuestByUserId(string userId)
        {
            return _guestService.GetGuestByUserId(userId);
        }
        [HttpPost]
        [AllowAnonymous]
        public GuestResponse CreateGuest([FromBody] GuestRequest guestRequest)
        {
            return _guestService.AddGuest(guestRequest);
        }
        [HttpPut]
        public GuestResponse UpdateGuest([FromBody] GuestRequest guestRequest)
        {
            return _guestService.UpdateGuest(guestRequest);
        }
        [HttpDelete]
        public GuestResponse DeleteGuest(string userId)
        {
            return _guestService.DeleteGuest(userId);
        }
    }
}
