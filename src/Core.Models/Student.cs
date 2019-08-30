using Core.Common.Entity;
using Dapper;

namespace Core.Models.Identity.Entity
{
    /// <summary>
    /// 学生
    /// </summary>
    [Table("Student")]
    public class Student : EntityBase<long>
    {
        /// <summary>
        /// 用户Id        
        /// </summary>
        [Key]
        public override long Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public int ClassRoomId { get; set; }
    }
}
