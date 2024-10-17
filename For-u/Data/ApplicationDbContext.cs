using For_u.Models; // Asegúrate de que todos los modelos estén en este espacio de nombres
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace For_u.Data
{
    public class ApplicationDbContext : DbContext // Clase que hereda de DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Configuración para la conexión a la base de datos
        {
        }

        // Representación de cada tabla de la base de datos
        public DbSet<Users> Users { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Comunidades> Comunidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad 'comentarios'
            modelBuilder.Entity<Comentarios>()
                .HasKey(c => c.ComentarioId);  // Define ComentarioId como la clave primaria

            modelBuilder.Entity<Comentarios>()
                .HasOne(c => c.UsuarioCreador)  // Relación con Users
                .WithMany()  // Sin colección en Users
                .HasForeignKey(c => c.ComentarioCreadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentarios>()
                .HasOne(c => c.Post)  // Relación con posts
                .WithMany()  // Sin colección en posts
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la entidad 'comunidades'
            modelBuilder.Entity<Comunidades>()
                .HasKey(c => c.ComunidadId);  // Define la clave primaria

            modelBuilder.Entity<Comunidades>()
       .HasOne<Users>() // Indica que hay una relación con Users
       .WithMany()  // Sin colección de comunidades en 'Users'
       .HasForeignKey(c => c.ComunidadCreadaPor) // Clave foránea
       .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la entidad 'posts'
            modelBuilder.Entity<Posts>()
                .HasKey(p => p.postId);  // Define la clave primaria

            modelBuilder.Entity<Posts>()
                .HasOne(p => p.UsuarioCreador)  // Relación con Users
                .WithMany()  // Sin colección en Users para posts
                .HasForeignKey(p => p.postCreadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Posts>()
                .HasOne(p => p.Comunidades)  // Relación con comunidades
                .WithMany()  // Sin colección en comunidades para posts
                .HasForeignKey(p => p.comunidadId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la entidad 'Users'
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserId);  // Define UserId como la clave primaria
        }
    }
}
