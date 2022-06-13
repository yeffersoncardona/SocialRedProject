using Microsoft.EntityFrameworkCore;
using Model.Domain;

namespace Model
{
    public class SocialRedDbContext : DbContext
    {

        public SocialRedDbContext(DbContextOptions<SocialRedDbContext> options)
            : base(options)
        {

        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Publicaciones> Publicaciones { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }
        public DbSet<Detalles> Detalles { get; set; }
        public DbSet<Receptor> Receptor { get; set; }
 
    }
}
