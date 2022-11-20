using MvcJqGrid;
using ProjectMaster.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Bussiness.DataModels;

namespace ProjectMaster.Application.Controllers
{
    public class TelefoneController : DropDownController
    {
        private Telefone telefone;
        private DomainValue domainValue;
        public TelefoneController()
        {
            telefone = new Telefone();
            domainValue = new DomainValue();
        }
        public ActionResult Index(Int64 idPessoa)
        {
            TelefoneModel model = new TelefoneModel() { id_pessoa = idPessoa };
            model.proximoId = telefone.Procurar(idPessoa).Count();
            CarregarTipoTelefone();
            return PartialView(model);
        }

        public JsonResult Procurar(Int64 idPessoa)
        {
            if (idPessoa > 0)
            {
                var filtradas = telefone.Procurar(idPessoa);
                var total = filtradas.Count();
                Int64 idTelefone = -1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = total,
                    rows = (
                        from tell in filtradas.ToList()
                        select new
                        {
                            id = ++idTelefone,
                            cell = new[]
                    {
                        idTelefone.ToString(),
                        tell.dm_tipo_telefone,
                        domainValue.PegarDescricaoTelefone(tell.dm_tipo_telefone),
                        tell.nr_telefone,
                        tell.nm_contato,
                        idTelefone.ToString(),
                    }
                        }).ToArray()
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}
