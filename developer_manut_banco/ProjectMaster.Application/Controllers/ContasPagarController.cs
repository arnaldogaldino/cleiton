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
    public class ContasPagarController : Controller
    {
        ContasPagar bContasPagar = new ContasPagar();

        [HttpGet]
        public ActionResult Index(string acao, long? id_conta_pagar)
        {
            if (acao == "Delete" && id_conta_pagar != null)
            {
                pm_conta_pagar adoContasPagar = bContasPagar.GetContasPagarById((long)id_conta_pagar);
                var success = bContasPagar.ContasPagarExcluir(adoContasPagar);
            }

            ViewData["query"] = null;

            ContaPagarModels form = new ContaPagarModels();

            return View(form);
        }

        [HttpPost]
        public ActionResult Index(ContaPagarModels form)
        {            
            long? id = null;
            string fornecedor = string.Empty;
            DateTime? vencimento_de = null;
            DateTime? vencimento_ate = null;

            if (form.id_conta_pagar != 0)
                id = form.id_conta_pagar;
            
            if (!string.IsNullOrEmpty(form.ds_razao_social))
                fornecedor = form.ds_razao_social;

            if (vencimento_de != DateTime.MinValue)
                vencimento_de = form.dta_vencimento;

            if (form.dta_emissao != DateTime.MinValue)
                vencimento_ate = form.dta_emissao;

            ViewData["query"] = bContasPagar.GetContasPagarGrid(id, fornecedor, vencimento_de, vencimento_ate);

            return View(form);
        }

        [HttpGet]
        public ActionResult ContasPagar(long? id_conta_pagar, string acao)
        {
            var conta_pagar = new ContaPagarModels();
            var oContasPagar = new ContasPagar();

            pm_conta_pagar adoContasPagar = new pm_conta_pagar();

            if (id_conta_pagar != null)
                adoContasPagar = oContasPagar.GetContasPagarById((long)id_conta_pagar);

            ViewData["acao"] = acao;

            var form = ExtensionMethods.ToObjects<ContaPagarModels>(adoContasPagar);
            
            if (adoContasPagar.pm_pessoa != null)
            {
                form.ds_razao_social = adoContasPagar.pm_pessoa.ds_razao_social;
                form.ds_marca = adoContasPagar.pm_pessoa.ds_marca;
            }

            return View(form);
        }

        [HttpPost]
        public ActionResult ContasPagar(ContaPagarModels form)
        {
            pm_conta_pagar adoContasPagar = new pm_conta_pagar();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_conta_pagar != 0)
            {
                if (ModelState.IsValid)
                {
                    adoContasPagar = ExtensionMethods.ToObjects<pm_conta_pagar>(form);
                    result = bContasPagar.ContasPagarEditar(ref adoContasPagar);
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
                    adoContasPagar = ExtensionMethods.ToObjects<pm_conta_pagar>(form);
                    result = bContasPagar.ContasPagarCadastrar(ref adoContasPagar);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<ContaPagarModels>(adoContasPagar);

                if (adoContasPagar.pm_pessoa != null)
                {
                    form.ds_marca = adoContasPagar.pm_pessoa.ds_marca;
                    form.ds_razao_social = adoContasPagar.pm_pessoa.ds_razao_social;
                }
            }

            return View(form);
        }

        private bool ValidaForm(ContaPagarModels form)
        {
            ModelState.Clear();

            if (form.id_pessoa == 0)
                ModelState.AddModelError("id_pessoa", "Campo (Fornecedor) é obrigatório.");

            if (form.id_centro_de_custo == 0)
                ModelState.AddModelError("id_centro_de_custo", "Campo (Centro de Custo) é obrigatório.");

            if (form.dta_emissao == DateTime.MinValue)
                ModelState.AddModelError("dta_emissao", "Campo (Data de Emissão) é obrigatório.");

            if (form.dta_vencimento == DateTime.MinValue)
                ModelState.AddModelError("dta_vencimento", "Campo (Data de Vencimento) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_conta_pagar adoContasPagar = bContasPagar.GetContasPagarById(id);

            var success = bContasPagar.ContasPagarExcluir(adoContasPagar);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse centro de custo pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }

    }
}
