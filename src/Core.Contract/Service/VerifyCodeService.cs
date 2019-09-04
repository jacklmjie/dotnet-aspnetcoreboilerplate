using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Core.Common.Data;
using Core.Common.Extensions;
using Core.IContract.Service;
using EasyCaching.Core;

namespace Core.Contract.Service
{
    /// <summary>
    /// 验证码处理服务
    /// </summary>
    public class VerifyCodeService : IVerifyCodeService
    {
        private const string Separator = "#$#";
        private readonly IRedisCachingProvider _cache;

        /// <summary>
        /// 初始化一个<see cref="VerifyCodeService"/>类型的新实例
        /// </summary>
        public VerifyCodeService(IEasyCachingProviderFactory factory)
        {
            _cache = factory.GetRedisProvider("redis2");
        }

        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="id">验证码编号</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        public bool CheckCode(string code, string id, bool removeIfSuccess = true)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            string key = $"{CoreConstants.VerifyCodeKeyPrefix}_{id}";
            bool flag = code.Equals(_cache.StringGet(key), StringComparison.OrdinalIgnoreCase);
            if (removeIfSuccess && flag)
            {
                _cache.KeyDel(key);
            }

            return flag;
        }

        /// <summary>
        /// 设置验证码到缓存中
        /// </summary>
        public void SetCode(string code, out string id)
        {
            id = Guid.NewGuid().ToString("N");
            string key = $"{CoreConstants.VerifyCodeKeyPrefix}_{id}";
            const int seconds = 60 * 3;
            _cache.StringSet(key, code, TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// 将图片序列化成字符串
        /// </summary>
        public string GetImageString(Image image, string id)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                string str = $"data:image/png;base64,{bytes.ToBase64String()}{Separator}{id}";
                return str.ToBase64String();
            }
        }
    }
}
