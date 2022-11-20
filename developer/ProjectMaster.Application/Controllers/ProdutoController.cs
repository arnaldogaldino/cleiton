using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Application.Models;
using ProjectMaster.Bussiness.DataModels;
using ProjectMaster.Data;
using ProjectMaster.Core.Extensions;
using System.Collections;
using MvcJqGrid;


namespace ProjectMaster.Application.Controllers
{
    public class ProdutoController : DropDownController
    {
        private readonly Produto produto;

        public ProdutoController()
        {
            produto = new Produto();
        }

        public ActionResult Procurar()
        {
            return View();
        }

        public ActionResult Editar(string cprod)
        {
            var pmProduto = produto.PegarProduto(cprod);

            if (pmProduto != null)
            {
                ProdutoModel produtoModel = ExtensionMethods.ToObjects<ProdutoModel>(pmProduto);
                return View("Novo", pmProduto);
            }
            return View("Novo");
        }
        public ActionResult Novo()
        {
            CarregarTipoEmbalagem();
            CarregarTipoEan();
            CarregarCst();
            CarregarCstOrigem();
            CarregarModBc();
            CarregarModBcSt();

            return View();
        }

        public JsonResult Deletar(string cprod)
        {
            var msg = string.Empty;
            try
            {
                produto.Deletar(cprod);
                msg = "Registro deletado com sucesso.";
            }
            catch
            {
                msg = "Ocorreu um erro ao deletar pessoa.";
            }
            return Json(new { msg = msg });
        }

        public JsonResult ProcurarProduto(GridSettings gridSettings)
        {
            var produtosFiltradas = produto.PegarProduto(gridSettings);
            var totalDeProdutos = produto.ContarProduto(gridSettings);
            var jsonData = new
            {
                total = totalDeProdutos / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalDeProdutos,
                rows = (
                    from produtos in produtosFiltradas.ToList()
                    select new
                    {
                        id = produtos.cprod,
                        cell = new[]
                    {
                        produtos.xprod
                    }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Salvar(ProdutoModel produtoModel)
        {
            var msg = "Registro salvo com sucesso";
            string cprod = "";
            try
            {
                pm_produto pmProduto = ExtensionMethods.ToObjects<pm_produto>(produtoModel);

                produto.Salvar(pmProduto);
                cprod = pmProduto.cprod;
            }
            catch (Exception ex)
            {
                msg = string.Format("Ocorreu um erro ao salvar registro: {0}", ex.Message);
            }

            return Json(new { cprod = cprod, msg = msg });
        }
    }
}
