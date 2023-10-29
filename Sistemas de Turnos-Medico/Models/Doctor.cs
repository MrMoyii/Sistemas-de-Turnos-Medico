using System.ComponentModel.DataAnnotations;

namespace Sistemas_de_Turnos_Medico.Models
{
    public class Doctor
    {
        [Key] public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        
        [Required]
        public string Apellido { get;}

        [Required]
        public int Celular { get; set; }

        public string? Foto { get; set; }

        [Required]
        public int EspecializacionId { get; set; }

        [Required]
        [Display(Name = "Especialización")]
        public Especializacion Especializacion { get; set; }
    }
}