using System.ComponentModel.DataAnnotations;

namespace Sistemas_de_Turnos_Medico.Models
{
    public class Cita
    {
        [Key] public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Hora { get; set; }

        public int EstadoId { get; set; }
        
        public EstadoCita? Estado { get; set; }

        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        public int PacienteId { get; set; }

        public Paciente? Paciente { get; set; }
    }
}
