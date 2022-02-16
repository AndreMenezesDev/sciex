using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class InscricaoCadastralHistoricoSituacaoInscricaoVM
    {
        public int? IdHistoricoSituacaoInscricao { get; set; }

        public int? IdInscricaoCadastral { get; set; }

        public int? IdMotivoSituacaoInscricao { get; set; }

        public bool? IsBloqueio { get; set; }

        public string Justificativa { get; set; }
    }
}