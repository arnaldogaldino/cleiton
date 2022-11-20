using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Bussiness.DataModels;
using ProjectMaster.Core;
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

        public void CarregarTipoCliente()
        {
            ViewBag.TipoCliente = valoresDeDominio.PegarTipoDeCliente().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarOpcaoDeAceitacao()
        {
            ViewBag.OpcaoDeAceitacao = valoresDeDominio.PegarOpcoesDeAceitacao().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarTipoDocumento()
        {
            ViewBag.TipoDocumento = valoresDeDominio.PegarTipoDeDocumento().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarLimiteDeCredito()
        {
            ViewBag.LimiteDeCredito = valoresDeDominio.PegarLimiteDeCredito().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarTipoTelefone()
        {
            ViewBag.TipoTelefone = valoresDeDominio.PegarTipoDeTelefone().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarTipoDeEndereco()
        {
            ViewBag.TipoEndereco = valoresDeDominio.PegarTipoDeEndereco().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarPaises()
        {
            Pais pais = new Pais();
            ViewBag.Paises = pais.PegarPaises().ToSelectList(texto => texto.nm_nome, valor => valor.id_pais);
        }
        public void CarregarBancos()
        {
            Banco banco = new Banco();
            ViewBag.Bancos = banco.PegarBancos().ToSelectList(texto => texto.ds_banco, valor => valor.id_banco);
        }
        public void CarregarTiposDeMoedas(int? idTipoMoeda = null)
        {
            TipoMoeda tipoMoeda = new TipoMoeda();
            ViewBag.TiposDeMoedas = tipoMoeda.PegarTiposDeMoedas().Where(s => !idTipoMoeda.HasValue || idTipoMoeda == s.id_tipo_moeda).ToSelectList(texto => texto.nm_tipo_moeda, valor => valor.id_tipo_moeda);
        }


        public void CarregarTipoEmbalagem()
        {
            ViewBag.TipoEmbalagem = valoresDeDominio.PegarTipoEmbalagem().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarTipoEan()
        {
            ViewBag.TipoEan = valoresDeDominio.PegarTipoEAN().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarCst()
        {
            ViewBag.Cst = valoresDeDominio.PegarCST().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarCstOrigem()
        {
            ViewBag.CstOrigem = valoresDeDominio.PegarCSTOrigem().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarModBc()
        {
            ViewBag.ModBc = valoresDeDominio.PegarModBC().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
        public void CarregarModBcSt()
        {
            ViewBag.ModBcSt = valoresDeDominio.PegarMODBCST().ToSelectList(texto => texto.label, valor => valor.domain_value);
        }
    }
}
