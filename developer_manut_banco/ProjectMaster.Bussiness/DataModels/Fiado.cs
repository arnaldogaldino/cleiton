using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data;
using System.Web.Mvc;
using ProjectMaster.Core;

namespace ProjectMaster.Bussiness
{
    public class Fiado
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_fiado> GetFiadoGrid(long? id_fiado, long? id_pedido, long? id_pessoa, long? id_produto)
        {
            var result = (from m in entities.pm_fiado
                          where m.id_filial == Context.idFilial && m.bl_excluido == false
                          select m);

            if (id_fiado != 0)
                result = result.Where(o => o.id_fiado == id_fiado);

            if (id_pedido != null)
                result = result.Where(o => o.id_pedido == id_pedido);

            if (id_pessoa != 0)
                result = result.Where(o => o.id_pessoa == id_pessoa);

            if (id_produto != 0)
                result = result.Where(o => o.id_produto == id_produto);

            return result.OrderBy(o => o.id_fiado).OrderBy(o => o.id_pessoa);
        }

        public pm_fiado GetFiadoById(long id)
        {
            return (from m in entities.pm_fiado
                    where m.id_filial == Context.idFilial &&
                         m.id_fiado == id
                    orderby m.id_fiado
                    select m).FirstOrDefault();
        }

        public void DeleteFiado(long id)
        {
            pm_fiado fiado = entities.pm_fiado.First(i => i.id_fiado == id);

            fiado.bl_excluido = true;
            FiadoEditar(ref fiado);
        }

        public bool FiadoCadastrar(ref pm_fiado adoFiado)
        {
            try
            {
                adoFiado.id_filial = Context.idFilial;
                adoFiado.dta_emissao = DateTime.Parse(adoFiado.dta_emissao.ToString("dd/MM/yyyy HH:mm:ss"));
                adoFiado.total_fruta = adoFiado.qtd_caixas * adoFiado.vlr_frutas;
                adoFiado.total_caixa = adoFiado.qtd_caixas * (adoFiado.vlr_caixa.HasValue ? adoFiado.vlr_caixa.Value : 0);
                adoFiado.total_venda = adoFiado.total_fruta + adoFiado.total_caixa;

                entities.AddTopm_fiado(adoFiado);
                entities.SaveChanges();

                #region # Lançamento Conta Corrente #
                //VDA	VDA - Venda
                //DCX	DCX - Devolução de Caixa
                //PFR	PFR - Pagto Frutas
                //PCX	PCX - Pagto Caixas
                //DCF	DCF - Devolução de Caixa + Pagto Frutas
                //TRC	TRC - Troco
                switch(adoFiado.ocorrencia)
                {
                    case "PFR":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = adoFiado.vlr_frutas.Value * adoFiado.qtd_frutas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "C";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }

                        break;
                    case "PCX":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = (adoFiado.vlr_caixa.HasValue ? adoFiado.vlr_caixa.Value : 0) * adoFiado.qtd_caixas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "C";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }
                        break;
                    case "DCF":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = adoFiado.vlr_frutas.Value * adoFiado.qtd_frutas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "C";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }
                        break;
                    case "TRC":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = adoFiado.qtd_frutas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "D";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }
                        break;
                }
                #endregion

                try
                {
                    AtualizarSaldos(adoFiado.id_pessoa, adoFiado.id_produto.Value, adoFiado.dta_emissao);
                }
                catch { }

                AtualizarSaldosCxPoder(adoFiado.id_pessoa, adoFiado.pm_produto.id_embalagem.Value, adoFiado.dta_emissao);
            }
            catch { return false; }

            return true;
        }

        public bool FiadoEditar(ref pm_fiado adoFiado)
        {
            try
            {
                adoFiado.total_fruta = (adoFiado.qtd_caixas.HasValue ? adoFiado.qtd_caixas.Value : 0) * adoFiado.vlr_frutas;
                adoFiado.total_caixa = (adoFiado.qtd_caixas.HasValue ? adoFiado.qtd_caixas.Value : 0) * adoFiado.vlr_caixa;
                adoFiado.total_venda = adoFiado.total_fruta + adoFiado.total_caixa;

                EntityKey key = entities.CreateEntityKey("pm_fiado", adoFiado);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoFiado);
                }
                entities.SaveChanges();


                #region # Lançamento Conta Corrente #
                //VDA	VDA - Venda
                //DCX	DCX - Devolução de Caixa
                //PFR	PFR - Pagto Frutas
                //PCX	PCX - Pagto Caixas
                //DCF	DCF - Devolução de Caixa + Pagto Frutas
                //TRC	TRC - Troco
                switch (adoFiado.ocorrencia)
                {
                    case "PFR":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = adoFiado.vlr_frutas.Value * adoFiado.qtd_frutas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "C";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }

                        break;
                    case "PCX":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = (adoFiado.vlr_caixa.Value / 100) * adoFiado.qtd_caixas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "C";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }
                        break;
                    case "DCF":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = adoFiado.vlr_frutas.Value * adoFiado.qtd_frutas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "C";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }
                        break;
                    case "TRC":
                        if (adoFiado.vlr_frutas.HasValue && adoFiado.pago_ate.HasValue)
                        {
                            ContaCorrente conta = new ContaCorrente();
                            pm_conta_corrente conta_corrente;
                            bool edit = false;

                            conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                            if (conta_corrente != null)
                                edit = true;
                            else
                                conta_corrente = new pm_conta_corrente();

                            conta_corrente.id_fiado = adoFiado.id_fiado;
                            conta_corrente.id_filial = Context.idFilial;
                            conta_corrente.id_pessoa = adoFiado.id_pessoa;

                            conta_corrente.valor = adoFiado.qtd_frutas.Value;
                            conta_corrente.dta_pagamento = adoFiado.pago_ate.Value;

                            conta_corrente.dta_vencimento = adoFiado.pago_ate.Value;
                            conta_corrente.dta_emissao = DateTime.Now;
                            conta_corrente.dm_tipo_dc = "D";

                            if (!edit)
                                conta.ContaCorrenteCadastrar(ref conta_corrente);
                            else
                                conta.ContaCorrenteEditar(ref conta_corrente);
                        }
                        break;
                }
                #endregion

                if (adoFiado.id_produto != null)
                    AtualizarSaldos(adoFiado.id_pessoa, adoFiado.id_produto.Value, adoFiado.dta_emissao);
            }
            catch { return false; }

            return true;
        }

        public bool FiadoExcluir(pm_fiado adoFiado)
        {
            try
            {
                adoFiado.bl_excluido = true;
                FiadoEditar(ref adoFiado);

                ContaCorrente conta = new ContaCorrente();
                pm_conta_corrente conta_corrente;

                conta_corrente = conta.GetContaCorrenteByIdFiado(adoFiado.id_fiado);

                if (conta_corrente != null)
                {
                    conta.ContaCorrenteExcluir(conta_corrente);
                }
            }
            catch { return false; }

            return true;
        }

        public void AtualizarSaldos(long id_pessoa, long id_produto, DateTime dta_emissao)
        {
            long id_fiado_max = 0;

            try
            {
                id_fiado_max = entities.pm_fiado.Where(f => f.id_pessoa == id_pessoa &&
                                      f.dta_emissao < dta_emissao &&
                                      f.id_produto == id_produto).Max(o => o.id_fiado);
            }
            catch { }

            List<pm_fiado> list_fiados = new List<pm_fiado>();

            try
            {
                list_fiados = entities.pm_fiado.Where(f => f.id_produto == id_produto &&
                                         f.id_pessoa == id_pessoa &&
                                         f.id_fiado >= id_fiado_max).ToList();
            }
            catch 
            { } 

            int total = 0;

            if (list_fiados.Count() > 1)
                total = (list_fiados.Count() - 1);
            else
                total = list_fiados.Count();

            for (int row = 0; row < total; row++)
            {
                int rowx = row;
                int row_old = 0;

                if (list_fiados.Count() > 1)
                {
                    rowx = row + 1;
                    row_old = rowx - 1;
                }                

                if (id_fiado_max == 0)
                {
                    list_fiados[rowx].saldo_caixa = null;
                    list_fiados[rowx].saldo_fruta = null;
                    list_fiados[rowx].saldo_caixa_fisico = null;
                    list_fiados[rowx].saldo_geral = null;
                    list_fiados[rowx].qtd_cx_poder = 0;
                }

                if (list_fiados[rowx].qtd_frutas.HasValue && list_fiados[rowx].vlr_frutas.HasValue)
                    list_fiados[rowx].total_fruta = list_fiados[rowx].qtd_frutas * list_fiados[rowx].vlr_frutas;

                if (list_fiados[rowx].qtd_caixas.HasValue && list_fiados[rowx].vlr_caixa.HasValue)
                    list_fiados[rowx].total_caixa = list_fiados[rowx].qtd_caixas * list_fiados[rowx].vlr_caixa.Value;

                if (list_fiados[rowx].total_fruta.HasValue && list_fiados[rowx].total_caixa.HasValue)
                    list_fiados[rowx].total_venda = list_fiados[rowx].total_fruta + list_fiados[rowx].total_caixa;

                if (list_fiados[rowx].dm_tipo_dc == "D")
                {
                    list_fiados[rowx].saldo_caixa = (list_fiados[row_old].saldo_caixa.HasValue ? list_fiados[row_old].saldo_caixa : 0) +
                                               ((list_fiados[rowx].total_caixa.HasValue ? list_fiados[rowx].total_caixa : 0) * -1);

                    list_fiados[rowx].saldo_fruta = (list_fiados[row_old].saldo_fruta.HasValue ? list_fiados[row_old].saldo_fruta : 0) +
                                               ((list_fiados[rowx].total_fruta.HasValue ? list_fiados[rowx].total_fruta : 0) * -1);

                    //if (list_fiados[rowx].ocorrencia == "DCX" ||
                    //list_fiados[rowx].ocorrencia == "DCF" ||
                    //list_fiados[rowx].ocorrencia == "VDA")
                    //{
                    //    list_fiados[rowx].saldo_caixa_fisico = (list_fiados[row_old].saldo_caixa_fisico.HasValue ? list_fiados[row_old].saldo_caixa_fisico : 0) +
                    //                                      ((list_fiados[rowx].qtd_caixas.HasValue ? list_fiados[rowx].qtd_caixas : 0) * -1);
                    //}
                    //if (list_fiados[rowx].ocorrencia == "PCX")
                    //{
                    //    list_fiados[rowx].saldo_caixa_fisico = (list_fiados[row_old].saldo_caixa_fisico.HasValue ? list_fiados[row_old].saldo_caixa_fisico : 0) +
                    //                                      ((list_fiados[rowx].qtd_caixas.HasValue ? list_fiados[rowx].qtd_caixas : 0) * -1);
                    //}

                    list_fiados[rowx].saldo_geral = (list_fiados[row_old].saldo_geral.HasValue ? list_fiados[row_old].saldo_geral : 0) + (list_fiados[rowx].saldo_caixa + list_fiados[rowx].saldo_fruta);
                }

                if (list_fiados[rowx].dm_tipo_dc == "C")
                {
                    list_fiados[rowx].saldo_caixa = (list_fiados[row_old].saldo_caixa.HasValue ? list_fiados[row_old].saldo_caixa : 0) +
                                               (list_fiados[rowx].total_caixa.HasValue ? list_fiados[rowx].total_caixa : 0);

                    list_fiados[rowx].saldo_fruta = (list_fiados[row_old].saldo_fruta.HasValue ? list_fiados[row_old].saldo_fruta : 0) +
                                               (list_fiados[rowx].total_fruta.HasValue ? list_fiados[rowx].total_fruta : 0);

                    //if (list_fiados[rowx].ocorrencia == "DCX" ||
                    //list_fiados[rowx].ocorrencia == "DCF" ||
                    //list_fiados[rowx].ocorrencia == "VDA")
                    //{
                    //    list_fiados[rowx].saldo_caixa_fisico = (list_fiados[row_old].saldo_caixa_fisico.HasValue ? list_fiados[row_old].saldo_caixa_fisico : 0) +
                    //                                  (list_fiados[rowx].qtd_caixas.HasValue ? list_fiados[rowx].qtd_caixas : 0);
                    //}
                    //if (list_fiados[rowx].ocorrencia == "PCX")
                    //{
                    //    list_fiados[rowx].saldo_caixa_fisico = (list_fiados[row_old].saldo_caixa_fisico.HasValue ? list_fiados[row_old].saldo_caixa_fisico : 0) +
                    //                                  (list_fiados[rowx].qtd_caixas.HasValue ? list_fiados[rowx].qtd_caixas : 0);
                    //}

                    list_fiados[rowx].saldo_geral = (list_fiados[row_old].saldo_geral.HasValue ? list_fiados[row_old].saldo_geral : 0) + 
                        ((list_fiados[rowx].total_caixa.HasValue ? list_fiados[rowx].total_caixa : 0) + (list_fiados[rowx].total_fruta.HasValue ? list_fiados[rowx].total_fruta : 0));
                }

                #region
                //switch (list_fiados[row].ocorrencia)
                //{
                //    case "VDA":
                //        #region # VDA - Venda #                        
                //        if (list_fiados[row].dm_tipo_dc == "D")
                //        {
                //            //list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : 0) + (list_fiados[row].qtd_caixas * -1);
                //            list_fiados[row].qtd_cx_poder = cx_poder + (list_fiados[row].qtd_caixas * -1);
                //        }
                //        if (list_fiados[row].dm_tipo_dc == "C")
                //        {
                //            //list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : 0) + list_fiados[row].qtd_caixas;
                //            list_fiados[row].qtd_cx_poder = cx_poder + list_fiados[row].qtd_caixas;
                //        }
                //        #endregion
                //        break;

                //    case "DCX":
                //        #region # DCX - Devolução de Caixa #
                //        if (list_fiados[row].dm_tipo_dc == "D")
                //        {
                //            //list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : 0) + (list_fiados[row].qtd_caixas * -1);
                //            list_fiados[row].qtd_cx_poder = cx_poder + (list_fiados[row].qtd_caixas * -1);
                //        }
                //        if (list_fiados[row].dm_tipo_dc == "C")
                //        {
                //            //list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : 0) + list_fiados[row].qtd_caixas;
                //            list_fiados[row].qtd_cx_poder = cx_poder + list_fiados[row].qtd_caixas;
                //        }
                //        #endregion
                //        break;

                //    case "PFR":
                //        #region # PFR - Pagto Frutas #
                //        #endregion
                //        list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : cx_poder);
                //        break;

                //    case "PCX":
                //        #region # PCX - Pagto Caixas #
                //        #endregion
                //        list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : cx_poder);
                //        break;

                //    case "DCF":
                //        #region # DCF - Devolução de Caixa + Pagto Frutas #
                //        if (list_fiados[row].ocorrencia == "DCX")
                //        {
                //            if (list_fiados[row].dm_tipo_dc == "D")
                //            {
                //                //list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : 0) + (list_fiados[row].qtd_caixas * -1);
                //                list_fiados[row].qtd_cx_poder = cx_poder + (list_fiados[row].qtd_caixas * -1);
                //            }
                //            if (list_fiados[row].dm_tipo_dc == "C")
                //            {
                //                //list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : 0) + list_fiados[row].qtd_caixas;
                //                list_fiados[row].qtd_cx_poder = cx_poder + list_fiados[row].qtd_caixas;
                //            }
                //        }
                //        #endregion
                //        break;

                //    case "TRC":
                //        #region # TRC - Troco #
                //    #endregion
                //        list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : cx_poder);
                //        break;

                //    default:
                //        list_fiados[row].qtd_cx_poder = (list_fiados[row_old].qtd_cx_poder.HasValue ? list_fiados[row_old].qtd_cx_poder : cx_poder);
                //        break;
                //}
                #endregion

                entities.ApplyOriginalValues("pm_fiado", list_fiados[rowx]);
            }

            entities.SaveChanges();
        }

        public void AtualizarSaldosCxPoder(long id_pessoa, long id_embalagem, DateTime dta_emissao)
        {
            long id_fiado_max = 0;

            try
            {
                id_fiado_max = (from f in entities.pm_fiado
                                where f.id_pessoa == id_pessoa &&
                                 f.id_embalagem == id_embalagem &&
                                 f.dta_emissao < dta_emissao
                                    select f).Max(o => o.id_fiado);
            }
            catch { }

            List<pm_fiado> list_fiados = new List<pm_fiado>();

            list_fiados = (from f in entities.pm_fiado
                           where f.id_pessoa == id_pessoa &&
                            f.id_embalagem == id_embalagem &&
                            f.id_fiado >= id_fiado_max
                           select f).ToList();
            
            int total = 0;

            if (list_fiados.Count() > 1)
                total = (list_fiados.Count() - 1);
            else
                total = list_fiados.Count();

            for (int row = 0; row < total; row++)
            {
                int rowx = row;
                int row_old = 0;

                if (list_fiados.Count() > 1)
                {
                    rowx = row + 1;
                    row_old = rowx - 1;
                }                

                if (id_fiado_max == 0)
                {
                    list_fiados[rowx].qtd_cx_poder = 0;
                    list_fiados[rowx].saldo_caixa_fisico = 0;
                    id_fiado_max = 1;
                }

                if (list_fiados[rowx].ocorrencia == "DCX" ||
                    list_fiados[rowx].ocorrencia == "DCF" ||
                    list_fiados[rowx].ocorrencia == "VDA")
                {
                    if (list_fiados[rowx].dm_tipo_dc == "D")
                    {
                        list_fiados[rowx].qtd_cx_poder = list_fiados[row_old].qtd_cx_poder - (list_fiados[rowx].qtd_caixas);
                        list_fiados[rowx].saldo_caixa_fisico = list_fiados[row_old].saldo_caixa_fisico - (list_fiados[rowx].qtd_caixas);
                    }

                    if (list_fiados[rowx].dm_tipo_dc == "C")
                    {
                        list_fiados[rowx].qtd_cx_poder = list_fiados[row_old].qtd_cx_poder + (list_fiados[rowx].qtd_caixas);
                        list_fiados[rowx].saldo_caixa_fisico = list_fiados[row_old].saldo_caixa_fisico + (list_fiados[rowx].qtd_caixas);
                    }
                }
                else
                {
                    list_fiados[rowx].qtd_cx_poder = list_fiados[row_old].qtd_cx_poder;
                    //list_fiados[rowx].saldo_caixa_fisico = list_fiados[row_old].saldo_caixa_fisico;
                    list_fiados[rowx].saldo_caixa_fisico = list_fiados[row_old].saldo_caixa_fisico + (list_fiados[rowx].qtd_caixas);
                }

                entities.ApplyOriginalValues("pm_fiado", list_fiados[rowx]);
            }

            entities.SaveChanges();
        }
    }
}
