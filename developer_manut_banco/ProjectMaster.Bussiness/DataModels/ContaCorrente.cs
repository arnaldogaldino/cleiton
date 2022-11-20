using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using System.Web.Mvc;
using System.Data;

namespace ProjectMaster.Bussiness
{
    public class ContaCorrente
    {
        private static PMEntities entities = new PMEntities();
        
        public static SelectList GetDomainByBanco(bool Selecione)
        {
            var domain = (from d in entities.pm_banco
                          where d.id_filial == Context.idFilial
                          orderby d.ds_banco
                          select new { texto = d.ds_banco, valor = d.id_banco, id_banco = d.id_banco, ordem = 1 });

            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_banco = 0, ordem = 0 });

                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_banco.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
            else
            {
                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_banco.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
        }

        public static string GetDomainByBanco(string id_banco)
        {
            var domain = (from d in entities.pm_banco
                          where d.id_filial == Context.idFilial && d.id_banco == id_banco
                          select new { texto = d.ds_banco, valor = d.id_banco, id_banco = d.id_banco, ordem = 1 }).FirstOrDefault();

            return domain.texto;
        }

        public IQueryable<pm_conta_corrente> GetContaCorrenteGrid(long? id, long? id_pessoa, long? id_fiado, long? id_contas_pagar, DateTime? vencimento_de, DateTime? vencimento_ate, DateTime? emissao_de, DateTime? emissao_ate)
        {
            var result = (from m in entities.pm_conta_corrente
                          where m.id_filial == Context.idFilial
                          select m);

            if (id != null && id > 0)
                result = result.Where(o => o.id_conta_corrente == id.Value);

            if (id_pessoa != null && id_pessoa > 0)
                result = result.Where(o => o.id_pessoa == id_pessoa);

            if (id_fiado != null && id_fiado > 0)
                result = result.Where(o => o.id_fiado == id_fiado);

            if (emissao_de != null)
                result = result.Where(o => o.dta_emissao >= emissao_de);

            if (emissao_ate != null)
                result = result.Where(o => o.dta_emissao <= emissao_ate);
                        
            if (vencimento_de != null)
                result = result.Where(o => o.dta_vencimento >= vencimento_de);

            if (vencimento_ate != null)
                result = result.Where(o => o.dta_vencimento <= vencimento_ate);

            return result.OrderBy(p=>p.dta_pagamento);
        }

        public pm_conta_corrente GetContaCorrenteById(long id)
        {
            return (from m in entities.pm_conta_corrente
                    where m.id_filial == Context.idFilial &&
                        m.id_conta_corrente == id
                    select m).FirstOrDefault();
        }

        public pm_conta_corrente GetContaCorrenteByIdContasPagar(long id)
        {
            return (from m in entities.pm_conta_corrente
                    where m.id_filial == Context.idFilial &&
                        m.id_contas_pagar == id
                    select m).FirstOrDefault();
        }
        public pm_conta_corrente GetContaCorrenteByIdFiado(long id)
        {
            return (from m in entities.pm_conta_corrente
                    where m.id_filial == Context.idFilial &&
                        m.id_fiado == id
                    select m).FirstOrDefault();
        }
        

        public void DeleteContaCorrente(long id)
        {
            pm_conta_corrente conta_corrente = entities.pm_conta_corrente.First(i => i.id_conta_corrente == id);

            entities.pm_conta_corrente.DeleteObject(conta_corrente);
            entities.SaveChanges();
        }

        public bool ContaCorrenteCadastrar(ref pm_conta_corrente adoContaCorrente)
        {
            try
            {
                adoContaCorrente.id_filial = Context.idFilial;

                entities.AddTopm_conta_corrente(adoContaCorrente);
                entities.SaveChanges();
                
                if(adoContaCorrente.dta_pagamento.HasValue)
                    AtualizaSaldo(adoContaCorrente.id_conta_corrente, adoContaCorrente.dta_pagamento.Value);
            }
            catch { return false; }

            return true;
        }

        public bool ContaCorrenteEditar(ref pm_conta_corrente adoContaCorrente)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_conta_corrente", adoContaCorrente);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoContaCorrente);
                }

                if (adoContaCorrente.dta_pagamento.HasValue)
                    AtualizaSaldo(adoContaCorrente.id_conta_corrente, adoContaCorrente.dta_pagamento.Value);

                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        
        public bool ContaCorrenteExcluir(pm_conta_corrente adoContaCorrente)
        {
            try
            {
                entities.DeleteObject(adoContaCorrente);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public void AtualizaSaldo(long id_conta_corrente, DateTime data)
        {
            long id_conta_corrente_max = 0;

            try
            {
                id_conta_corrente_max = (from f in entities.pm_conta_corrente
                                                    where f.id_conta_corrente == id_conta_corrente &&
                                                          f.dta_pagamento < data &&
                                                          f.id_filial == Context.idFilial
                                                    select f).Max(o => o.id_conta_corrente);
            }
            catch { }

            var list_conta_corrente = (from f in entities.pm_conta_corrente
                                 where f.id_conta_corrente >= id_conta_corrente &&
                                       f.id_filial == Context.idFilial
                                  select f).ToArray();

            for (int row = 0; row < list_conta_corrente.Length; row++)
            {
                int row_old = ((id_conta_corrente_max) == 0 ? 0 : ((row - 1) == -1 ? 0 : (row - 1)));

                if (id_conta_corrente_max == 0)
                    if (list_conta_corrente[row].dm_tipo_dc == "C")
                        list_conta_corrente[row].saldo = list_conta_corrente[row].valor;
                    else
                        list_conta_corrente[row].saldo = list_conta_corrente[row].valor * -1;
                else
                    if(list_conta_corrente[row].dm_tipo_dc == "C")
                        list_conta_corrente[row].saldo = list_conta_corrente[row_old].saldo + list_conta_corrente[row].valor;
                    else
                        list_conta_corrente[row].saldo = list_conta_corrente[row_old].saldo - list_conta_corrente[row].valor;

                entities.ApplyOriginalValues("pm_conta_corrente", list_conta_corrente[row]);
            }

            entities.SaveChanges();
        }

    }
}
