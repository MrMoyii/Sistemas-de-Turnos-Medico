using System.ComponentModel.DataAnnotations;

namespace Sistemas_de_Turnos_Medico.Models
{
    public class Paciente
    {
        [Key] public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public int DNI { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "No es un número de teléfono válido")]
        public string Celular { get; set; }

        public string? Foto { get; set; }

        public List<Cita>? citas { get; set; }
    }
}