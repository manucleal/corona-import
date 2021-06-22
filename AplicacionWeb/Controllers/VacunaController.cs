using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using AccesoDatos.Repositorios;
using Repositorio;
using AplicacionWeb.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class VacunaController : Controller
    {
        RepositorioVacuna repositorioVacuna = new RepositorioVacuna();
        RepositorioTipoVacuna repositorioTipoVacuna = new RepositorioTipoVacuna();
        RepositorioLaboratorio repositorioLaboratorio = new RepositorioLaboratorio();
        HttpClient cliente = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        Uri vacunaUri = null;

        public VacunaController()
        {
            cliente.BaseAddress = new Uri("http://localhost:49340");
            vacunaUri = new Uri("http://localhost:49340/api/vacunas");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewModelImportador model = new ViewModelImportador();
            RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
            model.Usuarios = repositorioUsuario.FindAll();
            model.Vacunas = repositorioVacuna.FindAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            return View(ViewModelVacuna.MapearAViewModelVacuna(repositorioVacuna.FindById((int)id)));
        }

        [HttpGet]
        public ActionResult IndexAuth()
        {
            if ((string)Session["documento"] == null && (string)Session["nombre"] == null)
            {
                Session["documento"] = null;
                Session["nombre"] = null;
                return RedirectToAction("Index", "Vacuna");
            }
            ViewModelVacunaAPI model = new ViewModelVacunaAPI();
            ViewBag.Laboratorios = repositorioLaboratorio.FindAll();
            ViewBag.TiposVacuna = repositorioTipoVacuna.FindAll();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri("http://localhost:49340/api/");
                //HTTP GET
                var responseTask = client.GetAsync("vacunas/filters");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<VacunasAPI>>();
                    readTask.Wait();

                    model.Vacunas = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    model.Vacunas = Enumerable.Empty<VacunasAPI>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
            //response = cliente.GetAsync(vacunaUri).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var vacunasAPI = response.Content.ReadAsAsync<IEnumerable<ViewModelVacuna>>().Result;
            //    if (vacunasAPI != null)
            //    {
            //        //return View("IndexAuth", vacunasAPI.ToList());
            //    }
            //    else
            //    {
            //        TempData["ResultadoOperacion"] = "No hay vacunas disponibles";
            //        //return View("IndexAuth", new List<ProductoModel>());
            //    }
            //}
            //else
            //{
            //    TempData["ResultadoOperacion"] = "Error desconocido";
            //    return View("IndexAuth");
            //}
            //return View();
        }

        [HttpPost]
        public ActionResult IndexAuth(ViewModelVacunaAPI viewModelVacunaAPI)
        {
            return View("IndexAuth");
        }

        [HttpGet]
        public ActionResult Comprar(int? id)
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();

            if ((string)Session["documento"] != null && Session["nombre"] != null)
            {
                ViewModelVacuna viewModelVacuna = ViewModelVacuna.MapearAViewModelVacuna(repositorioVacuna.FindById((int)id));
                if (viewModelVacuna != null)
                {
                    ViewBag.Mutualistas = repoMutualista.FindAll();
                    return View("Comprar", viewModelVacuna);
                }
            }
            return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public ActionResult Comprar(int? IdMutualista, ViewModelVacuna viewModelVacuna)
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            RepositorioVacuna repoVacuna = new RepositorioVacuna();
            Mutualista mutualista = repoMutualista.FindById((int)IdMutualista);
            Vacuna vacuna = repoVacuna.FindById(viewModelVacuna.Id);

            if (mutualista != null && vacuna != null)
            {
                decimal montoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);
                decimal montoCompra = Vacuna.ObtenerMontoCompra(viewModelVacuna.CantidadDosis, viewModelVacuna.Precio);
                decimal montoComprasRealizadas = repoMutualista.CalcularMontoTotalCompras(viewModelVacuna.Id);
                decimal saldoDisponible = montoAutorizado - montoComprasRealizadas;
                if (saldoDisponible > 0 && montoAutorizado > 0 && montoCompra > 0)
                {
                    if (mutualista.TopeComprasMensuales >= 1) {
                        CompraVacuna compra = new CompraVacuna
                        {
                            CantidadDosis = viewModelVacuna.CantidadDosis,
                            Monto = montoCompra,
                            PrecioUnitario = viewModelVacuna.Precio,
                            Mutualista = mutualista,
                            Vacuna = vacuna
                        };
                        repoVacuna.AddCompra(compra);
                    }
                    else
                    {
                        ModelState.AddModelError("CantidadDosis", "La mutualista superó las compras mensuales");
                    }
                }
                else
                {
                    ModelState.AddModelError("CantidadDosis", "No tiene saldo disponible para la compra");
                }
                
            }
            else
            {
                return RedirectToAction("IndexAuth","Vacuna");
            }

            return RedirectToAction("IndexAuth", "Vacuna");
        }
    }
}