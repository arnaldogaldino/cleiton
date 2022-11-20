using MvcJqGrid;
using ProjectMaster.Application.Models;
using ProjectMaster.Bussiness.DataModels;
//using ProjectMaster.Core.Extensions;
using ProjectMaster.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core.Extensions;

namespace ProjectMaster.Application.Controllers
{
    public class MoedaController : DropDownController
    {
        private readonly Moeda moeda;

        public MoedaController()
        {
            moeda = new Moeda();
        }

        public ActionResult Procurar()
        {
            CarregarTiposDeMoedas();
            return View();
        }
        public ActionResult Editar(Int64 id)
        {
          
            pm_moeda pmMoeda = moeda.PegarMoeda(id);
            if (pmMoeda != null)
            {
                MoedaModel model = ExtensionMethods.ToObjects<MoedaModel>(pmMoeda);
                CarregarTiposDeMoedas(model.id_tipo_moeda);
                return View("Novo", model);
            }
            return View("Novo");
        }

        public ActionResult Novo()
        {
            CarregarTiposDeMoedas();
            return View();
        }

        public JsonResult ProcurarMoedas(GridSettings gridSettings)
        {
            var pessoasFiltradas = moeda.PegarMoedas(gridSettings);
            var totalDePessoas = moeda.ContarMoedas(gridSettings);
            var jsonData = new
            {
                total = totalDePessoas / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalDePessoas,
                rows = (
                    from pessoas in pessoasFiltradas.ToList()
                    select new
                    {
                        id = pessoas.id_moeda,
                        cell = new[]
                    {
                        pessoas.id_moeda.ToString(),
                        pessoas.pm_tipo_moeda.nm_tipo_moeda,
                        pessoas.dt_cotacao.ToString("dd-MM-yyyy"),
                        pessoas.nr_valor.ToString("0.00"), 
                        pessoas.id_moeda.ToString()
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
                moeda.Deletar(id);
                msg = "Registro deletado com sucesso.";
            }
            catch (Exception ex)
            {
                msg = string.Format("Ocorreu um erro ao deletar moeda: {0}", ex.Message);
            }
            return Json(new { msg = msg });
        }

        public JsonResult Salvar(MoedaModel moedaModel)
        {
            string msg = "Registro salvo com sucesso";
            bool erro = false;
            long idMoeda = 0;
            try
            {
                var obj = ExtensionMethods.ToObjects<pm_moeda>(moedaModel);
                if (!moeda.PossuiCotacaoNestaData(obj) || obj.id_moeda > 0 )
                {
                    moeda.Salvar(obj);
                    idMoeda = obj.id_moeda;
                }
                else
                {
                    erro = true;
                    msg = "Já existe uma cotação cadastrada para está moeda nesta data.";
                }
            }
            catch (Exception ex)
            {
                erro = true;
                msg = string.Format("Ocorreu um erro ao salvar moeda: {0}", ex.Message);
            }
            return Json(new { id_moeda = idMoeda, msg = msg, erro = erro });
        }
    }
}
