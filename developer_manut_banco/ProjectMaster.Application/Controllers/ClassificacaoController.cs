using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Controllers
{
    public class ClassificacaoController : ControllerMaster
    {
        Classificacao bClassificacao = new Classificacao();

        [HttpGet]
        public ActionResult Index(string acao, long? id_classificacao)
        {
            var oClassificacao = new Classificacao();

            if (acao == "Delete" && id_classificacao != null)
            {
                pm_classificacao adoClassificacao = bClassificacao.GetClassificacaoById((long)id_classificacao);
                var success = bClassificacao.ClassificacaoExcluir(adoClassificacao);
            }

            ViewData["queryClassificacao"] = oClassificacao.GetClassificacaoGrid();

            return View();
        }

        [HttpGet]
        public ActionResult Classificacao(long? id_classificacao, string acao)
        {
            var classificacao = new ClassificacaoModels();
            var oClassificacao = new Classificacao();

            pm_classificacao adoClassificacao = new pm_classificacao();

            if (id_classificacao != null)
                adoClassificacao = oClassificacao.GetClassificacaoById((long)id_classificacao);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<ClassificacaoModels>(adoClassificacao));
        }

        [HttpPost]
        public ActionResult Classificacao(ClassificacaoModels form)
        {
            pm_classificacao adoClassificacao = new pm_classificacao();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_classificacao != 0)
            {
                if (ModelState.IsValid)
                {
                    adoClassificacao = ExtensionMethods.ToObjects<pm_classificacao>(form);
                    adoClassificacao.id_filial = Context.idFilial;
                    result = bClassificacao.ClassificacaoEditar(ref adoClassificacao);
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
                    adoClassificacao = ExtensionMethods.ToObjects<pm_classificacao>(form);
                    result = bClassificacao.ClassificacaoCadastrar(ref adoClassificacao);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<ClassificacaoModels>(adoClassificacao);
            }

            return View(form);
        }

        private bool ValidaForm(ClassificacaoModels form)
        {
            ModelState.Clear();

            if (bClassificacao.GetClassificacaoByCodigo(form.id_classificacao ,form.ds_codigo) != null)
                ModelState.AddModelError("ds_codigo", "Classificação de operação já cadastrado.");

            if (string.IsNullOrEmpty(form.ds_codigo))
                ModelState.AddModelError("ds_codigo", "Campo (Código) é obrigatório.");

            if (string.IsNullOrEmpty(form.ds_descricao))
                ModelState.AddModelError("ds_descricao", "Campo (Descrição) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_classificacao adoClassificacao = bClassificacao.GetClassificacaoById(id);

            var success = bClassificacao.ClassificacaoExcluir(adoClassificacao);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse classificacao pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
