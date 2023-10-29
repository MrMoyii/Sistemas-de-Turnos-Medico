using System.ComponentModel.DataAnnotations;

namespace Sistemas_de_Turnos_Medico.Models
{
    public class Especializacion
    {
        [Key] public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public List<Doctor>? Doctores { get; set; }
    }
}