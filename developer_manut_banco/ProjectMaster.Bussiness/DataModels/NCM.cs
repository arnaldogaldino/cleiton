using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using System.Web.Mvc;
using System.Data;
using System.Data.Objects;

namespace ProjectMaster.Bussiness
{
    public class NCM
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_ncm> GetNCMGrid()
        {
            return (from m in entities.pm_ncm
                    select m);
        }

        public pm_ncm GetNCMById(long id)
        {
            return (from m in entities.pm_ncm
                    where m.id_ncm == id
                    orderby m.ncm
                    select m).FirstOrDefault();
        }

        public pm_ncm NCMExistDiferente(long id_ncm, string ncm)
        {
            return (from m in entities.pm_ncm
                    where m.ncm == ncm && id_ncm != id_ncm
                    orderby m.ncm
                    select m).FirstOrDefault();
        }

        public pm_ncm GetNCMByNCM(string ncm)
        {
            return (from m in entities.pm_ncm
                    where m.ncm == ncm
                    orderby m.ncm
                    select m).FirstOrDefault();
        }
        
        public void DeleteNCM(long id)
        {
            pm_ncm ncm = entities.pm_ncm.First(i => i.id_ncm== id);

            entities.pm_ncm.Detach(ncm);
            entities.SaveChanges();
        }

        public bool NCMCadastrar(ref pm_ncm adoNCM)
        {
            try
            {
                entities.AddTopm_ncm(adoNCM);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool NCMEditar(ref pm_ncm adoNCM)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_ncm", adoNCM);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoNCM);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool NCMExcluir(pm_ncm adoNCM)
        {
            try
            {
                entities.DeleteObject(adoNCM);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public static SelectList GetDomainByNCM(bool Selecione)
        {
            var domain = (from d in entities.pm_ncm
                          orderby d.ncm, d.ds_descricao
                          select new { texto = d.ds_descricao, valor = d.ncm, id_ncm = d.id_ncm, ordem = 1 } );
            
            List<object> ds = new List<object>();

            if (Selecione)
            {
                ds.Add(new { texto = "Selecione...", valor = "", id_ncm = 0, ordem = 0 });

                foreach(var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_ncm.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
            else
            {
                foreach (var i in domain)
                    ds.Add(new { texto = i.valor.ToString() + " - " + i.texto, valor = i.id_ncm.ToString(), ordem = 1 });

                return new SelectList(ds, "valor", "texto");
            }
        }

    }
}
