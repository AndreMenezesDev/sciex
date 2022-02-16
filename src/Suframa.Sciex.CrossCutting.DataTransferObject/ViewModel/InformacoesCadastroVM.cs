using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class InformacoesCadastroVM
    {
        public DadosCadastroVM DadosCadastro { get; set; }
        public IEnumerable<ResumoVM> Resumo { get; set; }
    }
}