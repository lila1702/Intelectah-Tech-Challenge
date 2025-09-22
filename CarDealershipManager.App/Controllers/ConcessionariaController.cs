using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace CarDealershipManager.App.Controllers
{
    public class ConcessionariaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConcessionariaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Concessionaria
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Concessionarias.ToListAsync());
        }

        // GET: Concessionaria/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concessionaria = await _context.Concessionarias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concessionaria == null)
            {
                return NotFound();
            }

            return View(concessionaria);
        }

        // GET: Concessionaria/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Concessionaria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Nome,Endereco,Cidade,Estado,CEP,Telefone,Email,CapacidadeMaximaVeiculos,Id")] Concessionaria concessionaria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concessionaria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concessionaria);
        }

        // GET: Concessionaria/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria == null)
            {
                return NotFound();
            }
            return View(concessionaria);
        }

        // POST: Concessionaria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Nome,Endereco,Cidade,Estado,CEP,Telefone,Email,CapacidadeMaximaVeiculos,Id")] Concessionaria concessionaria)
        {
            if (id != concessionaria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concessionaria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcessionariaExists(concessionaria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(concessionaria);
        }

        // GET: Concessionaria/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concessionaria = await _context.Concessionarias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concessionaria == null)
            {
                return NotFound();
            }

            return View(concessionaria);
        }

        // POST: Concessionaria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria != null)
            {
                _context.Concessionarias.Remove(concessionaria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcessionariaExists(int id)
        {
            return _context.Concessionarias.Any(e => e.Id == id);
        }
    }
}
