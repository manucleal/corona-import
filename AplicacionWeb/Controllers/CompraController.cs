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
        // GET: Compra
        public ActionResult Index(Mutualista mutualista)
        {
            //Al pie del listado de las compras realizadas por esa institución, deberá mostrarse, 
            //cuántas compras le quedan en el mes actual, cuánto dinero autorizado tiene para compras y 
            //el saldo que le queda disponible.
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            decimal montoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);
            decimal montoTotalCompras = repoMutualista.CalcularMontoTotalCompras(mutualista.Id);
            decimal saldoDisponible = montoAutorizado - montoTotalCompras;
            RepositorioCompraVacuna repoCompraVacuna = new RepositorioCompraVacuna();
            ViewModelCompraVacuna model = new ViewModelCompraVacuna();
            model.CompraVacunasMutualista = repoCompraVacuna.FindAllByMutualista(mutualista.Id);

            return View(model);
        }
    }
}