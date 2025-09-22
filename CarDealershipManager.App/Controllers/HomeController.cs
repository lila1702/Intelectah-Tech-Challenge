using CarDealershipManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManager.App.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IVendaService _vendaService;
    private readonly IFabricanteService _fabricanteService;
    private readonly IVeiculoService _veiculoService;
    private readonly IConcessionariaService _concessionariaService;

    public HomeController(
        IVendaService vendaService,
        IFabricanteService fabricanteService,
        IVeiculoService veiculoService,
        IConcessionariaService concessionariaService)
    {
        _vendaService = vendaService;
        _fabricanteService = fabricanteService;
        _veiculoService = veiculoService;
        _concessionariaService = concessionariaService;
    }

    public async Task<IActionResult> Index()
    {
        var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var fimMes = inicioMes.AddMonths(1).AddDays(-1);

        var vendasMesAtual = await _vendaService.GetByPeriodAsync(inicioMes, fimMes);

        var dashboard = new
        {
            TotalVendasMes = vendasMesAtual.Count(),
            ValorTotalMes = vendasMesAtual.Sum(v => v.PrecoVenda),
            TotalFabricantes = (await _fabricanteService.GetAllAsync()).Count(),
            TotalVeiculos = (await _veiculoService.GetAllAsync()).Count(),
            TotalConcessionarias = (await _concessionariaService.GetAllAsync()).Count()
        };

        ViewBag.Dashboard = dashboard;
        return View(vendasMesAtual.Take(10));
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
