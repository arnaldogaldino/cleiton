using MvcJqGrid;
using ProjectMaster.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProjectMaster.Bussiness.DataModels
{
    public class Moeda : BaseBussiness
    {
        public IEnumerable<pm_moeda> PegarMoedas(GridSettings gridSettings)
        {
            var lista = OrdenarMoedas(Context.pm_moeda.Where(o => !o.bl_excluido).AsQueryable(), gridSettings.SortColumn, gridSettings.SortOrder);

            if (gridSettings.IsSearch)
            {
                lista = gridSettings.Where.rules.Aggregate(lista, FiltrarMoedas);
            }

            return lista.ToList().Skip((gridSettings.PageIndex - 1) * gridSettings.PageSize).Take(gridSettings.PageSize);
        }

        private IQueryable<pm_moeda> OrdenarMoedas(IQueryable<pm_moeda> Moedas, string sortColumn, string sortOrder)
        {
            if (sortColumn == "tipo_moeda")
                return (sortOrder == "desc") ? Moedas.OrderByDescending(c => c.pm_tipo_moeda.nm_tipo_moeda) : Moedas.OrderBy(c => c.pm_tipo_moeda.nm_tipo_moeda);
            if (sortColumn == "dt_cotacao")
                return (sortOrder == "desc") ? Moedas.OrderByDescending(c => c.dt_cotacao) : Moedas.OrderBy(c => c.dt_cotacao);
            if (sortColumn == "nr_valor")
                return (sortOrder == "desc") ? Moedas.OrderByDescending(c => c.nr_valor) : Moedas.OrderBy(c => c.nr_valor);

            return Moedas;
        }


        public int ContarMoedas(GridSettings gridSettings)
        {
            var customers = Context.pm_moeda.Where(o => !o.bl_excluido).AsQueryable();

            if (gridSettings.IsSearch)
            {
                customers = gridSettings.Where.rules.Aggregate(customers, FiltrarMoedas);
            }
            return customers.Count();
        }


        private static IQueryable<pm_moeda> FiltrarMoedas(IQueryable<pm_moeda> Moedas, MvcJqGrid.Rule rule)
        {
            if (rule.field == "tipo_moeda")
            {
                Int64 idTipoMoeda = Convert.ToInt64(rule.data);
                return Moedas.Where(c => c.id_tipo_moeda == idTipoMoeda);
            }
            if (rule.field == "dt_cotacao")
            {
                DateTime data;
                if (DateTime.TryParse(rule.data, out data))
                {
                    return Moedas.Where(c => c.dt_cotacao == data);
                }
            }
            if (rule.field == "nr_valor")
            {
                decimal valor = Convert.ToDecimal(rule.data);
                return Moedas.Where(c => c.nr_valor == valor);
            }

            return Moedas;
        }
        public void Deletar(Int64 id)
        {
            pm_moeda obj = Context.pm_moeda.First(i => i.id_moeda == id);
            obj.bl_excluido = true;
            Editar(obj);
            Context.SaveChanges();
        }

        public bool PossuiCotacaoNestaData(pm_moeda pmMoeda)
        {
            var qtd = Context.pm_moeda.Where(
                  m => m.id_tipo_moeda == pmMoeda.id_tipo_moeda
                  && m.dt_cotacao == pmMoeda.dt_cotacao
                  && !m.bl_excluido
                  ).Count();

            return qtd > 0;
        }

        public void Salvar(pm_moeda obj)
        {
            if (obj.id_moeda == 0)
            {
                Criar(obj);
            }
            else
            {
                Editar(obj);
            }
            Context.SaveChanges();
        }

        private void Criar(pm_moeda obj)
        {
            Context.pm_moeda.AddObject(obj);
        }

        private void Editar(pm_moeda obj)
        {
            EntityKey key = Context.CreateEntityKey("pm_moeda", obj);
            object originalItem;

            if (Context.TryGetObjectByKey(key, out originalItem))
            {
                obj.dt_cotacao = ((pm_moeda)originalItem).dt_cotacao;
                Context.ApplyCurrentValues(key.EntitySetName, obj);
            }
        }

        public pm_moeda PegarMoeda(long id)
        {
            return Context.pm_moeda.Where(p => p.id_moeda == id).FirstOrDefault();
        }
    }
}
