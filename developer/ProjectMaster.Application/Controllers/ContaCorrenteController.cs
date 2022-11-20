using ProjectMaster.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Bussiness.DataModels;

namespace ProjectMaster.Application.Controllers
{
    public class ContaCorrenteController : DropDownController
    {
        private ContaCorrente contaCorrente;
        private Banco banco;
        public ContaCorrenteController()
        {
            contaCorrente = new ContaCorrente();
            banco = new Banco();
        }

        public ActionResult Index(Int64 idPessoa)
        {
            CarregarBancos();
            ContaCorrenteModel model = new ContaCorrenteModel() { id_pessoa = idPessoa };
            model.proximoId = contaCorrente.Procurar(idPessoa).Count();
            return PartialView(model);
        }

        public ActionResult Procurar(Int64 idPessoa)
        {
            if (idPessoa > 0)
            {
                var filtradas = contaCorrente.Procurar(idPessoa);
                var total = filtradas.Count();
                Int64 id = -1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = total,
                    rows = (
                        from obj in filtradas.ToList()
                        select new
                        {
                            id = ++id,
                            cell = new[]
                    {
                        id.ToString(),
                        obj.id_banco.Value.ToString(),
                        banco.PegarDescricao(obj.id_banco.Value),
                        obj.ds_agencia,
                        obj.ds_numero_conta_corrente,
                        obj.ds_emitente,
                        obj.ds_conta_corrente,
                        id.ToString()
                    }
                        }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }
}
