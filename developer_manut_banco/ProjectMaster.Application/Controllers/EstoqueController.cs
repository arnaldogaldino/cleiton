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
    public class EstoqueController : ControllerMaster
    {
        Estoque bEstoque = new Estoque();

        [HttpGet]
        public ActionResult Index(string acao, long? id_produto_estoque_entrada)
        {
            if (acao == "Delete" && id_produto_estoque_entrada != null)
            {
                pm_produto_estoque_entrada adoEstoque = bEstoque.GetProdutoEstoqueEntradaById((long)id_produto_estoque_entrada);
                var success = bEstoque.ProdutoEstoqueEntradaEditar(ref adoEstoque);
            }

            ViewData["acao"] = acao;
            ViewData["query"] = null;
            EstoqueModels form = new EstoqueModels();
            return View(form);
        }
        
        [HttpPost]
        public ActionResult Index(EstoqueModels form)
        {
            ViewData["query"] = bEstoque.GetProdutoEstoqueEntradaGrid(form.id_produto, form.id_pessoa, form.descricao, form.dta_inicio, form.dta_fim);

            return View(form);
        }

        [HttpGet]
        public ActionResult Estoque(long? id_produto_estoque_entrada, string acao)
        {
            pm_produto_estoque_entrada adoEstoque = new pm_produto_estoque_entrada();

            if (id_produto_estoque_entrada != null)
                adoEstoque = bEstoque.GetProdutoEstoqueEntradaById((long)id_produto_estoque_entrada);

            ViewData["acao"] = acao;

            EstoqueModels form = new EstoqueModels();
            form = ExtensionMethods.ToObjects<EstoqueModels>(adoEstoque);

            if (adoEstoque.pm_produto != null)
            {
                form.cprod = adoEstoque.pm_produto.cprod;
                form.xprod = adoEstoque.pm_produto.xprod;
            }

            if (adoEstoque.pm_pessoa != null)
            {
                form.ds_marca = adoEstoque.pm_pessoa.ds_marca;
                form.ds_razao_social = adoEstoque.pm_pessoa.ds_razao_social;
            }

            return View(form);
        }

        [HttpPost]
        public ActionResult Estoque(EstoqueModels form)
        {
            pm_produto_estoque_entrada adoEstoque = new pm_produto_estoque_entrada();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_produto_estoque_entrada!= 0)
            {
                if (ModelState.IsValid)
                {
                    adoEstoque = ExtensionMethods.ToObjects<pm_produto_estoque_entrada>(form);
                    result = bEstoque.ProdutoEstoqueEntradaEditar(ref adoEstoque);
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
                    adoEstoque = ExtensionMethods.ToObjects<pm_produto_estoque_entrada>(form);
                    result = bEstoque.ProdutoEstoqueEntradaCadastrar(ref adoEstoque);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<EstoqueModels>(adoEstoque);
            }

            if (adoEstoque.pm_produto != null)
            {
                form.cprod = adoEstoque.pm_produto.cprod;
                form.xprod = adoEstoque.pm_produto.xprod;
            }

            if (adoEstoque.pm_pessoa != null)
            {
                form.ds_marca = adoEstoque.pm_pessoa.ds_marca;
                form.ds_razao_social = adoEstoque.pm_pessoa.ds_razao_social;
            }

            return View(form);
        }

        public ActionResult EfetivaEstoque(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string[] lista = ids.Split(',');

                foreach (string id in lista)
                {
                    pm_produto_estoque_entrada estoque = new pm_produto_estoque_entrada();
                    estoque = bEstoque.GetProdutoEstoqueEntradaById(long.Parse(id));

                    if (estoque.dm_tipo == "E")
                    {
                        bEstoque.IncrementaQuantidadeEstoque(estoque.id_produto, estoque.quantidade);
                        bEstoque.EstoqueHitorico(estoque.id_produto_estoque_entrada, null, estoque.id_produto, estoque.quantidade);
                    }

                    if (estoque.dm_tipo == "S")
                    {
                        bEstoque.DecrementaQuantidadeEstoque(estoque.id_produto, estoque.quantidade);
                        bEstoque.EstoqueHitorico(estoque.id_produto_estoque_entrada, null, estoque.id_produto, (estoque.quantidade *(-1)));
                    }
                    
                    estoque.bl_efetivo = true;

                    bEstoque.ProdutoEstoqueEntradaEditar(ref estoque);
                }
            }

            return RedirectToAction("Index", "Estoque");
        }
        
        private bool ValidaForm(EstoqueModels form)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(form.dm_tipo))
                ModelState.AddModelError("dm_tipo", "Campo (Tipo) é obrigatório.");

            if (form.dta_entrada == null)
                ModelState.AddModelError("dta_entrada", "Campo (Data Entrada) é obrigatório.");

            if (form.dta_doc == null)
                ModelState.AddModelError("dta_doc", "Campo (Data Documento) é obrigatório.");

            if (form.quantidade == 0)
                ModelState.AddModelError("quantidade", "Campo (Quantidade) é obrigatório.");

            if (form.id_pessoa == 0)
                ModelState.AddModelError("id_pessoa", "Campo (Empresa) é obrigatório.");

            if (form.id_produto == 0)
                ModelState.AddModelError("id_produto", "Campo (Produto) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_produto_estoque_entrada adoEstoque = bEstoque.GetProdutoEstoqueEntradaById(id);

            var success = bEstoque.ProdutoEstoqueEntradaExcluir(adoEstoque);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir essa entrada / saida de estoque pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowHistoricoEstoque(long id_produto)
        {
            return View(bEstoque.GetProdutoEstoqueHistoricoGrid(id_produto));
        }
    }    
}
