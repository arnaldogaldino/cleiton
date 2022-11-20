using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using System.Web.Mvc;

namespace ProjectMaster.Bussiness
{
    public class Domain
    {
        public enum Tipos
        {
            Bancos = 6,
            CodigoContabil = 32,
            CST = 12,
            CSTOrigem = 13,
            Enquadramento = 29,
            FormaPagamento = 34,
            LimiteCredito = 4,
            ModBC = 14,
            ModBCST = 15,
            OpcaoAceitacao = 3,
            StatusPedido = 35,
            TipoCliente = 1,
            TipoClienteCredito = 33,
            TipoDocumento = 2,
            TipoEan = 11,
            TipoEmbalagem = 10,
            ValorEmbalagem = 39,
            TipoEndereco = 7,
            TipoHistorico = 30,
            TipoMoeda = 9,
            TipoSituacaoFinanceira = 31,
            TipoTelefone = 5,
            Uf = 8,
            TipoEstoque = 36,
            TipoDebitoCredito = 37,
            FiadoOcorrencia = 38,
        }

        private static PMEntities entities = new PMEntities();
        
        public static SelectList GetDomainByTipo(Tipos tipo, bool Selecione)
        {
            var id_domain = (int)((Tipos)Enum.Parse(typeof(Tipos), tipo.ToString()));

            var domain = (from f in entities.pm_domain
                          where f.id_domain == id_domain
                          select f).FirstOrDefault();
            if (Selecione)
            {               
                var listValues = domain.pm_domain_value.ToList();
                listValues.Add(new pm_domain_value() { ordem = 0, label = "Selecione...", domain_value = "" });
 
                return new SelectList(listValues.OrderBy(x=>x.ordem), "domain_value", "label");
            }
            else
            {
                return ((IEnumerable<pm_domain_value>)domain.pm_domain_value).ToSelectList
                    (texto => texto.label, valor => valor.domain_value);
            }
        }

        public static SelectList GetDomainByTipo(Tipos tipo)
        {
            return GetDomainByTipo(tipo, false);
        }

        public static string GetTextByDomain(Tipos tipo, string value)
        {
            string result = string.Empty;

            var id_domain = (int)((Tipos)Enum.Parse(typeof(Tipos), tipo.ToString()));

            var domain = (from f in entities.pm_domain
                          where f.id_domain == id_domain
                          select f).FirstOrDefault();
            try
            {
                result = domain.pm_domain_value.Where(o => o.domain_value == value).FirstOrDefault().label;
            }
            catch{}

            return result;
        }
    }
}
