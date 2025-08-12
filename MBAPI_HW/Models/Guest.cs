using System;
using System.Collections.Generic;

namespace MBAPI_HW.Models
{
    public partial class Guest
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
