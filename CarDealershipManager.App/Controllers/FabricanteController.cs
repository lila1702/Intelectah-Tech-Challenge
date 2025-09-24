using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManager.App.Controllers
{
    public class FabricanteController : Controller
    {
        private readonly IFabricanteService _fabricanteService;
        private readonly ILogger<FabricanteController> _logger;

        public FabricanteController(IFabricanteService fabricanteService, ILogger<FabricanteController> logger)
        {
            _fabricanteService = fabricanteService;
            _logger = logger;
        }

        [Authorize]
        // GET: Fabricante
        public async Task<IActionResult> Index()
        {
            try
            {
                var fabricantes = await _fabricanteService.GetAllAsync();
                return View(fabricantes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de fabricantes");
                TempData["Error"] = "Erro ao carregar fabricantes. Tente novamente.";
                return View(new List<FabricanteDTO>());
            }
        }

        [Authorize]
        // GET: Fabricante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var fabricante = await _fabricanteService.GetByIdAsync(id.Value);
                return View(fabricante);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes do fabricante ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar detalhes do fabricante.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Fabricante/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fabricante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(FabricanteCreateDTO fabricante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _fabricanteService.CreateAsync(fabricante);
                    TempData["Success"] = "Fabricante criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("Nome", ex.Message);
                    _logger.LogWarning("Tentativa de criar fabricante com nome duplicado: {Nome}", fabricante.Nome);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao criar fabricante. Tente novamente.";
                    _logger.LogError(ex, "Erro ao criar fabricante: {Nome}", fabricante.Nome);
                }
            }
            return View(fabricante);
        }

        // GET: Fabricante/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var fabricante = await _fabricanteService.GetByIdAsync(id.Value);

                // Converter FabricanteDTO para FabricanteUpdateDTO
                var fabricanteUpdateDTO = new FabricanteUpdateDTO
                {
                    Nome = fabricante.Nome,
                    PaisOrigem = fabricante.PaisOrigem,
                    AnoFundacao = fabricante.AnoFundacao,
                    Website = fabricante.Website
                };

                return View(fabricanteUpdateDTO);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fabricante para edição ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar fabricante para edição.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Fabricante/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, FabricanteUpdateDTO fabricante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _fabricanteService.UpdateAsync(id, fabricante);
                    TempData["Success"] = "Fabricante atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message.Contains("não encontrado"))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("Nome", ex.Message);
                        _logger.LogWarning("Tentativa de atualizar fabricante com nome duplicado: {Nome}", fabricante.Nome);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao atualizar fabricante. Tente novamente.";
                    _logger.LogError(ex, "Erro ao atualizar fabricante ID: {Id}", id);
                }
            }
            return View(fabricante);
        }

        // GET: Fabricante/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var fabricante = await _fabricanteService.GetByIdAsync(id.Value);
                return View(fabricante);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fabricante para exclusão ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar fabricante para exclusão.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Fabricante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _fabricanteService.DeleteAsync(id);
                TempData["Success"] = "Fabricante deletado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = "Fabricante não encontrado.";
                _logger.LogWarning("Tentativa de deletar fabricante inexistente ID: {Id}", id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao deletar fabricante. Verifique se não há veículos cadastrados para este fabricante.";
                _logger.LogError(ex, "Erro ao deletar fabricante ID: {Id}", id);
            }

            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para buscar fabricantes com veículos (se necessário)
        [Authorize]
        public async Task<IActionResult> WithVeiculos()
        {
            try
            {
                var fabricantes = await _fabricanteService.GetWithVeiculosAsync();
                return View("Index", fabricantes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar fabricantes com veículos");
                TempData["Error"] = "Erro ao carregar fabricantes com veículos.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}