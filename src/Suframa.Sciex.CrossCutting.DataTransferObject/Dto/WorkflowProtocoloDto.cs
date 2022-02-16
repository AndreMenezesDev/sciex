using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class WorkflowProtocoloDto
    {
        /// <summary>Primeira data com status aguardando reanálise na tabela workflow</summary>
        public DateTime? DataAguardandoReanalise { get; set; }

        /// <summary>Data atual</summary>
        public DateTime? DataAtual { get; set; }

        /// <summary>Primeira data com status com pendência na tabela workflow</summary>
        public DateTime? DataComPendencia { get; set; }

        /// <summary>
        /// Última data com status aguardando conferência administrativa na tabela workflow
        /// </summary>
        public DateTime? DataConferenciaAdministrativa { get; set; }

        /// <summary>Primeira data aguardando analise na tabela workflow</summary>
        public DateTime? DataDesignacao { get; set; }

        /// <summary>Última data com status em analise na tabela workflow</summary>
        public DateTime? DataEmAnalise { get; set; }

        /// <summary>Protocolo.Requerimento.TipoRequerimento.Servico.QuantidadeDiasAnalise</summary>
        public int? QuantidadeDiasAnalise { get; set; }
    }
}