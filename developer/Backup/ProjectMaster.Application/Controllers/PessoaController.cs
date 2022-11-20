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
    public class PessoaController : DropDownController
    {
        private readonly Pessoa pessoa;

        public PessoaController()
        {
            pessoa = new Pessoa();
        }
        public ActionResult Procurar()
        {
            CarregarViewBagTipoCliente();
            CarregarViewBagTipoDocumento();
            return View();
        }

        public ActionResult Novo()
        {
            return View();
        }


        public JsonResult ProcurarPessoas(GridSettings gridSettings)
        {
            var pessoasFiltradas = pessoa.Procurar(new pm_pessoa());


            var jsonData = new
            {
                total = pessoasFiltradas.Count() / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = pessoasFiltradas.Count(),
                rows = (
                    from pessoas in pessoasFiltradas.ToList()
                    select new
                    {
                        id = pessoas.id_pessoa,
                        cell = new []
                    {
                        pessoas.id_pessoa.ToString(),
                        pessoas.ds_razao_social,
                        pessoas.ds_fisico_juridico == "F" ? "CPF" : "CNPJ",
                        pessoas.nr_documento,
                        pessoas.nr_ie,
                        pessoas.dm_tipo_pessoa == "C" ? "Cliente" : "Fornecedor"
                    }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
