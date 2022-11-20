using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class LimiteCreditoModels
    {
        [Display(Name = "ID")]
        public long id_limite_credito { get; set; }

        [Display(Name = "Código")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage = "Campo (Código) é obrigatório.")]
        public string ds_codigo { get; set; }

        [StringLength(60, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage = "Campo (Descrição) é obrigatório.")]
        [Display(Name = "Descrição")]
        public string ds_descricao { get; set; }

        [Required(ErrorMessage = "Campo (Dias de Crédito) é obrigatório.")]
        [Display(Name = "Dias de Crédito")]
        public int dias_credito { get; set; }

        [Required(ErrorMessage = "Campo (Valor de Crédito) é obrigatório.")]
        [Display(Name = "Valor de Crédito")]
        public decimal valor_credito { get; set; }
        
    }
}