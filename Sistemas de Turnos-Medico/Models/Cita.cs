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

        [Required]
        public Doctor Doctor { get; set; }

        [Required]
        public Paciente Paciente { get; set; }
    }
}
