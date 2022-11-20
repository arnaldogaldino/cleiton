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
    public class ClassificacaoController : Controller
    {
        private readonly Classificacao classificacao;

        public ClassificacaoController()
        {
            classificacao = new Classificacao();
        }

        public ActionResult Procurar()
        {
            return View();
        }
        public ActionResult Editar(Int64 id)
        {

            pm_classificacao obj = classificacao.Pegar(id);
            if (obj != null)
            {
                var model = ExtensionMethods.ToObjects<ClassificacaoModel>(obj);
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
            var filtradas = classificacao.Pegar(gridSettings);
            var totalDePessoas = classificacao.Contar(gridSettings);
            var jsonData = new
            {
                total = totalDePessoas / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalDePessoas,
                rows = (
                    from obj in filtradas.ToList()
                    select new
                    {
                        id = obj.id_classificacao,
                        cell = new[]
                    {
                        obj.id_classificacao.ToString(),
                        obj.ds_codigo,
                        obj.ds_descricao,
                        obj.id_classificacao.ToString()
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
                classificacao.Deletar(id);
                msg = "Registro deletado com sucesso.";
            }
            catch (Exception ex)
            {
                msg = string.Format("Ocorreu um erro ao deletar registro: {0}", ex.Message);
            }
            return Json(new { msg = msg });
        }

        public JsonResult Salvar(ClassificacaoModel moedaModel)
        {
            string msg = "Registro salvo com sucesso";
            bool erro = false;
            long id = 0;
            try
            {
                var obj = ExtensionMethods.ToObjects<pm_classificacao>(moedaModel);
                if (!classificacao.PossuiCodigoCadastrado(obj) || obj.id_classificacao > 0)
                {
                    classificacao.Salvar(obj);
                    id = obj.id_classificacao;
                }
                else
                {
                    erro = true;
                    msg = "Já existe uma classificação cadastrado com este código.";
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
