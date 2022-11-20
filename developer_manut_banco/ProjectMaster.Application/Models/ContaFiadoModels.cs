using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class FiadoModels
    {
        [Display(Name = "ID")]
        public long id_fiado { get; set; }

        public long? id_pedido { get; set; }

        [Display(Name = "ID Produto")]
        public long? id_produto { get; set; }
        public string cprod { get; set; }
        public string xprod { get; set; }

        [Display(Name = "ID Pessoa")]
        public long id_pessoa { get; set; }
        public string ds_marca { get; set; }
        public string ds_razao_social { get; set; }

        [Display(Name = "Data Emissão")]
        public DateTime dta_emissao { get; set; }

        [Display(Name = "Data Pagamento")]
        public DateTime? pago_ate { get; set; }

        [Display(Name = "Tipo Debito/Credito")]
        public string dm_tipo_dc { get; set; }

        [Display(Name = "Forma Pagto")]
        public string dm_forma_pagto { get; set; }

        [Display(Name = "Quantidade de Caixas")]
        public decimal? qtd_caixas { get; set; }

        [Display(Name = "Valor Frutas")]
        public decimal? vlr_frutas { get; set; }

        [Display(Name = "Quantidade de Frutas")]
        public int? qtd_frutas { get; set; }

        [Display(Name = "Ocorrência")]
        public string ocorrencia { get; set; }

        [Display(Name = "Valor Caixas")]
        public int? vlr_caixa { get; set; }

        [Display(Name = "Caixas Pagas")]
        public bool? caixa_pagas { get; set; }

        [Display(Name = "Banco")]
        public int? banco { get; set; }

        [Display(Name = "Nº do Vale")]
        public int? n_vale { get; set; }

        [Display(Name = "Observação")]
        public string obs { get; set; }
        
        public bool bl_excluido { get; set; }   //bool
        public long id_filial { get; set; }   //long

        public decimal? qtd_cx_poder { get; set; }   //decimal?

        public decimal? saldo_caixa { get; set; }   //decimal? 
        public decimal? saldo_caixa_fisico { get; set; }   //decimal?
        public decimal? saldo_fruta { get; set; }   //decimal?
        public decimal? saldo_geral { get; set; }   //decimal?
        public decimal? total_caixa { get; set; }   //decimal?
        public decimal? total_fruta { get; set; }   //decimal?
        public decimal? total_venda { get; set; }   //decimal?

    }
}
