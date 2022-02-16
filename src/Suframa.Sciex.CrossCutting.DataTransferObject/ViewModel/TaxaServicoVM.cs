using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class TaxaServicoVM
    {
        public int? AnoDebito { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string DescricaoMsgRetorno { get; set; }
        public string Extrato { get; set; }
        public int? IdProtocolo { get; set; }
        public int? IdTaxaServico { get; set; }
        public int? NumeroDebito { get; set; }
        public int? Status { get; set; }
    }
}