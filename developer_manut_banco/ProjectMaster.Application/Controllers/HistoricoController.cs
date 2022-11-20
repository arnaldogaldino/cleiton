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
    public class HistoricoController : ControllerMaster
    {
        Historico bHistorico = new Historico();

        [HttpGet]
        public ActionResult Index(string acao, long? id_historico)
        {
            var oHistorico = new Historico();

            if (acao == "Delete" && id_historico != null)
            {
                pm_historico adoHistorico = bHistorico.GetHistoricoById((long)id_historico);
                var success = bHistorico.HistoricoExcluir(adoHistorico);
            }

            ViewData["queryHistorico"] = oHistorico.GetHistoricoGrid();

            return View();
        }

        [HttpGet]
        public ActionResult Historico(long? id_historico, string acao)
        {
            var historico = new HistoricoModels();
            var oHistorico = new Historico();

            pm_historico adoHistorico = new pm_historico();

            if (id_historico != null)
                adoHistorico = oHistorico.GetHistoricoById((long)id_historico);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<HistoricoModels>(adoHistorico));
        }

        [HttpPost]
        public ActionResult Historico(HistoricoModels form)
        {
            pm_historico adoHistorico = new pm_historico();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_historico != 0)
            {
                if (ModelState.IsValid)
                {
                    adoHistorico = ExtensionMethods.ToObjects<pm_historico>(form);
                    adoHistorico.id_filial = Context.idFilial;
                    result = bHistorico.HistoricoEditar(ref adoHistorico);
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
                    adoHistorico = ExtensionMethods.ToObjects<pm_historico>(form);
                    result = bHistorico.HistoricoCadastrar(ref adoHistorico);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<HistoricoModels>(adoHistorico);
            }

            return View(form);
        }

        private bool ValidaForm(HistoricoModels form)
        {
            ModelState.Clear();

            if (bHistorico.GetHistoricoByCodigo(form.id_historico, form.ds_codigo) != null)
                ModelState.AddModelError("ds_codigo", "Histórico já cadastrado.");

            if (string.IsNullOrEmpty(form.ds_codigo))
                ModelState.AddModelError("ds_codigo", "Campo (Código) é obrigatório.");

            if (string.IsNullOrEmpty(form.ds_descricao))
                ModelState.AddModelError("ds_descricao", "Campo (Descrição) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_operacao))
                ModelState.AddModelError("dm_operacao", "Campo (Tipo do Histórico) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_associacao))
                ModelState.AddModelError("dm_associacao", "Campo (Tipo de Situação Financeira) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_codigo_contabil))
                ModelState.AddModelError("dm_codigo_contabil", "Campo (Código Contábil) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_historico adoHistorico = bHistorico.GetHistoricoById(id);

            var success = bHistorico.HistoricoExcluir(adoHistorico);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse histórico pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
