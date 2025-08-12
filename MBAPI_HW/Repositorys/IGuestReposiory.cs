using MBAPI_HW.Models;

namespace MBAPI_HW.Repositorys
{
    public interface IGuestReposiory
    {
        public IQueryable<Guest> GetAllGuests();
        public Guest GetGuestByUserId(string userId);
        public void AddGuest(Guest guest);
        public void UpdateGuest(Guest guest);
        public void DeleteGuest(Guest guest);
    }
}
