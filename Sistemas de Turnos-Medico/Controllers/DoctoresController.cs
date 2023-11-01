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
    public class DoctoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Doctores.Include(d => d.Especializacion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Doctores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Doctores == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctores
                .Include(d => d.Especializacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctores/Create
        public IActionResult Create()
        {
            ViewData["EspecializacionId"] = new SelectList(_context.Especializaciones, "Id", "Nombre");
            return View();
        }

        // POST: Doctores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Celular,Foto,EspecializacionId")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EspecializacionId"] = new SelectList(_context.Especializaciones, "Id", "Nombre", doctor.EspecializacionId);
            return View(doctor);
        }

        // GET: Doctores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctores == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctores.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["EspecializacionId"] = new SelectList(_context.Especializaciones, "Id", "Nombre", doctor.EspecializacionId);
            return View(doctor);
        }

        // POST: Doctores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Celular,Foto,EspecializacionId")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            ViewData["EspecializacionId"] = new SelectList(_context.Especializaciones, "Id", "Nombre", doctor.EspecializacionId);
            return View(doctor);
        }

        // GET: Doctores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctores == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctores
                .Include(d => d.Especializacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Doctores'  is null.");
            }
            var doctor = await _context.Doctores.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctores.Remove(doctor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
          return (_context.Doctores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
