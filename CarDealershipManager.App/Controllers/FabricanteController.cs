using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.Services;

namespace CarDealershipManager.App.Controllers
{
    [Authorize]
    public class FabricanteController : Controller
    {
        private readonly IFabricanteService _fabricanteService;

        public FabricanteController(IFabricanteService fabricanteService)
        {
            _fabricanteService = fabricanteService;
        }

        public async Task<IActionResult> Index()
        {
            var fabricantes = await _fabricanteService.GetAllAsync();
            return View(fabricantes);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var fabricantes = await _fabricanteService.GetByIdAsync(id);
                return View(fabricantes);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(FabricanteCreateDTO fabricanteDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _fabricanteService.CreateAsync(fabricanteDTO);
                    TempData["Sucess"] = "Fabricante criado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(fabricanteDTO);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var fabricante = await _fabricanteService.GetByIdAsync(id);
                var fabricanteDTO = new FabricanteUpdateDTO
                {
                    Nome = fabricante.Nome,
                    PaisOrigem = fabricante.PaisOrigem,
                    AnoFundacao = fabricante.AnoFundacao,
                    Website = fabricante.Website
                };
                return View(fabricante);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, FabricanteUpdateDTO fabricanteDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _fabricanteService.UpdateAsync(id, fabricanteDTO);
                    TempData["Sucess"] = "Fabricante atualizado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(fabricanteDTO);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var fabricante = await _fabricanteService.GetByIdAsync(id);
                return View(fabricante);
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
            await _fabricanteService.DeleteAsync(id);
            TempData["Success"] = "Fabricante excluído com sucesso";
            return RedirectToAction(nameof(Index));
        }
    }
}
