using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Sistemas_de_Turnos_Medico.Data;
using Sistemas_de_Turnos_Medico.Models;

namespace Sistemas_de_Turnos_Medico.Controllers
{
    [Authorize]
    public class EspecializacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EspecializacionesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Importar(IFormFile archivoExcel)
        {
            if (archivoExcel != null && archivoExcel.Length > 0)
            {
                using (var package = new ExcelPackage(archivoExcel.OpenReadStream()))
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.FirstOrDefault();

                    int rowCount = worksheet.Dimension.End.Row;     //get row count

                    List<Especializacion> especializacionesArch = new List<Especializacion>();

                    for (int row = 1; row <= rowCount; row++)
                    {
                        if(row != 1) {
                            Especializacion especializacionTemporal = new Especializacion()
                            {
                                Nombre = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                                Descripcion = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                            };
                            especializacionesArch.Add(especializacionTemporal);
                        }
                    }
                    if (especializacionesArch.Count > 0)
                    {
                        _context.Especializaciones.AddRange(especializacionesArch);
                        _context.SaveChanges();

                        ViewBag.resultado = "Se subio archivo";
                    }
                    else
                        ViewBag.resultado = "Error en el formato de archivo";
                }
            }
            else
                ViewBag.resultado = "Error en el archivo enviado";

            var applicationDbContext = _context.Especializaciones;
            return View("Index", await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Especializaciones
        public async Task<IActionResult> Index()
        {
              return _context.Especializaciones != null ? 
                          View(await _context.Especializaciones.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Especializaciones'  is null.");
        }

        [AllowAnonymous]
        // GET: Especializaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Especializaciones == null)
            {
                return NotFound();
            }

            var especializacion = await _context.Especializaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (especializacion == null)
            {
                return NotFound();
            }

            return View(especializacion);
        }

        // GET: Especializaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especializaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion")] Especializacion especializacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(especializacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especializacion);
        }

        // GET: Especializaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Especializaciones == null)
            {
                return NotFound();
            }

            var especializacion = await _context.Especializaciones.FindAsync(id);
            if (especializacion == null)
            {
                return NotFound();
            }
            return View(especializacion);
        }

        // POST: Especializaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Especializacion especializacion)
        {
            if (id != especializacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(especializacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecializacionExists(especializacion.Id))
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
            return View(especializacion);
        }

        // GET: Especializaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Especializaciones == null)
            {
                return NotFound();
            }

            var especializacion = await _context.Especializaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (especializacion == null)
            {
                return NotFound();
            }

            return View(especializacion);
        }

        // POST: Especializaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Especializaciones == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Especializaciones'  is null.");
            }
            var especializacion = await _context.Especializaciones.FindAsync(id);
            if (especializacion != null)
            {
                _context.Especializaciones.Remove(especializacion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspecializacionExists(int id)
        {
          return (_context.Especializaciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
