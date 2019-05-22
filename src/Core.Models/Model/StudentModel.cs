using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class StudentModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(200, ErrorMessage = "{0}长度不能超过{1}")]
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [Range(1, 2, ErrorMessage = "{0}格式不正确")]
        public int Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [Display(Name = "年龄")]
        [Range(0, 200, ErrorMessage = "{0}格式不正确")]
        public int Age { get; set; }
    }
}
