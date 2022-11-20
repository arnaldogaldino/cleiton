using MvcJqGrid;
using ProjectMaster.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProjectMaster.Bussiness.DataModels
{
    public class Classificacao : BaseBussiness
    {
        public IEnumerable<pm_classificacao> Pegar(GridSettings gridSettings)
        {
            var lista = Ordenar(Context.pm_classificacao.Where(o => !o.bl_excluido).AsQueryable(), gridSettings.SortColumn, gridSettings.SortOrder);

            if (gridSettings.IsSearch)
            {
                lista = gridSettings.Where.rules.Aggregate(lista, Filtrar);
            }

            return lista.ToList().Skip((gridSettings.PageIndex - 1) * gridSettings.PageSize).Take(gridSettings.PageSize);
        }

        private IQueryable<pm_classificacao> Ordenar(IQueryable<pm_classificacao> lista, string sortColumn, string sortOrder)
        {
            if (sortColumn == "ds_codigo")
                return (sortOrder == "desc") ? lista.OrderByDescending(c => c.ds_codigo) : lista.OrderBy(c => c.ds_codigo);
            if (sortColumn == "ds_descricao")
                return (sortOrder == "desc") ? lista.OrderByDescending(c => c.ds_descricao) : lista.OrderBy(c => c.ds_descricao);

            return lista;
        }


        public int Contar(GridSettings gridSettings)
        {
            var lista = Context.pm_classificacao.Where(o => !o.bl_excluido).AsQueryable();

            if (gridSettings.IsSearch)
            {
                lista = gridSettings.Where.rules.Aggregate(lista, Filtrar);
            }
            return lista.Count();
        }


        private static IQueryable<pm_classificacao> Filtrar(IQueryable<pm_classificacao> lista, MvcJqGrid.Rule rule)
        {
            if (rule.field == "ds_codigo")
            {
                return lista.Where(c => c.ds_codigo.Contains(rule.data));
            }
            if (rule.field == "ds_descricao")
            {
                return lista.Where(c => c.ds_descricao.Contains(rule.data));
            }

            return lista;
        }

        public void Deletar(Int64 id)
        {
            pm_classificacao obj = Context.pm_classificacao.First(i => i.id_classificacao == id);
            obj.bl_excluido = true;
            Editar(obj);
            Context.SaveChanges();
        }

        public bool PossuiCodigoCadastrado(pm_classificacao pmClassificacao)
        {
            var qtd = Context.pm_classificacao.Where(m => m.ds_codigo == pmClassificacao.ds_codigo && !m.bl_excluido ).Count();
            return qtd > 0;
        }

        public void Salvar(pm_classificacao obj)
        {
            if (obj.id_classificacao == 0)
            {
                Criar(obj);
            }
            else
            {
                Editar(obj);
            }
            Context.SaveChanges();
        }

        private void Criar(pm_classificacao obj)
        {
            Context.pm_classificacao.AddObject(obj);
        }

        private void Editar(pm_classificacao obj)
        {
            EntityKey key = Context.CreateEntityKey("pm_classificacao", obj);
            object originalItem;

            if (Context.TryGetObjectByKey(key, out originalItem))
            {
                Context.ApplyCurrentValues(key.EntitySetName, obj);
            }
        }

        public pm_classificacao Pegar(long id)
        {
            return Context.pm_classificacao.Where(p => p.id_classificacao == id && !p.bl_excluido).FirstOrDefault();
        }
    }
}
