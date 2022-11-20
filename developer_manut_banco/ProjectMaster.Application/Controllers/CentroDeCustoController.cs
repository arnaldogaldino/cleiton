using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;
using ProjectMaster.Application.Models;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Controllers
{
    public class CentroDeCustoController : ControllerMaster
    {
        CentroDeCusto bCentroDeCusto = new CentroDeCusto();

        [HttpGet]
        public ActionResult Index(string acao, long? id_centro_de_custo)
        {
            var oCentroDeCusto = new CentroDeCusto();

            if (acao == "Delete" && id_centro_de_custo != null)
            {
                pm_centro_de_custo adoCentroDeCusto = bCentroDeCusto.GetCentroDeCustoById((long)id_centro_de_custo);
                var success = bCentroDeCusto.CentroDeCustoExcluir(adoCentroDeCusto);
            }

            ViewData["queryCentroDeCusto"] = oCentroDeCusto.GetCentroDeCustoGrid();

            return View();
        }

        [HttpGet]
        public ActionResult CentroDeCusto(long? id_centro_de_custo, string acao)
        {
            var centro_de_custo = new CentroDeCustoModels();
            var oCentroDeCusto = new CentroDeCusto();

            pm_centro_de_custo adoCentroDeCusto = new pm_centro_de_custo();

            if (id_centro_de_custo != null)
                adoCentroDeCusto = oCentroDeCusto.GetCentroDeCustoById((long)id_centro_de_custo);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<CentroDeCustoModels>(adoCentroDeCusto));
        }

        [HttpPost]
        public ActionResult CentroDeCusto(CentroDeCustoModels form)
        {
            pm_centro_de_custo adoCentroDeCusto = new pm_centro_de_custo();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_centro_de_custo != 0)
            {
                if (ModelState.IsValid)
                {
                    adoCentroDeCusto = ExtensionMethods.ToObjects<pm_centro_de_custo>(form);
                    adoCentroDeCusto.id_filial = Context.idFilial;
                    result = bCentroDeCusto.CentroDeCustoEditar(ref adoCentroDeCusto);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    adoCentroDeCusto = ExtensionMethods.ToObjects<pm_centro_de_custo>(form);
                    result = bCentroDeCusto.CentroDeCustoCadastrar(ref adoCentroDeCusto);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<CentroDeCustoModels>(adoCentroDeCusto);
            }

            return View(form);
        }

        private bool ValidaForm(CentroDeCustoModels form)
        {
            ModelState.Clear();

            if (bCentroDeCusto.GetCentroDeCustoByCodigo(form.id_centro_de_custo, form.ds_codigo) != null)
                ModelState.AddModelError("ds_codigo", "Centro de custo já cadastrado.");

            if (string.IsNullOrEmpty(form.ds_codigo))
                ModelState.AddModelError("ds_codigo", "Campo (Código) é obrigatório.");

            if (string.IsNullOrEmpty(form.ds_descricao))
                ModelState.AddModelError("ds_descricao", "Campo (Descrição) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_centro_de_custo adoCentroDeCusto = bCentroDeCusto.GetCentroDeCustoById(id);

            var success = bCentroDeCusto.CentroDeCustoExcluir(adoCentroDeCusto);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse centro de custo pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
