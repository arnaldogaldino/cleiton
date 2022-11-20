using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Core.Validations;

namespace ProjectMaster.Application.Models
{
    public class ProdutoModel
    {
        [Display(Name = "Código do Produto")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [ValidationCProdExist("Código de produto já cadastrado", true)]
        public string cProd { get; set; }

        [Display(Name = "Descrição do Produto")]
        [StringLength(120, ErrorMessage = "Máximo 120 caracteres")]
        [Required(ErrorMessage = "*")]
        public string xProd { get; set; }

        [Display(Name = "Tipo de Embalagem do Produto")]
        [Required(ErrorMessage = "*")]
        public string TP_EMBALAGEM { get; set; }

        [Display(Name = "Valor da Embalagem")]
        [Required(ErrorMessage = "*")]
        public decimal? VL_EMBALAGEM { get; set; }

        [Display(Name = "Código EAN")]
        [StringLength(14, ErrorMessage = "Máximo 14 caracteres")]
        public string cEAN { get; set; }

        [Display(Name = "Código NCM")]
        [StringLength(8, ErrorMessage = "Máximo 8 caracteres")]
        public string NCM { get; set; }

        [Display(Name = "Código EXTIPI")]
        [StringLength(3, ErrorMessage = "Máximo 3 caracteres")]
        public string EXTIPI { get; set; }

        [Display(Name = "Gênero do Produto")]
        public int? Genero { get; set; }

        [Display(Name = "Unidade Comercial")]
        [StringLength(6, ErrorMessage = "Máximo 6 caracteres")]
        public string uCom { get; set; }

        [Display(Name = "Valor de Comercialização")]
        public int? vUnCom { get; set; }

        [Display(Name = "Código EAN Tributário")]
        public string cEANTrib { get; set; }

        [Display(Name = "Unidade Tributável")]
        [StringLength(6, ErrorMessage = "Máximo 6 caracteres")]
        public string uTrib { get; set; }

        [Display(Name = "Valor Unitário de Tributação")]
        public int? vUnTrib { get; set; }

        [Display(Name = "Quantidade Tributável")]
        public int? qTrib { get; set; }

        [Display(Name = "Marcador de Cadastro de IPI")]
        [Required(ErrorMessage = "*")]
        public int mIPI { get; set; }

        [Display(Name = "Quantidade de Registro N para o Produto")]
        [Required(ErrorMessage = "*")]
        public int qtdeN { get; set; }

        [Display(Name = "Classe de enquadramento do IPI para Cigarros e Bebidas")]
        [StringLength(5, ErrorMessage = "Máximo 5 caracteres")]
        public string cIEnq { get; set; }

        [Display(Name = "CNPJ Produtor")]
        [StringLength(14, ErrorMessage = "Máximo 14 caracteres")]
        public string CNPJProd { get; set; }

        [Display(Name = "Código Enquadramento Legal do IPI")]
        public string cEnq { get; set; }

        [Display(Name = "Tributação do ICMS")]
        [Required(ErrorMessage = "*")]
        public string CST { get; set; }

        [Display(Name = "Origem da Mercadoria")]
        public string Orig { get; set; }

        [Display(Name = "Modalidade de determinação da BC do ICMS")]
        public string modBC { get; set; }

        [Display(Name = "Alíquota de Imposto")]
        public int? pICMS { get; set; }

        [Display(Name = "Percentual da Redução de BC")]
        public int? pRedBC { get; set; }

        [Display(Name = "Modalidade de determinação da BC do ICMS ST")]
        public string modBCST { get; set; }

        [Display(Name = "Percentual da Redução de BC do ICMS ST")]
        public int? pRedBCST { get; set; }

        [Display(Name = "Percentual da margem de valor Adicionado do ICMS ST")]
        public int? pMVAST { get; set; }
    }
}