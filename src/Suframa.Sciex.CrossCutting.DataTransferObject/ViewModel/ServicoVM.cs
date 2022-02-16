using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ServicoVM
    {
        public int? Codigo { get; set; }

        public int? CodigoSac { get; set; }

        public string Descricao { get; set; }

        public int? IdServico { get; set; }

        public int? QuantidadeDiasAnalise { get; set; }

        public bool Status { get; set; }

        public IEnumerable<TipoRequerimentoVM> TipoRequerimento { get; set; }

        public EnumTipoRequerimentoGrupo? TipoRequerimentoGrupo { get; set; }

        public double? ValorTaxa { get; set; }
    }
}