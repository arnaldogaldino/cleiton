using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class ClassificacaoModels
    {
        [Display(Name = "ID")]
        public long id_classificacao { get; set; }
        
        [Display(Name = "Código")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage="Campo (Código) é obrigatório.")]
        public string ds_codigo { get; set; }

        [StringLength(60, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage="Campo (Descrição) é obrigatório.")]
        [Display(Name = "Descrição")]
        public string ds_descricao { get; set; }        
    }
}