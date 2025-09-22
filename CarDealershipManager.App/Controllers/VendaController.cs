using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class VendaController : Controller
{
    private readonly IVendaService _vendaService;
    private readonly IVeiculoService _veiculoService;
    private readonly IConcessionariaService _concessionariaService;
    private readonly ILogger<VendaController> _logger;

    public VendaController(
        IVendaService vendaService,
        IVeiculoService veiculoService,
        IConcessionariaService concessionariaService,
        ILogger<VendaController> logger)
    {
        _vendaService = vendaService;
        _veiculoService = veiculoService;
        _concessionariaService = concessionariaService;
        _logger = logger;
    }

    // GET: Venda
    [Authorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            var vendas = await _vendaService.GetAllAsync();
            return View(vendas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar lista de vendas");
            TempData["Error"] = "Erro ao carregar as vendas. Tente novamente.";
            return View(new List<VendaDTO>());
        }
    }

    // GET: Venda/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            return View(venda);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar detalhes da venda {VendaId}", id);
            TempData["Error"] = "Erro ao carregar os detalhes da venda.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Venda/Create
    [Authorize(Roles = "Vendedor")]
    public async Task<IActionResult> Create()
    {
        try
        {
            await PopulateViewDataAsync();
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar página de nova venda");
            TempData["Error"] = "Erro ao carregar o formulário. Tente novamente.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Venda/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Vendedor")]
    public async Task<IActionResult> Create(VendaCreateDTO vendaDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewDataAsync(vendaDTO);
                return View(vendaDTO);
            }

            await _vendaService.CreateAsync(vendaDTO);
            TempData["Success"] = "Venda realizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            // Erros de validação de negócio
            _logger.LogWarning(ex, "Erro de validação ao criar venda: {Message}", ex.Message);
            ModelState.AddModelError("", ex.Message);
            await PopulateViewDataAsync(vendaDTO);
            return View(vendaDTO);
        }
        catch (InvalidOperationException ex)
        {
            // Erros de operação inválida
            _logger.LogWarning(ex, "Operação inválida ao criar venda: {Message}", ex.Message);
            ModelState.AddModelError("", ex.Message);
            await PopulateViewDataAsync(vendaDTO);
            return View(vendaDTO);
        }
        catch (Exception ex)
        {
            // Outros erros inesperados
            _logger.LogError(ex, "Erro inesperado ao criar venda");
            ModelState.AddModelError("", "Ocorreu um erro inesperado. Tente novamente.");
            await PopulateViewDataAsync(vendaDTO);
            return View(vendaDTO);
        }
    }

    // GET: /Venda/GetPrecoVeiculo?id=1
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPrecoVeiculo(int id)
    {
        try
        {
            var veiculo = await _veiculoService.GetByIdAsync(id);
            if (veiculo == null)
            {
                return Json(new { success = false, message = "Veículo não encontrado" });
            }

            return Json(new { success = true, preco = veiculo.Preco });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar preço do veículo {VeiculoId}", id);
            return Json(new { success = false, message = "Erro ao buscar preço do veículo" });
        }
    }

    private async Task PopulateViewDataAsync(VendaCreateDTO vendaDTO = null)
    {
        ViewData["VeiculoId"] = new SelectList(
            await _veiculoService.GetAllAsync(),
            "Id",
            "Modelo",
            vendaDTO?.VeiculoId);

        ViewData["ConcessionariaId"] = new SelectList(
            await _concessionariaService.GetAllAsync(),
            "Id",
            "Nome",
            vendaDTO?.ConcessionariaId);
    }
}