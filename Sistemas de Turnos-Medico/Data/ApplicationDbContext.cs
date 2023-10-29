using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistemas_de_Turnos_Medico.Models;

namespace Sistemas_de_Turnos_Medico.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Especializacion> Especializaciones { get; set; }
    }
}