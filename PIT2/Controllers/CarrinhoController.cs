using Microsoft.AspNetCore.Mvc;
using PIT2.Models;
using PIT2.Service;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PIT2.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly CupomService _cupomService;
        private readonly PedidoService _pedidoService;
        public CarrinhoController(CupomService cupomService, PedidoService pedidoService)
        {
            _cupomService = cupomService;
            _pedidoService = pedidoService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDesconto(string cupom)
        {
            return Ok(await _cupomService.GetDesconto(cupom));
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarPedido([FromBody] Carrinho carrinho)
        {
            if (_pedidoService.FinalizarPedido(carrinho, out var pedido))
            {
                return Json(new { sucesso = true, pedido });
            }

            return BadRequest();
        }
    }
}
