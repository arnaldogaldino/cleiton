using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;
using ProjectMaster.Bussiness.ChavesConstantes;

namespace ProjectMaster.Bussiness.DataModels
{

    public class Endereco : BaseBussiness
    {

        public IEnumerable<pm_endereco> Procurar(Int64 idPessoa)
        {
            if (idPessoa > 0)
            {
                return Context.pm_pessoa.AsQueryable().Where(p => idPessoa == p.id_pessoa).First().pm_endereco.ToList();
            }
            return new List<pm_endereco>();
        }
    }
}
