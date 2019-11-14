using Microsoft.AspNetCore.Mvc;


namespace DemoPlugin1.Controllers
{
    public class Plugin1Controller : Controller
    {
        public IActionResult HelloWorld()
        {
            return View();
        }
    }
}
