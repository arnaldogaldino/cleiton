using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data;
using System.Web.Mvc;
using ProjectMaster.Core;

namespace ProjectMaster.Bussiness
{
    public class Estoque
    {
        private static PMEntities entities = new PMEntities();

        public IQueryable<pm_produto_estoque_entrada> GetProdutoEstoqueEntradaGrid(long? id_produto, long? id_pessoa, string descricao, DateTime? dtInicio, DateTime? dtFim)
        {
            var result = (from m in entities.pm_produto_estoque_entrada
                          where m.id_filial == Context.idFilial && m.bl_excluido == false
                          orderby m.id_produto_estoque_entrada
                          select m).AsQueryable();

            if (id_produto.HasValue && id_produto.Value != 0)
                result = result.Where(o => o.id_produto == id_produto.Value);

            if (id_pessoa.HasValue && id_pessoa.Value != 0)
                result = result.Where(o => o.id_pessoa == id_pessoa.Value);

            if (!string.IsNullOrEmpty(descricao))
                result = result.Where(o => o.descricao.Contains(descricao));

            if (dtInicio.HasValue)
                result = result.Where(o => o.dta_entrada >= dtInicio.Value);

            if (dtFim.HasValue)
                result = result.Where(o => o.dta_entrada >= dtFim.Value);

            return result;
        }

        public IQueryable<pm_produto_estoque_historico> GetProdutoEstoqueHistoricoGrid(long id_produto)
        {
            var result = (from m in entities.pm_produto_estoque_historico
                          where m.id_filial == Context.idFilial
                          orderby m.id_produto_estoque_historico descending
                          select m).AsQueryable();

            result = result.Where(o => o.id_produto == id_produto);

            return result;
        }

        public pm_produto_estoque_entrada GetProdutoEstoqueEntradaById(long id_produto_estoque_entrada)
        {
            return (from m in entities.pm_produto_estoque_entrada
                    where m.id_produto_estoque_entrada == id_produto_estoque_entrada && m.id_filial == Context.idFilial
                    orderby m.dta_entrada
                    select m).FirstOrDefault();
        }

        public bool ProdutoEstoqueEntradaCadastrar(ref pm_produto_estoque_entrada adoEstoque)
        {
            try
            {
                adoEstoque.id_filial = Context.idFilial;

                entities.AddTopm_produto_estoque_entrada(adoEstoque);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public bool ProdutoEstoqueEntradaExcluir(pm_produto_estoque_entrada adoEstoque)
        {
            try
            {
                adoEstoque.bl_excluido = true;
                ProdutoEstoqueEntradaEditar(ref adoEstoque);
            }
            catch { return false; }

            return true;
        }
        public bool ProdutoEstoqueEntradaEditar(ref pm_produto_estoque_entrada adoEstoque)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_produto_estoque_entrada", adoEstoque);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoEstoque);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        #region # Estoque #
        public void ProdutoEstoqueCadastrar(long id_produto)
        {
            var produtos = from e in entities.pm_produto
                           where e.id_produto == id_produto
                           select e;

            foreach (var p in produtos)
            {
                pm_produto_estoque estoque = new pm_produto_estoque();
                estoque.id_produto = p.id_produto;
                entities.AddTopm_produto_estoque(estoque);
            }

            entities.SaveChanges();
        }
        public void ProdutoEstoqueEditar(List<pm_produto_estoque> estoques)
        {
            foreach (var e in estoques)
            {
                EntityKey key = entities.CreateEntityKey("pm_produto_estoque", e);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, e);
                }
            }
            entities.SaveChanges();
        }

        public decimal IncrementaQuantidadeEstoque(long id_produto, decimal quantidade)
        {
            decimal result = 0;

            var estoque = (from p in entities.pm_produto_estoque
                           where p.id_produto == id_produto
                           select p).SingleOrDefault();

            if (estoque == null)
                return result;

            estoque.quantidade_estoque += quantidade;

            EntityKey key = entities.CreateEntityKey("pm_produto_estoque", estoque);
            object originalItem;

            if (entities.TryGetObjectByKey(key, out originalItem))
            {
                entities.ApplyCurrentValues(key.EntitySetName, estoque);
            }

            entities.SaveChanges();

            return result;
        }
        public decimal IncrementaQuantidadeReservada(long id_produto, decimal quantidade)
        {
            decimal result = 0;

            var estoque = (from p in entities.pm_produto_estoque
                           where p.id_produto == id_produto
                           select p).SingleOrDefault();

            estoque.quantidade_reservada += quantidade;

            result = estoque.quantidade_reservada;

            EntityKey key = entities.CreateEntityKey("pm_produto_estoque", estoque);
            object originalItem;

            if (entities.TryGetObjectByKey(key, out originalItem))
            {
                entities.ApplyCurrentValues(key.EntitySetName, estoque);
            }

            entities.SaveChanges();

            return result;
        }

        public decimal DecrementaQuantidadeEstoque(long id_produto, decimal quantidade)
        {
            decimal result = 0;

            var estoque = (from p in entities.pm_produto_estoque
                           where p.id_produto == id_produto
                           select p).SingleOrDefault();

            estoque.quantidade_estoque -= quantidade;

            EntityKey key = entities.CreateEntityKey("pm_produto_estoque", estoque);
            object originalItem;

            if (entities.TryGetObjectByKey(key, out originalItem))
            {
                entities.ApplyCurrentValues(key.EntitySetName, estoque);
            }

            entities.SaveChanges();

            return result;
        }
        public decimal DecrementaQuantidadeReservada(long id_produto, decimal quantidade)
        {
            decimal result = 0;

            var estoque = (from p in entities.pm_produto_estoque
                           where p.id_produto == id_produto
                           select p).SingleOrDefault();

            estoque.quantidade_reservada -= quantidade;

            result = estoque.quantidade_reservada;

            EntityKey key = entities.CreateEntityKey("pm_produto_estoque", estoque);
            object originalItem;

            if (entities.TryGetObjectByKey(key, out originalItem))
            {
                entities.ApplyCurrentValues(key.EntitySetName, estoque);
            }

            entities.SaveChanges();

            return result;
        }

        public void EstoqueHitorico(long? id_compra, long? id_pedido, long id_produto, decimal quantidade)
        {
            try
            {
                long id_filial = Context.FilialLogin.id_filial;

                pm_produto_estoque_historico historico = new pm_produto_estoque_historico();

                historico.id_produto = id_produto;
                historico.id_filial = id_filial;

                if (id_pedido != null)
                    historico.id_pedido = id_pedido.Value;
                if (id_compra != null)
                    historico.id_compra = id_compra.Value;

                historico.quantidade = quantidade;
                historico.data = DateTime.Now;

                entities.AddTopm_produto_estoque_historico(historico);
                entities.SaveChanges();

                AtualizarSaldo(historico.id_produto, historico.data);
            }
            catch { return; }
        }

        public void AtualizarSaldo(long id_produto, DateTime data)
        {
            long id_produto_estoque_historico_max = 0;
            
            try
            {
                id_produto_estoque_historico_max = (from f in entities.pm_produto_estoque_historico
                                where f.id_produto == id_produto &&
                                      f.data < data
                                select f).Max(o => o.id_produto_estoque_historico);
            }
            catch { }

            var list_historico = (from f in entities.pm_produto_estoque_historico
                               where f.id_produto == id_produto &&
                                     f.id_produto_estoque_historico >= id_produto_estoque_historico_max
                               select f).ToArray();

            for (int row = 0; row < list_historico.Length; row++)
            {
                int row_old = ((id_produto_estoque_historico_max) == 0 ? 0 : ((row - 1) == -1 ? 0 : (row - 1)));

                list_historico[row].saldo = list_historico[row_old].saldo + list_historico[row].quantidade;

                entities.ApplyOriginalValues("pm_produto_estoque_historico", list_historico[row]);
            }

            entities.SaveChanges();
        }
        #endregion
    }
}
