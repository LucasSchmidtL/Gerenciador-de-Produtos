CREATE TABLE `Perfil`(
    `id` INT NOT NULL,
    `Descricao` TEXT NOT NULL,
    `SecaoId` TEXT NULL,
    `Peso` FLOAT(53) NULL,
    `AreaBruta` FLOAT(53) NULL,
    `AreaLiq` FLOAT(53) NULL,
    `AreaEq` FLOAT(53) NULL,
    `Ix` FLOAT(53) NULL,
    `Sxt` FLOAT(53) NULL,
    `Sxb` FLOAT(53) NULL,
    `Zx` FLOAT(53) NULL,
    `rx` FLOAT(53) NULL,
    `yt` FLOAT(53) NULL,
    `yb` FLOAT(53) NULL,
    `Ixy` FLOAT(53) NULL,
    `Iy` FLOAT(53) NULL,
    `Syl` FLOAT(53) NULL,
    `Syr` FLOAT(53) NULL,
    `Zy` FLOAT(53) NULL,
    `ry` FLOAT(53) NULL,
    `xl` FLOAT(53) NULL,
    `xr` FLOAT(53) NULL,
    `xo` FLOAT(53) NULL,
    `yo` FLOAT(53) NULL,
    `jx` FLOAT(53) NULL,
    `jy` FLOAT(53) NULL,
    `Cw` FLOAT(53) NULL,
    `J` FLOAT(53) NULL,
    `Ixe` FLOAT(53) NULL,
    `Sxet` FLOAT(53) NULL,
    `Sxeb` FLOAT(53) NULL,
    `Iye` FLOAT(53) NULL,
    `Syel` FLOAT(53) NULL,
    `Syer` FLOAT(53) NULL,
    `p1` FLOAT(53) NULL,
    `p2` FLOAT(53) NULL,
    `p3` FLOAT(53) NULL,
    `SimetricoX` BOOLEAN NULL,
    `SimetricoY` BOOLEAN NULL,
    PRIMARY KEY(`id`)
);
ALTER TABLE
    `Perfil` ADD UNIQUE `perfil_descricao_unique`(`Descricao`);
CREATE TABLE `ItemERP`(
    `Id` INT NOT NULL,
    `ERP` INT UNSIGNED NOT NULL DEFAULT '11111' AUTO_INCREMENT,
    `TipoItem` TEXT NOT NULL COMMENT '-> Item comprado
-> Item Eng.
-> Item Produzido
-> MP',
    `Descricao` TEXT NOT NULL DEFAULT 'PERFIL_#1,00_XXxXXxXXMM',
    `Revisao` TEXT NOT NULL DEFAULT '1',
    `DataCriacao` DATE NOT NULL DEFAULT 'XX/XX/25',
    `Status` TEXT NOT NULL DEFAULT 'Obsoleto',
    `ComAcabamento` BOOLEAN NOT NULL DEFAULT 'Zincado' COMMENT '-> S/ Acabamento
-> Zincado
-> Galvanizado',
    `Aco` TEXT NULL,
    `ChapaAberta` INT NULL,
    `AreaSuperficial` FLOAT(53) NULL,
    `PesoLiquidoMetro` FLOAT(53) NULL,
    `PesoBrutoMetro` FLOAT(53) NULL,
    `PerimetroSolda` FLOAT(53) NULL,
    `SRId` INT NULL,
    `DesenvolvimentoId` BIGINT NULL,
    `QuantidadeDobras` INT NULL,
    `MateriaPrimaId` INT NULL,
    `TagId` BIGINT NULL,
    `Altura` FLOAT(53) NULL,
    `Comprimento` FLOAT(53) NULL,
    `Profundidade` FLOAT(53) NULL,
    `ComprimentoMaximo` FLOAT(53) NULL,
    `Passo` INT NULL,
    `Classificacao` INT NULL,
    `PerfilId` BIGINT NOT NULL,
    PRIMARY KEY(`ERP`)
);
CREATE TABLE `Produto`(
    `Id` INT NOT NULL,
    `Familia` TEXT NOT NULL,
    `NomeComercial` TEXT NOT NULL,
    `VariaveisId` INT UNSIGNED NOT NULL DEFAULT 'Coluna' AUTO_INCREMENT,
    `Precificacao` TEXT NOT NULL,
    `DesenvolvimentoId` BIGINT NULL,
    `Nivel` INT NOT NULL DEFAULT '1',
    PRIMARY KEY(`Id`)
);
ALTER TABLE
    `Produto` ADD UNIQUE `produto_familia_unique`(`Familia`);
CREATE TABLE `Agrupador`(
    `Id` INT NOT NULL,
    `Nome` TEXT NOT NULL,
    `DesenvolvimentoId` BIGINT NULL,
    `AgrupadorPaiId` INT NOT NULL,
    `Nivel` INT NOT NULL COMMENT '2',
    `Precificador` TEXT NULL,
    PRIMARY KEY(`Id`)
);
ALTER TABLE
    `Agrupador` ADD UNIQUE `agrupador_nome_unique`(`Nome`);
CREATE TABLE `Desenho`(
    `DesenhoId` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `nome` TEXT NOT NULL,
    `Descricao` TEXT NULL,
    `DataCriacao` DATE NOT NULL,
    `RevisaoAtual` INT NOT NULL,
    `Status` TEXT NOT NULL,
    `Classificacao` TEXT NOT NULL,
    `SoliciatacaoAlteracaoId` INT NULL,
    `DesenhadoPor` TEXT NOT NULL,
    `AprovadoPor` TEXT NOT NULL,
    `DesenhoAntigo` TEXT NULL,
    `ArquivoDesenho` BLOB NULL
);
CREATE TABLE `Componente`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` TEXT NOT NULL,
    `Nivel` INT NOT NULL DEFAULT '3',
    `DesenvolvimentoId` BIGINT NULL,
    `Precificador` TEXT NOT NULL
);
ALTER TABLE
    `Componente` ADD UNIQUE `componente_nome_unique`(`Nome`);
CREATE TABLE `Tag`(
    `Id` INT NOT NULL,
    `Nome` TEXT NOT NULL,
    `ItemERPId` INT NOT NULL,
    PRIMARY KEY(`Id`)
);
ALTER TABLE
    `Tag` ADD INDEX `tag_nome_index`(`Nome`);
ALTER TABLE
    `Tag` ADD UNIQUE `tag_nome_unique`(`Nome`);
CREATE TABLE `Desenvolvimento`(
    `Id` INT NOT NULL,
    `Classificacao` TEXT NOT NULL COMMENT 'DPN',
    `Dificuldade` TEXT NOT NULL,
    `Produto` TEXT NOT NULL,
    `Descricao` LONGTEXT NOT NULL,
    `ERP` INT NOT NULL,
    `DataInicio` DATE NOT NULL,
    `DataFim` DATE NOT NULL,
    `ProjetoFinep` TEXT NOT NULL,
    `ProjetoLeiBem` TEXT NOT NULL,
    `Custo` FLOAT(53) NOT NULL,
    `Fase` TEXT NOT NULL,
    `Status` TEXT NOT NULL,
    `TempoDesenvolvimento` INT NOT NULL,
    `Solicitante` TEXT NOT NULL,
    `TRL` TEXT NOT NULL,
    `Agrupador` TEXT NOT NULL,
    `Componente` TEXT NOT NULL,
    `MarcoId` BIGINT NOT NULL,
    PRIMARY KEY(`Id`)
);
CREATE TABLE `ProdutoAgrupador`(
    `id` INT NOT NULL,
    `ProdutoId` INT UNSIGNED NOT NULL,
    `AgrupadorId` INT NOT NULL,
    `Variavel` TEXT NULL,
    `Status` BOOLEAN NOT NULL,
    PRIMARY KEY(`id`)
);
ALTER TABLE
    `ProdutoAgrupador` ADD INDEX `produtoagrupador_produtoid_index`(`ProdutoId`);
ALTER TABLE
    `ProdutoAgrupador` ADD INDEX `produtoagrupador_agrupadorid_index`(`AgrupadorId`);
CREATE TABLE `AgrupadorComponente`(
    `id` INT NOT NULL,
    `AgrupadorId` INT UNSIGNED NOT NULL,
    `ComponenteId` INT NOT NULL,
    `Quantidade` TEXT NULL,
    `Comprimento` TEXT NULL,
    `Profundidade` TEXT NULL,
    `Altura` TEXT NULL,
    `Status` BOOLEAN NOT NULL,
    PRIMARY KEY(`id`)
);
ALTER TABLE
    `AgrupadorComponente` ADD INDEX `agrupadorcomponente_agrupadorid_index`(`AgrupadorId`);
ALTER TABLE
    `AgrupadorComponente` ADD INDEX `agrupadorcomponente_componenteid_index`(`ComponenteId`);
CREATE TABLE `ComponenteItemERP`(
    `Id` INT NOT NULL,
    `ComponenteId` INT NOT NULL,
    `ItemERPId` INT UNSIGNED NOT NULL,
    `Comprimento` TEXT NULL,
    `Altura` TEXT NULL,
    `Profundidade` TEXT NULL,
    `Quantidade` TEXT NOT NULL,
    `Status` BOOLEAN NOT NULL,
    PRIMARY KEY(`Id`)
);
ALTER TABLE
    `ComponenteItemERP` ADD INDEX `componenteitemerp_componenteid_index`(`ComponenteId`);
ALTER TABLE
    `ComponenteItemERP` ADD INDEX `componenteitemerp_itemerpid_index`(`ItemERPId`);
CREATE TABLE `AgrupadorItemERP`(
    `id` INT NOT NULL,
    `AgrupadorId` INT UNSIGNED NOT NULL,
    `ItemERPId` INT NOT NULL,
    `Comprimento` TEXT NULL,
    `Profundidade` TEXT NULL,
    `Altura` TEXT NULL,
    `Quantidade` TEXT NOT NULL,
    `Status` BOOLEAN NOT NULL,
    PRIMARY KEY(`id`)
);
ALTER TABLE
    `AgrupadorItemERP` ADD INDEX `agrupadoritemerp_agrupadorid_index`(`AgrupadorId`);
ALTER TABLE
    `AgrupadorItemERP` ADD INDEX `agrupadoritemerp_itemerpid_index`(`ItemERPId`);
CREATE TABLE `VariaveisProduto`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` TEXT NOT NULL,
    `Descricao` TEXT NOT NULL,
    `Tipo` TEXT NOT NULL,
    `ProdutoId` INT NOT NULL,
    `Status` BOOLEAN NOT NULL,
    `Valor` TEXT NULL
);
ALTER TABLE
    `VariaveisProduto` ADD INDEX `variaveisproduto_produtoid_index`(`ProdutoId`);
CREATE TABLE `VariaveisAgrupador`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` TEXT NOT NULL,
    `Descricao` TEXT NOT NULL,
    `Tipo` TEXT NOT NULL,
    `AgrupadorId` INT NOT NULL,
    `Status` BOOLEAN NOT NULL,
    `Valor` TEXT NULL
);
ALTER TABLE
    `VariaveisAgrupador` ADD INDEX `variaveisagrupador_agrupadorid_index`(`AgrupadorId`);
CREATE TABLE `AgrupadorConjunto`(
    `id` INT NOT NULL,
    `AgrupadorId_Pai` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `AgrupadorId_Filho` INT NOT NULL,
    `Quantidade` TEXT NULL,
    `Comprimento` TEXT NULL,
    `Profundidade` TEXT NULL,
    `Altura` TEXT NULL,
    `Status` BOOLEAN NOT NULL,
    PRIMARY KEY(`AgrupadorId_Filho`)
);
CREATE TABLE `VariaveisComponente`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` TEXT NOT NULL,
    `Descricao` TEXT NULL,
    `Tipo` TEXT NOT NULL,
    `ComponenteId` INT NOT NULL,
    `Status` BOOLEAN NOT NULL,
    `Valor` TEXT NULL
);
ALTER TABLE
    `VariaveisComponente` ADD INDEX `variaveiscomponente_componenteid_index`(`ComponenteId`);
CREATE TABLE `TagItemERP`(
    `Tagid` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `ERP` INT NOT NULL,
    PRIMARY KEY(`ERP`)
);
CREATE TABLE `ItemERPComposto`(
    `id` INT NOT NULL,
    `ItemERPid_Pai` INT UNSIGNED NOT NULL,
    `ItemERPid_Filho` INT NOT NULL,
    `EquacaoComprimento` TEXT NULL,
    `EquacaoProfundidade` TEXT NULL,
    `EquacaoAltura` TEXT NULL,
    `EquacaoQuantidade` TEXT NOT NULL,
    PRIMARY KEY(`id`)
);
ALTER TABLE
    `ItemERPComposto` ADD INDEX `itemerpcomposto_itemerpid_pai_index`(`ItemERPid_Pai`);
ALTER TABLE
    `ItemERPComposto` ADD INDEX `itemerpcomposto_itemerpid_filho_index`(`ItemERPid_Filho`);
CREATE TABLE `VariaveisItemERPComposto`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `Nome` TEXT NOT NULL,
    `Descricao` TEXT NULL,
    `Tipo` TEXT NOT NULL,
    `ItemERPCompostoId` INT NOT NULL,
    `Status` BOOLEAN NOT NULL,
    `Valor` TEXT NULL,
    PRIMARY KEY(`ItemERPCompostoId`)
);
ALTER TABLE
    `VariaveisItemERPComposto` ADD INDEX `variaveisitemerpcomposto_nome_index`(`Nome`);
CREATE TABLE `ItemERPVinculado`(
    `id` INT NOT NULL,
    `ItemERP_SemAcabamentoId` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `ItemERP_PintadoId` INT NULL,
    `ItemERP_GalvanizadoId` INT NULL,
    `ItemERP_Zincado` INT NULL,
    PRIMARY KEY(`id`)
);
ALTER TABLE
    `ItemERPVinculado` ADD INDEX `itemerpvinculado_itemerp_pintadoid_index`(`ItemERP_PintadoId`);
ALTER TABLE
    `ItemERPVinculado` ADD INDEX `itemerpvinculado_itemerp_galvanizadoid_index`(`ItemERP_GalvanizadoId`);
ALTER TABLE
    `ItemERPVinculado` ADD INDEX `itemerpvinculado_itemerp_zincado_index`(`ItemERP_Zincado`);
CREATE TABLE `DesenhoRevisao`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `DesenhoId` INT NOT NULL,
    `Data` DATE NOT NULL,
    `Numero` INT NOT NULL,
    `Motivo` TEXT NOT NULL,
    `AprovadoPor` TEXT NOT NULL,
    `ImplementadoPor` TEXT NOT NULL,
    `CaminhoDesenho` VARCHAR(255) NOT NULL
);
CREATE TABLE `DesenhoItemERP`(
    `DesenhoId` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `ItemERP` BIGINT NOT NULL
);
CREATE TABLE `Equacoes`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY
);
CREATE TABLE `PerfisRevisao`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Numero` INT NOT NULL,
    `Motivo` TEXT NOT NULL,
    `DataRevisao` DATE NOT NULL,
    `PerfilId` INT NOT NULL
);
CREATE TABLE `Secao`(
    `Secao` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY
);
CREATE TABLE `EquacaoRevisao`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY
);
CREATE TABLE `ItemERPRevisao`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Numero` INT NOT NULL,
    `Motivo` TEXT NOT NULL,
    `DataRevisao` DATE NOT NULL,
    `ItemERPId` INT NOT NULL
);
ALTER TABLE
    `ItemERP` ADD CONSTRAINT `itemerp_erp_foreign` FOREIGN KEY(`ERP`) REFERENCES `VariaveisItemERPComposto`(`ItemERPCompostoId`);
ALTER TABLE
    `Agrupador` ADD CONSTRAINT `agrupador_id_foreign` FOREIGN KEY(`Id`) REFERENCES `AgrupadorConjunto`(`AgrupadorId_Pai`);
ALTER TABLE
    `ItemERP` ADD CONSTRAINT `itemerp_erp_foreign` FOREIGN KEY(`ERP`) REFERENCES `ItemERPRevisao`(`id`);
ALTER TABLE
    `DesenhoItemERP` ADD CONSTRAINT `desenhoitemerp_desenhoid_foreign` FOREIGN KEY(`DesenhoId`) REFERENCES `Desenho`(`DesenhoId`);
ALTER TABLE
    `ComponenteItemERP` ADD CONSTRAINT `componenteitemerp_componenteid_foreign` FOREIGN KEY(`ComponenteId`) REFERENCES `Componente`(`Id`);
ALTER TABLE
    `DesenhoItemERP` ADD CONSTRAINT `desenhoitemerp_itemerp_foreign` FOREIGN KEY(`ItemERP`) REFERENCES `ItemERP`(`ERP`);
ALTER TABLE
    `AgrupadorItemERP` ADD CONSTRAINT `agrupadoritemerp_agrupadorid_foreign` FOREIGN KEY(`AgrupadorId`) REFERENCES `Agrupador`(`Id`);
ALTER TABLE
    `AgrupadorComponente` ADD CONSTRAINT `agrupadorcomponente_componenteid_foreign` FOREIGN KEY(`ComponenteId`) REFERENCES `Componente`(`Id`);
ALTER TABLE
    `ProdutoAgrupador` ADD CONSTRAINT `produtoagrupador_agrupadorid_foreign` FOREIGN KEY(`AgrupadorId`) REFERENCES `Agrupador`(`Id`);
ALTER TABLE
    `AgrupadorComponente` ADD CONSTRAINT `agrupadorcomponente_agrupadorid_foreign` FOREIGN KEY(`AgrupadorId`) REFERENCES `Agrupador`(`Id`);
ALTER TABLE
    `ItemERP` ADD CONSTRAINT `itemerp_perfilid_foreign` FOREIGN KEY(`PerfilId`) REFERENCES `Perfil`(`id`);
ALTER TABLE
    `Produto` ADD CONSTRAINT `produto_variaveisid_foreign` FOREIGN KEY(`VariaveisId`) REFERENCES `VariaveisProduto`(`id`);
ALTER TABLE
    `ItemERPComposto` ADD CONSTRAINT `itemerpcomposto_itemerpid_pai_foreign` FOREIGN KEY(`ItemERPid_Pai`) REFERENCES `ItemERP`(`ERP`);
ALTER TABLE
    `Produto` ADD CONSTRAINT `produto_desenvolvimentoid_foreign` FOREIGN KEY(`DesenvolvimentoId`) REFERENCES `Desenvolvimento`(`Id`);
ALTER TABLE
    `ItemERP` ADD CONSTRAINT `itemerp_desenvolvimentoid_foreign` FOREIGN KEY(`DesenvolvimentoId`) REFERENCES `Desenvolvimento`(`Id`);
ALTER TABLE
    `ItemERPVinculado` ADD CONSTRAINT `itemerpvinculado_itemerp_semacabamentoid_foreign` FOREIGN KEY(`ItemERP_SemAcabamentoId`) REFERENCES `ItemERP`(`ERP`);
ALTER TABLE
    `Componente` ADD CONSTRAINT `componente_desenvolvimentoid_foreign` FOREIGN KEY(`DesenvolvimentoId`) REFERENCES `Desenvolvimento`(`Id`);
ALTER TABLE
    `Tag` ADD CONSTRAINT `tag_nome_foreign` FOREIGN KEY(`Nome`) REFERENCES `TagItemERP`(`Tagid`);
ALTER TABLE
    `ItemERPComposto` ADD CONSTRAINT `itemerpcomposto_itemerpid_filho_foreign` FOREIGN KEY(`ItemERPid_Filho`) REFERENCES `ItemERP`(`ERP`);
ALTER TABLE
    `ComponenteItemERP` ADD CONSTRAINT `componenteitemerp_itemerpid_foreign` FOREIGN KEY(`ItemERPId`) REFERENCES `ItemERP`(`ERP`);
ALTER TABLE
    `DesenhoRevisao` ADD CONSTRAINT `desenhorevisao_id_foreign` FOREIGN KEY(`Id`) REFERENCES `Desenho`(`DesenhoId`);
ALTER TABLE
    `AgrupadorItemERP` ADD CONSTRAINT `agrupadoritemerp_itemerpid_foreign` FOREIGN KEY(`ItemERPId`) REFERENCES `ItemERP`(`ERP`);
ALTER TABLE
    `VariaveisComponente` ADD CONSTRAINT `variaveiscomponente_componenteid_foreign` FOREIGN KEY(`ComponenteId`) REFERENCES `Componente`(`Id`);
ALTER TABLE
    `ItemERP` ADD CONSTRAINT `itemerp_id_foreign` FOREIGN KEY(`Id`) REFERENCES `TagItemERP`(`Tagid`);
ALTER TABLE
    `Agrupador` ADD CONSTRAINT `agrupador_desenvolvimentoid_foreign` FOREIGN KEY(`DesenvolvimentoId`) REFERENCES `Desenvolvimento`(`Id`);
ALTER TABLE
    `ProdutoAgrupador` ADD CONSTRAINT `produtoagrupador_produtoid_foreign` FOREIGN KEY(`ProdutoId`) REFERENCES `Produto`(`Id`);
ALTER TABLE
    `PerfisRevisao` ADD CONSTRAINT `perfisrevisao_perfilid_foreign` FOREIGN KEY(`PerfilId`) REFERENCES `Perfil`(`id`);
ALTER TABLE
    `VariaveisAgrupador` ADD CONSTRAINT `variaveisagrupador_agrupadorid_foreign` FOREIGN KEY(`AgrupadorId`) REFERENCES `Agrupador`(`Id`);