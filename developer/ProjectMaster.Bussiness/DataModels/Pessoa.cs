using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using MvcJqGrid;
using System.Data;

namespace ProjectMaster.Bussiness.DataModels
{

    public class Pessoa : BaseBussiness
    {
        public IEnumerable<pm_pessoa> PegarPessoas(GridSettings gridSettings)
        {
            var customers = OrdenarPessoas(Context.pm_pessoa.AsQueryable(), gridSettings.SortColumn, gridSettings.SortOrder);

            if (gridSettings.IsSearch)
            {
                customers = gridSettings.Where.rules.Aggregate(customers, FiltrarPessoas);
            }

            return customers.ToList().Skip((gridSettings.PageIndex - 1) * gridSettings.PageSize).Take(gridSettings.PageSize);
        }

        private IQueryable<pm_pessoa> OrdenarPessoas(IQueryable<pm_pessoa> pessoas, string sortColumn, string sortOrder)
        {
            if (sortColumn == "Codigo")
                return (sortOrder == "desc") ? pessoas.OrderByDescending(c => c.ds_marca) : pessoas.OrderBy(c => c.ds_marca);
            if (sortColumn == "Nome")
                return (sortOrder == "desc") ? pessoas.OrderByDescending(c => c.ds_nome_fantasia) : pessoas.OrderBy(c => c.ds_nome_fantasia);
            if (sortColumn == "TipoDocumento")
                return (sortOrder == "desc") ? pessoas.OrderByDescending(c => c.ds_fisico_juridico) : pessoas.OrderBy(c => c.ds_fisico_juridico);
            if (sortColumn == "NumeroDocumento")
                return (sortOrder == "desc") ? pessoas.OrderByDescending(c => c.nr_documento) : pessoas.OrderBy(c => c.nr_documento);
            if (sortColumn == "InscricaoEstadual")
                return (sortOrder == "desc") ? pessoas.OrderByDescending(c => c.nr_ie) : pessoas.OrderBy(c => c.nr_ie);
            if (sortColumn == "TipoPessoa")
                return (sortOrder == "desc") ? pessoas.OrderByDescending(c => c.dm_tipo_pessoa) : pessoas.OrderBy(c => c.dm_tipo_pessoa);
            return pessoas;
        }


        public int ContarPessoas(GridSettings gridSettings)
        {
            var customers = Context.pm_pessoa.AsQueryable();

            if (gridSettings.IsSearch)
            {
                customers = gridSettings.Where.rules.Aggregate(customers, FiltrarPessoas);
            }
            return customers.Count();
        }


        private static IQueryable<pm_pessoa> FiltrarPessoas(IQueryable<pm_pessoa> pessoas, MvcJqGrid.Rule rule)
        {
            if (rule.field == "Codigo")
            {
                return pessoas.Where(c => c.ds_marca.Contains(rule.data));
            }
            if (rule.field == "Nome")
            {
                return pessoas.Where(c => c.ds_razao_social.Contains(rule.data));
            }
            if (rule.field == "TipoDocumento")
            {
                return pessoas.Where(c => c.ds_fisico_juridico.Contains(rule.data));
            }
            if (rule.field == "NumeroDocumento")
            {
                return pessoas.Where(c => c.nr_documento.Contains(rule.data));
            }
            if (rule.field == "InscricaoEstadual")
            {
                return pessoas.Where(c => c.nr_ie.Contains(rule.data));
            }
            if (rule.field == "TipoPessoa")
            {
                return pessoas.Where(c => c.dm_tipo_pessoa.Contains(rule.data));
            }
            return pessoas;
        }

        public void Deletar(Int64 id)
        {
            pm_pessoa obj = Context.pm_pessoa.First(i => i.id_pessoa == id);
            DeletarColecoes(obj);
            Context.pm_pessoa.DeleteObject(obj);
            Context.SaveChanges();
        }

        private void DeletarColecoes(pm_pessoa obj)
        {
            foreach (var item in obj.pm_telefone.ToList())
            {
                Context.pm_telefone.DeleteObject(item);
            }
            foreach (var item in obj.pm_endereco.ToList())
            {
                Context.pm_endereco.DeleteObject(item);
            }
            foreach (var item in obj.pm_conta_corrente.ToList())
            {
                Context.pm_conta_corrente.DeleteObject(item);
            }
        }

        public void Salvar(pm_pessoa pmPessoa)
        {
            if (pmPessoa.id_pessoa == null || pmPessoa.id_pessoa == 0)
            {
                CriarPessoa(pmPessoa);
            }
            else
            {
                EditarPessoa(pmPessoa);
            }
            Context.SaveChanges();
        }

        private void CriarPessoa(pm_pessoa pmPessoa)
        {
            pmPessoa.dt_cadastro = DateTime.Now;
            Context.pm_pessoa.AddObject(pmPessoa);
        }

        private void EditarPessoa(pm_pessoa pm_pessoaNova)
        {
            Deletar(pm_pessoaNova.id_pessoa);
            pm_pessoaNova.id_pessoa = 0;
            CriarPessoa(pm_pessoaNova);
        }

        public pm_pessoa PegarPessoa(long id)
        {
            return Context.pm_pessoa.Where(p => p.id_pessoa == id).FirstOrDefault();
        }
    }
}
