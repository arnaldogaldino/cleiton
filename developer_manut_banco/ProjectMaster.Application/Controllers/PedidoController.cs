using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;
using Microsoft.Reporting.WebForms;
using ProjectMaster.Application.Reports.DataSets;
using System.Threading;
//using Excel = Microsoft.Office.Interop.Excel;

namespace ProjectMaster.Application.Controllers
{
    public class PedidoController : ControllerMaster
    {
        Pedido bPedido = new Pedido();
        Pessoa bPessoa = new Pessoa();
        Produto bProduto = new Produto();
        Estoque bEstoque = new Estoque();
        Fiado bFiado = new Fiado();

        [HttpGet]
        public ActionResult Index(string acao, long? id_pedido)
        {
            if (acao == "Delete" && id_pedido != null)
            {
                pm_pedido adoPedido = bPedido.GetPedidoById((long)id_pedido);
                var success = bPedido.PedidoExcluir(adoPedido);
            }
            var result = new PedidoModels();

            if (acao == "New")
            {
                result.dm_status_pedido = "A";
            }

            ViewData["queryPedido"] = null;
            ViewData["acao"] = acao;

            return View(result);
        }

        [HttpPost]
        public ActionResult Index(PedidoModels form)
        {
            ViewData["queryPedido"] = bPedido.GetPedidoGrid(form.nr_pedido, (form.Pessoa != null ? form.Pessoa.ds_razao_social : ""), form.dm_forma_pagto);

            return View(form);
        }

        public ActionResult Pedido(long? id_pedido, string acao)
        {
            var pedido = new PedidoModels();

            pm_pedido adoPedido = new pm_pedido();

            if (id_pedido != null)
            {
                adoPedido = bPedido.GetPedidoById(id_pedido.Value);
            }

            ViewData["acao"] = acao;

            var result = ExtensionMethods.ToObjects<PedidoModels>(adoPedido);

            List<PedidoModels.ItemModels> items = new List<PedidoModels.ItemModels>();

            try
            {
                if (adoPedido.pm_pedido_item != null)
                    adoPedido.pm_pedido_item.Load();
            }
            catch { }

            foreach (var item in adoPedido.pm_pedido_item)
            {
                PedidoModels.ItemModels i = new PedidoModels.ItemModels();
                i = ExtensionMethods.ToObjects<PedidoModels.ItemModels>(item);
                i.Produto = ExtensionMethods.ToObjects<ProdutoModels>(new Produto().GetProdutoByID(i.id_produto));
                items.Add(i);
            }

            result.Items = items;

            if (id_pedido == null)
            {
                result.dt_emissao = DateTime.Now;
                result.dt_vencimento = DateTime.Now;
            }
            else
            {
                result.Pessoa = ExtensionMethods.ToObjects<PessoaModels>(adoPedido.pm_pessoa);
            }

            return View(result);
        }

        [HttpPost]
        public ActionResult Pedido(PedidoModels form)
        {
            pm_pedido adoPedido = new pm_pedido();

            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_pedido != 0)
            {
                if (ModelState.IsValid)
                {
                    adoPedido = ExtensionMethods.ToObjects<pm_pedido>(form);
                    adoPedido.id_pessoa = form.Pessoa.id_pessoa;
                    
                    if (form.Items != null)
                    {
                        foreach (var item in form.Items)
                            if (item.dm_tipo_embalagem != "DL")
                                adoPedido.pm_pedido_item.Add(ExtensionMethods.ToObjects<pm_pedido_item>(item));
                    }

                    result = bPedido.PedidoEditar(ref adoPedido);
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
                    adoPedido = ExtensionMethods.ToObjects<pm_pedido>(form);
                    adoPedido.id_pessoa = form.Pessoa.id_pessoa;
                    adoPedido.dt_emissao = DateTime.Now;
                    adoPedido.dm_status_pedido = "A";

                    if (form.Items != null)
                    {
                        foreach (var item in form.Items)
                            if (item.Produto != null && item.Produto.tp_embalagem != "DL")
                                adoPedido.pm_pedido_item.Add(ExtensionMethods.ToObjects<pm_pedido_item>(item));
                    }

                    result = bPedido.PedidoCadastrar(ref adoPedido);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<PedidoModels>(adoPedido);

                List<PedidoModels.ItemModels> items = new List<PedidoModels.ItemModels>();

                try
                {
                    if (adoPedido.pm_pedido_item != null)
                        adoPedido.pm_pedido_item.Load();
                }
                catch { }

                foreach (var item in adoPedido.pm_pedido_item)
                {
                    PedidoModels.ItemModels i = new PedidoModels.ItemModels();
                    i = ExtensionMethods.ToObjects<PedidoModels.ItemModels>(item);
                    i.Produto = ExtensionMethods.ToObjects<ProdutoModels>(new Produto().GetProdutoByID(i.id_produto));
                    items.Add(i);
                }

                form.Items = items;
            }

            return View(form);
        }

        private bool ValidaForm(PedidoModels form)
        {
            ModelState.Clear();

            if (form.Pessoa.id_pessoa == 0)
                ModelState.AddModelError("id_pessoa", "Campo (Cliente / Fornecedor) é obrigatório.");

            if (form.dt_vencimento == null)
                ModelState.AddModelError("dt_vencimento", "Campo (Data Vencimento) é obrigatório.");

            if (string.IsNullOrEmpty(form.dm_forma_pagto))
                ModelState.AddModelError("dm_forma_pagto", "Campo (Forma de Pagamento) é obrigatório.");

            return ModelState.IsValid;
        }

        public ActionResult Items(IEnumerable<PedidoModels.ItemModels> items, string acao)
        {
            ViewData["acao"] = acao;
            items = items.Where(t => t.Produto.tp_embalagem != "DL");

            return PartialView(items);
        }

        public ActionResult ExportPedido(long id_pedido)
        {
            var pedido = bPedido.GetPedidoById(id_pedido);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/ReportViews/PedidoVenda.rdlc");

            var dtPedido = new dsPedido.dtPedidoDataTable();
            var dtPedidoItem = new dsPedido.dtPeidoItemDataTable();

            dtPedido.AdddtPedidoRow(pedido.nr_pedido.ToString().PadLeft(5, '0'), pedido.pm_pessoa.ds_marca, pedido.pm_pessoa.ds_razao_social, ProjectMaster.Bussiness.Domain.GetTextByDomain(ProjectMaster.Bussiness.Domain.Tipos.FormaPagamento, pedido.dm_forma_pagto), pedido.dt_emissao.ToString("dd/MM/yyyy"), pedido.pm_pedido_item.Sum(o => o.quantidade * o.valor_unitario).ToString("n2"), pedido.pm_pedido_item.Sum(o => o.quantidade * o.pm_produto.vl_embalagem).Value.ToString("N2"), pedido.pm_pedido_item.Sum(o => o.quantidade * (o.valor_unitario + o.pm_produto.vl_embalagem)).Value.ToString("n2"));

            foreach (var linha in pedido.pm_pedido_item)
            {
                dtPedidoItem.AdddtPeidoItemRow(linha.pm_produto.cprod, linha.pm_produto.xprod, linha.quantidade.ToString(), linha.valor_unitario.ToString("N2"), (linha.quantidade * linha.valor_unitario).ToString("N2"), ProjectMaster.Bussiness.Domain.GetTextByDomain(ProjectMaster.Bussiness.Domain.Tipos.TipoEmbalagem, linha.pm_produto.tp_embalagem), linha.pm_produto.vl_embalagem.Value.ToString("N2"), (linha.quantidade * linha.pm_produto.vl_embalagem).Value.ToString("N2"), (linha.quantidade * (linha.valor_unitario + linha.pm_produto.vl_embalagem)).Value.ToString("N2"));
            }

            ReportDataSource rdsPedido = new ReportDataSource();
            rdsPedido.Name = "dsPedido";
            rdsPedido.Value = dtPedido;

            ReportDataSource rdsPedidoItem = new ReportDataSource();
            rdsPedidoItem.Name = "dsPedidoItem";
            rdsPedidoItem.Value = dtPedidoItem;

            localReport.DataSources.Add(rdsPedido);
            localReport.DataSources.Add(rdsPedidoItem);

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
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);             

            return File(renderedBytes, mimeType, "Pedido" + id_pedido.ToString().PadLeft(5, '0') + ".xls");
        }

        public JsonResult FinalizarPedidoALOT(long[] id_pedido)
        {
            if (id_pedido != null)
            {
                foreach (var id in id_pedido)
                {
                    FinalizarPedido(id);
                }
            }
            return Json(null);
        }

        public ActionResult FinalizarPedidoSCR(long id_pedido)
        {
            FinalizarPedido(id_pedido);

            return RedirectToAction("Pedido", "Pedido", new { id_pedido = id_pedido, acao = "View" });
        }

        public void FinalizarPedido(long id_pedido)
        {
            var pedido = bPedido.GetPedidoById(id_pedido);
            pedido.dm_status_pedido = "F";

            bPedido.PedidoEditar(ref pedido);

            Estoque estoque = new Estoque();

            foreach (var p in pedido.pm_pedido_item)
            {
                #region # Lança Estoque #
                estoque.DecrementaQuantidadeEstoque(p.id_produto, p.quantidade);
                estoque.EstoqueHitorico(null, id_pedido, p.id_produto, (p.quantidade * (-1)));

                pm_produto_estoque_entrada entrada = new pm_produto_estoque_entrada();

                entrada.id_filial = pedido.id_filial;
                entrada.id_pessoa = pedido.id_pessoa;
                entrada.bl_excluido = false;
                entrada.bl_efetivo = true;

                entrada.dta_doc = DateTime.Now;
                entrada.dta_entrada = DateTime.Now;

                entrada.descricao = "Venda - " + pedido.id_pedido.ToString().PadLeft(5, '0');
                entrada.dm_tipo = "S";
                entrada.quantidade = p.quantidade;
                entrada.id_produto = p.id_produto;

                bEstoque.ProdutoEstoqueEntradaCadastrar(ref entrada);
                #endregion

                #region # Lança Fiado #
                pm_fiado fiado = new pm_fiado();

                fiado.id_pedido = pedido.id_pedido;
                fiado.id_pessoa = pedido.id_pessoa;
                fiado.id_produto = p.id_produto;
                fiado.id_filial = pedido.id_filial;
                fiado.id_embalagem = p.pm_produto.id_embalagem;

                fiado.dta_emissao = DateTime.Now;
                fiado.pago_ate = pedido.dt_vencimento;

                if (pedido.dm_forma_pagto == "1")
                    fiado.banco = 1;

                fiado.dm_forma_pagto = pedido.dm_forma_pagto;
                fiado.dm_tipo_dc = "D";
                fiado.ocorrencia = "VDA";
                fiado.qtd_frutas = p.quantidade;
                fiado.qtd_caixas = p.quantidade;
                fiado.qtd_cx_poder = 0;

                fiado.total_fruta = p.valor_unitario * p.quantidade;

                fiado.vlr_caixa = p.pm_produto.vl_embalagem;
                fiado.vlr_frutas = p.valor_unitario;
                fiado.total_caixa = (p.quantidade * (fiado.vlr_caixa.HasValue ? fiado.vlr_caixa.Value : 0));

                bFiado.FiadoCadastrar(ref fiado);
                Thread.Sleep(1000);
                // Lançamento de Caixas Pagas
                if (p.qtd_cx_pag_int > 0)
                {
                    pm_fiado fiado_pg_cx = new pm_fiado();

                    fiado_pg_cx.id_pedido = pedido.id_pedido;
                    fiado_pg_cx.id_pessoa = pedido.id_pessoa;
                    fiado_pg_cx.id_produto = p.id_produto;
                    fiado_pg_cx.id_filial = pedido.id_filial;
                    fiado_pg_cx.id_embalagem = p.pm_produto.id_embalagem;
                    fiado_pg_cx.dta_emissao = DateTime.Now;
                    fiado_pg_cx.pago_ate = pedido.dt_vencimento;
                    fiado_pg_cx.dm_forma_pagto = pedido.dm_forma_pagto;
                    fiado_pg_cx.dm_tipo_dc = "C";

                    fiado_pg_cx.ocorrencia = "PCX";

                    fiado_pg_cx.qtd_caixas = p.qtd_cx_pag_int;

                    fiado_pg_cx.vlr_caixa = p.pm_produto.pm_embalagem.valor;
                    fiado_pg_cx.qtd_cx_poder = 0;
                    fiado_pg_cx.total_caixa = (p.qtd_cx_pag_int * (fiado_pg_cx.vlr_caixa.HasValue ? fiado_pg_cx.vlr_caixa.Value : 0));

                    bFiado.FiadoCadastrar(ref fiado_pg_cx);
                }

                Thread.Sleep(1000);
                // Lançamento de Devolução de Caixas
                if (p.qtd_cx_devol > 0)
                {
                    pm_fiado fiado_cx_dev = new pm_fiado();

                    fiado_cx_dev.id_pedido = pedido.id_pedido;
                    fiado_cx_dev.id_pessoa = pedido.id_pessoa;
                    fiado_cx_dev.id_produto = p.id_produto;
                    fiado_cx_dev.id_filial = pedido.id_filial;
                    fiado_cx_dev.id_embalagem = p.pm_produto.id_embalagem;
                    fiado_cx_dev.dta_emissao = DateTime.Now;
                    fiado_cx_dev.pago_ate = pedido.dt_vencimento;
                    fiado_cx_dev.dm_forma_pagto = pedido.dm_forma_pagto;
                    fiado_cx_dev.dm_tipo_dc = "C";

                    fiado_cx_dev.ocorrencia = "DCX";

                    fiado_cx_dev.qtd_caixas = p.qtd_cx_devol;

                    fiado_cx_dev.vlr_caixa = (p.pm_produto.pm_embalagem != null ? p.pm_produto.pm_embalagem.valor : 0);
  
                    fiado_cx_dev.qtd_cx_poder = 0;
                    fiado_cx_dev.total_caixa = (p.qtd_cx_devol * (fiado_cx_dev.vlr_caixa.HasValue ? fiado_cx_dev.vlr_caixa.Value : 0));

                    bFiado.FiadoCadastrar(ref fiado_cx_dev);
                }
                #endregion
            }

            //#region # Lança Conta Corrente #
            //ContaCorrente cc = new ContaCorrente();
            //pm_conta_corrente conta_corrente = new pm_conta_corrente();
            //conta_corrente.dta_emissao = DateTime.Now;
            //conta_corrente.dm_tipo_conta = "C";
            //conta_corrente.
            //#endregion
        }


        public ActionResult CancelarPedido(long id_pedido)
        {
            var pedido = bPedido.GetPedidoById(id_pedido);
            pedido.dm_status_pedido = "C";

            bPedido.PedidoEditar(ref pedido);

            return RedirectToAction("Pedido", "Pedido", new { id_pedido = id_pedido, acao = "View" });
        }

    }
}

