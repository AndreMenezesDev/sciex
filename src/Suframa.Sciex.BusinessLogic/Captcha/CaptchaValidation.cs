using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.Resources;

namespace Suframa.Sciex.BusinessLogic
{
    public sealed class CaptchaValidation : AbstractValidator<CaptchaDto>
    {
        public CaptchaValidation()
        {
            ValidarCaptcha();
        }

        public void ValidarCaptcha()
        {
            RuleFor(c => c.IsCaptchaValido)
                .Equal(true).WithMessage(Resources.CAPTCHA_INVALIDO);
        }
    }
}