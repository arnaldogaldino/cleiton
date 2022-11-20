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
    public class FiadoController : ControllerMaster
    {
        Fiado bFiado = new Fiado();

        [HttpGet]
        public ActionResult Index(string acao, long? id_fiado)
        {
            if (acao == "Delete" && id_fiado != null)
            {
                pm_fiado adoFiado = bFiado.GetFiadoById(id_fiado.Value);
                var success = bFiado.FiadoEditar(ref adoFiado);
            }

            ViewData["acao"] = acao;
            ViewData["query"] = null;
            FiadoModels form = new FiadoModels();

            return View(form);
        }

        [HttpPost]
        public ActionResult Index(FiadoModels form)
        {
            ViewData["query"] = bFiado.GetFiadoGrid(form.id_fiado, form.id_pedido, form.id_pessoa, form.id_produto);

            return View(form);
        }

        [HttpGet]
        public ActionResult Fiado(long? id_fiado, string acao)
        {
            pm_fiado adoFiado = new pm_fiado();

            if (id_fiado != null)
                adoFiado = bFiado.GetFiadoById((long)id_fiado);

            var form = ExtensionMethods.ToObjects<FiadoModels>(adoFiado);

            if (adoFiado.pm_pessoa != null)
            {
                form.ds_razao_social = adoFiado.pm_pessoa.ds_razao_social;
                form.ds_marca = adoFiado.pm_pessoa.ds_marca;
            }

            ViewData["acao"] = acao;

            return View(form);
        }

        [HttpPost]
        public ActionResult Fiado(FiadoModels form)
        {
            pm_fiado adoFiado = new pm_fiado();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_fiado != 0)
            {
                if (ModelState.IsValid)
                {
                    adoFiado = bFiado.GetFiadoById(form.id_fiado);
                    adoFiado = ExtensionMethods.ToObjects<pm_fiado>(form);
                    adoFiado.id_filial = Context.idFilial;
                    
                    result = bFiado.FiadoEditar(ref adoFiado);
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
                    adoFiado = ExtensionMethods.ToObjects<pm_fiado>(form);
                    adoFiado.id_filial = Context.idFilial;

                    result = bFiado.FiadoCadastrar(ref adoFiado);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<FiadoModels>(adoFiado);
            }

            if (adoFiado.pm_pessoa != null)
            {
                form.ds_marca = adoFiado.pm_pessoa.ds_marca;
                form.ds_razao_social = adoFiado.pm_pessoa.ds_razao_social;
            }

            return View(form);
        }

        private bool ValidaForm(FiadoModels form)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(form.dm_forma_pagto))
                ModelState.AddModelError("dm_forma_pagto", "Campo (Forma Pagamento) é obrigatório.");
            
            if (string.IsNullOrEmpty(form.dm_tipo_dc))
                ModelState.AddModelError("dm_tipo_dc", "Campo (Tipo) é obrigatório.");
            
            if (form.pago_ate == null)
                ModelState.AddModelError("pago_ate", "Campo (Pago até) é obrigatório.");

            if (form.id_pessoa == 0)
                ModelState.AddModelError("id_pessoa", "Campo (Empresa) é obrigatório.");

            if (string.IsNullOrEmpty(form.ocorrencia))
                ModelState.AddModelError("ocorrencia", "Campo (Ocorrência) é obrigatório.");

            if (form.banco == null)
                ModelState.AddModelError("banco", "Campo (Banco) é obrigatório.");


            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_fiado adoFiado = bFiado.GetFiadoById(id);

            var success = bFiado.FiadoExcluir(adoFiado);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir essa conta fiado pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }
    }
}
