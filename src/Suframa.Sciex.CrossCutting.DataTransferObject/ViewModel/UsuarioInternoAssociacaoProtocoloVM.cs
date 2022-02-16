using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class UsuarioInternoAssociacaoProtocoloVM
    {
        public int? IdUsuarioInterno { get; set; }

        public IEnumerable<int> Protocolos { get; set; }
    }
}