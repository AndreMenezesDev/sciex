using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliMercadoriaExisteRelacionamentoValidation : PliMercadoriaValidation<PliMercadoriaDto>
	{
		public PliMercadoriaExisteRelacionamentoValidation()
		{
			ValidarPliMercadoriaExisteRelacionamento();
		}
	}
}