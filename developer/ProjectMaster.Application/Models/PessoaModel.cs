using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMaster.Application.Models
{
    public class PessoaModel
    {
        public Int64? id_pessoa { get; set; }
        public Int64 proximoId { get; set; }
        [Display(Name = "Data de Emissão")]
        public DateTime? dt_cadastro { get; set; }

        [Display(Name = "Marca/Código")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_marca { get; set; }

        [Display(Name = "Razão Social")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_razao_social { get; set; }

        [Display(Name = "Nome Fantasia")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_nome_fantasia { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "*")]
        public string dm_tipo_pessoa { get; set; }

        [Display(Name = "Tipo Documento")]
        [Required(ErrorMessage = "*")]
        public string ds_fisico_juridico { get; set; }

        [Display(Name = "Número Documento")]
        [StringLength(18, ErrorMessage = "Máximo 18 caracteres")]
        [Required(ErrorMessage = "*")]
        public string nr_documento { get; set; }

        [Display(Name = "Inscrição Estadual")]
        [StringLength(14, ErrorMessage = "Máximo 14 caracteres")]
        public string nr_ie { get; set; }

        [Display(Name = "Inscrição SUFRAMA")]
        [StringLength(9)]
        public string nr_inscricao_suframa { get; set; }

        [Display(Name = "Limite de Crédito")]
        [StringLength(6, ErrorMessage = "Máximo 6 caracteres")]
        [Required(ErrorMessage = "*")]
        public string dm_dias_limite_credito { get; set; }

        [Display(Name = "Venda Agendada")]
        [Required(ErrorMessage = "*")]
        public string dm_venda_agendada { get; set; }

        public List<EnderecoModel> enderecos { get; set; }
        public List<TelefoneModel> telefones { get; set; }
        public List<ContaCorrenteModel> contas_correntes { get; set; }
    }
}