using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class EmbalagemModels
    {
        [Display(Name = "ID")]
        public long id_embalagem { get; set; }
        
        public long id_filial { get; set; }
        
        [Display(Name = "Tipo de Embalagem")]
        [Required(ErrorMessage = "Campo (Tipo de Embalagem) é obrigatório.")]
        public string dm_tipo_embalagem { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Campo (Valor) é obrigatório.")]
        public decimal valor { get; set; }

    }
}