using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using ProjectMaster.Core;
using ProjectMaster.Bussiness;
using ProjectMaster.Data;
using ProjectMaster.Application.Models;

namespace ProjectMaster.Application.Controllers
{
    public class PedidoController : ControllerMaster
    {
        Pedido bPedido = new Pedido();
        Pessoa bPessoa = new Pessoa();

        [HttpGet]
        public ActionResult Index(string acao, long? id_pedido)
        {
            if (acao == "Delete" && id_pedido != null)
            {
                pm_pedido adoPedido = bPedido.GetPedidoById((long)id_pedido);
                var success = bPedido.PedidoExcluir(adoPedido);
            }

            ViewData["queryPedido"] = null;
            ViewData["acao"] = acao;

            return View(new PedidoModels());
        }

        [HttpPost]
        public ActionResult Index(PedidoModels form)
        {
            ViewData["queryPedido"] = bPedido.GetPedidoGrid(form.nr_pedido, (form.Pessoa != null ? form.Pessoa.ds_razao_social : ""), form.dm_forma_pagto);

            return View(form);
        }

        public ActionResult Pedido(long? id_pedido, string acao)
        {
            var pedido = new PedidoModels();

            pm_pedido adoPedido = new pm_pedido();

            if (id_pedido != null)
            {
                adoPedido = bPedido.GetPedidoById(id_pedido.Value);
                pedido = ExtensionMethods.ToObjects<PedidoModels>(adoPedido);
            }
            else
            {
                pedido.dt_emissao = DateTime.Now;
                pedido.dt_vencimento = DateTime.Now;
            }

            ViewData["acao"] = acao;

            return View(pedido);            
        }

    }
}
