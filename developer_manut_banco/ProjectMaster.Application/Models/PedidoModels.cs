using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{

    public class PedidoModels
    {
        [Display(Name = "ID")]
        public long id_pedido { get; set; }

        [Display(Name = "Número do Pedido")]
        public long nr_pedido { get; set; }

        public PessoaModels Pessoa { get; set; }
        public PessoaModels.EnderecoModels Endereco { get; set; }

        [Display(Name = "Data Emissão")]
        public DateTime dt_emissao { get; set; }

        [Display(Name = "Data Vencimento")]
        public DateTime dt_vencimento { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public string dm_forma_pagto { get; set; }
        
        [Display(Name = "Staus")]
        public string dm_status_pedido { get; set; }

        [Display(Name = "Observação")]
        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        public string obs { get; set; }

        public IEnumerable<ItemModels> Items { get; set; }

        public class ItemModels
        {

            [Display(Name = "ID Pedido Item")]
            [Required]
            public long id_pedido_item { get; set; }

            [Display(Name = "ID Pedido")]
            [Required]
            public long id_pedido { get; set; }

            [Display(Name = "ID Produto")]
            [Required]
            public long id_produto { get; set; }
            
            [Display(Name = "Quantidade")]
            [Required]
            public decimal quantidade { get; set; }

            [Display(Name = "Valor Unitário")]
            [Required]
            public decimal valor_unitario { get; set; }

            [Display(Name = "Tipo Embalagem")]
            [Required]
            public string dm_tipo_embalagem { get; set; }

            [Display(Name = "Quantidade Caixas Vazias Emprestadas")]
            [Required]
            public int qtd_cx_empr { get; set; }

            [Display(Name = "Quantidade Caixas Vazias Devolvidas")]
            [Required]
            public int qtd_cx_devol { get; set; }

            [Display(Name = "Quantidade Caixas Vazias Pagas")]
            [Required]
            public int qtd_cx_pag_int { get; set; }
            
            public ProdutoModels Produto { get; set; }
        }
    }
}