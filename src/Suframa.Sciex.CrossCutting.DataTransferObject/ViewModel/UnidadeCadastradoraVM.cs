using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class UnidadeCadastradoraVM
    {
        public int? Codigo { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public DateTime? DataInclusao { get; set; }

        public string Descricao { get; set; }

        public int? IdUnidadeCadastradora { get; set; }
    }
}