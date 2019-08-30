using Core.Common.Entity;
using Dapper;

namespace Core.Models.Identity.Entity
{
    /// <summary>
    ///班级
    /// </summary>
    [Table("ClassRoom")]
    public class ClassRoom : EntityBase<long>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public override long Id { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string Name { get; set; }
    }
}
