using Microsoft.AspNetCore.Mvc;

namespace PIT2.Controllers
{
    public class ContatoController : Controller
    {
        [HttpPost]
        public IActionResult EnviarContato()
        {
            return View();
        }
    }
}
