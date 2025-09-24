using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.External;
using CarDealershipManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManager.App.Controllers
{
    public class ConcessionariaController : Controller
    {
        private readonly IConcessionariaService _concessionariaService;
        private readonly ILogger<ConcessionariaController> _logger;

        public ConcessionariaController(IConcessionariaService concessionariaService, ILogger<ConcessionariaController> logger)
        {
            _concessionariaService = concessionariaService;
            _logger = logger;
        }

        [Authorize]
        // GET: Concessionaria
        public async Task<IActionResult> Index()
        {
            try
            {
                var concessionarias = await _concessionariaService.GetAllAsync();
                return View(concessionarias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de concession�rias");
                TempData["Error"] = "Erro ao carregar concession�rias. Tente novamente.";
                return View(new List<ConcessionariaDTO>());
            }
        }

        [Authorize]
        // GET: Concessionaria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var concessionaria = await _concessionariaService.GetByIdAsync(id.Value);
                return View(concessionaria);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes da concession�ria ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar detalhes da concession�ria.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Concessionaria/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Concessionaria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(ConcessionariaCreateDTO concessionaria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _concessionariaService.CreateAsync(concessionaria);
                    TempData["Success"] = "Concession�ria criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("Nome", ex.Message);
                    _logger.LogWarning("Tentativa de criar concession�ria com nome duplicado: {Nome}", concessionaria.Nome);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao criar concession�ria. Tente novamente.";
                    _logger.LogError(ex, "Erro ao criar concession�ria: {Nome}", concessionaria.Nome);
                }
            }
            return View(concessionaria);
        }

        // GET: Concessionaria/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var concessionaria = await _concessionariaService.GetByIdAsync(id.Value);

                var concessionariaUpdateDTO = new ConcessionariaUpdateDTO
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

                return View(concessionariaUpdateDTO);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar concession�ria para edi��o ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar concession�ria para edi��o.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Concessionaria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, ConcessionariaUpdateDTO concessionaria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _concessionariaService.UpdateAsync(id, concessionaria);
                    TempData["Success"] = "Concession�ria atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message.Contains("n�o encontrada"))
                        return NotFound();

                    ModelState.AddModelError("Nome", ex.Message);
                    _logger.LogWarning("Tentativa de atualizar concession�ria com nome duplicado: {Nome}", concessionaria.Nome);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao atualizar concession�ria. Tente novamente.";
                    _logger.LogError(ex, "Erro ao atualizar concession�ria ID: {Id}", id);
                }
            }
            return View(concessionaria);
        }

        // GET: Concessionaria/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var concessionaria = await _concessionariaService.GetByIdAsync(id.Value);
                return View(concessionaria);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar concession�ria para exclus�o ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar concession�ria para exclus�o.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Concessionaria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _concessionariaService.DeleteAsync(id);
                TempData["Success"] = "Concession�ria deletada com sucesso!";
            }
            catch (ArgumentException)
            {
                TempData["Error"] = "Concession�ria n�o encontrada.";
                _logger.LogWarning("Tentativa de deletar concession�ria inexistente ID: {Id}", id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao deletar concession�ria.";
                _logger.LogError(ex, "Erro ao deletar concession�ria ID: {Id}", id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarEnderecoPorCEP(string cep, [FromServices] ICEPService cepService)
        {
            try
            {
                var endereco = await cepService.BuscarEnderecoPorCEPAsync(cep);
                if (endereco == null)
                    return NotFound(new { mensagem = "CEP n�o encontrado." });

                return Ok(endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar endere�o pelo CEP {Cep}", cep);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar CEP." });
            }
        }
    }
}