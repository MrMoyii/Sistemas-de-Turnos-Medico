using System.ComponentModel.DataAnnotations;

namespace Sistemas_de_Turnos_Medico.Models
{
    public class Turno
    {
        [Key] public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Hora { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public Doctor Doctor { get; set; }
    }
}
