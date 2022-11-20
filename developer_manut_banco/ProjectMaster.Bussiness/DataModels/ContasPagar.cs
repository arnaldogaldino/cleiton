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
    public class ContasPagar
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_conta_pagar> GetContasPagarGrid(long? id, string fornecedor, DateTime? vencimento_de, DateTime? vencimento_ate)
        {
            var result = (from m in entities.pm_conta_pagar
                          where m.id_filial == Context.idFilial &&
                              m.bl_excluido == false
                          select m);

            if (id != null)
                result = result.Where(o => o.id_conta_pagar == id.Value);

            if (!string.IsNullOrEmpty(fornecedor))
                result = result.Where(o => o.pm_pessoa.ds_razao_social.Contains(fornecedor));

            if (vencimento_de != null)
                result = result.Where(o => o.dta_vencimento >= vencimento_de);

            if (vencimento_ate != null)
                result = result.Where(o => o.dta_vencimento <= vencimento_ate);

            return result;
        }

        public pm_conta_pagar GetContasPagarById(long id)
        {
            return (from m in entities.pm_conta_pagar
                    where m.id_filial == Context.idFilial &&
                        m.id_conta_pagar == id
                    orderby m.dta_vencimento, m.bl_quitado
                    select m).FirstOrDefault();
        }

        public void DeleteContasPagar(long id)
        {
            pm_conta_pagar conta_pagar = entities.pm_conta_pagar.First(i => i.id_conta_pagar == id);

            conta_pagar.bl_excluido = true;
            ContasPagarEditar(ref conta_pagar);
        }

        public bool ContasPagarCadastrar(ref pm_conta_pagar adoContasPagar)
        {
            try
            {
                adoContasPagar.id_filial = Context.idFilial;

                entities.AddTopm_conta_pagar(adoContasPagar);
                entities.SaveChanges();

                if (adoContasPagar.vlr_pagto.HasValue && adoContasPagar.dta_pagto.HasValue)
                {
                    ContaCorrente conta = new ContaCorrente();
                    pm_conta_corrente conta_corrente = new pm_conta_corrente();

                    conta_corrente.id_contas_pagar = adoContasPagar.id_conta_pagar;
                    conta_corrente.id_filial = Context.idFilial;
                    conta_corrente.id_pessoa = adoContasPagar.id_pessoa;

                    conta_corrente.valor = adoContasPagar.vlr_pagto.Value;
                    conta_corrente.dta_pagamento = adoContasPagar.dta_pagto.Value;

                    conta_corrente.dta_vencimento = adoContasPagar.dta_vencimento;
                    conta_corrente.dta_emissao = DateTime.Now;
                    conta_corrente.dm_tipo_dc = "D";

                    conta.ContaCorrenteCadastrar(ref conta_corrente);
                }
            }
            catch { return false; }

            return true;
        }

        public bool ContasPagarEditar(ref pm_conta_pagar adoContasPagar)
        {
            try
            {
                adoContasPagar.id_filial = Context.idFilial;
                EntityKey key = entities.CreateEntityKey("pm_conta_pagar", adoContasPagar);

                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoContasPagar);
                }
                entities.SaveChanges();

                if (adoContasPagar.vlr_pagto.HasValue && adoContasPagar.dta_pagto.HasValue)
                {
                    ContaCorrente conta = new ContaCorrente();
                    pm_conta_corrente conta_corrente;
                    bool edit = false;

                    conta_corrente = conta.GetContaCorrenteByIdContasPagar(adoContasPagar.id_conta_pagar);

                    if (conta_corrente != null)
                        edit = true;
                    else
                        conta_corrente = new pm_conta_corrente();

                    conta_corrente.id_contas_pagar = adoContasPagar.id_conta_pagar;
                    conta_corrente.id_filial = Context.idFilial;
                    conta_corrente.id_pessoa = adoContasPagar.id_pessoa;

                    conta_corrente.valor = adoContasPagar.vlr_pagto.Value;
                    conta_corrente.dta_pagamento = adoContasPagar.dta_pagto.Value;

                    conta_corrente.dta_vencimento = adoContasPagar.dta_vencimento;
                    conta_corrente.dta_emissao = DateTime.Now;
                    conta_corrente.dm_tipo_dc = "D";

                    if (!edit)
                        conta.ContaCorrenteCadastrar(ref conta_corrente);
                    else
                        conta.ContaCorrenteEditar(ref conta_corrente);
                }
            }
            catch { return false; }

            return true;
        }

        public bool ContasPagarExcluir(pm_conta_pagar adoContasPagar)
        {
            try
            {
                adoContasPagar.bl_excluido = true;
                ContasPagarEditar(ref adoContasPagar);

                if (adoContasPagar.vlr_pagto.HasValue && adoContasPagar.dta_pagto.HasValue)
                {
                    ContaCorrente conta = new ContaCorrente();
                    pm_conta_corrente conta_corrente;
                    bool edit = false;

                    conta_corrente = conta.GetContaCorrenteByIdContasPagar(adoContasPagar.id_conta_pagar);

                    if (conta_corrente != null)
                        edit = true;
                    else
                        conta_corrente = new pm_conta_corrente();

                    conta_corrente.id_contas_pagar = adoContasPagar.id_conta_pagar;
                    conta_corrente.id_filial = Context.idFilial;
                    conta_corrente.id_pessoa = adoContasPagar.id_pessoa;

                    conta_corrente.valor = adoContasPagar.vlr_pagto.Value;
                    conta_corrente.dta_pagamento = adoContasPagar.dta_pagto.Value;

                    conta_corrente.dta_vencimento = adoContasPagar.dta_vencimento;
                    conta_corrente.dta_emissao = DateTime.Now;
                    conta_corrente.dm_tipo_dc = "D";

                    if (edit)
                        conta.ContaCorrenteExcluir(conta_corrente);
                }
            }
            catch { return false; }

            return true;
        }

    }
}
