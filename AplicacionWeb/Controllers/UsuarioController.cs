using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using Repositorio;
using AplicacionWeb.Models;
using AccesoDatos.Repositorios;

namespace AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
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
            RepositorioUsuario repoUsuario = new RepositorioUsuario();
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
                    RepositorioUsuario repoUsuario = new RepositorioUsuario();
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

        public ActionResult RegistroMutualista()
        {
            if ((string)Session["documento"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Vacuna");
            
        }

        [HttpPost]
        public ActionResult RegistroMutualista(ViewModelMutualista viewModelMutualista)
        {
            if (ModelState.IsValid)
            {
                RepositorioMutualista repoMutualista = new RepositorioMutualista();
                Mutualista mutualista = repoMutualista.FindById(viewModelMutualista.Id);

                if (mutualista != null)
                {
                    ModelState.AddModelError("Id", "Ya existe la mutualista");
                    return View("RegistroMutualista");
                }
                if(repoMutualista.FindByName(viewModelMutualista.Nombre) != null)
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