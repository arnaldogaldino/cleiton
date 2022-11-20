using MvcJqGrid;
using ProjectMaster.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ProjectMaster.Core.Extensions;

namespace ProjectMaster.Bussiness.DataModels
{
    public class CentroDeCusto : BaseBussiness
    {
        public IEnumerable<pm_centro_de_custo> Pegar(GridSettings gridSettings)
        {
            var lista = Ordenar(Context.pm_centro_de_custo.Where(o => !o.bl_excluido).AsQueryable(), gridSettings.SortColumn, gridSettings.SortOrder);

            if (gridSettings.IsSearch)
            {
                lista = gridSettings.Where.rules.Aggregate(lista, Filtrar);
            }

            return lista.ToList().Skip((gridSettings.PageIndex - 1) * gridSettings.PageSize).Take(gridSettings.PageSize);
        }

        private IQueryable<pm_centro_de_custo> Ordenar(IQueryable<pm_centro_de_custo> lista, string sortColumn, string sortOrder)
        {
            if (sortColumn == "ds_codigo")
                return (sortOrder == "desc") ? lista.OrderByDescending(c => c.ds_codigo) : lista.OrderBy(c => c.ds_codigo);
            if (sortColumn == "ds_descricao")
                return (sortOrder == "desc") ? lista.OrderByDescending(c => c.ds_descricao) : lista.OrderBy(c => c.ds_descricao);

            return lista;
        }


        public int Contar(GridSettings gridSettings)
        {
            var lista = Context.pm_centro_de_custo.Where(o => !o.bl_excluido).AsQueryable();

            if (gridSettings.IsSearch)
            {
                lista = gridSettings.Where.rules.Aggregate(lista, Filtrar);
            }
            return lista.Count();
        }


        private static IQueryable<pm_centro_de_custo> Filtrar(IQueryable<pm_centro_de_custo> lista, MvcJqGrid.Rule rule)
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
            pm_centro_de_custo obj = Context.pm_centro_de_custo.First(i => i.id_centro_de_custo == id );
            obj.bl_excluido = true;
            Editar(obj);
            Context.SaveChanges();
        }

        public bool PossuiCodigoCadastrado(pm_centro_de_custo pmCentroDeCusto)
        {
            var qtd = Context.pm_centro_de_custo.Where(m => m.ds_codigo == pmCentroDeCusto.ds_codigo && !m.bl_excluido).Count();
            return qtd > 0;
        }

        public void Salvar(pm_centro_de_custo obj)
        {
            if (obj.id_centro_de_custo == 0)
            {
                Criar(obj);
            }
            else
            {
                Editar(obj);
            }
            Context.SaveChanges();
        }

        private void Criar(pm_centro_de_custo obj)
        {
            Context.pm_centro_de_custo.AddObject(obj);
        }

        private void Editar(pm_centro_de_custo obj)
        {
            EntityKey key = Context.CreateEntityKey("pm_centro_de_custo", obj);
            object originalItem;

            if (Context.TryGetObjectByKey(key, out originalItem))
            {
                Context.ApplyCurrentValues(key.EntitySetName, obj);
            }
        }

        public pm_centro_de_custo Pegar(long id)
        {
            return Context.pm_centro_de_custo.Where(p => p.id_centro_de_custo == id && !p.bl_excluido).FirstOrDefault();
        }
    }
}
