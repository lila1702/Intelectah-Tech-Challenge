using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Interfaces.Services;
using CarDealershipManager.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipManager.App.Controllers
{
    public class VeiculoController : Controller
    {
        private readonly IVeiculoService _veiculoService;
        private readonly IFabricanteService _fabricanteService;
        private readonly ILogger<VeiculoController> _logger;

        public VeiculoController(
            IVeiculoService veiculoService,
            IFabricanteService fabricanteService,
            ILogger<VeiculoController> logger)
        {
            _veiculoService = veiculoService;
            _fabricanteService = fabricanteService;
            _logger = logger;
        }

        // GET: Veiculo
        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var veiculos = await _veiculoService.GetAllAsync();
                return View(veiculos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de veículos");
                TempData["Error"] = "Erro ao carregar veículos. Tente novamente.";
                return View(new List<VeiculoDTO>());
            }
        }

        // GET: Veiculo/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var veiculo = await _veiculoService.GetByIdAsync(id.Value);
                return View(veiculo);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes do veículo ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar detalhes do veículo.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Veiculo/Create
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Create()
        {
            var fabricantes = await _fabricanteService.GetAllAsync();
            ViewData["FabricanteId"] = new SelectList(fabricantes, "Id", "Nome");
            return View();
        }

        // POST: Veiculo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Create(VeiculoCreateDTO veiculo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _veiculoService.CreateAsync(veiculo);
                    TempData["Success"] = "Veículo criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao criar veículo.";
                    _logger.LogError(ex, "Erro ao criar veículo: {Modelo}", veiculo.Modelo);
                }
            }

            var fabricantes = await _fabricanteService.GetAllAsync();
            ViewData["FabricanteId"] = new SelectList(fabricantes, "Id", "Nome", veiculo.FabricanteId);

            return View(veiculo);
        }

        // GET: Veiculo/Edit/5
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var veiculo = await _veiculoService.GetByIdAsync(id.Value);

                var veiculoUpdateDTO = new VeiculoUpdateDTO
                {
                    Modelo = veiculo.Modelo,
                    AnoFabricacao = veiculo.AnoFabricacao,
                    Preco = veiculo.Preco,
                    FabricanteId = veiculo.FabricanteId,
                    TipoVeiculo = veiculo.TipoVeiculo,
                    Descricao = veiculo.Descricao
                };

                var fabricantes = await _fabricanteService.GetAllAsync();
                ViewData["FabricanteId"] = new SelectList(fabricantes, "Id", "Nome", veiculoUpdateDTO.FabricanteId);

                return View(veiculoUpdateDTO);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar veículo para edição ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar veículo para edição.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Veiculo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Edit(int id, VeiculoUpdateDTO veiculo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _veiculoService.UpdateAsync(id, veiculo);
                    TempData["Success"] = "Veículo atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message.Contains("não encontrado"))
                        return NotFound();

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao atualizar veículo.";
                    _logger.LogError(ex, "Erro ao atualizar veículo ID: {Id}", id);
                }
            }

            var fabricantes = await _fabricanteService.GetAllAsync();
            ViewData["FabricanteId"] = new SelectList(fabricantes, "Id", "Nome", veiculo.FabricanteId);

            return View(veiculo);
        }

        // GET: Veiculo/Delete/5
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var veiculo = await _veiculoService.GetByIdAsync(id.Value);
                return View(veiculo);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar veículo para exclusão ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar veículo para exclusão.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Veiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _veiculoService.DeleteAsync(id);
                TempData["Success"] = "Veículo deletado com sucesso!";
            }
            catch (ArgumentException)
            {
                TempData["Error"] = "Veículo não encontrado.";
                _logger.LogWarning("Tentativa de deletar veículo inexistente ID: {Id}", id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao deletar veículo.";
                _logger.LogError(ex, "Erro ao deletar veículo ID: {Id}", id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> SearchFabricante(string term)
        {
            var fabricantes = await _fabricanteService.SearchFabricanteByNameAsync(term);

            var result = fabricantes.Select(f => new {
                id = f.Id,
                label = f.Nome,
                value = f.Nome
            });

            return Json(result);
        }
    }
}