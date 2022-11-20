using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data;
using ProjectMaster.Core;
using System.Data.Objects;

namespace ProjectMaster.Bussiness
{
    public class Pedido
    {
        private PMEntities entities = new PMEntities();

        public IQueryable<pm_pedido> GetPedidoGrid(long nr_pedido, string ds_razao_social, string forma_pagto)
        {
            IQueryable<pm_pedido> query = (from m in entities.pm_pedido
                                           where m.bl_excluido == false && m.id_filial == Context.idFilial
                                           orderby m.nr_pedido descending
                                           select m);

            if (!string.IsNullOrEmpty(ds_razao_social))
                query = query.Where(o => o.pm_pessoa.ds_razao_social.Contains(ds_razao_social));

            if (!string.IsNullOrEmpty(forma_pagto))
                query = query.Where(o => o.dm_forma_pagto.Contains(forma_pagto));

            if (nr_pedido > 0)
                query = query.Where(o => o.nr_pedido == nr_pedido);

            return query;
        }

        public pm_pedido GetPedidoById(long id)
        {
            return (from m in entities.pm_pedido
                    where m.id_pedido == id && m.id_filial == Context.idFilial && m.bl_excluido == false
                    select m).FirstOrDefault();
        }

        public pm_pedido GetPedidoByNumero(long numero)
        {
            return (from m in entities.pm_pedido
                    where m.nr_pedido == numero && m.id_filial == Context.idFilial && m.bl_excluido == false
                    select m).FirstOrDefault();
        }

        public void DeletePedido(long id)
        {
            pm_pedido pedido = entities.pm_pedido.First(i => i.id_pedido == id);

            pedido.bl_excluido = true;
            PedidoEditar(ref pedido);
        }

        public bool PedidoCadastrar(ref pm_pedido adoPedido)
        {
            try
            {
                adoPedido.id_filial = Context.idFilial;
                adoPedido.nr_pedido = (long)GetNrPedido();
                
                entities.AddTopm_pedido(adoPedido);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool PedidoEditar(ref pm_pedido adoPedido)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_pedido", adoPedido);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    adoPedido.id_filial = ((pm_pedido)originalItem).id_filial;

                    entities.ApplyCurrentValues(key.EntitySetName, adoPedido);
                }
                entities.SaveChanges();

                foreach (var item in adoPedido.pm_pedido_item)
                {
                    if(item.id_pedido_item > 0)
                        PedidoItemEditar(item);
                    else
                        PedidoItemCadastrar(item);
                }
            }
            catch { return false; }

            return true;
        }

        public bool PedidoExcluir(pm_pedido adoPedido)
        {
            try
            {
                adoPedido.bl_excluido = true;
                PedidoEditar(ref adoPedido);
            }
            catch { return false; }

            return true;
        }

        public bool PedidoItemCadastrar(pm_pedido_item adoItem)
        {
            try
            {
                pm_pedido_item item = new pm_pedido_item();

                item.id_pedido = adoItem.id_pedido;
                item.id_produto = adoItem.id_produto;
                item.quantidade = adoItem.quantidade;
                item.valor_unitario = adoItem.id_pedido;
                item.qtd_cx_devol = adoItem.qtd_cx_devol;
                item.qtd_cx_empr = adoItem.qtd_cx_empr;
                item.qtd_cx_pag_int = adoItem.qtd_cx_pag_int;

                entities.AddTopm_pedido_item(item);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool PedidoItemEditar(pm_pedido_item adoItem)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_pedido_item", adoItem);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoItem);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public long GetNrPedido()
        {
            var result = entities.ExecuteFunction<long>("sp_get_nr_pedido", new ObjectParameter("id_filial", Context.FilialLogin.id_filial));
            
            return result.FirstOrDefault<long>();
        }
    }
}
