using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class WorkflowMensagemPadraoVM
    {
        public string Descricao { get; set; }
        public int? IdMensagemPadrao { get; set; }
        public int? IdWorkflowMensagemPadrao { get; set; }
        public int? IdWorkflowProtocolo { get; set; }
    }
}