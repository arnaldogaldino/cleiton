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
    public class Classificacao
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_classificacao> GetClassificacaoGrid()
        {
            return (from m in entities.pm_classificacao
                    where m.id_filial == Context.idFilial &&
                        m.bl_excluido == false
                    select m);
        }

        public pm_classificacao GetClassificacaoById(long id)
        {
            return (from m in entities.pm_classificacao
                    where m.id_filial == Context.idFilial && 
                        m.id_classificacao == id
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }
        public pm_classificacao GetClassificacaoByCodigo(long id_classificacao, string codigo)
        {
            return (from m in entities.pm_classificacao
                    where m.id_filial == Context.idFilial &&
                        m.ds_codigo == codigo && m.id_classificacao != id_classificacao
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }
        public void DeleteClassificacao(long id)
        {
            pm_classificacao classificacao = entities.pm_classificacao.First(i => i.id_classificacao == id);

            classificacao.bl_excluido = true;
            ClassificacaoEditar(ref classificacao);
        }

        public bool ClassificacaoCadastrar(ref pm_classificacao adoClassificacao)
        {
            try
            {
                adoClassificacao.id_filial = Context.idFilial;

                entities.AddTopm_classificacao(adoClassificacao);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool ClassificacaoEditar(ref pm_classificacao adoClassificacao)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_classificacao", adoClassificacao);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoClassificacao);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool ClassificacaoExcluir(pm_classificacao adoClassificacao)
        {
            try
            {
                adoClassificacao.bl_excluido = true;
                ClassificacaoEditar(ref adoClassificacao);
            }
            catch { return false; }

            return true;
        }

        public static SelectList GetDomainByClassificacao(bool Selecione)
        {
            var domain = (from d in entities.pm_classificacao
                          where d.id_filial == Context.idFilial && 
                            d.bl_excluido == false
                          orderby d.ds_codigo, d.ds_descricao
                          select new { texto = d.ds_descricao, valor = d.ds_codigo, id_classificacao = d.id_classificacao, ordem = 1 });

            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_classificacao = 0, ordem = 0 });

                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_classificacao.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
            else
            {
                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_classificacao.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
        }

    }
}
