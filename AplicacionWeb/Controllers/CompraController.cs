using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using Repositorio;
using AplicacionWeb.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;


namespace AplicacionWeb.Controllers
{
    public class CompraController : Controller
    {

        private readonly string baseUrl = "http://localhost:49340/api/";
        private HttpClient cliente = new HttpClient();

        public ActionResult Index(Mutualista mutualista)
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            RepositorioCompraVacuna repoCompraVacuna = new RepositorioCompraVacuna();
            ViewModelCompraVacuna model = new ViewModelCompraVacuna();

            decimal montoTotalCompras = repoMutualista.CalcularMontoTotalCompras(mutualista.Codigo);
            model.MontoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);
            model.SaldoDisponible = model.MontoAutorizado - (montoTotalCompras == -1 ? 0 : montoTotalCompras);
            model.ComprasDisponibles = mutualista.TopeComprasMensuales - mutualista.ComprasRealizadas;

            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            cliente.BaseAddress = new Uri(baseUrl);
            string url = "compra/mutualista";
            var responseTask = cliente.GetAsync(url + "?id=" + mutualista.Codigo);
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<CompraVacuna>>();
                readTask.Wait();
                model.CompraVacunasMutualista = readTask.Result;
            }
            else
            {
                model.CompraVacunasMutualista = Enumerable.Empty<CompraVacuna>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(model);
        }
    }
}