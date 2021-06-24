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
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            RepositorioCompraVacuna repoCompraVacuna = new RepositorioCompraVacuna();
            ViewModelCompraVacuna model = new ViewModelCompraVacuna();

            decimal montoTotalCompras = repoMutualista.CalcularMontoTotalCompras(mutualista.Id);
            model.CompraVacunasMutualista = repoCompraVacuna.FindAllByMutualista(mutualista.Id);
            model.MontoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);
            model.SaldoDisponible = model.MontoAutorizado - (montoTotalCompras == -1 ? 0 : montoTotalCompras);
            model.ComprasDisponibles = mutualista.TopeComprasMensuales - mutualista.ComprasRealizadas;

            return View(model);
        }
    }
}