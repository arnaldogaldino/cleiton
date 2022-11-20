using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data.Objects;

namespace ProjectMaster.Bussiness.DataModels
{
    public abstract class BaseBussiness
    {
        protected PMEntities Context { get; set; }

        public BaseBussiness()
        {
            Context = new PMEntities();
        }       
    }
}
