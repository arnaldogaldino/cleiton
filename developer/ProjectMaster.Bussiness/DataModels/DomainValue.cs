using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ProjectMaster.Data;
using ProjectMaster.Bussiness.ChavesConstantes;

namespace ProjectMaster.Bussiness.DataModels
{
    public class DomainValue : BaseBussiness
    {
        public IEnumerable<pm_domain_value> PegarTipoDeCliente()
        {
            return PegarValoresPorDominio(NomeDominio.TIPO_DE_CLIENTE);
        }

        public IEnumerable<pm_domain_value> PegarOpcoesDeAceitacao()
        {
            return PegarValoresPorDominio(NomeDominio.OPCOES_DE_ACEITACAO);
        }

        public IEnumerable<pm_domain_value> PegarTipoDeDocumento()
        {
            return PegarValoresPorDominio(NomeDominio.TIPO_DE_DOCUMENTO);
        }

        public IEnumerable<pm_domain_value> PegarLimiteDeCredito()
        {
            return PegarValoresPorDominio(NomeDominio.LIMITE_CREDITO);
        }

        public IEnumerable<pm_domain_value> PegarTipoDeTelefone()
        {
            return PegarValoresPorDominio(NomeDominio.TIPO_TELEFONE);
        }
        public IEnumerable<pm_domain_value> PegarTipoDeEndereco()
        {
            return PegarValoresPorDominio(NomeDominio.TIPO_ENDERECO);
        }

        private IEnumerable<pm_domain_value> PegarValoresPorDominio(string nomeDominio)
        {
            return (from valores in Context.pm_domain_value
                    join dominio in Context.pm_domain on valores.id_domain equals dominio.id_domain
                    where dominio.domain_name == nomeDominio
                    orderby valores.ordem
                    select valores);
        }

        public string PegarDescricaoTelefone(string valorDoDominio)
        {
            return PegarDescricao(NomeDominio.TIPO_TELEFONE, valorDoDominio);
        }

        public string PegarDescricaoTipoEndereco(string valorDoDominio)
        {
            return PegarDescricao(NomeDominio.TIPO_ENDERECO, valorDoDominio);
        }

        private string PegarDescricao(string nomeDominio, string valorDoDominio)
        {
            return (from valores in Context.pm_domain_value
                    join dominio in Context.pm_domain on valores.id_domain equals dominio.id_domain
                    where dominio.domain_name == nomeDominio &&
                    valores.domain_value == valorDoDominio
                    orderby valores.ordem
                    select valores.label).First();
        }
        
        public IEnumerable<pm_domain_value> PegarTipoEmbalagem()
        {
            return PegarValoresPorDominio(NomeDominio.TIPO_EMBALAGEM);
        }

        public IEnumerable<pm_domain_value> PegarTipoEAN()
        {
            return PegarValoresPorDominio(NomeDominio.TIPO_EAN);
        }

        public IEnumerable<pm_domain_value> PegarCST()
        {
            return PegarValoresPorDominio(NomeDominio.CST);
        }

        public IEnumerable<pm_domain_value> PegarCSTOrigem()
        {
            return PegarValoresPorDominio(NomeDominio.CST_ORIGEM);
        }

        public IEnumerable<pm_domain_value> PegarModBC()
        {
            return PegarValoresPorDominio(NomeDominio.MOD_BC);
        }

        public IEnumerable<pm_domain_value> PegarMODBCST()
        {
            return PegarValoresPorDominio(NomeDominio.MOD_BC_ST);
        }

    }
}
