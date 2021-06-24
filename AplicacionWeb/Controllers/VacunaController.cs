using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using Repositorio;
using AplicacionWeb.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class VacunaController : Controller
    {
        readonly string baseUrl = "http://localhost:49340/api/";
        HttpClient cliente = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        RepositorioVacuna repositorioVacuna = new RepositorioVacuna();
        RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
        RepositorioTipoVacuna repositorioTipoVacuna = new RepositorioTipoVacuna();
        RepositorioLaboratorio repositorioLaboratorio = new RepositorioLaboratorio();

        [HttpGet]
        public ActionResult Index()
        {
            ViewModelImportador model = new ViewModelImportador();
            model.Usuarios = repositorioUsuario.FindAll();
            model.Vacunas = repositorioVacuna.FindAll();
            model.Tipos = repositorioTipoVacuna.FindAll();
            model.Laboratorios = repositorioLaboratorio.FindAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            return View(ViewModelVacuna.MapearAViewModelVacuna(repositorioVacuna.FindById((int)id)));
        }

        [HttpGet]
        public ActionResult IndexAuth(ModelFiltro data)
        {
            if ((string)Session["documento"] == null)
            {
                return RedirectToAction("Index", "Vacuna");
            }

            ViewModelVacunaAPI model = new ViewModelVacunaAPI();
            ViewBag.Laboratorios = repositorioLaboratorio.FindAll();
            ViewBag.TiposVacuna = repositorioTipoVacuna.FindAll();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(baseUrl);
                string url = "vacunas/filters" + BuildUrl(data);
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<VacunasAPI>>();
                    readTask.Wait();
                    model.Vacunas = readTask.Result;
                }
                else 
                {
                    //web api sent error response 
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
            
            if(viewModelVacunaAPI != null)
            {
                //TODO: implementar mensajes de error y validaciones.
            }
            return RedirectToAction("IndexAuth", viewModelVacunaAPI.Filtro);
            //return View(viewModelVacunaAPI.Data);
        }

        [HttpGet]
        public ActionResult Comprar(int? id)
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();

            if ((string)Session["documento"] != null)
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
                if (montoComprasRealizadas != -1)
                {

                }
                decimal saldoDisponible = montoAutorizado - montoComprasRealizadas;
                if (saldoDisponible > 0 && montoAutorizado > 0 && montoCompra > 0)
                {
                    if (mutualista.TopeComprasMensuales >= 1) {
                        CompraVacuna compra = new CompraVacuna
                        {
                            CantidadDosis = (int)cantidadDosis,
                            Monto = montoCompra,
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

        private static string BuildUrl(ModelFiltro modelFiltro)
        {
            string stringUrl = "?";

            if (modelFiltro.FaseClinicaDeAprobacionFiltro != "" && modelFiltro.FaseClinicaDeAprobacionFiltro != null)
            {
                stringUrl += "faseClinicaAprob=" + modelFiltro.FaseClinicaDeAprobacionFiltro + "&";
            }
            if (modelFiltro.PrecioMinFiltro != "" && modelFiltro.PrecioMinFiltro != null)
            {
                stringUrl += "precioMin=" + modelFiltro.PrecioMinFiltro + "&";
            }
            if (modelFiltro.PrecioMaxFiltro != "" && modelFiltro.PrecioMaxFiltro != null)
            {
                stringUrl += "precioMax=" + modelFiltro.PrecioMaxFiltro + "&";
            }
            if (modelFiltro.TipoFiltro != "" && modelFiltro.TipoFiltro != null)
            {
                stringUrl += "tipo=" + modelFiltro.TipoFiltro + "&";
            }
            if (modelFiltro.LaboratorioFiltro != "" && modelFiltro.LaboratorioFiltro != null)
            {
                stringUrl += "laboratorio=" + modelFiltro.LaboratorioFiltro + "&";
            }
            if (modelFiltro.PaisFiltro != "" && modelFiltro.PaisFiltro != null)
            {
                stringUrl += "paisAceptada=" + modelFiltro.PaisFiltro;
            }
            if (stringUrl != "")
            {
                string ultimoCaracter = stringUrl[stringUrl.Length - 1].ToString();
                int ultimoIndex = stringUrl.Length - 1;
                if ("&".Equals(ultimoCaracter)) { stringUrl = stringUrl.Remove(ultimoIndex); };
            }
            return (modelFiltro.Criterio != "or" ? "/all" : "/any") + stringUrl;
        }
    }
}