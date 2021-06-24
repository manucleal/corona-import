using System;
using AccesoDatos.Contexto;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorio;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repositorio
{
    public class RepositorioCompraVacuna : IRepositorioCompraVacuna
    {
        public bool Add(CompraVacuna compra)
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
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al guardar una compra" + exp.Message);
                return false;
            }
        }

        public IEnumerable<CompraVacuna> FindAllByMutualista(int codigo)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Configuration.LazyLoadingEnabled = false;
                    dataBase.Configuration.ProxyCreationEnabled = false;

                    var resultado = dataBase.CompraVacunas
                        .Where(c => c.Mutualista.Codigo == codigo)
                        .Include(c => c.Vacuna)
                        .Include(c => c.Mutualista).ToList();
                    return resultado;
                }
            }
            catch (Exception ex) { return null; }
        }
    }
}
