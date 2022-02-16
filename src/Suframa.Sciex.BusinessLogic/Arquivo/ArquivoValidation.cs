using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Resources;

namespace Suframa.Sciex.BusinessLogic
{
    /// <summary>https://github.com/JeremySkinner/FluentValidation/wiki/f.-Localization</summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ArquivoValidation<T> : AbstractValidator<T> where T : ArquivoDto
    {
        protected void ValidarTamanhoArquivo()
        {
            RuleFor(c => c.Tamanho)
                .LessThan(20 * 1024 * 1024).WithMessage(Resources.TAMANHO_DOCUMENTO_INVALIDO);
        }
    }
}