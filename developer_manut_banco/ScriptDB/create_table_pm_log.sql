begin transaction

create table [dbo].[pm_usuario](
	[id_usuario] [bigint] identity(1,1) not null,
	[nome] [varchar](50) not null,
	[usuario] [varchar](20) not null,
	[senha] [varchar](10) not null,
	[email] [varchar](150) not null,
	[data_cadastro] [datetime] not null,
	[excluido] [bit] not null,
 constraint [pk_pm_usuario] primary key clustered 
(
	[id_usuario] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_usuario] add  constraint [df_pm_usuario_data_cadastro]  default (getdate()) for [data_cadastro]
alter table [dbo].[pm_usuario] add  constraint [df_pm_usuario_excluido]  default ((0)) for [excluido]

create table [dbo].[pm_log] (
    [id_log] [int] identity (1, 1) not null,
    [date] [datetime] not null,
    [thread] [varchar] (255) not null,
    [level] [varchar] (50) not null,
    [logger] [varchar] (255) not null,
    [message] [varchar] (4000) not null,
    [exception] [varchar] (2000) null
)

create table [dbo].[pm_auditoria_log](
	[id_auditoria_log] [bigint] identity(1,1) not null,
	[id_usuario] [bigint] null,
	[id_record] [bigint] not null,
	[usuario] [varchar](50) not null,
	[nome_tabela] [varchar](50) not null,
	[acao] [varchar](50) not null,
	[data_acao] [datetime] not null,
 constraint [pk_pm_auditoria_log] primary key clustered 
(
	[id_auditoria_log] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_auditoria_log]  with check add  constraint [fk_pm_auditoria_log_pm_usuario] foreign key([id_usuario])
references [dbo].[pm_usuario] ([id_usuario])

alter table [dbo].[pm_auditoria_log] check constraint [fk_pm_auditoria_log_pm_usuario]

alter table [dbo].[pm_auditoria_log] add  constraint [df_pm_auditoria_log_data_acao]  default (getdate()) for [data_acao]

create table [dbo].[pm_auditoria_log_detalhe](
	[id_auditoria_log_detalhe] [bigint] identity(1,1) not null,
	[id_auditoria_log] [bigint] not null,
	[nome_coluna] [varchar](255) not null,
	[valor_original] [varchar](255) null,
	[valor_corrente] [varchar](255) null,
 constraint [pk_pm_auditoria_log_detalhe] primary key clustered 
(
	[id_auditoria_log_detalhe] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_auditoria_log_detalhe]  with check add  constraint [fk_pm_auditoria_log_detalhe_pm_auditoria_log] foreign key([id_auditoria_log])
references [dbo].[pm_auditoria_log] ([id_auditoria_log])

alter table [dbo].[pm_auditoria_log_detalhe] check constraint [fk_pm_auditoria_log_detalhe_pm_auditoria_log]

create table [dbo].[pm_menu](
	[id_menu] [bigint] identity(1,1) not null,
	[id_menu_pai] [bigint] null,
	[nome] [varchar](50) not null,
	[area_name] [varchar](50) not null,
	[action_name] [varchar](50) not null,
	[controller_name] [varchar](50) not null,
	[router_name] [varchar](50) not null,
	[descricao] [varchar](50) null,
 constraint [pk_pm_menu] primary key clustered 
(
	[id_menu] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_menu]  with check add  constraint [fk_pm_menu_pm_menu] foreign key([id_menu_pai])
references [dbo].[pm_menu] ([id_menu])

alter table [dbo].[pm_menu] check constraint [fk_pm_menu_pm_menu]

create table [dbo].[pm_menu_detalhe](
	[id_menu_detalhe] [bigint] identity(1,1) not null,
	[id_menu] [bigint] not null,
	[nome] [varchar](25) not null,
	[area_name] [varchar](50) not null,
	[action_name] [varchar](50) not null,
	[controller_name] [varchar](50) not null,	
	[route_name] [varchar](50) not null,
	[descricao] [varchar](50) null,
 constraint [pk_pm_menu_detalhe] primary key clustered 
(
	[id_menu_detalhe] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_menu_detalhe]  with check add  constraint [fk_pm_menu_detalhe_pm_menu] foreign key([id_menu])
references [dbo].[pm_menu] ([id_menu])

alter table [dbo].[pm_menu_detalhe] check constraint [fk_pm_menu_detalhe_pm_menu]

create table [dbo].[pm_menu_detalhe_permissao](
	[id_menu_detalhe] [bigint] not null,
	[id_usuario] [bigint] not null,
 constraint [pk_pm_menu_detalhe_permissao] primary key clustered 
(
	[id_menu_detalhe] asc,
	[id_usuario] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_menu_detalhe_permissao]  with check add  constraint [fk_pm_menu_detalhe_permissao_pm_menu_detalhe] foreign key([id_menu_detalhe])
references [dbo].[pm_menu_detalhe] ([id_menu_detalhe])

alter table [dbo].[pm_menu_detalhe_permissao] check constraint [fk_pm_menu_detalhe_permissao_pm_menu_detalhe]

alter table [dbo].[pm_menu_detalhe_permissao]  with check add  constraint [fk_pm_menu_detalhe_permissao_pm_usuario] foreign key([id_usuario])
references [dbo].[pm_usuario] ([id_usuario])

alter table [dbo].[pm_menu_detalhe_permissao] check constraint [fk_pm_menu_detalhe_permissao_pm_usuario]

create table [dbo].[pm_menu_permissao](
	[id_menu] [bigint] not null,
	[id_usuario] [bigint] not null,
 constraint [pk_pm_menu_permissao] primary key clustered 
(
	[id_menu] asc,
	[id_usuario] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_menu_permissao]  with check add  constraint [fk_pm_menu_permissao_pm_menu] foreign key([id_menu])
references [dbo].[pm_menu] ([id_menu])

alter table [dbo].[pm_menu_permissao] check constraint [fk_pm_menu_permissao_pm_menu]

alter table [dbo].[pm_menu_permissao]  with check add  constraint [fk_pm_menu_permissao_pm_usuario] foreign key([id_usuario])
references [dbo].[pm_usuario] ([id_usuario])

alter table [dbo].[pm_menu_permissao] check constraint [fk_pm_menu_permissao_pm_usuario]


create table [dbo].[pm_empresa](
	[id_empresa] [bigint] identity(1,1) not null,
	[id_fiscal_cnae_pri] [bigint] null,
	[id_fiscal_cnae_sec] [bigint] null,
	[apelido] [varchar](20) null,
	[data_cadastro] [datetime] not null,
	[tipo_pessoa] [varchar](9) not null,
	[crt] [int] null,
	[logotipo] [image] null,
	[razao_social] [varchar](70) not null,
	[nome_fantasia] [varchar](70) not null,
	[cnpj] [varchar](18) not null,
	[i_estadual] [varchar](15) not null,
	[i_municipal] [varchar](15) null,
	[telefone] [varchar](15) null,
	[fax] [varchar](15) null,
	[email] [varchar](100) null,
	[end_cep] [varchar](9) null,
	[end_endereco] [varchar](70) null,
	[end_numero] [varchar](10) null,
	[end_complemento] [varchar](20) null,
	[end_bairro] [varchar](50) null,
	[end_cidade] [varchar](30) null,
	[end_estado] [varchar](2) null,
	[end_municipio] [varchar](10) null,
	[cob_cep] [varchar](9) null,
	[cob_endereco] [varchar](70) null,
	[cob_numero] [varchar](10) null,
	[cob_complemento] [varchar](20) null,
	[cob_bairro] [varchar](50) null,
	[cob_cidade] [varchar](30) null,
	[cob_estado] [varchar](2) null,
	[cob_municipio] [varchar](10) null,
	[ent_cep] [varchar](9) null,
	[ent_endereco] [varchar](70) null,
	[ent_numero] [varchar](10) null,
	[ent_complemento] [varchar](20) null,
	[ent_bairro] [varchar](50) null,
	[ent_cidade] [varchar](30) null,
	[ent_estado] [varchar](2) null,
	[ent_municipio] [varchar](10) null,
	[excluido] [bit] not null,
 constraint [pk_pm_empresa] primary key clustered 
(
	[id_empresa] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary] textimage_on [primary]

alter table [dbo].[pm_empresa] add  constraint [df_pm_empresa_data_cadastro]  default (getdate()) for [data_cadastro]

alter table [dbo].[pm_empresa] add  constraint [df_pm_empresa_excluido]  default ((0)) for [excluido]


create table [dbo].[pm_filial](
	[id_filial] [bigint] identity(1,1) not null,
	[id_empresa] [bigint] not null,
	[id_fiscal_cnae_pri] [bigint] null,
	[id_fiscal_cnae_sec] [bigint] null,
	[apelido] [varchar](20) null,
	[data_cadastro] [datetime] not null,
	[tipo_pessoa] [varchar](9) not null,
	[crt] [int] null,
	[logotipo] [image] null,
	[razao_social] [varchar](70) not null,
	[nome_fantasia] [varchar](70) not null,
	[cnpj] [varchar](18) not null,
	[i_estadual] [varchar](15) not null,
	[i_municipal] [varchar](15) null,
	[telefone] [varchar](15) null,
	[fax] [varchar](15) null,
	[email] [varchar](100) null,
	[end_cep] [varchar](9) null,
	[end_endereco] [varchar](70) null,
	[end_numero] [varchar](10) null,
	[end_complemento] [varchar](20) null,
	[end_bairro] [varchar](50) null,
	[end_cidade] [varchar](30) null,
	[end_estado] [varchar](2) null,
	[end_municipio] [varchar](10) null,
	[cob_cep] [varchar](9) null,
	[cob_endereco] [varchar](70) null,
	[cob_numero] [varchar](10) null,
	[cob_complemento] [varchar](20) null,
	[cob_bairro] [varchar](50) null,
	[cob_cidade] [varchar](30) null,
	[cob_estado] [varchar](2) null,
	[cob_municipio] [varchar](10) null,
	[ent_cep] [varchar](9) null,
	[ent_endereco] [varchar](70) null,
	[ent_numero] [varchar](10) null,
	[ent_complemento] [varchar](20) null,
	[ent_bairro] [varchar](50) null,
	[ent_cidade] [varchar](30) null,
	[ent_estado] [varchar](2) null,
	[ent_municipio] [varchar](10) null,
	[excluido] [bit] not null,
 constraint [pk_pm_filial] primary key clustered 
(
	[id_filial] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary] textimage_on [primary]

alter table [dbo].[pm_filial]  with check add  constraint [fk_pm_filial_pm_empresa] foreign key([id_empresa])
references [dbo].[pm_empresa] ([id_empresa])

alter table [dbo].[pm_filial] check constraint [fk_pm_filial_pm_empresa]

alter table [dbo].[pm_filial] add  constraint [df_pm_filial_id_empresa]  default ((0)) for [id_empresa]

alter table [dbo].[pm_filial] add  constraint [df_pm_filial_data_cadastro]  default (getdate()) for [data_cadastro]

alter table [dbo].[pm_filial] add  constraint [df_pm_filial_excluido]  default ((0)) for [excluido]

create table [dbo].[pm_usuario_filial](
	[id_usuario] [bigint] not null,
	[id_filial] [bigint] not null,
 constraint [pk_pm_usuario_filial] primary key clustered 
(
	[id_usuario] asc,
	[id_filial] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
) on [primary]

alter table [dbo].[pm_usuario_filial]  with check add  constraint [fk_pm_usuario_filial_pm_filial] foreign key([id_filial])
references [dbo].[pm_filial] ([id_filial])

alter table [dbo].[pm_usuario_filial] check constraint [fk_pm_usuario_filial_pm_filial]

alter table [dbo].[pm_usuario_filial]  with check add  constraint [fk_pm_usuario_filial_pm_usuario] foreign key([id_usuario])
references [dbo].[pm_usuario] ([id_usuario])

alter table [dbo].[pm_usuario_filial] check constraint [fk_pm_usuario_filial_pm_usuario]

commit
--rollback