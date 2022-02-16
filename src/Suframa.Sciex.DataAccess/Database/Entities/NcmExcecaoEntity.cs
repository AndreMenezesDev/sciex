using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_NCM_EXCECAO")]
	public partial class NcmExcecaoEntity : BaseEntity
	{
		[Key]
		[Column("NEX_ID")]
		public int IdNcmExcecao { get; set; }

		[Required]
		[StringLength(8)]
		[Column("NEX_CO_NCM")]
		public string Codigo { get; set; }

		[Required]
		[StringLength(120)]
		[Column("NEX_DS_NCM")]
		public string DescricaoNcm { get; set; }

		[Required]
		[Column("NEX_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("NEX_CO_MUNICIPIO")]
		public int CodigoMunicipio { get; set; }

		[Required]
		[StringLength(100)]
		[Column("NEX_DS_MUNICIPIO")]
		public string DescricaoMunicipio { get; set; }

		[Required]
		[Column("NEX_CO_SETOR")]
		public int CodigoSetor { get; set; }

		[Required]
		[StringLength(200)]
		[Column("NEX_DS_SETOR")]
		public string DescricaoSetor { get; set; }

		[Required]
		[Column("NEX_DT_INICIO_VIGENCIA")]
		public DateTime DataInicioVigencia { get; set; }

		[Column("NEX_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		[Required]
		[StringLength(2)]
		[Column("NEX_SG_UF")]
		public string UF { get; set; }

	}
}