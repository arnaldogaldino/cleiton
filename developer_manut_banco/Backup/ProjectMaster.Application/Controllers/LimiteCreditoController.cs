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
    public class LimiteCreditoController : ControllerMaster
    {
        LimiteCredito bLimiteCredito = new LimiteCredito();

        [HttpGet]
        public ActionResult Index(string acao, long? id_limite_credito)
        {
            var oLimiteCredito = new LimiteCredito();

            if (acao == "Delete" && id_limite_credito != null)
            {
                pm_limite_credito adoLimiteCredito = bLimiteCredito.GetLimiteCreditoById((long)id_limite_credito);
                var success = bLimiteCredito.LimiteCreditoExcluir(adoLimiteCredito);
            }

            ViewData["queryLimiteCredito"] = oLimiteCredito.GetLimiteCreditoGrid();

            return View();
        }

        [HttpGet]
        public ActionResult LimiteCredito(long? id_limite_credito, string acao)
        {
            var limite_credito = new LimiteCreditoModels();
            var oLimiteCredito = new LimiteCredito();

            pm_limite_credito adoLimiteCredito = new pm_limite_credito();

            if (id_limite_credito != null)
                adoLimiteCredito = oLimiteCredito.GetLimiteCreditoById((long)id_limite_credito);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<LimiteCreditoModels>(adoLimiteCredito));
        }

        [HttpPost]
        public ActionResult LimiteCredito(LimiteCreditoModels form)
        {
            pm_limite_credito adoLimiteCredito = new pm_limite_credito();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_limite_credito != 0)
            {
                if (ModelState.IsValid)
                {
                    adoLimiteCredito = ExtensionMethods.ToObjects<pm_limite_credito>(form);
                    result = bLimiteCredito.LimiteCreditoEditar(ref adoLimiteCredito);
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
                    adoLimiteCredito = ExtensionMethods.ToObjects<pm_limite_credito>(form);
                    result = bLimiteCredito.LimiteCreditoCadastrar(ref adoLimiteCredito);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<LimiteCreditoModels>(adoLimiteCredito);
            }

            return View(form);
        }

        private bool ValidaForm(LimiteCreditoModels form)
        {
            ModelState.Clear();

            if (bLimiteCredito.GetLimiteCreditoByCodigo(form.ds_codigo) != null)
                ModelState.AddModelError("ds_codigo", "Limite de Crédito já cadastrado.");

            if (string.IsNullOrEmpty(form.ds_codigo))
                ModelState.AddModelError("ds_codigo", "Campo (Código) é obrigatório.");

            if (string.IsNullOrEmpty(form.ds_descricao))
                ModelState.AddModelError("ds_descricao", "Campo (Descrição) é obrigatório.");

            if (form.dias_credito == 0)
                ModelState.AddModelError("dias_credito", "Campo (Dias de Crédito) é obrigatório.");

            if (form.valor_credito == 0)
                ModelState.AddModelError("valor_credito", "Campo (Valor de Crédito) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_limite_credito adoLimiteCredito = bLimiteCredito.GetLimiteCreditoById(id);

            var success = bLimiteCredito.LimiteCreditoExcluir(adoLimiteCredito);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse histórico pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
