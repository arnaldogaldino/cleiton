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
    public class Historico
    {
       private static PMEntities entities = new PMEntities();

        public IQueryable<pm_historico> GetHistoricoGrid()
        {
            return (from m in entities.pm_historico
                    where m.bl_excluido == false && m.id_filial == Context.idFilial
                    select m);
        }

        public pm_historico GetHistoricoById(long id)
        {
            return (from m in entities.pm_historico
                    where m.id_historico == id && m.id_filial == Context.idFilial
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }

        public pm_historico GetHistoricoByCodigo(long id_historico, string codigo)
        {
            return (from m in entities.pm_historico
                    where m.ds_codigo == codigo && m.id_filial == Context.idFilial
                    && m.id_historico != id_historico
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }

        public void DeleteHistorico(long id)
        {
            pm_historico historico = entities.pm_historico.First(i => i.id_historico == id);

            historico.bl_excluido = true;
            HistoricoEditar(ref historico);
        }

        public bool HistoricoCadastrar(ref pm_historico adoHistorico)
        {
            try
            {
                adoHistorico.id_filial = Context.idFilial;

                entities.AddTopm_historico(adoHistorico);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool HistoricoEditar(ref pm_historico adoHistorico)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_historico", adoHistorico);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoHistorico);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool HistoricoExcluir(pm_historico adoHistorico)
        {
            try
            {
                adoHistorico.bl_excluido = true;
                HistoricoEditar(ref adoHistorico);
            }
            catch { return false; }

            return true;
        }

        public static SelectList GetDomainByHistorico(bool Selecione)
        {
            var domain = (from d in entities.pm_historico
                          orderby d.ds_codigo, d.ds_descricao
                          select new { texto = d.ds_descricao, valor = d.ds_codigo, id_historico = d.id_historico, ordem = 1 });

            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_historico = 0, ordem = 0 });

                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_historico.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
            else
            {
                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_historico.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
        }
    }
}
