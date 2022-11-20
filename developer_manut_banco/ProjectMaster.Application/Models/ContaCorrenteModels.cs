using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMaster.Application.Models
{
    public class ContaCorrenteModels
    {
        [Display(Name = "ID")]
        public long id_conta_corrente { get; set; }
        public long id_filial { get; set; }

        public long? id_fiado { get; set; }
        public long? id_pedido { get; set; }
        public long? id_contas_pagar { get; set; }
        
        [Display(Name = "ID Pessoa")]
        public long id_pessoa { get; set; }
        public string ds_marca { get; set; }
        public string ds_razao_social { get; set; }
        public PessoaModels Pessoa { get; set; }

        [Display(Name = "Data Emissão")]
        public DateTime dta_emissao { get; set; }
        
        public DateTime? dta_emissao_de { get; set; }
        public DateTime? dta_emissao_ate { get; set; }

        [Display(Name = "Data Vencimento")]
        public DateTime? dta_vencimento { get; set; }

        public DateTime? dta_vencimento_de { get; set; }
        public DateTime? dta_vencimento_ate { get; set; }

        [Display(Name = "Data Pagamento")]
        public DateTime? dta_pagamento { get; set; }

        [Display(Name = "Tipo Debito/Credito")]
        public string dm_tipo_dc { get; set; }

        [Display(Name = "Valor")]
        public decimal valor { get; set; }

        [Display(Name = "Saldo")]
        public decimal saldo { get; set; }
        
    }
}