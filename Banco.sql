CREATE DATABASE IF NOT EXISTS topicos;

USE topicos;

CREATE TABLE IF NOT EXISTS venda (
	numerovenda VARCHAR(32) UNIQUE NOT NULL,
    data DATE NOT NULL,
    cpf_cnpj VARCHAR(18) NOT NULL,
    nome VARCHAR(70) DEFAULT '',
    cpf_vendedor VARCHAR(18) NOT NULL,
    valordeentrada DOUBLE DEFAULT '0',
    quantidadeparcelas SMALLINT DEFAULT '0',
    PRIMARY KEY(numerovenda)
);

CREATE TABLE IF NOT EXISTS mov_estoque (
	codvenda VARCHAR(32) DEFAULT '',
	codproduto MEDIUMINT NOT NULL,
    descricao VARCHAR(70) DEFAULT '',
    quantidade MEDIUMINT UNSIGNED NOT NULL,
    valorunitario DOUBLE UNSIGNED NOT NULL,
    FOREIGN KEY (codvenda) REFERENCES venda(numerovenda)
);