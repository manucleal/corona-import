using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Dominio.InterfacesRepositorio;
using Dominio.EntidadesNegocio;
using Dominio.DtosRepositorio;
using AccesoDatos.Contexto;
using System.Data.Entity;

namespace Repositorio
{
    public class RepositorioVacuna : IRepositorioVacuna
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
                    dataBase.Configuration.LazyLoadingEnabled = false;
                    dataBase.Configuration.ProxyCreationEnabled = false;
                    var resultado = dataBase.Vacunas
                        .Where(v => v.Id == idVacuna)
                        .Include(v => v.Laboratorios)
                        .Include(v => v.Tipo)
                        .FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al buscar vacuna" + e.Message);
                return null;
            }
        }

        public IEnumerable<Vacuna> FindAll()
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Configuration.LazyLoadingEnabled = false;
                    dataBase.Configuration.ProxyCreationEnabled = false;
                    return dataBase.Vacunas.Include(v => v.Tipo).ToList();                    
                }
            }
            catch (Exception exp) { return null; }
        }

        public IEnumerable<VacunaFilterDTO> FindAllByFiltersOR(int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Configuration.LazyLoadingEnabled = false;
                    dataBase.Configuration.ProxyCreationEnabled = false;

                    List<VacunaFilterDTO> vacunasPorFaseClinicaAprobacion = new List<VacunaFilterDTO>();
                    List<VacunaFilterDTO> vacunasPorPrecioMin = new List<VacunaFilterDTO>();
                    List<VacunaFilterDTO> vacunasPorPrecioMax = new List<VacunaFilterDTO>();
                    List<VacunaFilterDTO> vacunasPorTipo = new List<VacunaFilterDTO>();
                    List<VacunaFilterDTO> vacunasPorPaisAceptacion = new List<VacunaFilterDTO>();
                    List<VacunaFilterDTO> vacunasPorLaboratorios = new List<VacunaFilterDTO>();

                    var resultado = dataBase.Vacunas
                        .Include(v => v.Laboratorios)
                        .Include(v => v.Tipo).ToList()
                    .Select(v => new VacunaFilterDTO
                    {
                        Id = v.Id,
                        Nombre = v.Nombre,
                        Tipo = v.Tipo.Descripcion,
                        Precio = v.Precio,
                        FaseClinicaDeAprobacion = v.FaseClinicaAprob,
                        Paises = v.Paises,
                        Labs = String.Join(",", v.Laboratorios.Select(l => l.Nombre).ToList())
                    });


                    if (faseClinicaAprob != -1)
                    {
                        vacunasPorFaseClinicaAprobacion = resultado.Where(v => v.FaseClinicaDeAprobacion == faseClinicaAprob).ToList();
                    }
                    if (PrecioMin != -1)
                    {
                        vacunasPorPrecioMin = vacunasPorFaseClinicaAprobacion.Union(resultado.Where(v => v.Precio >= PrecioMin)).ToList();
                    }
                    if (PrecioMax != -1)
                    {
                        vacunasPorPrecioMax = vacunasPorPrecioMin.Union(resultado.Where(v => v.Precio <= PrecioMax)).ToList();
                    }
                    if (!"".Equals(tipo))
                    {
                        vacunasPorTipo = vacunasPorPrecioMax.Union(resultado.Where(v => v.Tipo.Equals(tipo))).ToList();
                    }
                    if (!"".Equals(paisAceptada))
                    {
                        vacunasPorPaisAceptacion = vacunasPorTipo.Union(resultado.Where(v => v.Paises.ToUpper().Contains(paisAceptada.ToUpper()))).ToList();
                    }
                    if (!"".Equals(laboratorio))
                    {
                        vacunasPorLaboratorios = vacunasPorPaisAceptacion.Union(resultado.Where(v => v.Labs.ToUpper().Contains(laboratorio.ToUpper()))).ToList();
                    }
                    var vacunas = vacunasPorFaseClinicaAprobacion
                                .Concat(vacunasPorPrecioMin)
                                .Concat(vacunasPorPrecioMax)
                                .Concat(vacunasPorTipo)
                                .Concat(vacunasPorPaisAceptacion)
                                .Concat(vacunasPorLaboratorios).ToList();

                    return vacunas.Distinct().ToList();
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al filtrar Vacunas OR" + exp.Message);
                return null;
            }
        }

        public IEnumerable<VacunaFilterDTO> FindAllByFiltersAND(int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Configuration.LazyLoadingEnabled = false;
                    dataBase.Configuration.ProxyCreationEnabled = false;

                    var resultado = dataBase.Vacunas
                        .Include(v => v.Laboratorios)
                        .Include(v => v.Tipo).ToList()
                    .Select(v => new VacunaFilterDTO {
                        Id = v.Id,
                        Nombre = v.Nombre,
                        Tipo = v.Tipo.Descripcion,
                        Precio = v.Precio,
                        FaseClinicaDeAprobacion = v.FaseClinicaAprob,
                        Paises = v.Paises,
                        Labs = String.Join(",", v.Laboratorios.Select(l => l.Nombre).ToList())
                    });

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
                    if (!"".Equals(tipo))
                    {
                        resultado = resultado.Where(v => v.Tipo.Equals(tipo));
                    }
                    if (!"".Equals(paisAceptada))
                    {
                        resultado = resultado.Where(v => v.Paises.ToUpper().Contains(paisAceptada.ToUpper()));
                    }
                    if (!"".Equals(laboratorio))
                    {
                        resultado = resultado.Where(v => v.Labs.ToUpper().Contains(laboratorio.ToUpper()));
                    }

                    return resultado.ToList();
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al filtrar Vacunas AND" + exp.Message);
                return null;
            }
        }
    }
}
