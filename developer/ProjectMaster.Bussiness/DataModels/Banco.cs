using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;

namespace ProjectMaster.Bussiness.DataModels
{

    public class Banco : BaseBussiness
    {
        public IEnumerable<pm_banco> PegarBancos()
        {
            return Context.pm_banco.AsQueryable();
        }

        public string PegarDescricao(Int64 idBanco)
        {
            return Context.pm_banco.AsQueryable()
                       .Where(b => b.id_banco == idBanco)
                       .Select(b => b.ds_banco).First();
        }
    }
}
