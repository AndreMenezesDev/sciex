namespace Suframa.Sciex.CrossCutting.Validation
{
    public class TranslationLanguageManager : FluentValidation.Resources.LanguageManager
    {
        public TranslationLanguageManager()
        {
            AddTranslation("pt-br", "NotNullValidator", "O campo '{PropertyName}' é obrigatório");
        }
    }
}