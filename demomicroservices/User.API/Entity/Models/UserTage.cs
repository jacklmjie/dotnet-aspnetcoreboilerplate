using System;

namespace User.API.Entity.Models
{
    public class UserTage
    {
        public int AppUserId { get; set; }

        public string Tag { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
