using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMaster.Application.Models
{
    public class CentroDeCustoModel
    {
        public Int64 id_centro_de_custo { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "*")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        public string ds_codigo { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_descricao { get; set; }
    }
}