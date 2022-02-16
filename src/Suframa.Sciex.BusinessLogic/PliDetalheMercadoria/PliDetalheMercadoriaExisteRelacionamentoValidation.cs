using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliDetalheMercadoriaExisteRelacionamentoValidation : PliDetalheMercadoriaValidation<PliDetalheMercadoriaDto>
	{
		public PliDetalheMercadoriaExisteRelacionamentoValidation()
		{
			ValidarPliDetalheMercadoriaExisteRelacionamento();
		}
	}
}