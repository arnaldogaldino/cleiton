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
                {
                    adoPessoa.pm_pessoa_telefone.Load();
                }
                if (adoPessoa.pm_pessoa_endereco != null)
                    adoPessoa.pm_pessoa_endereco.Load();

                if (adoPessoa.pm_pessoa_conta_corrente != null)
                    adoPessoa.pm_pessoa_conta_corrente.Load();
            }
            catch { }

            foreach (var item in oPessoa.GetPessoaTelefoneByIdPessoa(adoPessoa.id_pessoa))
                telefones.Add(ExtensionMethods.ToObjects<PessoaModels.TelefoneModels>(item));

            foreach (var item in oPessoa.GetPessoaEnderecoEditarByIdPessoa(adoPessoa.id_pessoa))
                enderecos.Add(ExtensionMethods.ToObjects<PessoaModels.EnderecoModels>(item));

            foreach (var item in oPessoa.GetPessoaContaCorrenteByIdPessoa(adoPessoa.id_pessoa))
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
                    result = bPessoa.PessoaEditar(ref adoPessoa);

                    if (form.Telefone != null)
                    {
                        foreach (var item in form.Telefone)
                            if (item.dm_tipo_telefone != "DL")
                            {
                                if (item.id_pessoa_telefone == 0)
                                {
                                    item.id_pessoa = adoPessoa.id_pessoa;
                                    bPessoa.PessoaTelefoneCadastrar(ExtensionMethods.ToObjects<pm_pessoa_telefone>(item));
                                }
                                else
                                    bPessoa.PessoaTelefoneEditar(ExtensionMethods.ToObjects<pm_pessoa_telefone>(item));

                                adoPessoa.pm_pessoa_telefone.Add(ExtensionMethods.ToObjects<pm_pessoa_telefone>(item));
                            }
                    }

                    if (form.Endereco != null)
                    {
                        foreach (var item in form.Endereco)
                            if (item.dm_tipo_endereco != "DL")
                            {
                                if (item.id_pessoa_endereco == 0)
                                {
                                    item.id_pessoa = adoPessoa.id_pessoa;
                                    bPessoa.PessoaEnderecoCadastrar(ExtensionMethods.ToObjects<pm_pessoa_endereco>(item));
                                }
                                else
                                    bPessoa.PessoaEnderecoEditar(ExtensionMethods.ToObjects<pm_pessoa_endereco>(item));

                                adoPessoa.pm_pessoa_endereco.Add(ExtensionMethods.ToObjects<pm_pessoa_endereco>(item));
                            }
                    }

                    if (form.ContaCorrente != null)
                    {
                        foreach (var item in form.ContaCorrente)
                            if (item.ds_agencia != "DL")
                            {
                                if (item.id_pessoa_conta_corrente == 0)
                                {
                                    item.id_pessoa = adoPessoa.id_pessoa;
                                    bPessoa.PessoaContaCorrenteCadastrar(ExtensionMethods.ToObjects<pm_pessoa_conta_corrente>(item));
                                }
                                else
                                    bPessoa.PessoaContaCorrenteEditar(ExtensionMethods.ToObjects<pm_pessoa_conta_corrente>(item));

                                adoPessoa.pm_pessoa_conta_corrente.Add(ExtensionMethods.ToObjects<pm_pessoa_conta_corrente>(item));
                            }
                    }
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

                    if (form.Telefone != null)
                    {
                        foreach (var item in form.Telefone)
                            if (item.dm_tipo_telefone != "DL")
                            {
                                item.id_pessoa = adoPessoa.id_pessoa;
                                adoPessoa.pm_pessoa_telefone.Add(ExtensionMethods.ToObjects<pm_pessoa_telefone>(item));
                            }
                    }

                    if (form.Endereco != null)
                    {
                        foreach (var item in form.Endereco)
                            if (item.dm_tipo_endereco != "DL")
                            {
                                item.id_pessoa = adoPessoa.id_pessoa;
                                adoPessoa.pm_pessoa_endereco.Add(ExtensionMethods.ToObjects<pm_pessoa_endereco>(item));
                            }
                    }

                    if (form.ContaCorrente != null)
                    {
                        foreach (var item in form.ContaCorrente)
                            if (item.ds_agencia != "DL")
                            {
                                item.id_pessoa = adoPessoa.id_pessoa;
                                adoPessoa.pm_pessoa_conta_corrente.Add(ExtensionMethods.ToObjects<pm_pessoa_conta_corrente>(item));
                            }
                    }

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

            if (adoPessoa == null || adoPessoa.Count() == 0)
                success = false;

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Nenhuma marca encontrada!" : ""),
                        list = (success ? adoPessoa.Select(o => new { ds_marca = o.ds_marca, ds_razao_social = o.ds_razao_social, id_pessoa = o.id_pessoa }) : null)
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

        public ActionResult ExportExtrato(long id_pessoa, DateTime dtInicio, DateTime dtFim)
        {
            var pessoa = bPessoa.GetPessoaById(id_pessoa);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/ReportViews/ExtratoCliente.rdlc");

            var dtExtrato = new dsExtrato.dtExtratoDataTable();
            var dtExtratoItem = new dsExtrato.dtExtratoItemDataTable();
            var fiado = pessoa.pm_fiado.Where(o => (o.dta_emissao >= dtInicio) && (o.dta_emissao <= dtFim));

            dtExtrato.AdddtExtratoRow(pessoa.ds_marca.ToString(), pessoa.ds_razao_social, DateTime.Now.ToString("dd/MM/yyyy"), dtInicio.ToString("dd/MM/yyyy"), dtFim.ToString("dd/MM/yyyy"), fiado.Sum(o => (o.total_fruta.HasValue ? o.total_fruta.Value : 0)).ToString("n2"), fiado.Sum(o => (o.total_caixa.HasValue ? o.total_caixa.Value : 0)).ToString("n2"), (fiado.Sum(o => (o.total_fruta.HasValue ? o.total_fruta.Value : 0)) + fiado.Sum(o => (o.total_caixa.HasValue ? o.total_caixa.Value : 0))).ToString("n2"));

            foreach (var linha in fiado)
            {
                dtExtratoItem.AdddtExtratoItemRow(linha.dta_emissao.ToString("dd/MM/yyyy"),
                            (linha.pago_ate.HasValue ? linha.pago_ate.Value.ToString("dd/MM/yyyy") : ""),
                            linha.dm_tipo_dc,
                            (linha.pm_pedido != null ? linha.pm_pedido.nr_pedido.ToString().PadLeft(5, '0') : ""),
                            linha.ocorrencia,
                            (linha.pm_produto != null ? linha.pm_produto.xprod : ""),
                            (linha.pm_produto != null ? ProjectMaster.Bussiness.Domain.GetTextByDomain(ProjectMaster.Bussiness.Domain.Tipos.TipoEmbalagem, linha.pm_produto.tp_embalagem) : ""),
                            (linha.pm_produto != null ? (linha.pm_produto.vl_embalagem.HasValue ? linha.pm_produto.vl_embalagem.Value : 0).ToString("n2") : ""),
                            (linha.qtd_caixas.HasValue ? linha.qtd_caixas.Value.ToString("n2") : ""),
                            (linha.vlr_frutas.HasValue ? linha.vlr_frutas.Value.ToString("n2") : ""),
                            (linha.total_fruta.HasValue ? linha.total_fruta.Value.ToString("n2") : ""),
                            (linha.saldo_fruta.HasValue ? linha.saldo_fruta.Value.ToString("n2") : ""),
                            (linha.total_caixa.HasValue ? linha.total_caixa.Value.ToString("n2") : ""),
                            (linha.saldo_caixa_fisico.HasValue ? linha.saldo_caixa_fisico.Value.ToString("n2") : ""),
                            (linha.saldo_caixa.HasValue ? linha.saldo_caixa.Value.ToString("n2") : ""),
                            (linha.total_venda.HasValue ? linha.total_venda.Value.ToString("n2") : ""),
                            (linha.saldo_geral.HasValue ? linha.saldo_geral.Value.ToString("n2") : ""));
            }

            //var pedidos = pessoa.pm_pedido.Where(o => (o.dt_emissao >= dtInicio) && (o.dt_emissao <= dtFim));
            //foreach (var linha in pedidos)
            //{
            //    foreach(var item in linha.pm_pedido_item)
            //    {
            //        dtExtratoItem.AdddtExtratoItemRow(linha.dt_emissao.ToString("dd/MM/yyyy"), 
            //            linha.dt_vencimento.ToString("dd/MM/yyyy"),
            //            "D", 
            //            linha.nr_pedido.ToString().PadLeft(5, '0'),
            //            "VEND - VENDA",
            //            item.pm_produto.xprod, 
            //            ProjectMaster.Bussiness.Domain.GetTextByDomain(ProjectMaster.Bussiness.Domain.Tipos.TipoEmbalagem, item.pm_produto.tp_embalagem),
            //            item.pm_produto.vl_embalagem.Value.ToString("n2"),
            //            (item.quantidade * (-1)).ToString(),
            //            item.valor_unitario.ToString("n2"),
            //            ((item.quantidade * (-1)) * item.valor_unitario).ToString("n2"),
            //            (((item.quantidade * (-1)) * item.valor_unitario) + vlr_old).ToString("n2"),
            //            "",
            //            "",
            //            "",
            //            "",
            //            "");

            //        vlr_old += ((item.quantidade * (-1)) * item.valor_unitario);

            //        count++;
            //    }

            //public dtExtratoItemRow AdddtExtratoItemRow(
            //        string emissao, 
            //        string vencimento, 
            //        string d_c, 
            //        string nr_pedido, 
            //        string ocorrencia, 
            //        string produto, 
            //        string tipo_embalagem, 
            //        string quantidade, 
            //        string vlr_unitario, 
            //        string vlr_operacao_frutas, 
            //        string saldo_frutas, 
            //        string vlr_operacao_caixa, 
            //        string saldo_caixa_fisico, 
            //        string caixa_poder, 
            //        string saldo_caixa, 
            //        string saldo_total) {
            //}

            ReportDataSource rdsExtrato = new ReportDataSource();
            rdsExtrato.Name = "dsExtrato";
            rdsExtrato.Value = dtExtrato;

            ReportDataSource rdsExtratoItem = new ReportDataSource();
            rdsExtratoItem.Name = "dsExtratoItem";
            rdsExtratoItem.Value = dtExtratoItem;

            localReport.DataSources.Add(rdsExtrato);
            localReport.DataSources.Add(rdsExtratoItem);

            string reportType = "Excel";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType             
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx             
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>xls</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report             
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType, "Extrato_" + pessoa.ds_marca + ".xls");
        }

    }
}
