using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class UsuarioInternoPapelVM : PagedOptions
    {
        public string Cpf { get; set; }

        public int? IdPapel { get; set; }

        public int? IdUnidadeCadastradora { get; set; }
        public int? IdUsuarioInterno { get; set; }
        public int? IdUsuarioInternoPapel { get; set; }

        public bool? IsAtivo { get; set; }

        public string Nome { get; set; }

        public string Papel { get; set; }

        public int? QuantidadeProtocolos { get; set; }
    }
}