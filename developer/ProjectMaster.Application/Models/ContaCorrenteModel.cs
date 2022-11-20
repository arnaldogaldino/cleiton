using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMaster.Application.Models
{
    public class ContaCorrenteModel
    {
        public Int64? id_conta_corrente { get; set; }
        public Int64? id_pessoa { get; set; }
        public Int64 proximoId { get; set; }

        [Display(Name = "Banco")]
        [Required(ErrorMessage = "*")]
        public string id_banco { get; set; }

        [Display(Name = "Agencia")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_agencia { get; set; }
        
        [Display(Name = "Conta Corrente")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_numero_conta_corrente { get; set; }
        
        [Display(Name = "Emitente")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_emitente { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Required(ErrorMessage = "*")]
        public string ds_conta_corrente { get; set; }

    }
}