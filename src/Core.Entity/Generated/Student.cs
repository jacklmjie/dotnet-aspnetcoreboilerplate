using Dapper.Contrib.Extensions;

namespace Core.Entity
{
    /// <summary>
    /// 学生
    /// </summary>
    [Table("ClassRoom")]
    public partial class Student
    {
        /// <summary>
        /// 用户Id        
        /// </summary>
        [Key]
        public long Id { get; set; }
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
    }
}
