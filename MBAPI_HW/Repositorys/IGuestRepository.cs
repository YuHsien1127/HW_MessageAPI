using MBAPI_HW.Models;

namespace MBAPI_HW.Repositorys
{
    public interface IGuestRepository
    {
        public IQueryable<Guest> GetAllGuests();
        public Guest GetGuestByUserId(string userId);
        public Guest GetGuestById(int id);
        public void AddGuest(Guest guest);
        public void UpdateGuest(Guest guest);
        public void DeleteGuest(Guest guest);
    }
}
