using System;
using System.Collections.Generic;

namespace MBAPI_HW.Models
{
    public partial class MessagesHistory
    {
        public int Id { get; set; }
        public int Mbid { get; set; }
        public string Message { get; set; } = null!;
        public bool IsDel { get; set; }
        public bool IsMark { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; } = null!;

        public virtual MessagesBorad Mb { get; set; } = null!;
    }
}
