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
    public class CentroDeCusto
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_centro_de_custo> GetCentroDeCustoGrid()
        {
            return (from m in entities.pm_centro_de_custo
                    where m.id_filial == Context.idFilial && 
                        m.bl_excluido == false
                    select m);
        }

        public pm_centro_de_custo GetCentroDeCustoById(long id)
        {
            return (from m in entities.pm_centro_de_custo
                    where m.id_filial == Context.idFilial && 
                        m.id_centro_de_custo == id
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }

        public pm_centro_de_custo GetCentroDeCustoByCodigo(long id_centro_de_custo, string codigo)
        {
            return (from m in entities.pm_centro_de_custo
                    where m.id_filial == Context.idFilial &&
                        m.ds_codigo == codigo && m.id_centro_de_custo != id_centro_de_custo
                    orderby m.ds_codigo
                    select m).FirstOrDefault();
        }

        public void DeleteCentroDeCusto(long id)
        {
            pm_centro_de_custo centro_de_custo = entities.pm_centro_de_custo.First(i => i.id_centro_de_custo == id);

            centro_de_custo.bl_excluido = true;
            CentroDeCustoEditar(ref centro_de_custo);
        }

        public bool CentroDeCustoCadastrar(ref pm_centro_de_custo adoCentroDeCusto)
        {
            try
            {
                adoCentroDeCusto.id_filial = Context.idFilial;

                entities.AddTopm_centro_de_custo(adoCentroDeCusto);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool CentroDeCustoEditar(ref pm_centro_de_custo adoCentroDeCusto)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_centro_de_custo", adoCentroDeCusto);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoCentroDeCusto);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool CentroDeCustoExcluir(pm_centro_de_custo adoCentroDeCusto)
        {
            try
            {
                adoCentroDeCusto.bl_excluido = true;
                CentroDeCustoEditar(ref adoCentroDeCusto);
            }
            catch { return false; }

            return true;
        }

        public static SelectList GetDomainByCentroDeCusto(bool Selecione)
        {
            var domain = (from d in entities.pm_centro_de_custo
                          where d.id_filial == Context.idFilial && 
                            d.bl_excluido == false
                          orderby d.ds_codigo, d.ds_descricao
                          select new { texto = d.ds_descricao, valor = d.ds_codigo, id_centro_de_custo = d.id_centro_de_custo, ordem = 1 });

            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_centro_de_custo = 0, ordem = 0 });

                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_centro_de_custo.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
            else
            {
                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_centro_de_custo.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
        }
    }
}
