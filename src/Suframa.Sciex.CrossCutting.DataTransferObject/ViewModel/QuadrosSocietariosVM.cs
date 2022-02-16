using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class QuadrosSocietariosVM
    {
        public int? IdPessoaJuridica { get; set; }
        public IEnumerable<QuadroSocietarioVM> QuadrosSocietarios { get; set; }
    }
}