using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class HistoricoModels
    {
        [Display(Name = "ID")]
        public long id_historico { get; set; }

        [Display(Name = "Código")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage = "Campo (Código) é obrigatório.")]
        public string ds_codigo { get; set; }

        [StringLength(60, ErrorMessage = "Máximo 15 caracteres")]
        [Required(ErrorMessage = "Campo (Descrição) é obrigatório.")]
        [Display(Name = "Descrição")]
        public string ds_descricao { get; set; }

        [Required(ErrorMessage = "Campo (Tipo do Histórico) é obrigatório.")]
        [Display(Name = "Tipo do Histórico")]
        public string dm_operacao { get; set; }

        [Required(ErrorMessage = "Campo (Tipo de Situação Financeira) é obrigatório.")]
        [Display(Name = "Tipo de Situação Financeira")]
        public string dm_associacao { get; set; }

        [Required(ErrorMessage = "Campo (Código Contábil) é obrigatório.")]
        [Display(Name = "Código Contábil")]
        public string dm_codigo_contabil { get; set; }

    }
}