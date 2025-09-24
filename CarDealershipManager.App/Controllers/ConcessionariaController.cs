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
                _logger.LogError(ex, "Erro ao carregar lista de concessionárias");
                TempData["Error"] = "Erro ao carregar concessionárias. Tente novamente.";
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
                _logger.LogError(ex, "Erro ao carregar detalhes da concessionária ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar detalhes da concessionária.";
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
                    TempData["Success"] = "Concessionária criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("Nome", ex.Message);
                    _logger.LogWarning("Tentativa de criar concessionária com nome duplicado: {Nome}", concessionaria.Nome);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao criar concessionária. Tente novamente.";
                    _logger.LogError(ex, "Erro ao criar concessionária: {Nome}", concessionaria.Nome);
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
                _logger.LogError(ex, "Erro ao carregar concessionária para edição ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar concessionária para edição.";
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
                    TempData["Success"] = "Concessionária atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message.Contains("não encontrada"))
                        return NotFound();

                    ModelState.AddModelError("Nome", ex.Message);
                    _logger.LogWarning("Tentativa de atualizar concessionária com nome duplicado: {Nome}", concessionaria.Nome);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erro ao atualizar concessionária. Tente novamente.";
                    _logger.LogError(ex, "Erro ao atualizar concessionária ID: {Id}", id);
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
                _logger.LogError(ex, "Erro ao carregar concessionária para exclusão ID: {Id}", id);
                TempData["Error"] = "Erro ao carregar concessionária para exclusão.";
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
                TempData["Success"] = "Concessionária deletada com sucesso!";
            }
            catch (ArgumentException)
            {
                TempData["Error"] = "Concessionária não encontrada.";
                _logger.LogWarning("Tentativa de deletar concessionária inexistente ID: {Id}", id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao deletar concessionária.";
                _logger.LogError(ex, "Erro ao deletar concessionária ID: {Id}", id);
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
                    return NotFound(new { mensagem = "CEP não encontrado." });

                return Ok(endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar endereço pelo CEP {Cep}", cep);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar CEP." });
            }
        }
    }
}