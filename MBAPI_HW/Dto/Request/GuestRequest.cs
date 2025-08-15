namespace MBAPI_HW.Dto.Request
{
    public class GuestRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        //public DateTime CreateDate { get; set; }
        //public DateTime ModifyDate { get; set; }
    }
}
