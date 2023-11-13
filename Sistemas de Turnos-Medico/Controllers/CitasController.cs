using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sistemas_de_Turnos_Medico.Data;
using Sistemas_de_Turnos_Medico.Models;
using Sistemas_de_Turnos_Medico.ModelView;

namespace Sistemas_de_Turnos_Medico.Controllers
{
    [Authorize]
    public class CitasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Citas
        public async Task<IActionResult> Index(string? busqApellidoDoctor, string? busqApellidoPaciente, int? busqEstado, int pagina = 1)
        {
            Paginador paginas = new Paginador();
            paginas.PaginaActual = pagina;
            paginas.RegistrosPorPagina = 5;

            var applicationDbContext = _context.Citas.Include(c => c.Doctor).Include(c => c.Estado).Include(c => c.Paciente).Select(e => e);

            //busqueda Apellido Doctor
            if (!String.IsNullOrEmpty(busqApellidoDoctor))
            {
                applicationDbContext = applicationDbContext.Where(p => p.Doctor.Apellido.Contains(busqApellidoDoctor));
                paginas.ValoresQueryString.Add("busqApellidoDoctor", busqApellidoDoctor);
            }
            //busqueda Apellido Doctor
            if (!String.IsNullOrEmpty(busqApellidoPaciente))
            {
                applicationDbContext = applicationDbContext.Where(p => p.Paciente.Apellido.Contains(busqApellidoPaciente));
                paginas.ValoresQueryString.Add("busqApellidoPaciente", busqApellidoPaciente);
            }
            //busqueda Estado
            if (busqEstado != null)
            {
                applicationDbContext = applicationDbContext.Where(p => p.Estado.Id.Equals(busqEstado));
                paginas.ValoresQueryString.Add("busqEstado", busqEstado.ToString());
            }

            //VM - paginacion
            paginas.TotalRegistros = applicationDbContext.Count();

            var registrosMostrar = applicationDbContext
                        .Skip((pagina - 1) * paginas.RegistrosPorPagina)
                        .Take(paginas.RegistrosPorPagina);

            ViewData["Estado"] = new SelectList(_context.Estados, "Id", "Descripcion");

            CitaVM datos = new CitaVM()
            {
                Citas = registrosMostrar.ToList(),
                ListDoctores = new SelectList(_context.Doctores, "Id", "Nombre"),
                ListPacientes = new SelectList(_context.Pacientes, "Id", "Nombre"),
                busqApellidoDoctor = busqApellidoDoctor,
                busqApellidoPaciente = busqApellidoPaciente,
                busqEstado = busqEstado,
                paginador = paginas
            };

            return View(datos);
        }

        [AllowAnonymous]
        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Doctor)
                .Include(c => c.Estado)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            ViewData["Doctor"] = ListDoctores(-1);
            ViewData["Paciente"] = ListPacientes(-1);
            ViewData["Estado"] = new SelectList(_context.Estados, "Id", "Descripcion");
            return View();
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Hora,EstadoId,DoctorId,PacienteId")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Doctor"] = ListDoctores(cita.DoctorId);
            ViewData["Estado"] = new SelectList(_context.Estados, "Id", "Descripcion", cita.EstadoId);
            ViewData["Paciente"] = ListPacientes(cita.PacienteId);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["Doctor"] = ListDoctores(cita.DoctorId);
            ViewData["Estado"] = new SelectList(_context.Estados, "Id", "Descripcion", cita.EstadoId);
            ViewData["Paciente"] = ListPacientes(cita.PacienteId);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Hora,EstadoId,DoctorId,PacienteId")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            ViewData["Doctor"] = ListDoctores(cita.DoctorId);
            ViewData["Estado"] = new SelectList(_context.Estados, "Id", "Descripcion", cita.EstadoId);
            ViewData["Paciente"] = ListPacientes(cita.PacienteId);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Doctor)
                .Include(c => c.Estado)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Citas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Citas'  is null.");
            }
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
          return (_context.Citas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region Tools
        private SelectList ListDoctores(int citaId)
        {
            List<Doctor> listaDoctores = _context.Doctores.Include(m => m.Especializacion).OrderBy(x => x.Apellido).ToList();

            List<SelectListItem> selectListItems = listaDoctores.Select(m => new SelectListItem
            {
                Text = m.Nombre + " " + m.Apellido + " - " + m.Especializacion.Nombre,
                Value = m.Id.ToString()
            }).ToList();
            if (citaId == -1)
                return new SelectList(selectListItems, "Value", "Text");

            return new SelectList(selectListItems, "Value", "Text", citaId);
        }
        private SelectList ListPacientes(int citaId)
        {
            List<Paciente> listaDePacientes = _context.Pacientes.ToList();

            List<SelectListItem> selectListItems = listaDePacientes.Select(m => new SelectListItem
            {
                Text = m.Nombre + " " + m.Apellido,
                Value = m.Id.ToString()
            }).ToList();

            if (citaId == -1)
                return new SelectList(selectListItems, "Value", "Text");

            return new SelectList(selectListItems, "Value", "Text", citaId);
        }
        #endregion
    }
}
