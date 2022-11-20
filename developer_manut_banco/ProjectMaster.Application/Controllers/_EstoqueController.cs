using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;
using Microsoft.Reporting.WebForms;
using ProjectMaster.Application.Reports.DataSets;

namespace ProjectMaster.Application.Controllers
{
    public class _EstoqueController : ControllerMaster
    {
        Estoque bEstoque = new Estoque(); 

        [HttpGet]
        public ActionResult Index()
        {
            EstoqueModels form = new EstoqueModels();
            return View(form);
        }

        [HttpPost]
        public ActionResult Index(EstoqueModels form)
        {
            ViewData["queryProdutoEstoque"] = bEstoque.GetProdutoEstoqueEntradaGrid(form.id_produto, form.id_pessoa, form.descricao, form.dta_inicio, form.dta_fim);

            return View(form);
        }

        [HttpGet]
        public ActionResult Estoque(long? id_produto_estoque_entrada, string acao)
        {
            var estoque = new EstoqueModels();

            pm_produto_estoque_entrada adoEstoque = new pm_produto_estoque_entrada();

            if (id_produto_estoque_entrada != null)
                adoEstoque = bEstoque.GetProdutoEstoqueEntradaById((long)id_produto_estoque_entrada);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<EstoqueModels>(adoEstoque));
        }

        [HttpPost]
        public ActionResult Estoque(EstoqueModels form)
        {
            pm_produto_estoque_entrada adoEstoque = new pm_produto_estoque_entrada();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (ModelState.IsValid)
            {
                adoEstoque = ExtensionMethods.ToObjects<pm_produto_estoque_entrada>(form);
                result = bEstoque.ProdutoEstoqueEntradaCadastrar(ref adoEstoque);
            }
            else
                result = false;

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<EstoqueModels>(adoEstoque);
            }

            return View(form);
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
    }
}

