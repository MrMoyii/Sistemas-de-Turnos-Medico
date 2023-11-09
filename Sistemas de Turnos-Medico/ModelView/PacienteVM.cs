using Sistemas_de_Turnos_Medico.Models;

namespace Sistemas_de_Turnos_Medico.ModelView
{
    public class PacienteVM
    {
        public List<Paciente> pacientes { get; set; }
        public string? busqNombre { get; set; }
        public string? busqApellido { get; set; }
        public int? busqDNI { get; set; }
        public Paginador paginador { get; set; }
    }
}
