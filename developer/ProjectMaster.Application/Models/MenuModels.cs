using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class MenuModels
    {     
        [Display(Name = "Id")]
        public long id_menu { get; set; }

        [Display(Name = "ID Menu Pai")]
        public long? id_menu_pai { get; set; }

        [Display(Name = "Menu Pai")]
        public string menu_pai { get; set; }

        [Required(ErrorMessage="Campo Nome é obrigatório.")]
        [Display(Name = "Nome")]
        [StringLength(50)]
        public string nome { get; set; }

        [Required(ErrorMessage = "Campo Texto é obrigatório.")]
        [Display(Name = "Texto")]
        [StringLength(50)]
        public string texto { get; set; }

        [Display(Name = "Area Name")]
        [StringLength(50)]
        public string area_name { get; set; }

        [Display(Name = "Controller Name")]
        [StringLength(50)]
        public string controller_name { get; set; }

        [Display(Name = "Action Name")]
        [StringLength(50)]
        public string action_name { get; set; }

        [Required(ErrorMessage = "Campo Ordem é obrigatório.")]
        [Display(Name = "Ordem")]
        public int ordem { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(50)]
        public string descricao { get; set; }    

    }
}