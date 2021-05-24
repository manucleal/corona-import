using Dominio.EntidadesNegocio;
using System.Data.Entity;

namespace AccesoDatos.Contexto
{
    public class CoronaImportContext : DbContext
    {
        public CoronaImportContext() : base("miConexion")
        {
            Database.SetInitializer<CoronaImportContext>(new CreateDatabaseIfNotExists<CoronaImportContext>());
        }

        DbSet<Mutualista> Mutualistas { get; set; }
        DbSet<Laboratorio> Laboratorios { get; set; }
        DbSet<TipoVacuna> TipoVacunas { get; set; }
        DbSet<Vacuna> Vacunas { get; set; }
        DbSet<Usuario> Usuarios { get; set; }
    }
}
