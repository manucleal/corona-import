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
            Mutualista mutualista1 = new Mutualista() {
                Codigo = 777,
                Nombre = "Blue Cross",
                Telefono = "234567",
                NombreContacto = "Seguro cheto",
                CantidadAfiliados = 1000,
                MontoMaxVacunasPorAfiliado = 50,
                TopeComprasMensuales = 1200
            };

            Mutualista mutualista2 = new Mutualista()
            {
                Codigo = 333,
                Nombre = "Cosem",
                Telefono = "200996",
                NombreContacto = "Fulano de tal",
                CantidadAfiliados = 5000,
                MontoMaxVacunasPorAfiliado = 40,
                TopeComprasMensuales = 100
            };

            Mutualista mutualista3 = new Mutualista()
            {
                Codigo = 222,
                Nombre = "SMI",
                Telefono = "207878",
                NombreContacto = "Fausto Perillo",
                CantidadAfiliados = 11111,
                MontoMaxVacunasPorAfiliado = 25,
                TopeComprasMensuales = 300
            };
            using (CoronaImportContext db = new CoronaImportContext())
            {
                db.Mutualistas.Add(mutualista1);
                db.Mutualistas.Add(mutualista2);
                db.Mutualistas.Add(mutualista3);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Vacuna");
        }

        public ActionResult Importador()
        {
            ManejadorArchivo.ImportarDatos();
            return RedirectToAction("Index","Vacuna");
        }
    }
}