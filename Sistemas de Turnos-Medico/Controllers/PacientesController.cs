using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Sistemas_de_Turnos_Medico.Data;
using Sistemas_de_Turnos_Medico.Models;
using Sistemas_de_Turnos_Medico.ModelView;

namespace Sistemas_de_Turnos_Medico.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PacientesController(ApplicationDbContext context, IWebHostEnvironment env)
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

                    List<Paciente> pacientesArch = new List<Paciente>();

                    for (int row = 1; row <= rowCount; row++)
                    {
                        int salida;
                        if (row != 1)
                        {
                            Paciente pacienteTemporal = new Paciente()
                            {
                                Nombre = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                                Apellido = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                                DNI = int.TryParse(worksheet.Cells[row, 3].Value?.ToString().Trim(), out salida) ? salida : 0,
                                Celular = worksheet.Cells[row, 4].Value?.ToString().Trim(),
                            };
                            pacientesArch.Add(pacienteTemporal);
                        }
                    }
                    if (pacientesArch.Count > 0)
                    {
                        _context.Pacientes.AddRange(pacientesArch);
                        _context.SaveChanges();

                        ViewBag.resultado = "Se subio archivo";
                    }
                    else
                        ViewBag.resultado = "Error en el formato de archivo";
                }
            }
            else
                ViewBag.resultado = "Error en el archivo enviado";

            var applicationDbContext = _context.Pacientes;
            return View("Index", await applicationDbContext.ToListAsync());
        }


        // GET: Pacientes
        public async Task<IActionResult> Index(string? busqNombre, string? busqApellido, int? busqDNI, string ordenActual, string filtroActual, int pagina = 1)
        {
            var pacientes = from paciente in _context.Pacientes select paciente;

            Paginador paginas = new Paginador();
            paginas.PaginaActual = pagina;
            paginas.RegistrosPorPagina = 3;

            //busquedas
            if (!String.IsNullOrEmpty(busqNombre))
            {
                pacientes = pacientes.Where(p => p.Nombre.Contains(busqNombre));
                paginas.ValoresQueryString.Add("busqNombre", busqNombre);
            }

            if (!String.IsNullOrEmpty(busqApellido))
            {
                pacientes = pacientes.Where(p => p.Nombre.Contains(busqApellido));
                paginas.ValoresQueryString.Add("busqApellido", busqApellido);
            }

            if (busqDNI != null && busqDNI > 0)
            {
                pacientes = pacientes.Where(p => p.DNI.Equals(busqDNI));
                paginas.ValoresQueryString.Add("busqDNI", busqDNI.ToString());
            }

            //ordenamiento de columnas
            ViewData["OrdenActual"] = ordenActual;
            ViewData["FiltroActual"] = filtroActual;

            ViewData["FiltroNombre"] = String.IsNullOrEmpty(ordenActual) ? "NombreDescendente" : "";
            ViewData["FiltroApellido"] = ordenActual == "ApellidoAscendente" ? "ApellidoDescendente" : "ApellidoAscendente";
            ViewData["FiltroDNI"] = ordenActual == "DNIAscendente" ? "DNIDescendente" : "DNIAscendente";

            pacientes = ordenActual switch
            {
                "NombreDescendente" => pacientes.OrderByDescending(p => p.Nombre),
                "ApellidoDescendente" => pacientes.OrderByDescending(p => p.Apellido),
                "ApellidoAscendente" => pacientes.OrderBy(p => p.Apellido),
                "DNIDescendente" => pacientes.OrderByDescending(p => p.DNI),
                "DNIAscendente" => pacientes.OrderBy(p => p.DNI),
                _ => pacientes.OrderBy(p => p.Nombre),
            };

            //VM - paginacion
            paginas.TotalRegistros = pacientes.Count();

            var registrosMostrar = pacientes
                        .Skip((pagina - 1) * paginas.RegistrosPorPagina)
                        .Take(paginas.RegistrosPorPagina);

            PacienteVM datos = new PacienteVM()
            {
                pacientes = registrosMostrar.ToList(),
                busqNombre = busqNombre,
                busqApellido = busqApellido,
                busqDNI = busqDNI,
                paginador = paginas
            };

            return View(datos);
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,DNI,Celular,Foto")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                paciente.Foto = cargarFoto("");

                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,DNI,Celular,Foto")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string nuevaFoto =
                    cargarFoto(string.IsNullOrEmpty(paciente.Foto) ? "" : paciente.Foto);

                    if (!string.IsNullOrEmpty(nuevaFoto))
                        paciente.Foto = nuevaFoto;

                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
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
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pacientes'  is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
          return (_context.Pacientes?.Any(e => e.Id == id)).GetValueOrDefault();
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
