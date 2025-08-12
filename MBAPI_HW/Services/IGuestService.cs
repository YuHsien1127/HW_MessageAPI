using MBAPI_HW.Dto.Request;
using MBAPI_HW.Dto.Response;

namespace MBAPI_HW.Services
{
    public interface IGuestService
    {
        public GuestResponse GetAllGuests(int page, int pageSize);
        public GuestResponse GetGuestByUserId(string userId);
        public GuestResponse AddGuest(GuestRequest guestRequest);
        public GuestResponse UpdateGuest(GuestRequest guestRequest);
        public GuestResponse DeleteGuest(string userId);
    }
}
