using System.Collections.Generic;

namespace User.API.Entity.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }

        public string Phone { get; set; }

        public int ProvinceId { get; set; }

        public string Province { get; set; }

        public int CityId { get; set; }

        public string City { get; set; }

        public int CountyId { get; set; }

        public string County { get; set; }

        /// <summary>
        /// 名片地址
        /// </summary>
        public string NameCard { get; set; }

        /// <summary>
        /// 用户属性列表
        /// </summary>
        public List<UserProperty> Properties { get; set; }
    }
}
