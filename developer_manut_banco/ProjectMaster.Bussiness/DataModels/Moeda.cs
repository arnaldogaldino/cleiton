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
    public class Moeda
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_moeda> GetMoedaGrid()
        {
            return (from m in entities.pm_moeda
                    where m.bl_excluido == false && m.id_filial == Context.idFilial
                    orderby m.dt_cotacao descending
                    select m);
        }

        public pm_moeda GetMoedaById(long id)
        {
            return (from m in entities.pm_moeda
                    where m.id_moeda == id && m.id_filial == Context.idFilial
                    orderby m.dt_cotacao descending
                    select m).FirstOrDefault();
        }

        public void DeleteMoeda(long id)
        {
            pm_moeda moeda = entities.pm_moeda.First(i => i.id_moeda == id);

            moeda.bl_excluido = true;
            MoedaEditar(ref moeda);
        }

        public bool MoedaCadastrar(ref pm_moeda adoMoeda)
        {
            try
            {
                adoMoeda.id_filial = Context.idFilial;

                entities.AddTopm_moeda(adoMoeda);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool MoedaEditar(ref pm_moeda adoMoeda)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_moeda", adoMoeda);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoMoeda);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool MoedaExcluir(pm_moeda adoMoeda)
        {
            try
            {
                adoMoeda.bl_excluido = true;
                MoedaEditar(ref adoMoeda);
            }
            catch { return false; }

            return true;
        }

    }
}
