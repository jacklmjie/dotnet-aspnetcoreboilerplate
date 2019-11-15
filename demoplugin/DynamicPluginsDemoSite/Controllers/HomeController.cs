using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DynamicPluginsDemoSite.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;
using DynamicPluginsDemoSite.Provider;
using System;

namespace DynamicPluginsDemoSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationPartManager _partManager;
        public HomeController(ApplicationPartManager applicationPart)
        {
            _partManager = applicationPart;
        }

        public IActionResult Enable()
        {
            //Modules需要找的到views
            var assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "Modules\\DemoPlugin2\\DemoPlugin2.dll");

            var controllerAssemblyPart = new AssemblyPart(assembly);
            _partManager.ApplicationParts.Add(controllerAssemblyPart);

            MyActionDescriptorChangeProvider.Instance.HasChanged = true;
            MyActionDescriptorChangeProvider.Instance.TokenSource.Cancel();

            return Content("Enabled");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
