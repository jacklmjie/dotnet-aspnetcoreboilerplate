using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Core.API
{
    /// <summary>
    /// Web API 约定
    /// https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/conventions?view=aspnetcore-2.2#create-web-api-conventions
    /// </summary>
    public static class MyAppConventions
    {
        /// <summary>
        /// Get约定 HttpGet
        /// 命名规范Get+方法名
        /// </summary>
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public static void Get()
        { }

        /// <summary>
        /// POST约定 HttpPost
        /// 命名规范Post+方法名
        /// </summary>
        /// <param name="dto"></param>
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public static void Post([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object dto)
        { }

        /// <summary>
        /// Check约定 HttpGet
        /// 命名规范Check+方法名
        /// </summary>
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(200)]
        public static void Check()
        { }

        /// <summary>
        /// 增约定 HttpPost
        /// 命名规范Create+方法名
        /// </summary>
        /// <param name="dto"></param>
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public static void Create([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object dto)
        { }

        /// <summary>
        /// 删约定 HttpDelete
        /// 命名规范Delete+方法名，参数匹配后缀为id
        /// </summary>
        /// <param name="id"></param>
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public static void Delete([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id)
        { }

        /// <summary>
        /// 改约定 HttpPatch(部分更新) HttpPut(全部更新)
        /// 命名规范Edit+方法名
        /// </summary>
        /// <param name="dto"></param>
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public static void Edit([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object dto)
        { }
    }
}
