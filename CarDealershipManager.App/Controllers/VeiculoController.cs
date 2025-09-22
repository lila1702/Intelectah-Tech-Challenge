using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace CarDealershipManager.App.Controllers
{
    [Authorize]
    public class VeiculoController : Controller
    {
        private readonly IVeiculoService _veiculoService;
        private readonly IFabricanteService _fabricanteService;

        public VeiculoController(IVeiculoService veiculoService, IFabricanteService fabricanteService)
        {
            _veiculoService = veiculoService;
            _fabricanteService = fabricanteService;
        }

        public async Task<IActionResult> Index()
        {
            var veiculos = await _veiculoService.GetAllAsync();
            return View(veiculos);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var veiculo = await _veiculoService.GetByIdAsync(id);
                return View(veiculo);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            await CarregarFabricantes();
            CarregarTiposVeiculos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create(VeiculoCreateDTO veiculoDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _veiculoService.CreateAsync(veiculoDTO);
                    TempData["Success"] = "Veículo criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await CarregarFabricantes();
            CarregarTiposVeiculos();
            return View(veiculoDTO);
        }

        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var veiculo = await _veiculoService.GetByIdAsync(id);
                var veiculoDTO = new VeiculoUpdateDTO
                {
                    Modelo = veiculo.Modelo,
                    AnoFabricacao = veiculo.AnoFabricacao,
                    Preco = veiculo.Preco,
                    FabricanteId = veiculo.FabricanteId,
                    TipoVeiculo = veiculo.TipoVeiculo,
                    Descricao = veiculo.Descricao
                };

                await CarregarFabricantes();
                CarregarTiposVeiculos();
                return View(veiculoDTO);

            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id, VeiculoUpdateDTO veiculoDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _veiculoService.UpdateAsync(id, veiculoDTO);
                    TempData["Success"] = "Veículo atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await CarregarFabricantes();
            CarregarTiposVeiculos();
            return View(veiculoDTO);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var veiculo = await _veiculoService.GetByIdAsync(id);
                return View(veiculo);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _veiculoService.DeleteAsync(id);
            TempData["Success"] = "Veículo excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetVeiculosByFabricante(int fabricanteId)
        {
            var veiculos = await _veiculoService.GetByFabricanteIdAsync(fabricanteId);
            return Json(veiculos.Select(v => new { value = v.Id, text = $"{v.Modelo} - {v.Preco.ToString("C", new CultureInfo("pt-BR"))}" }));
        }

        private async Task CarregarFabricantes()
        {
            var fabricantes = await _fabricanteService.GetAllAsync();
            ViewBag.Fabricantes = new SelectList(fabricantes, "Id", "Nome");
        }

        private void CarregarTiposVeiculos()
        {
            ViewBag.TiposVeiculos = new SelectList(Enum.GetValues(typeof(TipoVeiculo))
                                                       .Cast<TipoVeiculo>()
                                                       .Select(v => new {Value = (int)v, Text = v.ToString()}),
                                                       "Value", "Text");
        }

    }
}
