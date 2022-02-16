using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class ConsultaProtocoloResultadoVM
    {
        public int? Ano { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string DescricaoOrientacaoStatusProtocolo { get; set; }
        public string DescricaoServico { get; set; }
        public string DescricaoStatusProtocolo { get; set; }
        public int? IdProtocolo { get; set; }
        public int? IdServico { get; set; }
        public EnumStatusProtocolo IdStatusProtocolo { get; set; }
        public bool IsEditar { get; set; }
        public bool IsEnviarRecurso { get; set; }
        public int? NumeroSequencial { get; set; }
        public IEnumerable<WorkflowProtocoloVM> Workflows { get; set; }
    }
}