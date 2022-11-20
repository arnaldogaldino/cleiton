using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Controllers
{
    public class ContaCorrenteController : Controller
    {
        ContaCorrente bContaCorrente = new ContaCorrente();

        public ActionResult Index()
        {
            ContaCorrenteModels conta_corrente = new ContaCorrenteModels();

            return View(conta_corrente);
        }

        [HttpPost]
        public ActionResult Index(ContaCorrenteModels form)
        {
            ContaCorrenteModels conta_corrente = new ContaCorrenteModels();
            ViewData["queryContaCorrente"] = bContaCorrente.GetContaCorrenteGrid(form.id_conta_corrente, form.id_pessoa, form.id_fiado, form.id_contas_pagar, form.dta_vencimento_de, form.dta_vencimento_ate, form.dta_emissao_de, form.dta_emissao_ate);
            return View(conta_corrente);
        }
    }
}
