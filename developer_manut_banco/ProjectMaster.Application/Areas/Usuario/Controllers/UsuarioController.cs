using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Application.Areas.Usuario.Models;

namespace ProjectMaster.Application.Areas.Usuario.Controllers
{
    public class UsuarioController : Controller
    {

        [HttpGet]
        public ActionResult Entrar()
        {
            return View(new UsuarioModel());
        }

        [HttpPost]
        public ActionResult Entrar(UsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                //Employee.Add(vm.Employee);
                return View("Index"); // or wherever you go after successful add
            }

            return View(model);
        }

    }
}
