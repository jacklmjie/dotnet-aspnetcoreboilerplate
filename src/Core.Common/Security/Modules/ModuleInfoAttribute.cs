using System;

namespace Core.Common.Security.Modules
{
    /// <summary>
    /// 描述把当前功能(Controller或者Action)封装为一个模块(Module)节点
    /// 此特性用于系统自动提取模块树信息Module
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModuleInfoAttribute : Attribute
    {
        /// <summary>
        /// 模块树信息Module
        /// </summary>
        /// <param name="description">描述</param>
        /// <param name="action">方法名</param>
        public ModuleInfoAttribute(string description, string action = "")
        {
            this.Description = description;
            this.Action = action;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 控制器
        /// 默认取当前所在控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 方法名
        /// 默认取当前Action名
        /// </summary>
        public string Action { get; set; }
    }
}
