using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VM_CADSUF_DOCUMENTO")]
	public class VWDocumentoEntity : BaseEntity
	{
		[Column("TDO_CO")]
		public string CodigoTipoDocumento { get; set; }

		[Column("DT_EXPEDICAO")]
		public DateTime? DataExpedicao { get; set; }

		[Column("DT_VENCIMENTO")]
		public DateTime? DataVencimento { get; set; }

		[Column("DS_ORIGEM")]
		public string DescricaoOrigem { get; set; }

		[Column("TDO_DS")]
		public string DescricaoTipoDocumento { get; set; }

		[Column("HR_EXPEDICAO")]
		public string HoraExpedicao { get; set; }

		[Key]
		[Column("ARQ_ID")]
		public int? IdArquivo { get; set; }

		[Column("PFI_ID")]
		public int? IdPessoaFisica { get; set; }

		[Column("PJU_ID")]
		public int? IdPessoaJuridica { get; set; }

		[Column("PRT_ID")]
		public int? IdProtocolo { get; set; }

		[Column("REQ_ID")]
		public int? IdRequerimento { get; set; }

		[Column("RDO_ID")]
		public int? IdRequerimentoDocumento { get; set; }

		[Column("TDO_ID")]
		public int? IdTipoDocumento { get; set; }

		[Column("TRE_ID")]
		public int? IdTipoRequerimento { get; set; }

		[Column("ARQ_NO")]
		public string NomeArquivo { get; set; }

		[Column("NU_CERTIDAO")]
		public string NumeroCertidao { get; set; }

		[Column("RDO_ST")]
		public string Status { get; set; }
	}
}