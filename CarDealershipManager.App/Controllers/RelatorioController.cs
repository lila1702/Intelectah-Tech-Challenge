using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarDealershipManager.Core.Interfaces.Services;

namespace CarDealershipManager.App.Controllers
{
    [Authorize(Roles = "Administrador, Gerente")]
    public class RelatorioController : Controller
    {
        private readonly IVendaService _vendaService;

        public RelatorioController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [Authorize(Roles = "Gerente")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> VendasPorPeriodo(int mes, int ano)
        {
            var inicio = new DateTime(ano, mes, 1);
            var fim = inicio.AddMonths(1).AddDays(-1);

            var vendas = await _vendaService.GetByPeriodAsync(inicio, fim);

            var relatorio = new
            {
                Mes = mes,
                Ano = ano,
                TotalVendas = vendas.Count(),
                ValorTotal = vendas.Sum(v => v.PrecoVenda),
                VendasPorTipo = vendas.GroupBy(v => v.TipoVeiculo)
                 .Select(g => new { Tipo = g.Key.ToString(), Quantidade = g.Count(), Valor = g.Sum(x => x.PrecoVenda) }),
                VendasPorFabricante = vendas.GroupBy(v => v.FabricanteNome)
                 .Select(g => new { Fabricante = g.Key, Quantidade = g.Count(), Valor = g.Sum(x => x.PrecoVenda) }),
                VendasPorConcessionaria = vendas.GroupBy(v => v.ConcessionariaNome)
                 .Select(g => new { Concessionaria = g.Key, Quantidade = g.Count(), Valor = g.Sum(x => x.PrecoVenda) })
            };

            return Json(relatorio);
        }
    }
}
