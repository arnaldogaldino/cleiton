using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class NCMModels
    {
        [Display(Name = "ID")]
        public long id_ncm { get; set; }
        
        [Display(Name = "NCM")]
        [Required(ErrorMessage="Campo (NCM) é obrigatório.")]
        public string ncm { get; set; }

        [StringLength(80, ErrorMessage = "Máximo 80 caracteres")]
        [Required(ErrorMessage="Campo (Descrição) é obrigatório.")]
        [Display(Name = "Descrição")]
        public string ds_descricao { get; set; }        
    }
}