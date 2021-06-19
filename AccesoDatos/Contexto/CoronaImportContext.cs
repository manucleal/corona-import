using Dominio.EntidadesNegocio;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AccesoDatos.Contexto
{
    public class CoronaImportContext : DbContext
    {
        public CoronaImportContext() : base("name=miConexion") { }

        public DbSet<Mutualista> Mutualistas { get; set; }
        public DbSet<Laboratorio> Laboratorios { get; set; }
        public DbSet<TipoVacuna> TipoVacunas { get; set; }
        public DbSet<Vacuna> Vacunas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<TipoVacuna>().ToTable("TipoVacuna");

            //modelBuilder.Entity<TipoVacuna>().ToTable("TipoVacuna");
        }
    }
}
