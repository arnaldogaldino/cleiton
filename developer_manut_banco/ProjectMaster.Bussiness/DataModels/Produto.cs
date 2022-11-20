using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data;
using ProjectMaster.Core;

namespace ProjectMaster.Bussiness
{
    public class Produto
    {
        private PMEntities entities = new PMEntities();

        public IQueryable<pm_produto> GetProdutoGrid(string cprod, string xprod)
        {
            IQueryable<pm_produto> query = (from m in entities.pm_produto
                                            where m.id_filial == Context.idFilial
                                            select m);

            if (!string.IsNullOrEmpty(cprod))
                query = query.Where(o => o.cprod.Contains(cprod));

            if (!string.IsNullOrEmpty(xprod))
                query = query.Where(o => o.xprod.Contains(xprod));

            return query;
        }

        public IQueryable<pm_produto> GetProdutoByCprod(string cprod)
        {
            IQueryable<pm_produto> query = (from m in entities.pm_produto
                                            where m.id_filial == Context.idFilial
                                            select m);

            if (!string.IsNullOrEmpty(cprod))
                query = query.Where(o => o.cprod.Contains(cprod));

            return query;
        }
        
        public pm_produto GetProdutoByID(long id)
        {
            return (from m in entities.pm_produto
                    where m.id_produto == id && m.id_filial == Context.idFilial
                    orderby m.cprod, m.xprod
                    select m).FirstOrDefault();
        }

        public void DeleteProduto(long id)
        {
            pm_produto produto = entities.pm_produto.First(i => i.id_produto == id);

            entities.pm_produto.Detach(produto);
            entities.SaveChanges();
        }

        public bool ProdutoCadastrar(ref pm_produto adoProduto)
        {
            try
            {
                adoProduto.id_filial = Context.idFilial;

                entities.AddTopm_produto(adoProduto);
                entities.SaveChanges();
                Estoque estoque = new Estoque();

                estoque.ProdutoEstoqueCadastrar(adoProduto.id_produto);
            } 
            catch 
            {
                throw;
            }

            return true;
        }

        public bool ProdutoEditar(ref pm_produto adoProduto)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_produto", adoProduto);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoProduto);
                }
                entities.SaveChanges();
            }
            catch { 
                throw; 
                //return false; 
            }

            return true;
        }

        public bool ProdutoExcluir(pm_produto adoProduto)
        {
            try
            {
                entities.DeleteObject(adoProduto);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

    }
}
