using System;

namespace User.API.Models
{
    public class BpFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 上传的源文件地址
        /// </summary>
        public string OriginFilePath { get; set; }
        /// <summary>
        /// 格式转换后的文件地址
        /// </summary>
        public string FromatFilePath { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
