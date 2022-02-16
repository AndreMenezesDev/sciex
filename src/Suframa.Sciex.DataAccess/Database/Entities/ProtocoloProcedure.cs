using System;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	public class ProtocoloProcedure
	{
		public int? ANO_PROTOCOLO { get; set; }
		public string CPF_CNPJ { get; set; }
		public DateTime? DATA_DESIGNACAO { get; set; }
		public DateTime? DATA_DESIGNACAO_FINAL { get; set; }
		public DateTime? DATA_DESIGNACAO_INICIAL { get; set; }
		public DateTime? DATA_PROTOCOLO { get; set; }
		public DateTime? DATA_PROTOCOLO_FINAL { get; set; }
		public DateTime? DATA_PROTOCOLO_INICIAL { get; set; }
		public int? DIAS_RESTANTES { get; set; }
		public int? ID_RESPONSAVEL { get; set; }
		public int? ID_SITUACAO_PROTOCOLO { get; set; }
		public int? ID_TIPO_SERVICO { get; set; }
		public int? ID_UNIDADE { get; set; }
		public bool? IS_COCAD { get; set; }
		public bool? IS_GRUPOPROTOCOLOANALISE { get; set; }
		public string NOME_RAZAOSOCIAL { get; set; }
		public int? NUMERO_PROTOCOLO { get; set; }
		public int? PAGE { get; set; }
		public int? PRT_ID { get; set; }
		public int? PRT_NU_ANO { get; set; }
		public int? PRT_NU_SEQ { get; set; }
		public bool? REVERSE { get; set; }
		public int? SIZE { get; set; }
		public string SORT { get; set; }
		public string SPR_DS { get; set; }
		public int? SPR_ID { get; set; }
		public string SRV_DS { get; set; }
		public int? TOTAL { get; set; }
		public string USI_NO { get; set; }
	}
}