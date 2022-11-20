using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;
using System.Collections;

namespace ProjectMaster.Application.Models
{
    public class EstoqueModels
    {        
        [Display(Name = "ID")]
        public long id_produto_estoque_entrada { get; set; }

        [Display(Name = "ID Produto")]
        public long id_produto { get; set; }
        public string cprod { get; set; }
        public string xprod { get; set; }

        [Display(Name = "ID Pessoa")]
        public long id_pessoa { get; set; }
        public string ds_marca { get; set; }
        public string ds_razao_social { get; set; }

        public string dm_tipo { get; set; } 

        public DateTime dta_entrada { get; set; } 
        public DateTime dta_doc { get; set; } 

        public decimal quantidade { get; set; }
        
        [StringLength(50, ErrorMessage = "Máximo 60 caracteres")]
        [Required(ErrorMessage = "Campo (Descrição) é obrigatório.")]
        [Display(Name = "Descrição")]
        public string descricao { get; set; }

        public DateTime dta_inicio { get; set; }
        public DateTime dta_fim { get; set; }

        public PessoaModels Pessoa { get; set; }
        public ProdutoModels Produto { get; set; }

        public IList<long> chkEstoque { get; set; }
    }
}