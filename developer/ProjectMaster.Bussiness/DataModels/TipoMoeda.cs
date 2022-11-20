using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;

namespace ProjectMaster.Bussiness.DataModels
{

    public class TipoMoeda : BaseBussiness
    {
        public IEnumerable<pm_tipo_moeda> PegarTiposDeMoedas()
        {
            return Context.pm_tipo_moeda.AsQueryable();
        }
    }
}
