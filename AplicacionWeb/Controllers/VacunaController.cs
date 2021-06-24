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
        private readonly string baseUrl = "http://localhost:49340/api/";
        private HttpClient cliente = new HttpClient();
        private HttpResponseMessage response = new HttpResponseMessage();
        private RepositorioVacuna repositorioVacuna = new RepositorioVacuna();
        private RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
        private RepositorioTipoVacuna repositorioTipoVacuna = new RepositorioTipoVacuna();
        private RepositorioLaboratorio repositorioLaboratorio = new RepositorioLaboratorio();

        [HttpGet]
        public ActionResult Index()
        {
            ViewModelImportador model = new ViewModelImportador();
            IEnumerable<Usuario> Usuarios = repositorioUsuario.FindAll();
            IEnumerable<Vacuna> Vacunas = repositorioVacuna.FindAll();
            IEnumerable<TipoVacuna> Tipos = repositorioTipoVacuna.FindAll();
            IEnumerable<Laboratorio>  Laboratorios = repositorioLaboratorio.FindAll();
            if (Usuarios != null)
            {
                model.Usuarios = Usuarios;
            }
            if (Vacunas != null)
            {
                model.Vacunas = Vacunas;
            }
            if (Tipos != null)
            {
                model.Tipos = Tipos;
            }
            if (Laboratorios != null)
            {
                model.Laboratorios = Laboratorios;
            }
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
                string url = "vacunas/filters" + BuildUrlVacunasFilters(data);
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
                else
                {
                    ViewBag.Mutualistas = Enumerable.Empty<Mutualista>();
                } 

            }
            return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public ActionResult Comprar(int? IdMutualista, int? cantidadDosis, int? idVacuna)
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            Mutualista mutualista = repoMutualista.FindById((int)IdMutualista);// tengo id de mutualista que me llega por el select de la vista
            Vacuna vacuna = repositorioVacuna.FindById((int)idVacuna);// tengo un input hidden en la vista con este id que me llega 

            if (mutualista != null && vacuna != null)
            {
                decimal montoAutorizado = Mutualista.ObtenerMontoAutorizado(mutualista);// mutualista.MontoMaxVacunasPorAfiliado * mutualista.CantidadAfiliados;
                if(montoAutorizado != - 1) {
                    //recibo cantidad de dosis del formulario compra vacuna
                    decimal montoCompra = CompraVacuna.ObtenerMontoCompra((int)cantidadDosis, vacuna.Precio);
                    decimal montoComprasRealizadas = repoMutualista.CalcularMontoTotalCompras((int)idVacuna);
                    decimal saldoDisponible = montoAutorizado - (montoComprasRealizadas == -1 ? 0 : montoComprasRealizadas);
                    
                    if (saldoDisponible >= 0)
                    {
                        if (saldoDisponible > montoCompra)
                        {
                            if (mutualista.TopeComprasMensuales >= 1)
                            {
                                CompraVacuna compra = new CompraVacuna { CantidadDosis = (int)cantidadDosis, Monto = montoCompra, Mutualista = mutualista, Vacuna = vacuna };
                                using (var client = new HttpClient())
                                {                                   
                                    cliente.DefaultRequestHeaders.Accept.Clear();
                                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    Uri url = new Uri("http://localhost:49340/api/compra/mutualista");
                                    var tareaPost = cliente.PostAsJsonAsync(url, compra);                                    
                                    var result = tareaPost.Result;
                                    if (result.IsSuccessStatusCode)
                                    {
                                        return RedirectToAction("Index", "Compra", mutualista);
                                    }
                                    else
                                    {

                                    }
                                }                              
                            }
                            else
                            {
                                ModelState.AddModelError("CantidadDosis", "La mutualista superó el tope de compras mensuales");
                                return RedirectToAction("Comprar", "Vacuna", idVacuna);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("CantidadDosis", "No tiene saldo suficiente para realizar esa compra");
                            return RedirectToAction("Comprar", "Vacuna", idVacuna);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("CantidadDosis", "Ya no tiene saldo disponible para la compra");
                        return RedirectToAction("Comprar", "Vacuna", idVacuna);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No tiene monto autorizado para la compra");
                }                
            }
            else
            {
                ModelState.AddModelError("", "No se encontró la mutualista o la vacuna");
                return RedirectToAction("Comprar","Vacuna", idVacuna);
            }

            return RedirectToAction("IndexAuth", "Vacuna");
        }

        private static string BuildUrlVacunasFilters(ModelFiltro modelFiltro)
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