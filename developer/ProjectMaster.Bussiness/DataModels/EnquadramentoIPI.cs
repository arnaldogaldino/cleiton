using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;

namespace ProjectMaster.Bussiness.DataModels
{
    public class EnquadramentoIPI : BaseBussiness
    {
        public IEnumerable<pm_enquadramento_ipi> PegarEnquadramentoIPI()
        {
            return Context.pm_enquadramento_ipi.AsQueryable();
        }
    }
}
