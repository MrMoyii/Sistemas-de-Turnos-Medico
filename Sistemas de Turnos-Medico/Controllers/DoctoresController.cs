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
    public class DoctoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DoctoresController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

                    List<Doctor> doctoressArch = new List<Doctor>();

                    for (int row = 1; row <= rowCount; row++)
                    {
                        int salida;

                        string idEspecializacion = worksheet.Cells[row, 4].Value?.ToString().Trim();

                        int especializacion = (int.TryParse(idEspecializacion, out salida) ? salida : 0);
                        if (especializacion > 0 && _context.Especializaciones.Where(c => c.Id == especializacion).FirstOrDefault() != null)
                        {
                            Doctor doctorTemporal = new Doctor()
                            {
                                EspecializacionId = especializacion,
                                Nombre = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                                Apellido = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                                Celular = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                            };
                            doctoressArch.Add(doctorTemporal);
                        }
                    }
                    if (doctoressArch.Count > 0)
                    {
                        _context.Doctores.AddRange(doctoressArch);
                        _context.SaveChanges();

                        ViewBag.resultado = "Se subio archivo";
                    }
                    else
                        ViewBag.resultado = "Error en el formato de archivo";
                }
            }
            else
                ViewBag.resultado = "Error en el archivo enviado";

            var applicationDbContext = _context.Doctores.Include(a => a.Especializacion);
            return View("Index", await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Doctores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Doctores.Include(d => d.Especializacion);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
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
                doctor.Foto = cargarFoto("");

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
                    string nuevaFoto =
                        cargarFoto(string.IsNullOrEmpty(doctor.Foto) ? "" : doctor.Foto);

                    if (!string.IsNullOrEmpty(nuevaFoto))
                        doctor.Foto = nuevaFoto;

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

        private string cargarFoto(string fotoAnterior)
        {
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivoFoto = archivos[0];
                if (archivoFoto.Length > 0)
                {
                    var pathDestino = Path.Combine(_env.WebRootPath, "fotos");

                    fotoAnterior = Path.Combine(pathDestino, fotoAnterior);
                    if (System.IO.File.Exists(fotoAnterior))
                        System.IO.File.Delete(fotoAnterior);

                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "");
                    archivoDestino += Path.GetExtension(archivoFoto.FileName);

                    using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                    {
                        archivoFoto.CopyTo(filestream);
                        return archivoDestino;
                    };
                }
            }
            return "";
        }
    }
}
