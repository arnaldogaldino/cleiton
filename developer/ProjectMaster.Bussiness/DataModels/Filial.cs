using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;

namespace ProjectMaster.Bussiness
{
    public class Filial
    {
        public Filial()
        { }

        private PMEntities entities = new PMEntities();

        public IQueryable<pm_filial> GetFilialByUsuarioSenha(string user, string password)
        {            
            return (IQueryable<pm_filial>)(from f in entities.pm_filial
                                           from u in entities.pm_usuario
                                          where u.usuario.Equals(user, StringComparison.OrdinalIgnoreCase) &&
                                                u.senha.Equals(password, StringComparison.OrdinalIgnoreCase) &&
                                                f.pm_usuario.Contains(u)
                                         select f);
        }

        public IQueryable<pm_filial> GetFilialByUsuarioSenha(string user, string password, long id_empresa)
        {
            return (IQueryable<pm_filial>)(from f in entities.pm_filial
                                           from u in entities.pm_usuario
                                          where u.usuario.Equals(user, StringComparison.OrdinalIgnoreCase) &&
                                                u.senha.Equals(password, StringComparison.OrdinalIgnoreCase) &&
                                                f.pm_usuario.Contains(u) &&
                                                f.id_empresa == id_empresa
                                         select f);
        }
        
        public pm_filial GetFilialByID(long id_filial)
        {
            return (from f in entities.pm_filial
                    where f.id_filial.Equals(id_filial)
                    select f).FirstOrDefault();
        }

    }
}
