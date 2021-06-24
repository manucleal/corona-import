using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using Repositorio;
using AplicacionWeb.Models;

namespace AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private RepositorioUsuario repoUsuario = new RepositorioUsuario();

        public ActionResult ResetPassword()
        {
            if ((string)Session["documento"] != null)
            {
                return View();
            }
            return RedirectToAction( "Login", "Usuario");
        }

        [HttpPost]
        public ActionResult ResetPassword(ViewModelUsuario viewModelUsuario)
        {            
            Usuario usuario = repoUsuario.FindById(viewModelUsuario.Documento);
            if (usuario != null)
            {
                if (viewModelUsuario.Password != viewModelUsuario.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden");
                    return View("ResetPassword");
                }
                if (!Usuario.VerificoPass(viewModelUsuario.Password))
                {
                    ModelState.AddModelError("Password", "La contraseña es débil");
                    return View("ResetPassword");
                }

                if (repoUsuario.CambiarPassword(viewModelUsuario.Documento, viewModelUsuario.Password))
                {
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    return View("ResetPassword");
                }
            }
            else
            {
                ModelState.AddModelError("Documento", "El usuario no existe");
                return View("ResetPassword");
            }

        }

        public ActionResult Login()
        {
            if ((string)Session["documento"] != null){
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModelUsuario viewModelUsuario)
        { 
            if (ModelState.IsValid)
            {
                if (Usuario.VerificoPass(viewModelUsuario.Password))
                {
                    Usuario usuario = repoUsuario.Login(ViewModelUsuario.MapearAUsuario(viewModelUsuario));

                    if (usuario.Documento != null)
                    {
                        Session["documento"] = usuario.Documento;
                        //preguntar si es la primera vez que se loguea, si es asi lo mando a resetPass, sino sigo con el flujo normal
                        if (usuario.CantidadLogin < 2)
                        {
                            return RedirectToAction("ResetPassword","Usuario");
                        }
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
            return RedirectToAction("Index", "Vacuna");
        }
    }
}