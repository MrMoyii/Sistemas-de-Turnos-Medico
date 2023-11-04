using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemas_de_Turnos_Medico.Data;
using Sistemas_de_Turnos_Medico.Models;

namespace Sistemas_de_Turnos_Medico.Controllers
{
    public class EstadoCitasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoCitasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoCitas
        public async Task<IActionResult> Index()
        {
              return _context.Estados != null ? 
                          View(await _context.Estados.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Estados'  is null.");
        }

        // GET: EstadoCitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Estados == null)
            {
                return NotFound();
            }

            var estadoCita = await _context.Estados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoCita == null)
            {
                return NotFound();
            }

            return View(estadoCita);
        }

        // GET: EstadoCitas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoCitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] EstadoCita estadoCita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoCita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoCita);
        }

        // GET: EstadoCitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estados == null)
            {
                return NotFound();
            }

            var estadoCita = await _context.Estados.FindAsync(id);
            if (estadoCita == null)
            {
                return NotFound();
            }
            return View(estadoCita);
        }

        // POST: EstadoCitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] EstadoCita estadoCita)
        {
            if (id != estadoCita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoCita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoCitaExists(estadoCita.Id))
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
            return View(estadoCita);
        }

        // GET: EstadoCitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estados == null)
            {
                return NotFound();
            }

            var estadoCita = await _context.Estados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoCita == null)
            {
                return NotFound();
            }

            return View(estadoCita);
        }

        // POST: EstadoCitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estados == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Estados'  is null.");
            }
            var estadoCita = await _context.Estados.FindAsync(id);
            if (estadoCita != null)
            {
                _context.Estados.Remove(estadoCita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoCitaExists(int id)
        {
          return (_context.Estados?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
