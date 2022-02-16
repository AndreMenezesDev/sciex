using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class SacDto
    {
        /// <summary>Coluna TXS_ANO_DEBITO da tabela CADSUF_TAXA_SERVICO</summary>
        public int? AnoDebito { get; set; }

        /// <summary>Coluna PRT_ANO da tabela CADSUF_PROTOCOLO</summary>
        public int AnoSolicitacao { get; set; }

        public string Cnpj { get; set; }

        /// <summary>Coluna TXS_DT_EXPIRACAO da tabela CADSUF_TAXA_SERVICO</summary>
        public DateTime? DataExpiracao { get; set; }

        public DateTime? DataLiquidacao { get; set; }

        /// <summary>Coluna CES_NO_CPFCNPJ_INSC_CRED da tabela CADSUF_CONTROLE_EXEC_SERVICO</summary>
        public string Inscricao { get; set; }

        public string MensagemRetorno { get; set; }

        /// <summary>Coluna TXS_NU_DEBITO da tabela CADSUF_TAXA_SERVICO</summary>
        public int? NumeroDebito { get; set; }

        /// <summary>Coluna PRT_SEQ da tabela CADSUF_PROTOCOLO</summary>
        public int NumeroSolicitacao { get; set; }

        public int StatusRetorno { get; set; }
    }
}