using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.Services;
using CarDealershipManager.Core.Interfaces.External;

namespace CarDealershipManager.App.Controllers
{
    [Authorize]
    public class ConcessionariaController : Controller
    {
        private readonly IConcessionariaService _concessionariaService;
        private readonly ICEPService _cepService;

        public ConcessionariaController(IConcessionariaService concessionariaService, ICEPService cepService)
        {
            _concessionariaService = concessionariaService;
            _cepService = cepService;
        }

        public async Task<IActionResult> Index()
        {
            var concessionarias = await _concessionariaService.GetAllAsync();
            return View(concessionarias);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var concessionaria = await _concessionariaService.GetByIdAsync(id);
                return View(concessionaria);
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
        public async Task<IActionResult> Create(ConcessionariaCreateDTO concessionariaDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _concessionariaService.CreateAsync(concessionariaDTO);
                    TempData["Success"] = "Concessionária criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(concessionariaDTO);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var concessionaria = await _concessionariaService.GetByIdAsync(id);
                var concessionariaDTO = new ConcessionariaUpdateDTO
                {
                    Nome = concessionaria.Nome,
                    Endereco = concessionaria.Endereco,
                    Cidade = concessionaria.Cidade,
                    Estado = concessionaria.Estado,
                    CEP = concessionaria.CEP,
                    Telefone = concessionaria.Telefone,
                    Email = concessionaria.Email,
                    CapacidadeMaximaVeiculos = concessionaria.CapacidadeMaximaVeiculos
                };
                return View(concessionariaDTO);

            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, ConcessionariaUpdateDTO concessionariaDTO)
        {
            try
            {
                await _concessionariaService.UpdateAsync(id, concessionariaDTO);
                TempData["Success"] = "Concessionária atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(concessionariaDTO);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var concessionaria = await _concessionariaService.GetByIdAsync(id);
                return View(concessionaria);
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
            await _concessionariaService.DeleteAsync(id);
            TempData["Success"] = "Concessionária excluida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarEnderecoPorCep(string cep)
        {
            var endereco = await _cepService.BuscarEnderecoPorCEPAsync(cep);
            return Json(endereco);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByNome(string nome)
        {
            var concessionarias = await _concessionariaService.GetByNomeAsync(nome);
            return Json(concessionarias.Select(c => new { value = c.Id, text = c.Nome }));
        }
    }
}
