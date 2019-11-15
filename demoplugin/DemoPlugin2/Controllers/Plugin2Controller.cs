using Microsoft.AspNetCore.Mvc;


namespace DemoPlugin1.Controllers
{
    [Area("DemoPlugin2")]
    public class Plugin2Controller : Controller
    {
        public IActionResult HelloWorld()
        {
            return View();
        }
    }
}
