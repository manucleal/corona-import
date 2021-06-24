﻿using System;
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
        public IEnumerable<CompraVacuna> FindAllByMutualista(int id)
        {
            try
            {
                using (CoronaImportContext db = new CoronaImportContext())
                {
                    var resultado = db.CompraVacunas
                        .Where(c => c.Mutualista.Id == id)
                        .Include(c => c.Vacuna)
                        .Include(c => c.Mutualista);
                    return resultado.ToList();
                }
            }
            catch (Exception ex) { return null; }
        }
    }
}
