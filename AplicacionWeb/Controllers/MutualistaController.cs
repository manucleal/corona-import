using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using AplicacionWeb.Models;
using Repositorio;

namespace AplicacionWeb.Controllers
{
    public class MutualistaController : Controller
    {
        public ActionResult Registro()
        {
            if ((string)Session["documento"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Vacuna");
        }

        [HttpPost]
        public ActionResult Registro(ViewModelMutualista viewModelMutualista)
        {
            if (ModelState.IsValid)
            {
                RepositorioMutualista repoMutualista = new RepositorioMutualista();
                Mutualista mutualista = repoMutualista.FindById(viewModelMutualista.Codigo);

                if (mutualista != null)
                {
                    ModelState.AddModelError("Codigo", "Ya existe la mutualista");
                    return View("RegistroMutualista");
                }
                if (repoMutualista.FindByName(viewModelMutualista.Nombre) != null)
                {
                    ModelState.AddModelError("Nombre", "El nombre de la mutualista debe ser único");
                    return View("RegistroMutualista");
                }
                repoMutualista.Add(ViewModelMutualista.MapearAMutualista(viewModelMutualista));
            }
            return View();
        }
    }
}