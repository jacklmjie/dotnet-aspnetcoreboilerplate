using Microsoft.AspNetCore.Mvc;


namespace DemoPlugin1.Controllers
{
    [Area("DemoPlugin3")]
    public class Plugin3Controller : Controller
    {
        public IActionResult HelloWorld()
        {
            return View();
        }
    }
}
