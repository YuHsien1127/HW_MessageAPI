using MBAPI_HW.Models;

namespace MBAPI_HW.Repositorys
{
    public class GuestRepository : IGuestReposiory
    {
        private readonly MessageSQLContext _messageSQLContext;
        public GuestRepository(MessageSQLContext messageSQLContext)
        {
            _messageSQLContext = messageSQLContext;
        }

        public IQueryable<Guest> GetAllGuests()
        {
            return _messageSQLContext.Guests;
        }
        public Guest GetGuestByUserId(string userId)
        {
            return _messageSQLContext.Guests.FirstOrDefault(g => g.UserId == userId);
        }

        public void AddGuest(Guest guest)
        {
            _messageSQLContext.Guests.Add(guest);
        }

        public void DeleteGuest(Guest guest)
        {
            _messageSQLContext.Guests.Remove(guest);
        }              

        public void UpdateGuest(Guest guest)
        {
            _messageSQLContext.Guests.Update(guest);
        }
    }
}
