using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
	public class ComunicarExpiracaoApiDto
	{
		public int numDebito { get; set; }
		public int anoDebito { get; set; }
		public int numSolicitacao { get; set; }
		public int anoSolicitacao { get; set; }
		public string dtCancelamento { get; set; }
	}
}