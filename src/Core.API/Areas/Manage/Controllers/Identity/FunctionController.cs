using Core.Common.Extensions;
using Core.Models.Enum.Identity;
using Core.Models.Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Core.API.Areas.Manage.Controllers.Identity
{
    /// <summary>
    /// 管理-功能
    /// </summary>
    [Description("管理-功能")]
    public class FunctionController : ManageApiController
    {
        private readonly IActionDescriptorCollectionProvider _actionProvider;
        public FunctionController(IActionDescriptorCollectionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// 从程序集初始化功能信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("initialize")]
        [Description("从程序集初始化功能信息")]
        public List<IdentityFunction> Initialize()
        {
            var functions = new List<IdentityFunction>();
            var actionDescs = _actionProvider.ActionDescriptors.Items.Cast<ControllerActionDescriptor>();
            foreach (var item in actionDescs)
            {
                IdentityFunction action = GetFunction(item);
                if (IsRepeatMethod(action, functions))
                {
                    //此url重复
                    return null;
                }
                functions.Add(action);
            }
            return functions;
        }

        /// <summary>
        /// 获取功能信息
        /// </summary>
        /// <param name="actionDesc"></param>
        /// <returns></returns>
        private IdentityFunction GetFunction(ControllerActionDescriptor actionDesc)
        {
            EnumFunctionAccessType accessType = EnumFunctionAccessType.Anonymous;
            var ControllerName = actionDesc.ControllerTypeInfo.GetDescription();
            var ActionName = actionDesc.MethodInfo.CustomAttributes.FirstOrDefault(z => z.AttributeType == typeof(DescriptionAttribute))?.ConstructorArguments.FirstOrDefault().Value;
            var methods = string.Join(", ", actionDesc.ActionConstraints?.OfType<HttpMethodActionConstraint>().SingleOrDefault()?.HttpMethods ?? new string[] { "any" });
            IdentityFunction function = new IdentityFunction()
            {
                Name = $"{ControllerName}-{ActionName}",
                AccessType = accessType,
                Area = GetArea(actionDesc.ControllerTypeInfo),
                Controller = actionDesc.ControllerName,
                Action = actionDesc.ActionName,
                Methods = methods,
                Url = actionDesc.AttributeRouteInfo?.Template.ToLower()
            };
            return function;
        }

        /// <summary>
        /// 获取功能的区域信息
        /// </summary>
        private static string GetArea(Type type)
        {
            AreaAttribute attribute = type.GetAttribute<AreaAttribute>();
            return attribute?.RouteValue;
        }

        /// <summary>
        /// 是否存在重复的功能信息
        /// </summary>
        /// <param name="action">要判断的功能信息</param>
        /// <param name="functions">已存在的功能信息集合</param>
        /// <returns></returns>
        private bool IsRepeatMethod(IdentityFunction action, List<IdentityFunction> functions)
        {
            var exist = functions.FirstOrDefault(x => x.Url == action.Url);
            return exist != null;
        }
    }
}