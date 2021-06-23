using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dominio.EntidadesNegocio;
using AccesoDatos.Contexto;
using Dominio.InterfacesRepositorio;
using System.Data.Entity;
using AccesoDatos.Repositorios;

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
            catch (Exception ex) { return null; }
        }

        public IEnumerable<Models.VacunaFilterDTO> FindAllByFilters(int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada)
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
                    .Select(v => new Models.VacunaFilterDTO {
                        Id = v.Id,
                        Nombre = v.Nombre,
                        Tipo = v.Tipo.Descripcion,
                        Precio = v.Precio,
                        FaseClinicaDeAprobacion = v.FaseClinicaAprob,
                        Paises = v.Paises,
                        Labs = String.Join(",", v.Laboratorios.Select(l => l.Nombre).ToList())
                    });

                    //si flag any llega en true entro aca sino sigo flujo con todos los ifs
                    if (faseClinicaAprob != -1 && PrecioMin != -1 && PrecioMax != -1 && tipo != "" && paisAceptada != "")
                    {
                        //TODO: separar en otro metodo para los criterios con OR
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
                        resultado = resultado.Where(v => v.Tipo.ToUpper() == tipo.ToUpper());
                    }
                    if (paisAceptada != "")
                    {
                        resultado = resultado.Where(v => v.Paises.ToUpper().Contains(paisAceptada.ToUpper()));
                    }
                    if (laboratorio != "")
                    {
                        resultado = resultado.Where(v => v.Labs.ToUpper().Contains(laboratorio.ToUpper()));
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

        public bool AddCompra(CompraVacuna compra)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Configuration.LazyLoadingEnabled = false;
                    dataBase.Configuration.ProxyCreationEnabled = false;

                    if (compra.Mutualista != null)
                    {
                        compra.Mutualista.ComprasRealizadas += 1;
                        dataBase.Entry(compra.Mutualista).State = EntityState.Modified;
                    }
                    if (compra.Vacuna != null)
                    {
                        dataBase.Entry(compra.Vacuna).State = EntityState.Unchanged;
                        dataBase.Entry(compra.Vacuna.Tipo).State = EntityState.Unchanged;
                        foreach (Laboratorio lab in compra.Vacuna.Laboratorios)
                        {
                            dataBase.Entry(lab).State = EntityState.Unchanged;
                        }
                    }
                    compra.Fecha = DateTime.Now;
                    dataBase.CompraVacunas.Add(compra);
                    dataBase.SaveChanges();
                }
                return true;
            } catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al guardar una compra" + exp.Message);
                return false;
            }
        }
    }
}
