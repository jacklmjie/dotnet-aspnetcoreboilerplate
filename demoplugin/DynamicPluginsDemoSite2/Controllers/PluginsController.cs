using DynamicPlugins.Data;
using DynamicPlugins.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using DynamicPlugins.Extensions;

namespace DynamicPluginsDemoSite.Controllers
{
    public class PluginsController : Controller
    {
        private IPluginManager _pluginManager = null;
        private IReferenceContainer _referenceContainer = null;

        public PluginsController(IPluginManager pluginManager,
            IReferenceContainer referenceContainer)
        {
            _pluginManager = pluginManager;
            _referenceContainer = referenceContainer;
        }

        public IActionResult Assemblies()
        {
            var items = _referenceContainer.GetAll();
            return View(items);
        }

        public IActionResult Index()
        {
            return View(_pluginManager.GetAllPlugins());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload()
        {
            var package = new PluginPackage(Request.GetPluginStream());
            _pluginManager.AddPlugins(package);
            return RedirectToAction("Index");
        }

        public IActionResult Enable(Guid id)
        {
            _pluginManager.EnablePlugin(id);
            return RedirectToAction("Index");
        }

        public IActionResult Disable(Guid id)
        {
            _pluginManager.DisablePlugin(id);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            _pluginManager.DeletePlugin(id);

            return RedirectToAction("Index");
        }
    }
}
