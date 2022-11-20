using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMaster.Application.Models
{
    public class EnderecoModel
    {
        public Int64? id_endereco { get; set; }
        public Int64? id_pessoa { get; set; }
        public Int64 proximoId { get; set; }
        [Display(Name = "Tipo Endereço")]
        public string dm_tipo_endereco { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "*")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        public string nr_cep { get; set; }

        [Display(Name = "Logradouro")]
        [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
        [Required(ErrorMessage = "*")]
        public string nm_endereco { get; set; }

        [Display(Name = "Número")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [Required(ErrorMessage = "*")]
        public string nr_numero { get; set; }

        [Display(Name = "Complemento")]
        [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string ds_complemento { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_bairro { get; set; }

        [Display(Name = "Município")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_cidade { get; set; }

        [Display(Name = "UF")]
        [StringLength(2, ErrorMessage = "Máximo 2 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_uf { get; set; }

        [Display(Name = "País")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [Required(ErrorMessage = "*")]
        public string id_pais { get; set; }
    }
}