using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IArquivoBll
    {
        void Apagar(int id);

        ArquivoVM Salvar(ArquivoVM arquivoVM);

        ArquivoVM Selecionar(int idArquivo);
    }
}