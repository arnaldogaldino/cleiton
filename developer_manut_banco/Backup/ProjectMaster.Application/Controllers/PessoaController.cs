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
    public class PessoaController : ControllerMaster
    {
        Pessoa bPessoa = new Pessoa();

        [HttpGet]
        public ActionResult Index(string acao, long? id_pessoa)
        {
            PessoaModels form = new PessoaModels();
            if (acao == "Delete" && id_pessoa != null)
            {
                pm_pessoa adoPessoa = bPessoa.GetPessoaById((long)id_pessoa);
                var success = bPessoa.PessoaExcluir(adoPessoa);
            }

            ViewData["queryPessoa"] = null;
            ViewData["acao"] = acao;

            return View(form);
        }

        [HttpPost]
        public ActionResult Index(PessoaModels form)
        {
            ViewData["queryPessoa"] = bPessoa.GetPessoaGrid(form.ds_marca, form.ds_razao_social, form.nr_documento, form.dm_tipo_pessoa);
            return View(form); 
        }

        public ActionResult Pessoa(long? id_pessoa, string acao)
        {
            var oPessoa = new Pessoa();

            pm_pessoa adoPessoa = new pm_pessoa();

            if (id_pessoa != null)
                adoPessoa = oPessoa.GetPessoaById((long)id_pessoa);

            ViewData["acao"] = acao;

            var result = ExtensionMethods.ToObjects<PessoaModels>(adoPessoa);

            List<PessoaModels.TelefoneModels> telefones = new List<PessoaModels.TelefoneModels>();
            List<PessoaModels.EnderecoModels> enderecos = new List<PessoaModels.EnderecoModels>();
            List<PessoaModels.ContaCorrenteModels> contas = new List<PessoaModels.ContaCorrenteModels>();

            try
            {
                if (adoPessoa.pm_pessoa_telefone != null)
                    adoPessoa.pm_pessoa_telefone.Load();

                if (adoPessoa.pm_pessoa_endereco != null)
                    adoPessoa.pm_pessoa_endereco.Load();

                if (adoPessoa.pm_pessoa_conta_corrente != null)
                    adoPessoa.pm_pessoa_conta_corrente.Load();
            }
            catch { }

            foreach (var item in adoPessoa.pm_pessoa_telefone)
                telefones.Add(ExtensionMethods.ToObjects<PessoaModels.TelefoneModels>(item));

            foreach (var item in adoPessoa.pm_pessoa_endereco)
                enderecos.Add(ExtensionMethods.ToObjects<PessoaModels.EnderecoModels>(item));

            foreach (var item in adoPessoa.pm_pessoa_conta_corrente)
                contas.Add(ExtensionMethods.ToObjects<PessoaModels.ContaCorrenteModels>(item));

            result.Telefone = telefones;
            result.Endereco = enderecos;
            result.ContaCorrente = contas;

            return View(result);           
        }

        [HttpPost]
        public ActionResult Pessoa(PessoaModels form)
        {
            pm_pessoa adoPessoa = new pm_pessoa();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_pessoa != 0)
            {
                if (ModelState.IsValid)
                {
                    adoPessoa = ExtensionMethods.ToObjects<pm_pessoa>(form);
                    
                    adoPessoa.dt_cadastro = DateTime.Now;

                    foreach (var item in form.Telefone)
                        if (item.dm_tipo_telefone != "DL")
                            adoPessoa.pm_pessoa_telefone.Add(ExtensionMethods.ToObjects<pm_pessoa_telefone>(item));

                    foreach (var item in form.Endereco)
                        if (item.dm_tipo_endereco != "DL")
                            adoPessoa.pm_pessoa_endereco.Add(ExtensionMethods.ToObjects<pm_pessoa_endereco>(item));

                    foreach (var item in form.ContaCorrente)
                        if (item.ds_agencia != "DL")
                            adoPessoa.pm_pessoa_conta_corrente.Add(ExtensionMethods.ToObjects<pm_pessoa_conta_corrente>(item));

                    result = bPessoa.PessoaEditar(ref adoPessoa);
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
                    adoPessoa = ExtensionMethods.ToObjects<pm_pessoa>(form);

                    foreach (var item in form.Telefone)
                        if (item.dm_tipo_telefone != "DL")
                            adoPessoa.pm_pessoa_telefone.Add(ExtensionMethods.ToObjects<pm_pessoa_telefone>(item));

                    foreach (var item in form.Endereco)
                        if (item.dm_tipo_endereco != "DL")
                            adoPessoa.pm_pessoa_endereco.Add(ExtensionMethods.ToObjects<pm_pessoa_endereco>(item));

                    foreach (var item in form.ContaCorrente)
                        if (item.ds_agencia != "DL")
                            adoPessoa.pm_pessoa_conta_corrente.Add(ExtensionMethods.ToObjects<pm_pessoa_conta_corrente>(item));

                    result = bPessoa.PessoaCadastrar(ref adoPessoa);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<PessoaModels>(adoPessoa);

                List<PessoaModels.TelefoneModels> telefones = new List<PessoaModels.TelefoneModels>();
                List<PessoaModels.EnderecoModels> enderecos = new List<PessoaModels.EnderecoModels>();
                List<PessoaModels.ContaCorrenteModels> contas = new List<PessoaModels.ContaCorrenteModels>();

                try
                {
                    if (adoPessoa.pm_pessoa_telefone != null)
                        adoPessoa.pm_pessoa_telefone.Load();

                    if (adoPessoa.pm_pessoa_endereco != null)
                        adoPessoa.pm_pessoa_endereco.Load();

                    if (adoPessoa.pm_pessoa_conta_corrente != null)
                        adoPessoa.pm_pessoa_conta_corrente.Load();
                }
                catch { }

                foreach (var item in adoPessoa.pm_pessoa_telefone)
                    telefones.Add(ExtensionMethods.ToObjects<PessoaModels.TelefoneModels>(item));

                foreach (var item in adoPessoa.pm_pessoa_endereco)
                    enderecos.Add(ExtensionMethods.ToObjects<PessoaModels.EnderecoModels>(item));

                foreach (var item in adoPessoa.pm_pessoa_conta_corrente)
                    contas.Add(ExtensionMethods.ToObjects<PessoaModels.ContaCorrenteModels>(item));

                form.Telefone = telefones;
                form.Endereco = enderecos;
                form.ContaCorrente = contas;
            }

            return View(form);
        }

        private bool ValidaForm(PessoaModels form)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(form.ds_marca))
                ModelState.AddModelError("ds_marca", "Campo (Marca/Código) é obrigatório.");

            if (string.IsNullOrEmpty(form.ds_razao_social))
                ModelState.AddModelError("ds_razao_social", "Campo (Razão Social) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_tipo_pessoa))
                ModelState.AddModelError("dm_tipo_pessoa", "Campo (Tipo Cadastro) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_tipo_documento))
                ModelState.AddModelError("dm_tipo_documento", "Campo (Tipo Documento) é obrigatório.");

            if (string.IsNullOrEmpty(form.nr_documento))
                ModelState.AddModelError("nr_documento", "Campo (Número Documento) é obrigatório.");

            if (string.IsNullOrEmpty(form.nr_ie))
                ModelState.AddModelError("nr_ie", "Campo (Inscrição Estadual) é obrigatório.");

            if (string.IsNullOrEmpty(form.nr_inscricao_suframa))
                ModelState.AddModelError("nr_inscricao_suframa", "Campo (Inscrição SUFRAMA) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_tipo_cliente_credito))
                ModelState.AddModelError("dm_tipo_cliente_credito", "Campo (Tipo do Cliente para Crédito) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_limite_credito))
                ModelState.AddModelError("dm_limite_credito", "Campo (Limite de Crédito) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_pessoa adoPessoa = bPessoa.GetPessoaById(id);

            var success = bPessoa.PessoaExcluir(adoPessoa);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse histórico pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Lookup(string ds_marca)
        {
            IQueryable<pm_pessoa> adoPessoa = bPessoa.GetPessoaByMarca(ds_marca);

            var success = true;

            if(adoPessoa == null || adoPessoa.Count() == 0)
                success = false;

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Nenhuma marca encontrada!" : ""),
                        list = (success ? adoPessoa.Select(o=> new {ds_marca = o.ds_marca, ds_razao_social = o.ds_razao_social, id_pessoa = o.id_pessoa}) : null)
                    }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult LookupScreen()
        {
            PessoaModels form = new PessoaModels();
            ViewData["queryPessoa"] = null;
            return View(form);
        }

        [HttpPost]
        public ActionResult LookupScreen(PessoaModels form)
        {
            ViewData["queryPessoa"] = bPessoa.GetPessoaGrid(form.ds_marca, form.ds_razao_social, form.nr_documento, form.dm_tipo_pessoa);
            return View(form);
        }

        public ActionResult Telefone(IEnumerable<PessoaModels.TelefoneModels> telefones, string acao)
        {
            ViewData["acao"] = acao;
            telefones = telefones.Where(t => t.dm_tipo_telefone != "DL");

            return PartialView(telefones);
        }

        public ActionResult Endereco(IEnumerable<PessoaModels.EnderecoModels> enderecos, string acao)
        {
            ViewData["acao"] = acao;
            enderecos = enderecos.Where(t => t.dm_tipo_endereco != "DL");

            return PartialView(enderecos);
        }

        public ActionResult ContaCorrente(IEnumerable<PessoaModels.ContaCorrenteModels> conta_corrente, string acao)
        {
            ViewData["acao"] = acao;
            conta_corrente = conta_corrente.Where(t => t.ds_agencia != "DL");

            return PartialView(conta_corrente);
        }

    }
}
