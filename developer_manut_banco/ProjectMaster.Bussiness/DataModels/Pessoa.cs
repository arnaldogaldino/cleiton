using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ProjectMaster.Data;
using System.Data;
using System.Web.Mvc;
using ProjectMaster.Core;

namespace ProjectMaster.Bussiness
{
    public class Pessoa
    {
        private PMEntities entities = new PMEntities();
      
        public IQueryable<pm_pessoa> GetPessoaGrid(string ds_marca, string ds_razao_social, string nr_documento, string dm_tipo_pessoa)
        {
            IQueryable<pm_pessoa> query = (from m in entities.pm_pessoa
                                           where m.id_filial == Context.idFilial && m.bl_excluido == false 
                    orderby m.ds_razao_social
                    select m);
            
            if (!string.IsNullOrEmpty(ds_marca))
                query = query.Where(o => o.ds_marca.Contains(ds_marca));

            if (!string.IsNullOrEmpty(ds_razao_social))
                query = query.Where(o => o.ds_razao_social.Contains(ds_razao_social));

            if (!string.IsNullOrEmpty(nr_documento))
                query = query.Where(o => o.nr_documento.Contains(nr_documento));

            if (!string.IsNullOrEmpty(dm_tipo_pessoa))
                query = query.Where(o => o.dm_tipo_pessoa.Contains(dm_tipo_pessoa));

            return query;
        }

        public pm_pessoa GetPessoaById(long id)
        {
            return (from m in entities.pm_pessoa
                   where m.id_filial == Context.idFilial &&
                         m.id_pessoa == id 
                  select m).FirstOrDefault();
        }

        public IQueryable<pm_pessoa> GetPessoaByMarca(string ds_marca)
        {
            if (string.IsNullOrEmpty(ds_marca))
                return null;

            return (from m in entities.pm_pessoa
                    where m.id_filial == Context.idFilial && m.ds_marca.Contains(ds_marca)
                    select m);
        }
        
        public pm_pessoa GetPessoaByDocumento(string documento)
        {
            return (from m in entities.pm_pessoa
                    where m.id_filial == Context.idFilial && m.nr_documento == documento
                    select m).FirstOrDefault();
        }

        public void DeletePessoa(long id)
        {
            pm_pessoa pessoa = entities.pm_pessoa.First(i => i.id_pessoa == id);

            pessoa.bl_excluido = true;
            PessoaEditar(ref pessoa);
        }

        public bool PessoaCadastrar(ref pm_pessoa adoPessoa)
        {
            try
            {
                adoPessoa.id_filial = Context.idFilial;
                adoPessoa.dt_cadastro = DateTime.Now;

                entities.AddTopm_pessoa(adoPessoa);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool PessoaEditar(ref pm_pessoa adoPessoa)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_pessoa", adoPessoa);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoPessoa);
                }

                foreach (var item in adoPessoa.pm_pessoa_telefone)
                    if (item.id_pessoa_telefone > 0)
                        PessoaTelefoneEditar(item);
                    else
                    {
                        entities.AddTopm_pessoa_telefone(item);
                    }

                foreach (var item in adoPessoa.pm_pessoa_endereco)
                    if (item.id_pessoa_endereco > 0)
                        PessoaEnderecoEditar(item);
                    else
                    {
                        entities.AddTopm_pessoa_endereco(item);
                    }

                foreach (var item in adoPessoa.pm_pessoa_conta_corrente)
                    if (item.id_pessoa_conta_corrente > 0)
                        PessoaContaCorrenteEditar(item);
                    else
                    {
                        entities.AddTopm_pessoa_conta_corrente(item);
                    }

                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }


        public bool PessoaTelefoneCadastrar(pm_pessoa_telefone adoTelefone)
        {
            try
            {
                entities.AddTopm_pessoa_telefone(adoTelefone);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public bool PessoaTelefoneEditar(pm_pessoa_telefone adoTelefone)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_pessoa_telefone", adoTelefone);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoTelefone);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public IEnumerable<pm_pessoa_telefone> GetPessoaTelefoneByIdPessoa(long id)
        {
            return (from m in entities.pm_pessoa_telefone
                    where m.id_pessoa == id
                    select m);
        }

        public bool PessoaEnderecoCadastrar(pm_pessoa_endereco adoEndereco)
        {
            try
            {
                entities.AddTopm_pessoa_endereco(adoEndereco);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public bool PessoaEnderecoEditar(pm_pessoa_endereco adoEndereco)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_pessoa_endereco", adoEndereco);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoEndereco);
                }

                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public IEnumerable<pm_pessoa_endereco> GetPessoaEnderecoEditarByIdPessoa(long id)
        {
            return (from m in entities.pm_pessoa_endereco
                    where m.id_pessoa == id
                    select m);
        }

        public bool PessoaContaCorrenteCadastrar(pm_pessoa_conta_corrente adoConta)
        {
            try
            {
                entities.AddTopm_pessoa_conta_corrente(adoConta);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public bool PessoaContaCorrenteEditar(pm_pessoa_conta_corrente adoConta)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_pessoa_conta_corrente", adoConta);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoConta);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }
        public IEnumerable<pm_pessoa_conta_corrente> GetPessoaContaCorrenteByIdPessoa(long id)
        {
            return (from m in entities.pm_pessoa_conta_corrente
                    where m.id_pessoa == id
                    select m);
        }
        
        public bool PessoaExcluir(pm_pessoa adoPessoa)
        {
            try
            {
                adoPessoa.bl_excluido = true;
                PessoaEditar(ref adoPessoa);
            }
            catch { return false; }

            return true;
        }
    }
}
