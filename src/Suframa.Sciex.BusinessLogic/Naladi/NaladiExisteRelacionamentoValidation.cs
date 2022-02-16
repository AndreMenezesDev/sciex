using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
	public class NaladiExisteRelacionamentoValidation : NaladiValidation<NaladiDto>
	{
		public NaladiExisteRelacionamentoValidation()
		{
			ValidarNaladiExisteRelacionamento();
		}
	}
}