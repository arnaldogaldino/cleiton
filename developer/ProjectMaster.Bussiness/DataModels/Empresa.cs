using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data.Objects;

namespace ProjectMaster.Bussiness
{
    public class Empresa
    {
        private PMEntities entities = new PMEntities();

        public IQueryable<pm_empresa> GetEmpresaByUsuarioSenha(string user, string password)
        {
            var oFilial = new Filial();

            var filial = oFilial.GetFilialByUsuarioSenha(user, password);

            if (filial != null)
                return filial.Select(o => o.pm_empresa);

            return null;
        }

        public pm_empresa GetEmpresaByID(long id_empresa)
        {
            return (from e in entities.pm_empresa
                    where e.id_empresa.Equals(id_empresa)
                    select e).FirstOrDefault();
        }

    }
}
