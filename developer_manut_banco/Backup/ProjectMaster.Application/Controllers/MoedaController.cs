using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Application.Models;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Controllers
{
    public class MoedaController : ControllerMaster
    {
        Moeda bMoeda = new Moeda();

        [HttpGet]
        public ActionResult Index(string acao, long? id_moeda)
        {
            var oMoeda = new Moeda();

            if (acao == "Delete" && id_moeda != null)
            {
                pm_moeda adoMoeda = bMoeda.GetMoedaById((long)id_moeda);
                var success = bMoeda.MoedaExcluir(adoMoeda);
            }

            ViewData["queryMoeda"] = oMoeda.GetMoedaGrid();

            return View();
        }

        [HttpGet]
        public ActionResult Moeda(long? id_moeda, string acao)
        {
            var moeda = new MoedaModels();
            var oMoeda = new Moeda();

            pm_moeda adoMoeda = new pm_moeda();

            if (id_moeda != null)
                adoMoeda = oMoeda.GetMoedaById((long)id_moeda);
            else
                adoMoeda.dt_cotacao = DateTime.Now;

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<MoedaModels>(adoMoeda));
        }

        [HttpPost]
        public ActionResult Moeda(MoedaModels form)
        {
            pm_moeda adoMoeda = new pm_moeda();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_moeda != 0)
            {
                if (ModelState.IsValid)
                {
                    adoMoeda = ExtensionMethods.ToObjects<pm_moeda>(form);
                    result = bMoeda.MoedaEditar(ref adoMoeda);
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
                    adoMoeda = ExtensionMethods.ToObjects<pm_moeda>(form);
                    adoMoeda.dt_cotacao = DateTime.Now;
                    result = bMoeda.MoedaCadastrar(ref adoMoeda);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<MoedaModels>(adoMoeda);
            }

            return View(form);
        }

        private bool ValidaForm(MoedaModels form)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(form.dm_tipo_moeda))
                ModelState.AddModelError("dm_tipo_moeda", "Campo (Tipo de moeda) é obrigatório.");

            if (form.nr_valor == 0)
                ModelState.AddModelError("nr_valor", "Campo (Valor) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_moeda adoMoeda = bMoeda.GetMoedaById(id);

            var success = bMoeda.MoedaExcluir(adoMoeda);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse moeda pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }

    }
}
