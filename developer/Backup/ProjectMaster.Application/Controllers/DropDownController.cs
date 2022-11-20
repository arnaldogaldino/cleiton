using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Bussiness.DataModels;
using ProjectMaster.Core.Extensions;
namespace ProjectMaster.Application.Controllers
{
    public class DropDownController : Controller
    {
        private readonly DomainValue valoresDeDominio;
        public DropDownController()
        {
            valoresDeDominio = new DomainValue();
        }

        public void CarregarViewBagTipoCliente()
        {
            ViewBag.TipoCliente = valoresDeDominio.PegarTipoDeCliente().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarViewBagTipoDocumento()
        {
            ViewBag.TipoDocumento = valoresDeDominio.PegarTipoDeDocumento().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
    }
}
