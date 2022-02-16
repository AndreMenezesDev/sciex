using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject

{
	public enum EnumSituacaoSincronismoBloqueio
	{
		[Description("Informação Ainda Não Existente")]
		NaoExistente = 0,

		[Description("Não Enviado ao Cadastro Legado")]
		NaoEnviado = 1,

		[Description("Enviado ao Cadastro Legado")]
		Enviado = 2,

		[Description("Recebido do Cadastro Legado")]
		Recebido = 3
	}
}