using Dapper.Contrib.Extensions;

namespace Core.Entity
{
    /// <summary>
    ///班级
    /// </summary>
    [Table("ClassRoom")]
    public class ClassRoom
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string Name { get; set; }
    }
}
