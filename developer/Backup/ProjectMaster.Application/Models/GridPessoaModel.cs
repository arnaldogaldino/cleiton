using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMaster.Application.Models
{
    public class GridPessoaModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string InscricaoEstadual { get; set; }
        public string TipoPessoa { get; set; }
    }
}