using System;
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

            IEnumerable<ViewModelVacunaAPI> vacunasAPI = null;

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
                    var readTask = result.Content.ReadAsAsync<IList<ViewModelVacunaAPI>>();
                    readTask.Wait();

                    vacunasAPI = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    vacunasAPI = System.Linq.Enumerable.Empty<ViewModelVacunaAPI>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(vacunasAPI);
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

            //ViewBag.Vacunas = vacunas;
            //return View();
        }

        [HttpPost]
        public ActionResult IndexAuth(string tipoFiltro, string filtroText = "", int filtroNumber = 0)
        {
            if (tipoFiltro != null && (filtroText != "" || filtroNumber >= 0))
            {
                //switch (tipoFiltro)
                //{
                //    case "PorNombre":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorNombre(filtroText);
                //        break;
                //    case "PorFaseAprob":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorFaseAprob(filtroNumber);
                //        break;
                //    case "PorPaisLab":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorPaisLab(filtroText);
                //        break;
                //    case "PorTipoVac":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTipoVac(filtroText);
                //        break;
                //    case "PorTopeInferior":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTopeInferior(filtroNumber);
                //        break;
                //    case "PorTopeSuperior":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTopeSuperior(filtroNumber);
                //        break;
                //    case "PorNombreLab":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorNombreLab(filtroText);
                //        break;
                //    default:
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunas();
                //        break;
                //}
            }
            else
            {
                //ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunas();
            }

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
        public ActionResult Comprar(int? IdMutualista, int? cantidadDosis, int? idVacuna)
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            RepositorioVacuna repoVacuna = new RepositorioVacuna();
            Mutualista mutualista = repoMutualista.FindById((int)IdMutualista);
            Vacuna vacuna = repoVacuna.FindById((int)idVacuna);

            if (mutualista != null && vacuna != null)
            {
                decimal montoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);
                //TODO: cambiar metodo ObtenerMontoCompra a clase compra
                decimal montoCompra = Vacuna.ObtenerMontoCompra((int)cantidadDosis, vacuna.Precio);
                decimal montoComprasRealizadas = repoMutualista.CalcularMontoTotalCompras((int)idVacuna);
                decimal saldoDisponible = montoAutorizado - montoComprasRealizadas;
                if (saldoDisponible > 0 && montoAutorizado > 0 && montoCompra > 0)
                {
                    if (mutualista.TopeComprasMensuales >= 1) {
                        CompraVacuna compra = new CompraVacuna
                        {
                            CantidadDosis = (int)cantidadDosis,
                            Monto = montoCompra,
                            PrecioUnitario = vacuna.Precio,
                            Mutualista = mutualista,
                            Vacuna = vacuna
                        };
                        repoVacuna.AddCompra(compra);
                        return RedirectToAction("Index", "Compra",mutualista);
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