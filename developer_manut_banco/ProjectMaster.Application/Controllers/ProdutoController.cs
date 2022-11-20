using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Bussiness;
using ProjectMaster.Core;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;

namespace ProjectMaster.Application.Controllers
{
    public class ProdutoController : ControllerMaster
    {
        Produto bProduto = new Produto();

        [HttpGet]
        public ActionResult Index(string acao, long? id_produto)
        {
            if (acao == "Delete" && id_produto != null)
            {
                pm_produto adoProduto = bProduto.GetProdutoByID((long)id_produto);
                var success = bProduto.ProdutoExcluir(adoProduto);
            }

            ViewData["acao"] = acao;
            ViewData["queryProduto"] = null;
            ProdutoModels form = new ProdutoModels();
            return View(form);
        }

        [HttpPost]
        public ActionResult Index(ProdutoModels form)
        {
            ViewData["queryProduto"] = bProduto.GetProdutoGrid(form.cprod, form.xprod);
            return View(form);
        }

        [HttpGet]
        public ActionResult Produto(long? id_produto, string acao)
        {
            var produto = new ProdutoModels();

            pm_produto adoProduto = new pm_produto();

            if (id_produto != null)
                adoProduto = bProduto.GetProdutoByID((long)id_produto);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<ProdutoModels>(adoProduto));
        }

        [HttpPost]
        public ActionResult Produto(ProdutoModels form)
        {
            pm_produto adoProduto = new pm_produto();

            if (!ValidaForm(form))
                return View(form);

            bool result = false;

            if (form.id_produto != 0)
            {
                if (ModelState.IsValid)
                {
                    adoProduto = ExtensionMethods.ToObjects<pm_produto>(form);

                    result = bProduto.ProdutoEditar(ref adoProduto);
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
                    Context.UsuarioContext context = new Context.UsuarioContext();

                    adoProduto = ExtensionMethods.ToObjects<pm_produto>(form);

                    adoProduto.id_filial = Context.FilialLogin.id_filial;

                    result = bProduto.ProdutoCadastrar(ref adoProduto);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<ProdutoModels>(adoProduto);
            }

            return View(form);
        }

        private bool ValidaForm(ProdutoModels form)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(form.cprod))
                ModelState.AddModelError("cprod", "Campo (Código do Produto) é obrigatório.");

            if (string.IsNullOrEmpty(form.xprod))
                ModelState.AddModelError("xprod", "Campo (Descrição) é obrigatório.");

            if (form.id_embalagem == 0)
                ModelState.AddModelError("id_embalagem", "Campo (Valor) é obrigatório.");

            if (string.IsNullOrEmpty(form.tp_embalagem))
                ModelState.AddModelError("tp_embalagem", "Campo (Tipo de Embalagem) é obrigatório.");
            
            //if (string.IsNullOrEmpty(form.cean))
            //    ModelState.AddModelError("cean", "Campo (Código EAN) é obrigatório.");

            if (form.id_ncm == 0)
                ModelState.AddModelError("id_ncm", "Campo (Código NCM) é obrigatório.");

            if (string.IsNullOrEmpty(form.cst))
                ModelState.AddModelError("cst", "Campo (Tributação do ICMS) é obrigatório.");

            if (string.IsNullOrEmpty(form.orig))
                ModelState.AddModelError("orig", "Campo (Origem da Mercadoria) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_produto adoProduto = bProduto.GetProdutoByID(id);

            var success = bProduto.ProdutoExcluir(adoProduto);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse produto pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Lookup(string cprod)
        {
            IQueryable<pm_produto> adoProduto = bProduto.GetProdutoByCprod(cprod);

            var success = true;

            if (adoProduto == null || adoProduto.Count() == 0)
                success = false;

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Nenhuma código encontrada!" : ""),
                        list = (success ? adoProduto.Select(o => new { id_produto = o.id_produto, cprod = o.cprod, xprod = o.xprod, valor_unitario = o.vuncom, tp_embalagem = o.tp_embalagem }) : null)
                    }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LookupScreen()
        {
            ProdutoModels form = new ProdutoModels();
            ViewData["queryPessoa"] = null;
            return View(form);
        }

        [HttpPost]
        public ActionResult LookupScreen(ProdutoModels form)
        {
            ViewData["queryProduto"] = bProduto.GetProdutoByCprod(form.cprod);
            return View(form);
        }
    }
}
