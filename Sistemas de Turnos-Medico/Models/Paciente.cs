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

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int Celular { get; set; }

        public string? Foto { get; set; }

        public List<Cita>? citas { get; set; }
    }
}