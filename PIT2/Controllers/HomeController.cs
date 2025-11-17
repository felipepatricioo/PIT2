using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PIT2.Models;
using PIT2.Service;

namespace PIT2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProdutoService _prdService;

        public HomeController(ILogger<HomeController> logger, ProdutoService prdService)
        {
            _logger = logger;
            _prdService = prdService;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _prdService.GetProdutosAsync();
            return View(produtos);
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult Carrinho()
        {
            return View();
        }

        [HttpGet("Home/Sucesso")]
        public IActionResult Sucesso()
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
