using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumStatusProtocolo
    {
        [Description("Gerado")]
        Gerado = 1,

        [Description("Aguardando Pagamento")]
        AguardandoPagamento = 2,

        [Description("Pronto para Analise")]
        ProntoParaAnalise = 3,

        [Description("Pagamento Recebido")]
        PagamentoRecebido = 4,

        [Description("Aguardando Análise")]
        AguardandoAnalise = 5,

        [Description("Em Análise")]
        EmAnalise = 6,

        [Description("Com Pendência")]
        ComPendencia = 7,

        [Description("Aguardando Conferencia Documental")]
        AguardandoConferenciaDocumental = 8,

        [Description("Aguardando Reanalise")]
        AguardandoReanalise = 9,

        [Description("Aguardando Visita Técnica")]
        AguardandoVisitaTecnica = 10,

        [Description("Deferido")]
        Deferido = 11,

        [Description("Indeferido - Aguardando Recurso")]
        IndeferidoAguardandoRecurso = 12,

        [Description("Cancelado")]
        Cancelado = 13,

        [Description("Recurso em Análise")]
        RecursoEmAnalise = 14,

        [Description("Indeferido")]
        Indeferido = 15,

        [Description("Em julgamento")]
        EmJulgamento = 16
    }
}