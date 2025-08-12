namespace MBAPI_HW.Dto.Response
{
    public class MessagesBoradResponse : BaseResponse
    {
        public List<MessagesBoradDto> MessagesBorad { get; set; }
        public int PageCount { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
    }
    public class MessagesBoradDto
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Decription { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; } = "";
        public DateTime ModifyDate { get; set; }
        public string? ModifyUserId { get; set; }
        public List<MessageHistoryDTO> MessageHistories { get; set; } = new List<MessageHistoryDTO>();
    }
    public class MessageHistoryDTO
    {
        public int Id { get; set; }
        public int Mbid { get; set; }
        public string Message { get; set; } = "";
        public bool IsDel { get; set; } = false;
        public bool IsMark { get; set; } = false;
        public DateTime CreateDate { get; set; }
        public string? CreateUserId { get; set; }
    }
}
