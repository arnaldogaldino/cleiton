using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class MoedaModels
    {
        [Display(Name = "ID")]
        public long id_moeda { get; set; }
        
        [Display(Name = "Data da Cotação")]
        [Required(ErrorMessage="Campo (Data da Cotação) é obrigatório.")]
        public DateTime dt_cotacao { get; set; }

        [Required(ErrorMessage="Campo (Valor) é obrigatório.")]
        [Display(Name = "Valor")]
        public decimal nr_valor { get; set; }     
   
        [Display(Name = "Tipo Moeda")]
        [Required(ErrorMessage = "Campo (Tipo de Moeda) é obrigatório.")]
        public string dm_tipo_moeda { get; set; }
    }
}