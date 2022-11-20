using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;
using ProjectMaster.Bussiness.ChavesConstantes;

namespace ProjectMaster.Bussiness.DataModels
{

    public class ContaCorrente : BaseBussiness
    {

        public IEnumerable<pm_conta_corrente> Procurar(Int64 idPessoa)
        {
            if (idPessoa > 0)
            {
                return Context.pm_conta_corrente.AsQueryable().Where(t => t.id_pessoa == idPessoa);
            }
            return new List<pm_conta_corrente>();
        }
        public string PegarDescricaoTipoTelefone(string valorDoDominio)
        {
            return (from valores in Context.pm_domain_value
                    join dominio in Context.pm_domain on valores.id_domain equals dominio.id_domain
                    where dominio.domain_name == NomeDominio.TIPO_TELEFONE &&
                    valores.domain_value == valorDoDominio
                    orderby valores.ordem
                    select valores.label).First();
        }
    }
}
