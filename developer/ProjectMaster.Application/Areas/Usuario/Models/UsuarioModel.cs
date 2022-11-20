using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;

namespace ProjectMaster.Application.Areas.Usuario.Models
{
    public class UsuarioModel
    {
        private PMEntities EntityContext = new PMEntities();         
        
        public UsuarioModel()
        {

        }

        public UsuarioModel(int id_usuario)
        {
            var usuario = EntityContext.pm_usuario
            .Where(e => e.id_usuario == id_usuario).SingleOrDefault();

            // instantiate your dictionaries

            foreach (var staffType in (from u in usuario.pm_filial 
                                       select u.pm_empresa))
            {
                Empresas.Add(staffType.id_empresa, staffType.apelido);
            }

            // repeat similar loop for gender types
        }


        [Required]
        [StringLength(6, MinimumLength = 3)]
        [Display(Name = "Usuário")]
        [ScaffoldColumn(false)]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Senha")]
        [StringLength(6, MinimumLength = 3)]
        public string Senha { get; set; }

        [Required]
        [Display(Name = "Empresa")]
        public Dictionary<long, string> Empresas { get; set; }

        [Required()]
        [Display(Name = "Filial")]
        public Dictionary<long, string> Filiais { get; set; }

        public pm_usuario Usuario { get; set; }


    }
}