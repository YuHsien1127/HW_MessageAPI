namespace MBAPI_HW.Dto.Request
{
    public class MessagesBoradRequest
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Decription { get; set; }
        public List<MessageHistoryRequest> History { get; set; } = new List<MessageHistoryRequest>();
    }
    public class MessageHistoryRequest
    {
        public int Id { get; set; }
        public int Mbid { get; set; }
        public string Message { get; set; }
        public string? CreateUserId { get; set; }
    }
}
