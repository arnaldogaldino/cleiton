CREATE TABLE pm_auditoria_log ( 
	id_auditoria_log bigint identity(1,1)  NOT NULL,
	id_usuario bigint,
	id_record bigint NOT NULL,
	usuario varchar(50) NOT NULL,
	nome_tabela varchar(50) NOT NULL,
	acao varchar(50) NOT NULL,
	data_acao datetime DEFAULT (getdate()) NOT NULL
)
;

CREATE TABLE pm_auditoria_log_detalhe ( 
	id_auditoria_log_detalhe bigint identity(1,1)  NOT NULL,
	id_auditoria_log bigint NOT NULL,
	nome_coluna varchar(255) NOT NULL,
	valor_original varchar(255),
	valor_corrente varchar(255)
)
;

CREATE TABLE pm_empresa ( 
	id_empresa bigint identity(1,1)  NOT NULL,
	id_fiscal_cnae_pri bigint,
	id_fiscal_cnae_sec bigint,
	apelido varchar(20),
	data_cadastro datetime DEFAULT (getdate()) NOT NULL,
	tipo_pessoa varchar(9) NOT NULL,
	crt int,
	logotipo image,
	razao_social varchar(70) NOT NULL,
	nome_fantasia varchar(70) NOT NULL,
	cnpj varchar(18) NOT NULL,
	i_estadual varchar(15) NOT NULL,
	i_municipal varchar(15),
	telefone varchar(15),
	fax varchar(15),
	email varchar(100),
	end_cep varchar(9),
	end_endereco varchar(70),
	end_numero varchar(10),
	end_complemento varchar(20),
	end_bairro varchar(50),
	end_cidade varchar(30),
	end_estado varchar(2),
	end_municipio varchar(10),
	cob_cep varchar(9),
	cob_endereco varchar(70),
	cob_numero varchar(10),
	cob_complemento varchar(20),
	cob_bairro varchar(50),
	cob_cidade varchar(30),
	cob_estado varchar(2),
	cob_municipio varchar(10),
	ent_cep varchar(9),
	ent_endereco varchar(70),
	ent_numero varchar(10),
	ent_complemento varchar(20),
	ent_bairro varchar(50),
	ent_cidade varchar(30),
	ent_estado varchar(2),
	ent_municipio varchar(10),
	excluido bit DEFAULT ((0)) NOT NULL
)
;

CREATE TABLE pm_filial ( 
	id_filial bigint identity(1,1)  NOT NULL,
	id_empresa bigint DEFAULT ((0)) NOT NULL,
	id_fiscal_cnae_pri bigint,
	id_fiscal_cnae_sec bigint,
	apelido varchar(20),
	data_cadastro datetime DEFAULT (getdate()) NOT NULL,
	tipo_pessoa varchar(9) NOT NULL,
	crt int,
	logotipo image,
	razao_social varchar(70) NOT NULL,
	nome_fantasia varchar(70) NOT NULL,
	cnpj varchar(18) NOT NULL,
	i_estadual varchar(15) NOT NULL,
	i_municipal varchar(15),
	telefone varchar(15),
	fax varchar(15),
	email varchar(100),
	end_cep varchar(9),
	end_endereco varchar(70),
	end_numero varchar(10),
	end_complemento varchar(20),
	end_bairro varchar(50),
	end_cidade varchar(30),
	end_estado varchar(2),
	end_municipio varchar(10),
	cob_cep varchar(9),
	cob_endereco varchar(70),
	cob_numero varchar(10),
	cob_complemento varchar(20),
	cob_bairro varchar(50),
	cob_cidade varchar(30),
	cob_estado varchar(2),
	cob_municipio varchar(10),
	ent_cep varchar(9),
	ent_endereco varchar(70),
	ent_numero varchar(10),
	ent_complemento varchar(20),
	ent_bairro varchar(50),
	ent_cidade varchar(30),
	ent_estado varchar(2),
	ent_municipio varchar(10),
	excluido bit DEFAULT ((0)) NOT NULL
)
;

CREATE TABLE pm_menu ( 
	id_menu bigint identity(1,1)  NOT NULL,
	id_menu_pai bigint,
	nome varchar(50) NOT NULL,
	texto varchar(50) NOT NULL,
	descricao varchar(50)
)
;

CREATE TABLE pm_menu_detalhe ( 
	id_menu_detalhe bigint identity(1,1)  NOT NULL,
	id_menu bigint NOT NULL,
	nome varchar(25) NOT NULL,
	descricao varchar(50)
)
;

CREATE TABLE pm_menu_detalhe_permissao ( 
	id_menu_detalhe bigint NOT NULL,
	id_usuario bigint NOT NULL
)
;

CREATE TABLE pm_menu_permissao ( 
	id_menu bigint NOT NULL,
	id_usuario bigint NOT NULL
)
;

CREATE TABLE pm_usuario ( 
	id_usuario bigint identity(1,1)  NOT NULL,
	nome varchar(50) NOT NULL,
	usuario varchar(20) NOT NULL,
	senha varchar(10) NOT NULL,
	email varchar(150) NOT NULL,
	data_cadastro datetime DEFAULT (getdate()) NOT NULL,
	excluido bit DEFAULT ((0)) NOT NULL
)
;

CREATE TABLE pm_usuario_filial ( 
	id_usuario bigint NOT NULL,
	id_filial bigint NOT NULL
)
;


ALTER TABLE pm_auditoria_log ADD CONSTRAINT PK_pm_auditoria_log 
	PRIMARY KEY CLUSTERED (id_auditoria_log)
;

ALTER TABLE pm_auditoria_log_detalhe ADD CONSTRAINT PK_pm_auditoria_log_detalhe 
	PRIMARY KEY CLUSTERED (id_auditoria_log_detalhe)
;

ALTER TABLE pm_empresa ADD CONSTRAINT PK_pm_empresa 
	PRIMARY KEY CLUSTERED (id_empresa)
;

ALTER TABLE pm_filial ADD CONSTRAINT PK_pm_filial 
	PRIMARY KEY CLUSTERED (id_filial)
;

ALTER TABLE pm_menu ADD CONSTRAINT PK_pm_menu 
	PRIMARY KEY CLUSTERED (id_menu)
;

ALTER TABLE pm_menu_detalhe ADD CONSTRAINT PK_pm_menu_detalhe 
	PRIMARY KEY CLUSTERED (id_menu_detalhe)
;

ALTER TABLE pm_menu_detalhe_permissao ADD CONSTRAINT PK_pm_menu_detalhe_permissao 
	PRIMARY KEY CLUSTERED (id_menu_detalhe, id_usuario)
;

ALTER TABLE pm_menu_permissao ADD CONSTRAINT PK_pm_menu_permissao 
	PRIMARY KEY CLUSTERED (id_menu, id_usuario)
;

ALTER TABLE pm_usuario ADD CONSTRAINT PK_pm_usuario 
	PRIMARY KEY CLUSTERED (id_usuario)
;

ALTER TABLE pm_usuario_filial ADD CONSTRAINT PK_pm_usuario_filial 
	PRIMARY KEY CLUSTERED (id_usuario, id_filial)
;



ALTER TABLE pm_auditoria_log ADD CONSTRAINT FK_pm_auditoria_log_pm_usuario 
	FOREIGN KEY (id_usuario) REFERENCES pm_usuario (id_usuario)
;

ALTER TABLE pm_auditoria_log_detalhe ADD CONSTRAINT FK_pm_auditoria_log_detalhe_pm_auditoria_log 
	FOREIGN KEY (id_auditoria_log) REFERENCES pm_auditoria_log (id_auditoria_log)
;

ALTER TABLE pm_filial ADD CONSTRAINT FK_pm_filial_pm_empresa 
	FOREIGN KEY (id_empresa) REFERENCES pm_empresa (id_empresa)
;

ALTER TABLE pm_menu ADD CONSTRAINT FK_pm_menu_pm_menu 
	FOREIGN KEY (id_menu_pai) REFERENCES pm_menu (id_menu)
;

ALTER TABLE pm_menu_detalhe ADD CONSTRAINT FK_pm_menu_detalhe_pm_menu 
	FOREIGN KEY (id_menu) REFERENCES pm_menu (id_menu)
;

ALTER TABLE pm_menu_detalhe_permissao ADD CONSTRAINT FK_pm_menu_detalhe_permissao_pm_menu_detalhe 
	FOREIGN KEY (id_menu_detalhe) REFERENCES pm_menu_detalhe (id_menu_detalhe)
;

ALTER TABLE pm_menu_detalhe_permissao ADD CONSTRAINT FK_pm_menu_detalhe_permissao_pm_usuario 
	FOREIGN KEY (id_usuario) REFERENCES pm_usuario (id_usuario)
;

ALTER TABLE pm_menu_permissao ADD CONSTRAINT FK_pm_menu_permissao_pm_menu 
	FOREIGN KEY (id_menu) REFERENCES pm_menu (id_menu)
;

ALTER TABLE pm_menu_permissao ADD CONSTRAINT FK_pm_menu_permissao_pm_usuario 
	FOREIGN KEY (id_usuario) REFERENCES pm_usuario (id_usuario)
;

ALTER TABLE pm_usuario_filial ADD CONSTRAINT FK_pm_usuario_filial_pm_filial 
	FOREIGN KEY (id_filial) REFERENCES pm_filial (id_filial)
;

ALTER TABLE pm_usuario_filial ADD CONSTRAINT FK_pm_usuario_filial_pm_usuario 
	FOREIGN KEY (id_usuario) REFERENCES pm_usuario (id_usuario)
;
