using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMaster.Application.Models
{
    public class UsuarioModels
    {

        [Required]
        [StringLength(6, MinimumLength = 3)]
        [Display(Name = "Usuário")]
        [ScaffoldColumn(false)]
        public string login { get; set; }

        [Required]
        [Display(Name = "Senha")]
        [StringLength(6, MinimumLength = 3)]
        public string senha { get; set; }

        [Required]
        [Display(Name = "Empresa")]
        public long empresa { get; set; }

        [Required]
        [Display(Name = "Filial")]
        public long filial { get; set; }

    }
}