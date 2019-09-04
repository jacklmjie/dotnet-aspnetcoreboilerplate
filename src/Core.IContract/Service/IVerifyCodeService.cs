using System.Drawing;

namespace Core.IContract.Service
{
    /// <summary>
    /// 定义验证码处理功能
    /// </summary>
    public interface IVerifyCodeService
    {
        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="id">验证码编号</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        bool CheckCode(string code, string id, bool removeIfSuccess = true);

        /// <summary>
        /// 设置验证码到缓存中
        /// </summary>
        void SetCode(string code, out string id);

        /// <summary>
        /// 将图片序列化成字符串
        /// </summary>
        string GetImageString(Image image, string id);
    }
}
