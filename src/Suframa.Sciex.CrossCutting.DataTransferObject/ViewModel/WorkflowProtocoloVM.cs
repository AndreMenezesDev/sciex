using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class WorkflowProtocoloVM
    {
        public DateTime? Data { get; set; }

        public DateTime? DataNotificacao { get; set; }

        public string DescricaoStatusProtocolo { get; set; }

        public int? IdProtocolo { get; set; }

        public EnumStatusProtocolo IdStatusProtocolo { get; set; }

        public int? IdUsuarioInterno { get; set; }

        public int? IdWorkflowProtocolo { get; set; }

        public string Justificativa { get; set; }

        public string NomeUsuarioInterno { get; set; }

        public virtual IEnumerable<TipoDocumentoVM> TiposDocumentos { get; set; }
    }
}