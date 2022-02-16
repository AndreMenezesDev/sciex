using FluentValidation;

namespace Suframa.Sciex.CrossCutting.Validation
{
    public static class Initialize
    {
        public static void Init()
        {
            ValidatorOptions.LanguageManager = new TranslationLanguageManager();
        }
    }
}