using Microsoft.AspNetCore.Mvc.Rendering;
using Sistemas_de_Turnos_Medico.Models;

namespace Sistemas_de_Turnos_Medico.ModelView
{
    public class CitaVM
    {
        public List<Cita> Citas { get; set; }
        public SelectList ListPacientes { get; set; }
        public SelectList ListDoctores { get; set; }
        public string? busqApellidoDoctor { get; set; }
        public string? busqApellidoPaciente { get; set; }
        public int? busqEstado { get; set; }
        public int? EstadoId { get; set; }
        public Paginador paginador { get; set; }
    }
}
