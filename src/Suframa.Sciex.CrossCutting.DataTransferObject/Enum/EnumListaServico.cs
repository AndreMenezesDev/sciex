using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
	public enum EnumListaServico
	{
		[Description("Emitir Alerta da Paridade Cambial")]
		EmitirAlertaParidadeCambial = 1,

		[Description("REGISTRAR PARIDADE CAMBIAL AUTOMATICAMENTE")]
		RegistrarParidadeCambialAutomaticamente = 2,

		[Description("OBTER DADOS DO USUARIO INTERNO")]
		ObterDadosUsuarioInterno = 3,

		[Description("CALCULAR TCIF")]
		CalcularTCIF = 4,

		[Description("GERAR DÉBITO")]
		GerarDebito = 5,

		[Description("PROCESSAR PLI")]
		ProcessarPLI = 6,

		[Description("COMPLEMENTAR PLI")]
		ComplementarPLI = 7,

		[Description("GERAR ARQUIVO ALI")]
		GerarArquivoALI = 8,

		[Description("GERAR ARQUIVO ALI-CANCELAMENTO")]
		GerarArquivoAliCancelamento = 9,

		[Description("ENVIAR ARQUIVO DE ALI-NORMAL/SUBSTITUTIVO")]
		EnviarArquivoLiNormalSubstituto = 10,

		[Description("ENVIAR ARQUIVO DE ALI-CANCELAMENTO")]
		EnviarArquivoAliCancelamento = 11,

		[Description("SALVAR ARQUIVO DE LI-NORMAL")]
		SalvarArquivoLiNormal = 12,

		[Description("SALVAR ARQUIVO DE LI-CANCELAMENTO")]
		SalvarArquivoLiCancelamento = 13,

		[Description("LER ARQUIVO DE LI-NORMAL")]
		LerArquivoLiNormal = 14,

		[Description("LER ARQUIVO DE LI-CANCELAMENTO")]
		LerArquivoLiCancelamento = 15,

		[Description("EXTRAIR ARQUIVO DE ESTRUTURA PRÓPRIA")]
		ReceberBloqueioSac = 16,

		[Description("VALIDAR SOLICITAÇÃO DE PLI DE ESTRUTURA PRÓPRIA")]
		ReceberDesbloqueioSac = 17,

		[Description("DISTRIBUIÇÃO AUTOMÁTICA")]
		ReceberInscricaoSuframaAuditor = 18,

		[Description("SALVAR ARQUIVO DE DI")]
		SalvarArquivoDiNormal = 19,

		[Description("LER ARQUIVO DE DI")]
		LerArquivoDi = 20,

		[Description("PROCESSAR DI")]
		ProcessarDi = 21
	}
}