using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;

namespace ProjectMaster.Application.Controllers
{
    public class NCMController : ControllerMaster
    {
        NCM bNCM = new NCM();

        [HttpGet]
        public ActionResult Index(string acao, long? id_ncm)
        {
            var oNCM = new NCM();

            if (acao == "Delete" && id_ncm != null)
            {
                pm_ncm adoNCM = bNCM.GetNCMById((long)id_ncm);
                var success = bNCM.NCMExcluir(adoNCM);
            }

            ViewData["queryNCM"] = oNCM.GetNCMGrid();

            return View();
        }

        [HttpGet]
        public ActionResult NCM(long? id_ncm, string acao)
        {
            var ncm = new NCMModels();
            var oNCM = new NCM();

            pm_ncm adoNCM = new pm_ncm();

            if (id_ncm != null)
                adoNCM = oNCM.GetNCMById((long)id_ncm);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<NCMModels>(adoNCM));
        }

        [HttpPost]
        public ActionResult NCM(NCMModels form)
        {
            pm_ncm adoNCM = new pm_ncm();
            bool result = false;
            
            if(!ValidaForm(form))
                return View(form);

            if (form.id_ncm != 0)
            {
                if (ModelState.IsValid)
                {
                    adoNCM = ExtensionMethods.ToObjects<pm_ncm>(form);
                    result = bNCM.NCMEditar(ref adoNCM);
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
                    adoNCM = ExtensionMethods.ToObjects<pm_ncm>(form);
                    result = bNCM.NCMCadastrar(ref adoNCM);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<NCMModels>(adoNCM);
            }

            return View(form);
        }

        private bool ValidaForm(NCMModels form)
        {
            ModelState.Clear();

            if (bNCM.NCMExistDiferente(form.id_ncm, form.ncm) != null)
                ModelState.AddModelError("NCM", "NCM já cadastrado.");

            if (string.IsNullOrEmpty(form.ncm))
                ModelState.AddModelError("NCM", "Campo (NCM) é obrigatório.");

            if (string.IsNullOrEmpty(form.ds_descricao))
                ModelState.AddModelError("ds_descricao", "Campo (Descrição) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_ncm adoNCM = bNCM.GetNCMById(id);

            var success = bNCM.NCMExcluir(adoNCM);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse ncm pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
