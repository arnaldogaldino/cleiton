using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMaster.Application.Models
{
    public class MoedaModel
    {
        public Int64? id_moeda { get; set; }

        [Display(Name = "Data de Cotação")]
        [Required(ErrorMessage = "*")]
        public DateTime dt_cotacao { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{3})*(,[0-9]+)?$", ErrorMessage = "Valor inválido.")]
        public decimal nr_valor { get; set; }

        [Display(Name = "Tipo de moeda")]
        [Required(ErrorMessage = "*")]
        public int id_tipo_moeda { get; set; }
    }
}