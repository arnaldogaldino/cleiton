using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class ProdutoModels
    {
        [Display(Name = "Id Produto")]
        public long id_produto { get; set; }

        public long id_filial { get; set; }

        [Display(Name = "Código do Produto")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        public string cprod { get; set; }

        [Display(Name = "Descrição do Produto")]
        [StringLength(120, ErrorMessage = "Máximo 120 caracteres")]
        [Required(ErrorMessage = "Campo (Descrição do Produto) é obrigatório")]
        public string xprod { get; set; }

        [Display(Name = "Valor Embalagem")]
        [Required(ErrorMessage = "Campo (Valor Embalagem) é obrigatório")]
        public long? id_embalagem { get; set; }

        [Display(Name = "Tipo de Embalagem do Produto")]
        [Required(ErrorMessage = "Campo (Tipo de Embalagem do Produto) é obrigatório")]
        public string tp_embalagem { get; set; }

        //[DataType(DataType.Currency)]
        //[Display(Name = "Valor da Embalagem")]
        //[Required(ErrorMessage = "Campo (Valor da Embalagem) é obrigatrório")]
        //public int? vl_embalagem { get; set; }

        [Display(Name = "Código EAN")]
        [StringLength(14, ErrorMessage = "Máximo 14 caracteres")]
        public string cean { get; set; }

        [Display(Name = "Código NCM")]
        public long id_ncm { get; set; }

        [Display(Name = "Código EXTIPI")]
        [StringLength(3, ErrorMessage = "Máximo 3 caracteres")]
        public string extipi { get; set; }

        [Display(Name = "Gênero do Produto")]
        public int? genero { get; set; }

        [Display(Name = "Unidade Comercial")]
        [StringLength(6, ErrorMessage = "Máximo 6 caracteres")]
        [Required(ErrorMessage = "Campo (Unidade Comercial) é obrigatrório")]
        public string ucom { get; set; }

        [Display(Name = "Valor de Comercialização")]
        public decimal? vuncom { get; set; }

        [Display(Name = "Código EAN Tributário")]
        public string ceantrib { get; set; }

        [Display(Name = "Unidade Tributável")]
        [StringLength(6, ErrorMessage = "Máximo 6 caracteres")]
        public string utrib { get; set; }

        [Display(Name = "Valor Unitário de Tributação")]
        public decimal? vuntrib { get; set; }

        [Display(Name = "Quantidade Tributável")]
        public decimal? qtrib { get; set; }

        [Display(Name = "Marcador de Cadastro de IPI")]
        [Required(ErrorMessage = "Campo (Marcador de Cadastro de IPI) é obrigatório.")]
        public int mipi { get; set; }

        [Display(Name = "Quantidade de Registro N para o Produto")]
        [Required(ErrorMessage = "Campo (Quantidade de Registro N para o Produto) é obrigatório.")]
        public int qtden { get; set; }

        [Display(Name = "Classe de enquadramento do IPI para Cigarros e Bebidas")]
        public decimal? cienq { get; set; }

        [Display(Name = "CNPJ Produtor")]
        [StringLength(18, ErrorMessage = "Máximo 14 caracteres")]
        public string cnpjprod { get; set; }

        [Display(Name = "Código Enquadramento Legal do IPI")]
        public string cenq { get; set; }

        [Display(Name = "Tributação do ICMS")]
        [Required(ErrorMessage = "Campo (Tributação do ICMS) é obrigatório.")]
        public string cst { get; set; }

        [Display(Name = "Origem da Mercadoria")]
        public string orig { get; set; }

        [Display(Name = "Modalidade de determinação da BC do ICMS")]
        public string modbc { get; set; }

        [Display(Name = "Alíquota de Imposto")]
        public decimal? picms { get; set; }

        [Display(Name = "Percentual da Redução de BC")]
        public decimal? predbc { get; set; }

        [Display(Name = "Modalidade de determinação da BC do ICMS ST")]
        public string modbcst { get; set; }

        [Display(Name = "Percentual da Redução de BC do ICMS ST")]
        public decimal? predbcst { get; set; }

        [Display(Name = "Percentual da margem de valor Adicionado do ICMS ST")]
        public decimal? pmvast { get; set; }
    }
}