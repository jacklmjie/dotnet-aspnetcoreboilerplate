using System;

namespace User.API.Models
{
    public class UserTage
    {
        public int UserId { get; set; }

        public string Tag { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
