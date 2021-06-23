using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccesoDatos.Repositorios;
using AplicacionWeb.Models;
using Dominio.EntidadesNegocio;
using Repositorio;


namespace AplicacionWeb.Controllers
{
    public class CompraController : Controller
    {
        public ActionResult Index(Mutualista mutualista)
        {
            //Al pie del listado de las compras realizadas por esa institución, deberá mostrarse, 
            //cuántas compras le quedan en el mes actual, cuánto dinero autorizado tiene para compras y 
            //el saldo que le queda disponible.
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            RepositorioCompraVacuna repoCompraVacuna = new RepositorioCompraVacuna();

            decimal montoTotalCompras = repoMutualista.CalcularMontoTotalCompras(mutualista.Id);

            ViewModelCompraVacuna model = new ViewModelCompraVacuna();
            
            model.CompraVacunasMutualista = repoCompraVacuna.FindAllByMutualista(mutualista.Id);
            model.MontoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);
            model.SaldoDisponible = model.MontoAutorizado - montoTotalCompras;
            model.ComprasDisponibles = mutualista.TopeComprasMensuales - mutualista.ComprasRealizadas;

            return View(model);
        }
    }
}