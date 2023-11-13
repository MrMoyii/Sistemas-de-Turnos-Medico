using Microsoft.AspNetCore.Mvc.Rendering;
using Sistemas_de_Turnos_Medico.Models;

namespace Sistemas_de_Turnos_Medico.ModelView
{
    public class DoctorVM
    {
        public List<Doctor> Doctores { get; set; }
        public string? busqNombre{ get; set; }
        public string? busqApellido { get; set; }
        public int? busqEspecializacion { get; set; }
        public Paginador paginador { get; set; }
    }
}
