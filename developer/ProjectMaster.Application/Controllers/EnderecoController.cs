using ProjectMaster.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Bussiness.DataModels;

namespace ProjectMaster.Application.Controllers
{
    public class EnderecoController : DropDownController
    {
        private Endereco endereco;
        private Pais pais;
        private DomainValue domainValue;
        public EnderecoController()
        {
            endereco = new Endereco();
            domainValue = new DomainValue();
            pais = new Pais();
        }
        public ActionResult Index(Int64 idPessoa)
        {
            CarregarPaises();
            CarregarTipoDeEndereco();
            EnderecoModel model = new EnderecoModel() { id_pessoa = idPessoa };
            model.proximoId = endereco.Procurar(idPessoa).Count();
            return PartialView(model);
        }

        public JsonResult Procurar(Int64 idPessoa)
        {
            if (idPessoa > 0)
            {
                var filtradas = endereco.Procurar(idPessoa);
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
                        obj.dm_tipo_endereco,
                        domainValue.PegarDescricaoTipoEndereco(obj.dm_tipo_endereco),
                        obj.nr_cep,
                        obj.nm_endereco,
                        obj.nr_numero,
                        obj.ds_complemento,
                        obj.ds_bairro,
                        obj.ds_cidade,
                        obj.ds_uf,
                        obj.id_pais.Value.ToString(),
                        pais.PegarDescricao(obj.id_pais.Value),
                        id.ToString(),
                    }
                        }).ToArray()
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }
}
