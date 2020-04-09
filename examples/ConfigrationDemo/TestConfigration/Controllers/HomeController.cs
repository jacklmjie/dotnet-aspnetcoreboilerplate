using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestConfigration.Models;

namespace TestConfigration.Controllers
{
    public class HomeController : Controller
    {
        public TestSubSectionConfig _subSectionConfig;
        public ILogger<HomeController> _logger;

        //IOptions是单例Singleton的,一旦程序启动，该选项的值就无法更改
        //IOptionsSnapshot是Scoped的,当开启一个新Scoped时，就会重新计算选项的值
        //IOptionsMonitor也是单例的,依赖于IChangeToken，只要令牌源变更则立刻做出反应
        public HomeController(IOptions<TestSubSectionConfig> option, ILogger<HomeController> logger)
        {
            _subSectionConfig = option.Value;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"SubOption1: {_subSectionConfig.SubOption1}");
            _logger.LogInformation($"SubOption2: {_subSectionConfig.SubOption2}");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
