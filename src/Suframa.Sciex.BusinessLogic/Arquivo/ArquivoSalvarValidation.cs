using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
    public class ArquivoSalvarValidation : ArquivoValidation<ArquivoDto>
    {
        public ArquivoSalvarValidation()
        {
            ValidarTamanhoArquivo();
        }
    }
}