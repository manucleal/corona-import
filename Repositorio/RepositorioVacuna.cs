using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dominio.EntidadesNegocio;
using AccesoDatos.Contexto;
using Dominio.InterfacesRepositorio;
using System.Data.Entity;

namespace Repositorio
{
    public class RepositorioVacuna
    {
        public bool Add(Vacuna unaVacuna)
        {            
            try
            {
                if (!unaVacuna.ValidateTemperature(unaVacuna) || !unaVacuna.ValidateAge(unaVacuna) || !unaVacuna.ValidateCantidadDosis(unaVacuna) || !unaVacuna.ValidateProduccionAnual(unaVacuna)) return false;
                unaVacuna.Precio = Vacuna.ValidatePrice(unaVacuna);
                unaVacuna.LapsoDiasDosis = Vacuna.ValidateLapsoDiasDosis(unaVacuna);
                using (var context = new CoronaImportContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        context.Vacunas.Add(unaVacuna);
                        if (unaVacuna.Tipo != null) {
                            context.Entry(unaVacuna.Tipo).State = EntityState.Unchanged;                            
                        }
                        if (unaVacuna.Laboratorios != null)
                        {
                            foreach (Laboratorio lab in unaVacuna.Laboratorios)
                            {
                                context.Entry(lab).State = EntityState.Unchanged;
                            }
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Vacuna" + e.Message);
                return false;
            }
        }

        public Vacuna FindById(int idVacuna)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Vacunas
                        .Where(v => v.Id == idVacuna).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al buscar vacuna" + e.Message);
                return null;
            }
        }

        public IEnumerable<Models.VacunaFilterDTO> FindAllByFilters(int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada)
        {

            //Se podrán buscar por criterios combinados(más de un criterio simultáneamente) y en este caso se
            //deberá seleccionar si es una búsqueda que debe cumplir con todos los criterios seleccionados o con
            //cualquiera de ellos. Si no se especifica ningún filtro se desplegarán todas las vacunas
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Vacunas.Select(v => new Models.VacunaFilterDTO {
                        Nombre = v.Nombre,
                        Tipo = v.Tipo.Descripcion,
                        Precio = v.Precio,
                        FaseClinicaDeAprobacion = v.FaseClinicaAprob,
                        Paises = v.Paises,
                        Laboratorios =  v.Laboratorios
                    }).Include(v => v.Laboratorios);
                    //si flag any llega en true entro aca sino sigo flujo con todos los ifs
                    if (faseClinicaAprob != -1 && PrecioMin != -1 && PrecioMax != -1 && tipo != "" && paisAceptada != "")
                    {

                    }

                    if (faseClinicaAprob != -1)
                    {
                        resultado = resultado.Where(v => v.FaseClinicaDeAprobacion == faseClinicaAprob);
                    }
                    if (PrecioMin != -1)
                    {
                        resultado = resultado.Where(v => v.Precio >= PrecioMin);
                    }
                    if (PrecioMax != -1)
                    {
                        resultado = resultado.Where(v => v.Precio <= PrecioMax);
                    }
                    if (tipo != "")
                    {
                        resultado = resultado.Where(v => v.Tipo == tipo);
                    }
                    if (paisAceptada != "")
                    {
                        resultado = resultado.Where(v => v.Paises.Contains(paisAceptada));
                    }
                    if (laboratorio != "")
                    {
                        //resultado = resultado.Where(v => v.Laboratorios.(lab => lab.Nombre == laboratorio)));
                        //resultado = resultado.Where(v => v.Laboratorios.Any(lab => lab.Nombre == laboratorio));
                    }

                    return resultado.ToList();
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al filtrat Vacunas" + exp.Message);
                return null;
            }
        }

        private ICollection<Laboratorio> AddLabsToVacunas(int idVacuna, SqlConnection con)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM Laboratorios l " +
                                  "WHERE l.Id IN (SELECT IdLaboratorio FROM VacunaLaboratorios vl " +
                                  "WHERE IdVacuna=@IdVacuna)", con);
            query.Parameters.AddWithValue("@IdVacuna", idVacuna);
            SqlDataReader labs = query.ExecuteReader();
            ICollection<Laboratorio> ListaLaboratorios = new List<Laboratorio>();
            while (labs.Read())
            {
                Laboratorio lab = new Laboratorio()
                {
                    Id = (int)labs["Id"],
                    Nombre = (string)labs["Nombre"],
                    PaisOrigen = (string)labs["PaisOrigen"],
                    Experiencia = (bool)labs["Experiencia"]
                };

                ListaLaboratorios.Add(lab);
            }

            labs.Close();

            return ListaLaboratorios;
        }

        private TipoVacuna AddTipoVacunaToVacunas(string idTipo, SqlConnection con)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM TipoVacunas " +                                  
                                  "WHERE Id LIKE @idTipo", con);
            query.Parameters.AddWithValue("@idTipo", '%' + idTipo + '%');

            SqlDataReader reader = query.ExecuteReader();
            TipoVacuna tipoVacuna = null;
            while (reader.Read())
            {
                TipoVacuna unaVacuna = new TipoVacuna()
                {
                    Id = (string)reader["Id"],
                    Descripcion = (string)reader["Descripcion"]
                };

                tipoVacuna = unaVacuna;
            }

            reader.Close();

            return tipoVacuna;
        }
    }
}
