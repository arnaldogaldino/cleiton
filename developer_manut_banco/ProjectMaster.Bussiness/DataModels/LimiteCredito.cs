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
    public class LimiteCredito
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_limite_credito> GetLimiteCreditoGrid()
        {
            return (from m in entities.pm_limite_credito
                    where m.bl_excluido == false && m.id_filial == Context.idFilial
                    select m);
        }

        public pm_limite_credito GetLimiteCreditoById(long id)
        {
            return (from m in entities.pm_limite_credito
                    where m.id_limite_credito == id && m.id_filial == Context.idFilial
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }

        public pm_limite_credito GetLimiteCreditoByCodigo(long id_limite_credito, string codigo)
        {
            return (from m in entities.pm_limite_credito
                    where m.ds_codigo == codigo && m.id_filial == Context.idFilial && m.id_limite_credito == id_limite_credito
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }

        public void DeleteLimiteCredito(long id)
        {
            pm_limite_credito limite_credito = entities.pm_limite_credito.First(i => i.id_limite_credito == id);

            limite_credito.bl_excluido = true;
            LimiteCreditoEditar(ref limite_credito);
        }

        public bool LimiteCreditoCadastrar(ref pm_limite_credito adoLimiteCredito)
        {
            try
            {
                adoLimiteCredito.id_filial = Context.idFilial;
                entities.AddTopm_limite_credito(adoLimiteCredito);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool LimiteCreditoEditar(ref pm_limite_credito adoLimiteCredito)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_limite_credito", adoLimiteCredito);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoLimiteCredito);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool LimiteCreditoExcluir(pm_limite_credito adoLimiteCredito)
        {
            try
            {
                adoLimiteCredito.bl_excluido = true;
                LimiteCreditoEditar(ref adoLimiteCredito);
            }
            catch { return false; }

            return true;
        }

        public static SelectList GetDomainByLimiteCredito(bool Selecione)
        {
            var domain = (from d in entities.pm_limite_credito
                          orderby d.ds_codigo, d.ds_descricao
                          select new { texto = d.ds_descricao, valor = d.ds_codigo, id_limite_credito = d.id_limite_credito, ordem = 1 });

            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_limite_credito = 0, ordem = 0 });

                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_limite_credito.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
            else
            {
                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_limite_credito.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
        }
    }
}
