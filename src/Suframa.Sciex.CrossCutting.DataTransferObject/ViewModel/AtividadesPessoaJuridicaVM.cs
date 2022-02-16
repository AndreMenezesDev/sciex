using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class AtividadesPessoaJuridicaVM
    {
        public IEnumerable<AtividadePessoaJuridicaVM> AtividadeSecundariaPessoaJuridica { get; set; }
        public IEnumerable<AtividadePessoaJuridicaVM> AtividadesPessoaJuridica { get; set; }
        public int? IdPessoaJuridica { get; set; }
        public int? IdSubClasseAtividade { get; set; }
    }
}