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
            CarregarTipoCliente();
            CarregarTipoDocumento();
            return View();
        }
        public ActionResult Editar(Int64 id)
        {
            CarregarTipoCliente();
            CarregarTipoDocumento();
            CarregarOpcaoDeAceitacao();
            CarregarLimiteDeCredito();
            pm_pessoa pmPessoa = pessoa.PegarPessoa(id);
            if (pmPessoa != null)
            {
                PessoaModel pessoaModel = ExtensionMethods.ToObjects<PessoaModel>(pmPessoa);
                return View("Novo", pessoaModel);
            }
            return View("Novo");
        }
        public ActionResult Novo()
        {
            CarregarTipoCliente();
            CarregarTipoDocumento();
            CarregarOpcaoDeAceitacao();
            CarregarLimiteDeCredito();
            return View();
        }

        public JsonResult Deletar(Int64 id)
        {
            var msg = string.Empty;
            try
            {
                pessoa.Deletar(id);
                msg = "Registro deletado com sucesso.";
            }
            catch
            {
                msg = "Ocorreu um erro ao deletar pessoa.";
            }
            return Json(new { msg = msg });
        }

        public JsonResult ProcurarPessoas(GridSettings gridSettings)
        {
            var pessoasFiltradas = pessoa.PegarPessoas(gridSettings);
            var totalDePessoas = pessoa.ContarPessoas(gridSettings);
            var jsonData = new
            {
                total = totalDePessoas / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalDePessoas,
                rows = (
                    from pessoas in pessoasFiltradas.ToList()
                    select new
                    {
                        id = pessoas.id_pessoa,
                        cell = new[]
                    {
                        pessoas.id_pessoa.ToString(),
                        pessoas.ds_marca,
                        pessoas.ds_razao_social,
                        pessoas.ds_fisico_juridico == "C" ? "CPF" : "CNPJ",
                        pessoas.nr_documento,
                        pessoas.nr_ie,
                        pessoas.dm_tipo_pessoa == "C" ? "Cliente" : "Fornecedor",
                        pessoas.id_pessoa.ToString()
                    }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Salvar(PessoaModel pessoaModel)
        {
            var msg = "Registro salvo com sucesso";
            long idPessoa = 0;
            try
            {
                pm_pessoa pmPessoa = ExtensionMethods.ToObjects<pm_pessoa>(pessoaModel);
                foreach (TelefoneModel telefone in pessoaModel.telefones)
                {
                    pm_telefone pmTelefone = ExtensionMethods.ToObjects<pm_telefone>(telefone);
                    pmPessoa.pm_telefone.Add(pmTelefone);
                }
                foreach (EnderecoModel endereco in pessoaModel.enderecos)
                {
                    pm_endereco pmEndereco = ExtensionMethods.ToObjects<pm_endereco>(endereco);
                    pmEndereco.id_pais = Convert.ToInt64(endereco.id_pais);
                    pmPessoa.pm_endereco.Add(pmEndereco);
                }

                foreach (ContaCorrenteModel contaCorrente in pessoaModel.contas_correntes)
                {
                    pm_conta_corrente pmContaCorrente = ExtensionMethods.ToObjects<pm_conta_corrente>(contaCorrente);
                    pmContaCorrente.id_banco = Convert.ToInt64(contaCorrente.id_banco);
                    pmPessoa.pm_conta_corrente.Add(pmContaCorrente);
                }

                pessoa.Salvar(pmPessoa);
                idPessoa = pmPessoa.id_pessoa;
            }
            catch (Exception ex)
            {
                msg = string.Format("Ocorreu um erro ao salvar registro: {0}", ex.Message);
            }

            return Json(new { idPessoa = idPessoa, msg = msg });
        }

    }
}
