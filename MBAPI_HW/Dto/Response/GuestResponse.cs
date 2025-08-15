namespace MBAPI_HW.Dto.Response
{
    public class GuestResponse : BaseResponse
    {
        public List<GuestDto> Guest { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }
    public class GuestDto
    {
        //public int Id { get; set; }
        public string UserId { get; set; }
        //public string Password { get; set; }
        public string Role { get; set; }
        //public DateTime CreateDate { get; set; }
        //public DateTime ModifyDate { get; set; }
    }
}
