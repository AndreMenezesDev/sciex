using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Resources;

namespace Suframa.Sciex.BusinessLogic
{
	/// <summary>https://github.com/JeremySkinner/FluentValidation/wiki/f.-Localization</summary>
	/// <typeparam name="T"></typeparam>
	public abstract class PliProdutoValidation<T> : AbstractValidator<T> where T : PliProdutoDto
	{
		
		protected void ValidarPliProdutoExcluir()
		{
			RuleFor(c => c.TotalEncontradoPliProduto)
				.Equal(-1).WithErrorCode("ERR-112").WithMessage(Resources.ESTE_REGISTRO_NAO_PODE_SER_EXCLUIDO);

		}
	}
}