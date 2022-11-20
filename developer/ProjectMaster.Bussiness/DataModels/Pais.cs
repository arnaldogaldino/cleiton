using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;

namespace ProjectMaster.Bussiness.DataModels
{

    public class Pais : BaseBussiness
    {
        public IEnumerable<pm_pais> PegarPaises()
        {
            return Context.pm_pais.AsQueryable();
        }

        public string PegarDescricao(long idPais)
        {
            return Context.pm_pais.AsQueryable()
                     .Where(b => b.id_pais == idPais)
                     .Select(b => b.nm_nome).First();
        }
    }
}
