using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;

namespace ProjectMaster.Bussiness.DataModels
{
    public class NCM : BaseBussiness
    {
        public IEnumerable<pm_ncm> PegarNCM()
        {
            return Context.pm_ncm.AsQueryable();
        }
    }
}
