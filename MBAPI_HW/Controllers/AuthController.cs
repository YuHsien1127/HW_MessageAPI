using Microsoft.AspNetCore.Mvc;
using MBAPI_HW.Services;
using MBAPI_HW.Repositorys;
using MBAPI_HW.Dto.Request;

namespace MBAPI_HW.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGuestRepository _guestRepository;
        public AuthController(IAuthService authService, IGuestRepository guestRepository)
        {
            _authService = authService;
            _guestRepository = guestRepository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var guest = _guestRepository.GetGuestByUserId(loginRequest.UserId);
            if (guest != null && guest.Password == loginRequest.Password)
            {
                var token = _authService.GenerateJwtToken(guest.UserId, guest.Role);
                return Ok("Bearer " + token);
            }
            return Unauthorized("帳號或密碼錯誤");
        }
    }
}
