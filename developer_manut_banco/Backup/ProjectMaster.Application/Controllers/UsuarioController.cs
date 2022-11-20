using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Application.Models;
using ProjectMaster.Bussiness;
using System.Threading;
using ProjectMaster.Core;
using ProjectMaster.Data;

namespace ProjectMaster.Application.Controllers
{
    public class UsuarioController : Controller
    {

        public ActionResult Entrar()
        {
            //System.Web.HttpBrowserCapabilities browser = 
            //    Request.Browser;

            return View();
        }

        [HttpPost]
        public ActionResult Entrar(UsuarioModels model)
        {
            Context.UsuarioContext context = new Context.UsuarioContext();
            var oUsuario = new Usuario();
            var oEmpresa = new Empresa();
            var oFilial = new Filial();
            
            context.Usuario = oUsuario.GetUsuarioByUsuarioSenha(model.login, model.senha);
            context.Empresa = oEmpresa.GetEmpresaByID(model.empresa);
            context.Filial = oFilial.GetFilialByID(model.filial);
            
            if (context.Empresa == null)
            {
                if(((pm_usuario)context.Usuario).pm_filial.Count==1)
                {
                    context.Empresa = ((pm_usuario)context.Usuario).pm_filial.First().pm_empresa;
                    context.Filial = ((pm_usuario)context.Usuario).pm_filial.First();
                }
            }

            context.Domain = oUsuario.GetDomain();

            context.MenuHTML = oUsuario.GetMenuByIdUsuario(context.Usuario.id_usuario);

            Context.AddUsuarioOnLine(context);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Sair()
        {
            Context.Logoff();
            return RedirectToAction("Entrar", "Usuario");
        }

        [HttpGet]
        public JsonResult GetFilialByUsuarioSenha(string user, string password, int id_empresa)
        {
            var oUsuario = new Usuario();
            var oEmpresa = new Empresa();
            var oFilial = new Filial();

            //var usuario = oUsuario.GetUsuarioByUsuarioSenha(user, password);
            //var empresa = oEmpresa.GetEmpresaByUsuarioSenha(user, password).Distinct();
            var filial = oFilial.GetFilialByUsuarioSenha(user, password, id_empresa);
            
            return this.Json(
                    new
                    {
                        success = true,
                        results = filial.Select(o => new { o.id_filial, o.apelido })                        
                    }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmpresaByUsuarioSenha(string user, string password)
        {
            var oUsuario = new Usuario();
            var oEmpresa = new Empresa();
            var oFilial = new Filial();

            var usuario = oUsuario.GetUsuarioByUsuarioSenha(user, password);
            var empresa = oEmpresa.GetEmpresaByUsuarioSenha(user, password);
            
            return this.Json(
                    new
                    {
                        success = (usuario != null ? true : false),
                        login_falha = (usuario != null ? "" : "Usuario e senha inválido."),
                        results = empresa.Select(o => new { o.id_empresa, o.apelido }).Distinct()
                    }, JsonRequestBehavior.AllowGet);
        }

    }
}
