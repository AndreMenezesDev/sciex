using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Resources;

namespace Suframa.Sciex.BusinessLogic
{
	/// <summary>https://github.com/JeremySkinner/FluentValidation/wiki/f.-Localization</summary>
	/// <typeparam name="T"></typeparam>
	public abstract class UnidadeReceitaFederalValidation<T> : AbstractValidator<T> where T : UnidadeReceitaFederalDto
	{
		protected void ValidarUnidadeReceitaFederalExisteRelacionamento()
		{
			RuleFor(c => c.TotalEncontradoUnidadeReceitaFederal)
				.Equal(0).WithErrorCode("ERR-112").WithMessage(Resources.RELACIONAMENTO_EXISTE);


		}

		protected void ValidarUnidadeReceitaFederalExcluir()
		{
			RuleFor(c => c.TotalEncontradoUnidadeReceitaFederal)
				.Equal(-1).WithErrorCode("ERR-112").WithMessage(Resources.ESTE_REGISTRO_NAO_PODE_SER_EXCLUIDO);


		}

		protected void ValidarUnidadeReceitaFederalExistente()
		{
			RuleFor(c => c.TotalEncontradoUnidadeReceitaFederal)
				.Equal(0).WithMessage(Resources.RELACIONAMENTO_EXISTE);
		}
		
	}
}