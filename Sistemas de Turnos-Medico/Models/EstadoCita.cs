using System.ComponentModel.DataAnnotations;

namespace Sistemas_de_Turnos_Medico.Models
{
    public class EstadoCita
    {
        [Key] public int Id { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public List<Cita>? Citas { get; set; }
    }
}