using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MuseuAMSG3.InfraStructure.Data.Context;
using MuseuAMSG3.Models;

namespace MuseuAMSG3.Controllers
{
    public class CadastroController : Controller
    {
        private readonly ApplicationDataContext _context;

        public CadastroController(ApplicationDataContext context)
        {
            _context = context;
        }

        // GET: Cadastro
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cadastro.ToListAsync());
        }

        // GET: Cadastro/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cadastro = await _context.Cadastro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cadastro == null)
            {
                return NotFound();
            }

            return View(cadastro);
        }

        // GET: Cadastro/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cadastro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telefone,Senha")] Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                cadastro.Id = Guid.NewGuid();
                _context.Add(cadastro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cadastro);
        }

        // GET: Cadastro/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cadastro = await _context.Cadastro.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound();
            }
            return View(cadastro);
        }

        // POST: Cadastro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Email,Telefone,Senha")] Cadastro cadastro)
        {
            if (id != cadastro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cadastro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CadastroExists(cadastro.Id))
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
            return View(cadastro);
        }

        // GET: Cadastro/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cadastro = await _context.Cadastro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cadastro == null)
            {
                return NotFound();
            }

            return View(cadastro);
        }

        // POST: Cadastro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cadastro = await _context.Cadastro.FindAsync(id);
            if (cadastro != null)
            {
                _context.Cadastro.Remove(cadastro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CadastroExists(Guid id)
        {
            return _context.Cadastro.Any(e => e.Id == id);
        }
    }
}
