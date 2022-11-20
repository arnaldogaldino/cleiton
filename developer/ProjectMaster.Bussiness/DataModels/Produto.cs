using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcJqGrid;
using ProjectMaster.Data;

namespace ProjectMaster.Bussiness.DataModels
{
    public class Produto : BaseBussiness
    {
        public IEnumerable<pm_produto> PegarProduto(GridSettings gridSettings)
        {
            var produtos = OrdenarProduto(Context.pm_produto.AsQueryable(), gridSettings.SortColumn, gridSettings.SortOrder);

            if (gridSettings.IsSearch)
            {
                produtos = gridSettings.Where.rules.Aggregate(produtos, FiltrarProduto);
            }

            return produtos.ToList().Skip((gridSettings.PageIndex - 1) * gridSettings.PageSize).Take(gridSettings.PageSize);
        }

        private IQueryable<pm_produto> OrdenarProduto(IQueryable<pm_produto> produto, string sortColumn, string sortOrder)
        {
            if (sortColumn == "Codigo")
                return (sortOrder == "desc") ? produto.OrderByDescending(c => c.cprod) : produto.OrderBy(c => c.cprod);
            if (sortColumn == "Descricao")
                return (sortOrder == "desc") ? produto.OrderByDescending(c => c.xprod) : produto.OrderBy(c => c.xprod);

            return produto;
        }

        public int ContarProduto(GridSettings gridSettings)
        {
            var produtos = Context.pm_produto.AsQueryable();

            if (gridSettings.IsSearch)
            {
                produtos = gridSettings.Where.rules.Aggregate(produtos, FiltrarProduto);
            }
            return produtos.Count();
        }
        
        private static IQueryable<pm_produto> FiltrarProduto(IQueryable<pm_produto> produtos, MvcJqGrid.Rule rule)
        {
            if (rule.field == "Codigo")
            {
                return produtos.Where(c => c.cprod.Contains(rule.data));
            }
            if (rule.field == "Descricao")
            {
                return produtos.Where(c => c.xprod.Contains(rule.data));
            }
        
            return produtos;
        }

        public void Deletar(string cprod)
        {
            pm_produto obj = Context.pm_produto.First(i => i.cprod == cprod);
            DeletarColecoes(obj);
            Context.pm_produto.DeleteObject(obj);
            Context.SaveChanges();
        }

        private void DeletarColecoes(pm_produto obj)
        {

        }

        public void Salvar(pm_produto produto)
        {
            if (string.IsNullOrEmpty(produto.cprod))
            {
                CriarProduto(produto);
            }
            else
            {
                EditarProduto(produto);
            }
            Context.SaveChanges();
        }

        private void CriarProduto(pm_produto produto)
        {
            Context.pm_produto.AddObject(produto);
        }

        private void EditarProduto(pm_produto produto)
        {
            Deletar(produto.cprod);
            CriarProduto(produto);
        }

        public pm_produto PegarProduto(string cprod)
        {
            return Context.pm_produto.Where(p => p.cprod == cprod).FirstOrDefault();
        }
    }
}
