using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;
using ProjectMaster.Core.Extensions;

namespace ProjectMaster.Application.Controllers
{
    public class MenuController : ControllerMaster
    {
        ProjectMaster.Bussiness.Menu bMenu = new ProjectMaster.Bussiness.Menu();

        [HttpGet]
        public ActionResult Index(string acao, long? id_menu)
        {
            var oMenu = new Menu();

            if (acao == "Delete" && id_menu != null)
            {
                pm_menu adoMenu = bMenu.GetMenuByID((long)id_menu);
                var success = bMenu.MenuExcluir(adoMenu);
            }

            ViewData["queryMenus"] = oMenu.GetMenuGrid();
            return View();
        }

        [HttpGet]
        public ActionResult Menu(long? id_menu, string acao)
        {
            var menu = new MenuModels();
            var oMenu = new Menu();
            
            pm_menu adoMenu = new pm_menu();

            if(id_menu != null)
                adoMenu = oMenu.GetMenuByID((long)id_menu);
            
            ViewData["acao"] = acao;
            
            return View(ExtensionMethods.ToObjects<MenuModels>(adoMenu));
        }

        [HttpPost]
        public ActionResult Menu(MenuModels form)
        {
            pm_menu adoMenu = new pm_menu();
            bool result = false;

            if (form.id_menu != 0)
            {
                if (ModelState.IsValid)
                {
                    adoMenu = ExtensionMethods.ToObjects<pm_menu>(form);
                    result = bMenu.MenuEditar(ref adoMenu);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    adoMenu = ExtensionMethods.ToObjects<pm_menu>(form);
                    result = bMenu.MenuCadastrar(ref adoMenu);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<MenuModels>(adoMenu);
            }
            
            return View(form);
        }
        
        private bool ValidaForm(MenuModels form)
        {
            if (string.IsNullOrEmpty(form.nome))
                ModelState.AddModelError("Nome", "Campo Nome é obrigatório.");

            if (string.IsNullOrEmpty(form.texto))
                ModelState.AddModelError("Texto", "Campo Texto é obrigatório.");

            return ModelState.IsValid;
        }
        
        public JsonResult MenuDelete(long id_menu)
        {
            pm_menu adoMenu = bMenu.GetMenuByID(id_menu);
            
            var success = bMenu.MenuExcluir(adoMenu);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse menu pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
