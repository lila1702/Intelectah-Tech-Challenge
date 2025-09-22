using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.Services;

namespace CarDealershipManager.App.Controllers
{
    [Authorize]
    public class VendaController : Controller
    {
        private readonly IVendaService _vendaService;
        private readonly IFabricanteService _fabricanteService;
        private readonly IConcessionariaService _concessionariaService;

        public VendaController(
            IVendaService vendaService,
            IFabricanteService fabricanteService,
            IConcessionariaService concessionariaService)
        {
            _vendaService = vendaService;
            _fabricanteService = fabricanteService;
            _concessionariaService = concessionariaService;
        }

        public async Task<IActionResult> Index()
        {
            var vendas = await _vendaService.GetAllAsync();
            return View(vendas);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var venda = await _vendaService.GetByIdAsync(id);
                return View(venda);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrador, Gerente, Vendedor")]
        public async Task<IActionResult> Create()
        {
            await CarregarDadosFormulario();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Gerente, Vendedor")]
        public async Task<IActionResult> Create(VendaCreateDTO vendaDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var venda = await _vendaService.CreateAsync(vendaDTO);
                    TempData["Success"] = $"Venda realizada com sucesso! Protocolo: {venda.ProtocoloVenda}";
                    return RedirectToAction(nameof(Details), new { id = venda.Id });
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await CarregarDadosFormulario();
            return View(vendaDTO);
        }

        private async Task CarregarDadosFormulario()
        {
            var fabricantes = await _fabricanteService.GetAllAsync();
            var concessionarias = await _concessionariaService.GetAllAsync();

            ViewBag.Fabricantes = new SelectList(fabricantes, "Id", "Nome");
            ViewBag.Concessionarias = new SelectList(concessionarias, "Id", "Nome");
        }
    }
}
