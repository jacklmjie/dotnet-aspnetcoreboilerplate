using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace DynamicPlugins.Infrastructure
{
    /// <summary>
    /// net core 里的DefaultActionDescriptorCollectionProvider控制器的装载
    /// 实现控制器的重新装载
    /// </summary>
    public class ActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        public static ActionDescriptorChangeProvider Instance { get; } = new ActionDescriptorChangeProvider();

        public CancellationTokenSource TokenSource { get; private set; }

        public bool HasChanged { get; set; }

        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
    }
}
