using System;
using System.Collections.Generic;

namespace MBAPI_HW.Models
{
    public partial class MessagesBorad
    {
        public MessagesBorad()
        {
            MessagesHistories = new HashSet<MessagesHistory>();
        }

        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Decription { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; } = null!;
        public DateTime ModifyDate { get; set; }
        public string? ModifyUserId { get; set; }

        public virtual ICollection<MessagesHistory> MessagesHistories { get; set; }
    }
}
