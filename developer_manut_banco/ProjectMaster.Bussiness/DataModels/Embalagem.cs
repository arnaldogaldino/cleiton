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
    public class Embalagem
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_embalagem> GetEmbalagemGrid()
        {
            return (from m in entities.pm_embalagem
                    where m.id_filial == Context.idFilial &&
                        m.excluido == false
                    select m);
        }

        public pm_embalagem GetEmbalagemById(long id)
        {
            return (from m in entities.pm_embalagem
                    where m.id_filial == Context.idFilial && 
                        m.id_embalagem == id
                    orderby m.dm_tipo_embalagem
                    select m).FirstOrDefault();
        }

        public void DeleteEmbalagem(long id)
        {
            pm_embalagem embalagem = entities.pm_embalagem.First(i => i.id_embalagem == id);

            embalagem.excluido = true;
            EmbalagemEditar(ref embalagem);
        }

        public bool EmbalagemCadastrar(ref pm_embalagem adoEmbalagem)
        {
            try
            {
                adoEmbalagem.id_filial = Context.idFilial;

                entities.AddTopm_embalagem(adoEmbalagem);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool EmbalagemEditar(ref pm_embalagem adoEmbalagem)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_embalagem", adoEmbalagem);
                object originalItem;
                adoEmbalagem.id_filial = Context.idFilial;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoEmbalagem);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool EmbalagemExcluir(pm_embalagem adoEmbalagem)
        {
            try
            {
                adoEmbalagem.excluido = true;
                EmbalagemEditar(ref adoEmbalagem);
            }
            catch { return false; }

            return true;
        }

        public static SelectList GetDomainByEmbalagem(bool Selecione)
        {
            var domain = (from d in entities.pm_embalagem
                          where d.id_filial == Context.idFilial && 
                            d.excluido == false
                          select new { texto = d.dm_tipo_embalagem, valor = d.id_embalagem, id_embalagem = d.id_embalagem, ordem = 1, preco = d.valor });
            
            //d.dm_tipo_embalagem + " - R$ " + d.valor.ToString("n2"),

            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_embalagem = 0, ordem = 0 });

                foreach (var i in domain)
                    ds.Add(new { texto = i.texto + "-R$ " + i.preco.ToString("n2"), valor = i.id_embalagem.ToString(), ordem = 1 });
            }

            return new SelectList(ds, "valor", "texto");
        }

    }
}
