using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMaster.Application.Models
{
    public class TelefoneModel
    {
        public Int64? id_telefone { get; set; }
        public Int64? id_pessoa { get; set; }
        public Int64 proximoId { get; set; }

        [Display(Name = "Tipo de Telefone")]
        [Required(ErrorMessage = "*")]
        public string dm_tipo_telefone { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(14, ErrorMessage = "Máximo 14 caracteres")]
        [Required(ErrorMessage = "*")]
        public string nr_telefone { get; set; }

        [Display(Name = "Contato")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [Required(ErrorMessage = "*")]
        public string nm_contato { get; set; }
    }
}