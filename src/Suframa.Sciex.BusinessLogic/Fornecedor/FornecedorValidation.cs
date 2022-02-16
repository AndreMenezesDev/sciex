using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Resources;

namespace Suframa.Sciex.BusinessLogic
{
	/// <summary>https://github.com/JeremySkinner/FluentValidation/wiki/f.-Localization</summary>
	/// <typeparam name="T"></typeparam>
	public abstract class FornecedorValidation<T> : AbstractValidator<T> where T : FornecedorDto
	{
		protected void ValidarFornecedorExisteRelacionamento()
		{
			RuleFor(c => c.TotalEncontradoFornecedor)
				.Equal(0).WithErrorCode("ERR-112").WithMessage(Resources.RELACIONAMENTO_EXISTE);
		}

		protected void ValidarFornecedorExcluir()
		{
			RuleFor(c => c.TotalEncontradoFornecedor)
				.Equal(-1).WithErrorCode("ERR-112").WithMessage(Resources.ESTE_REGISTRO_NAO_PODE_SER_EXCLUIDO);

		}
	}
}