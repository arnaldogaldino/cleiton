using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class ContaPagarModels
    {

        [Display(Name = "ID")]
        public long id_conta_pagar { get; set; }

        [Display(Name = "Filial")]
        public long id_filial { get; set; }

        [Display(Name = "Fornecedor")]
        public long id_pessoa { get; set; }
        public PessoaModels Pessoa { get; set; }
        public string ds_marca { get; set; }
        public string ds_razao_social { get; set; }

        [Display(Name = "Centro de Custo")]
        public long id_centro_de_custo { get; set; }
        public string ds_codigo { get; set; }
        public string ds_descricao { get; set; }

        [Display(Name = "Data de Emissão")]
        public DateTime dta_emissao { get; set; }

        [Display(Name = "Data de Vencimento")]
        public DateTime dta_vencimento { get; set; }

        [Display(Name = "Data de Pagamento")]
        public DateTime? dta_pagto { get; set; }

        [Display(Name = "Valor do Titulo")]
        public decimal vlr_titulo { get; set; }

        [Display(Name = "Valor do Juros")]
        public decimal vlr_juros { get; set; }

        [Display(Name = "Valor da Multa")]
        public decimal vlr_multa { get; set; }

        [Display(Name = "Valor do Pagamento")]
        public decimal? vlr_pagto { get; set; }

        [Display(Name = "Observação")]
        public string obs { get; set; }

    }
}