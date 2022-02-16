using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.Mensagens
{
	public class Mensagens
	{
		protected Mensagens() { }

		public const string ESTRUTURA_PROPRIA_FORA_PADRAO = "Nome do arquivo não está no padrão definido pela SUFRAMA, não é possível realizar envio de PLI.";
		public const string ESTRUTURA_PROPRIA_INSCRICAO_SEM_CADASTRO = "Empresa sem cadastro na SUFRAMA";
		public const string ESTRUTURA_PROPRIA_INSCRICAO_BLOQUEADA = "Empresa bloqueada ou não cadastrada, não é possível realizar envio de PLI. É necessário regularizar a situação cadastral junto à SUFRAMA.";
		public const string ESTRUTURA_PROPRIA_COMPACTACAO = "Tipo de compactação não suportada, não é possível realizar envio de PLI. É necessário enviar arquivo com compactação ZIP";
		public const string ESTRUTURA_PROPRIA_ARQUIVOS = "Arquivo compactado contém mais de 1 (um) item. É permitido apenas 1 (um) arquivo de PLI para cada arquivo compactado.";
		public const string ESTRUTURA_PROPRIA_ESTRUTURA_ERRADA = "Arquivo com erro na estrutura, não é possível realizar envio de PLI. É necessário seguir o padrão de PLI de Estrutura Própria definido pela SUFRAMA.";
		public const string ESTRUTURA_PROPRIA_ESTRUTURA_ERRADA_ITENS = "Complemento dos Itens ou Informações Complementares de algum PLI dentro do arquivo está ultrapassando o limite de sequência do texto, não é possível realizar envio de PLI.";
		public const string ESTRUTURA_PROPRIA_ESTRUTURA_NENHUM_ARQUIVO_ENCONTRADO = "Não foi encontrado arquivo dentro do arquivo compactado (.PL5ZIP).";
		public const string ESTRUTURA_PROPRIA_JA_ENVIADO = "Arquivo já enviado.";
		public const string ESTRUTURA_PROPRIA_CORROMPIDO = "O arquivo não está zipado ou está corrompido.";
		public const string ESTRUTURA_PROPRIA_SUCESSO = "Arquivo enviado com sucesso. Protocolo gerado: ";
		public const string ESTRUTURA_PROPRIA_TIPO_DOCUMENTO = "Serão aceitos somente PLI Normal e Substitutivo. Foi identificado PLI com tipo de documento diferente dos aceitáveis, não é possível realizar envio de PLI.";
		public const string ESTRUTURA_PROPRIA_CNPJ_DIFERENTE = "Serão aceitos somente PLI correspondente a empresa especificada no nome do arquivo. Foi identificado PLI com empresa diferente, não é possível realizar envio de PLI.";
		public const string ESTRUTURA_PROPRIA_EMPRESA_REPRESENTADA = "Este usuário (logado ou representado) não está associado a empresa especificada no nome do arquivo, não é possível realizar envio de PLI.";
		public const string ESTRUTURA_PROPRIA_TAMANHO_EXCEDIDO = "Tamanho do arquivo ultrapassou o limite permitido de 20MB, não é possível realizar envio de PLI";
		public const string ESTRUTURA_PROPRIA_NOME_ARQUIVOS_DIFERENTE = "O arquivo compactado não corresponde ao arquivo descompactado, não é possível realizar envio de PLI.";
		public const string ESTRUTURA_PROPRIA_CODIGO_APLICACAO_INVALIDO = "Código da aplicação inválido. Informe somente PLIs de industrialização ou comercialização.";
		public const string ESTRUTURA_PROPRIA_ANO_CORRENTE_INVALIDO = "O ano de um ou mais PLIs do arquivo não corresponde ao ano corrente";
		public const string ESTRUTURA_PROPRIA_FORMATACAO_PLI = "Existe um ou mais PLIs com o formato do número do importador inválido. Utilizar o formato AAAA/##### (alfanumérico)";

	}
}
