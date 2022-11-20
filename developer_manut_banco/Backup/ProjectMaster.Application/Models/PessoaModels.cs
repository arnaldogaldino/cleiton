using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProjectMaster.Data;
using ProjectMaster.Bussiness;

namespace ProjectMaster.Application.Models
{
    public class PessoaModels
    {
        [Display(Name = "ID")]
        public long id_pessoa { get; set; }

        [Display(Name = "Data de Cadastro")]
        [Required(ErrorMessage = "Campo (Data de Cadastro) é obrigatório.")]
        public DateTime dt_cadastro { get; set; }

        [Display(Name = "Marca/Código")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Required(ErrorMessage = "Campo (Marca/Código) é obrigatório.")]
        public string ds_marca { get; set; }

        [Display(Name = "Razão Social")]
        [StringLength(80, ErrorMessage = "Máximo 80 caracteres")]
        [Required(ErrorMessage = "Campo (Razão Social) é obrigatório.")]
        public string ds_razao_social { get; set; }

        [Display(Name = "Nome Fantasia")]
        [StringLength(80, ErrorMessage = "Máximo 80 caracteres")]
        public string ds_nome_fantasia { get; set; }

        [Display(Name = "Tipo Cadastro")]
        [StringLength(1, ErrorMessage = "Máximo 1 caracteres")]
        [Required(ErrorMessage = "Campo (Tipo Cadastro) é obrigatório.")]
        public string dm_tipo_pessoa { get; set; }

        [Display(Name = "Tipo Documento")]
        [StringLength(1, ErrorMessage = "Máximo 1 caracteres")]
        [Required(ErrorMessage = "Campo (Tipo Documento) é obrigatório.")]
        public string dm_tipo_documento { get; set; }

        [Display(Name = "Número Documento")]
        [StringLength(14, ErrorMessage = "Máximo 18 caracteres")]
        [Required(ErrorMessage = "Campo (Número Documento) é obrigatório.")]
        public string nr_documento { get; set; }

        [Display(Name = "Inscrição Estadual")]
        [StringLength(14, ErrorMessage = "Máximo 18 caracteres")]
        [Required(ErrorMessage = "Campo (Inscrição Estadual) é obrigatório.")]
        public string nr_ie { get; set; }

        [Display(Name = "Inscrição SUFRAMA")]
        [StringLength(9, ErrorMessage = "Máximo 9 caracteres")]
        [Required(ErrorMessage = "Campo (Inscrição SUFRAMA) é obrigatório.")]
        public string nr_inscricao_suframa { get; set; }

        [Display(Name = "Tipo do Cliente para Crédito")]
        [StringLength(3, ErrorMessage = "Máximo 3 caracteres")]
        [Required(ErrorMessage = "Campo (Tipo do Cliente para Crédito) é obrigatório.")]
        public string dm_tipo_cliente_credito { get; set; }

        [Display(Name = "Limite de Crédito")]
        [StringLength(2, ErrorMessage = "Máximo 2 caracteres")]
        [Required(ErrorMessage = "Campo (Limite de Crédito) é obrigatório.")]
        public string dm_limite_credito { get; set; }

        [Display(Name = "Venda Agendada")]
        public bool venda_agendada { get; set; }

        public IEnumerable<TelefoneModels> Telefone { get; set; }
        public IEnumerable<EnderecoModels> Endereco { get; set; }
        public IEnumerable<ContaCorrenteModels> ContaCorrente { get; set; }

        public class TelefoneModels
        {
            [Display(Name = "ID")]
            public long id_pessoa_telefone { get; set; }

            [Display(Name = "ID Pessoa")]
            public long id_pessoa { get; set; }

            [Display(Name = "Telefone")]
            [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
            [Required(ErrorMessage = "Campo (Telefone) é obrigatório.")]
            public string nr_telefone { get; set; }

            [Display(Name = "Tipo Telefone")]
            [StringLength(1, ErrorMessage = "Máximo 1 caracteres")]
            [Required(ErrorMessage = "Campo (Tipo Telefone) é obrigatório.")]
            public string dm_tipo_telefone { get; set; }

            [Display(Name = "Contato")]
            [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
            [Required(ErrorMessage = "Campo (Contato) é obrigatório.")]
            public string contato { get; set; }
        }

        public class EnderecoModels
        {
            [Display(Name = "ID")]
            public long id_pessoa_endereco { get; set; }

            [Display(Name = "ID Pessoa")]
            public long id_pessoa { get; set; }

            [Display(Name = "Tipo Endereço")]
            [StringLength(1, ErrorMessage = "Máximo 1 caracteres")]
            [Required(ErrorMessage = "Campo (Tipo Endereço) é obrigatório.")]
            public string dm_tipo_endereco { get; set; }

            [Display(Name = "Logradouro")]
            [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
            [Required(ErrorMessage = "Campo (Logradouro) é obrigatório.")]
            public string nm_endereco { get; set; }

            [Display(Name = "Número")]
            [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
            [Required(ErrorMessage = "Campo (Número) é obrigatório.")]
            public string nr_numero { get; set; }

            [Display(Name = "Complemento")]
            [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
            [Required(ErrorMessage = "Campo (Complemento) é obrigatório.")]
            public string ds_complemento { get; set; }

            [Display(Name = "Bairro")]
            [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
            [Required(ErrorMessage = "Campo (Bairro) é obrigatório.")]
            public string ds_bairro { get; set; }

            [Display(Name = "Cidade")]
            [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
            [Required(ErrorMessage = "Campo (Cidade) é obrigatório.")]
            public string ds_cidade { get; set; }

            [Display(Name = "UF")]
            [StringLength(2, ErrorMessage = "Máximo 2 caracteres")]
            [Required(ErrorMessage = "Campo (UF) é obrigatório.")]
            public string ds_uf { get; set; }

            [Display(Name = "CEP")]
            [StringLength(8, ErrorMessage = "Máximo 8 caracteres")]
            [Required(ErrorMessage = "Campo (CEP) é obrigatório.")]
            public string nr_cep { get; set; }
                        
            [Display(Name = "Código Municipio")]
            [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
            [Required(ErrorMessage = "Campo (Código Municipio) é obrigatório.")]
            public string codigo_municipio { get; set; }
                        
            [Display(Name = "Municipio")]
            [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
            [Required(ErrorMessage = "Campo (Municipio) é obrigatório.")]
            public string municipio { get; set; }
            
            [Display(Name = "País")]
            [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
            [Required(ErrorMessage = "Campo (País) é obrigatório.")]
            public string nr_codigo_pais { get; set; }
        }

        public class ContaCorrenteModels
        {
            [Display(Name = "ID")]
            public long id_pessoa_conta_corrente { get; set; }

            [Display(Name = "ID Pessoa")]
            public long id_pessoa { get; set; }

            [Display(Name = "dsConta")]
            [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
            public string ds_conta_corrente { get; set; }

            [Display(Name = "Banco")]
            [Required(ErrorMessage = "Campo (Banco) é obrigatório.")]
            public string id_banco { get; set; }

            [Display(Name = "Agencia")]
            [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
            [Required(ErrorMessage = "Campo (Agencia) é obrigatório.")]
            public string ds_agencia { get; set; }

            [Display(Name = "Conta")]
            [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
            [Required(ErrorMessage = "Campo (Conta) é obrigatório.")]
            public string ds_numero_conta_corrente { get; set; }

            [Display(Name = "Emitente")]
            [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
            public string ds_emitente { get; set; }
        }

    }
}