using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AccesoDatos.Contexto;
using Dominio.EntidadesNegocio;
using Importador;

namespace AplicacionWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Create()
        {
            Mutualista mutualista = new Mutualista() {
                Id = 1,
                Nombre = "Blue Cross",
                Telefono = "234567",
                NombreContacto = "Seguro cheto",
                CantidadAfiliados = 1000,
                MontoMaxVacunasPorAfiliado = 50,
                TopeComprasMensuales = 1200
            };
            using (CoronaImportContext db = new CoronaImportContext())
            {
                db.Mutualistas.Add(mutualista);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Importador()
        {
            ManejadorArchivo.ImportarDatos();
            return RedirectToAction("Index","Vacuna");
        }
    }
}