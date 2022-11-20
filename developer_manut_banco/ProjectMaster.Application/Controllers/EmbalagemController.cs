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
    public class EmbalagemController : ControllerMaster
    {
        Embalagem bEmbalagem = new Embalagem();

        [HttpGet]
        public ActionResult Index(string acao, long? id_embalagem)
        {
            if (acao == "Delete" && id_embalagem != null)
            {
                pm_embalagem adoEmbalagem = bEmbalagem.GetEmbalagemById((long)id_embalagem);
                var success = bEmbalagem.EmbalagemExcluir(adoEmbalagem);
            }

            ViewData["queryEmbalagem"] = bEmbalagem.GetEmbalagemGrid();

            return View();
        }

        [HttpGet]
        public ActionResult Embalagem(long? id_embalagem, string acao)
        {
            var embalagem = new EmbalagemModels();

            pm_embalagem adoEmbalagem = new pm_embalagem();

            if (id_embalagem != null)
                adoEmbalagem = bEmbalagem.GetEmbalagemById((long)id_embalagem);

            ViewData["acao"] = acao;

            return View(ExtensionMethods.ToObjects<EmbalagemModels>(adoEmbalagem));
        }

        [HttpPost]
        public ActionResult Embalagem(EmbalagemModels form)
        {
            pm_embalagem adoEmbalagem = new pm_embalagem();
            bool result = false;

            if (!ValidaForm(form))
                return View(form);

            if (form.id_embalagem != 0)
            {
                if (ModelState.IsValid)
                {
                    adoEmbalagem = ExtensionMethods.ToObjects<pm_embalagem>(form);
                    adoEmbalagem.id_filial = Context.idFilial;
                    result = bEmbalagem.EmbalagemEditar(ref adoEmbalagem);
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
                    adoEmbalagem = ExtensionMethods.ToObjects<pm_embalagem>(form);
                    result = bEmbalagem.EmbalagemCadastrar(ref adoEmbalagem);
                }
                else
                    result = false;
            }

            if (result)
            {
                ViewData["acao"] = "View";
                form = ExtensionMethods.ToObjects<EmbalagemModels>(adoEmbalagem);
            }

            return View(form);
        }

        private bool ValidaForm(EmbalagemModels form)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(form.dm_tipo_embalagem))
                ModelState.AddModelError("dm_tipo_embalagem", "Campo (Tipo de Embalagem) é obrigatório.");

            if (form.valor == 0)
                ModelState.AddModelError("valor", "Campo (Valor) é obrigatório.");

            return ModelState.IsValid;
        }

        public JsonResult Delete(long id)
        {
            pm_embalagem adoEmbalagem = bEmbalagem.GetEmbalagemById(id);

            var success = bEmbalagem.EmbalagemExcluir(adoEmbalagem);

            return this.Json(
                    new
                    {
                        success = success,
                        error = (success != true ? "Não foi possivel excluir esse embalagem pelas suas dependencias." : "")
                    }, JsonRequestBehavior.AllowGet);
        }

    }
}
