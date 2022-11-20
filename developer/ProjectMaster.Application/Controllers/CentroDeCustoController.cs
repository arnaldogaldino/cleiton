using MvcJqGrid;
using ProjectMaster.Application.Models;
using ProjectMaster.Bussiness.DataModels;
using ProjectMaster.Core.Extensions;
using ProjectMaster.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMaster.Application.Controllers
{
    public class CentroDeCustoController : Controller
    {
        private readonly CentroDeCusto centroDeCusto;

        public CentroDeCustoController()
        {
            centroDeCusto = new CentroDeCusto();
        }

        public ActionResult Procurar()
        {
            return View();
        }
        public ActionResult Editar(Int64 id)
        {

            pm_centro_de_custo obj = centroDeCusto.Pegar(id);
            if (obj != null)
            {
                CentroDeCustoModel model = ExtensionMethods.ToObjects<CentroDeCustoModel>(obj);
                return View("Novo", model);
            }
            return View("Novo");
        }

        public ActionResult Novo()
        {
            return View();
        }

        public JsonResult ProcurarGrid(GridSettings gridSettings)
        {
            var pessoasFiltradas = centroDeCusto.Pegar(gridSettings);
            var totalDePessoas = centroDeCusto.Contar(gridSettings);
            var jsonData = new
            {
                total = totalDePessoas / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalDePessoas,
                rows = (
                    from obj in pessoasFiltradas.ToList()
                    select new
                    {
                        id = obj.id_centro_de_custo,
                        cell = new[]
                    {
                        obj.id_centro_de_custo.ToString(),
                        obj.ds_codigo,
                        obj.ds_descricao,
                        obj.id_centro_de_custo.ToString()
                    }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Deletar(Int64 id)
        {
            var msg = string.Empty;
            try
            {
                centroDeCusto.Deletar(id);
                msg = "Registro deletado com sucesso.";
            }
            catch (Exception ex)
            {
                msg = string.Format("Ocorreu um erro ao deletar registro: {0}", ex.Message);
            }
            return Json(new { msg = msg });
        }
        public JsonResult Salvar(CentroDeCustoModel moedaModel)
        {
            string msg = "Registro salvo com sucesso";
            bool erro = false;
            long id = 0;
            try
            {
                var obj = ExtensionMethods.ToObjects<pm_centro_de_custo>(moedaModel);
                if (!centroDeCusto.PossuiCodigoCadastrado(obj) || obj.id_centro_de_custo > 0)
                {
                    centroDeCusto.Salvar(obj);
                    id = obj.id_centro_de_custo;
                }
                else
                {
                    erro = true;
                    msg = "Já existe um centro de custo cadastrado com este código.";
                }
            }
            catch (Exception ex)
            {
                erro = true;
                msg = string.Format("Ocorreu um erro ao salvar registro: {0}", ex.Message);
            }
            return Json(new { id = id, msg = msg, erro = erro });
        }

    }
}
