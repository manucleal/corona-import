using System.Web.Mvc;
using Dominio.EntidadesNegocio;
//using Repositorios;

namespace WebApplication.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Registro()
        {
            if ((string)Session["documento"] != null && Session["nombre"] != null) {
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View("Registro");
        }

        //[HttpPost]
        //public ActionResult Registro(Usuario unUsuario)
        //{
        //    if (unUsuario.VerificoPass(unUsuario.Password))
        //    {
        //        RepositorioUsuario repoUsuario = new RepositorioUsuario();
        //        Usuario usuario = repoUsuario.FindById(unUsuario.Documento);

        //        if (usuario.Documento != null)
        //        {
        //            ModelState.AddModelError("documento", "El documento ya está registrado");
        //        }
        //        else if (repoUsuario.Add(unUsuario))
        //        {
        //            Session["documento"] = unUsuario.Documento;
        //            Session["nombre"] = usuario.Nombre;
        //            return RedirectToAction("IndexAuth", "Vacuna");
        //        }
        //        else
        //        {
        //            View("Registro");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("password", "Contraseña débil");
        //    }

        //    return View("Registro");
        //}

        public ActionResult Login()
        {
            if ((string)Session["documento"] != null && (string)Session["nombre"] != null){
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario unUsuario)
        { 
            if (ModelState.IsValid)
            {
                if (unUsuario.VerificoPass(unUsuario.Password))
                {
                    RepositorioUsuario repoUsuario = new RepositorioUsuario();
                    Usuario usuario = repoUsuario.Login(unUsuario);

                    if (usuario.Documento != null)
                    {
                        Session["documento"] = usuario.Documento;
                        Session["nombre"] = usuario.Nombre;
                        return RedirectToAction("IndexAuth", "Vacuna");
                    }
                    else
                    {
                        ModelState.AddModelError("password", "Documento y/o contraseña incorrectos");
                    }
                }
                else
                {
                    ModelState.AddModelError("password", "Contraseña débil");
                }
            }

            return View();
        }

        public ActionResult Salir()
        {
            Session["documento"] = null;
            Session["nombre"] = null;
            return RedirectToAction("Index", "Vacuna");
        }

        public ActionResult GenerarArchivos()
        {
            if ((string)Session["documento"] != null && Session["nombre"] != null)
            {
                AccesoArchivo.GenerarArchivos();
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View("Login");
        }
    }
}