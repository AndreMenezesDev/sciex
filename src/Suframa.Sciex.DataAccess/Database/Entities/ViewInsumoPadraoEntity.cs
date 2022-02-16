using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PRJ_INSUMO_PADRAO")]
	public partial class ViewInsumoPadraoEntity : BaseEntity
	{
		[Key]
		[Column("DME_ID")]
		public int IdInsumoPadrao { get; set; }

		[Column("DME_CO_PRODUTO")]
		public short CodigoProduto { get; set; }

		[Column("DME_CO_NCM_MERCADORIA")]
		[StringLength(8)]
		public string CodigoNCMMercadoria { get; set; }

		[Column("DME_CO_DETALHE_MERCADORIA")]
		public int CodigoDetalheMercadoria { get; set; }

		[Column("DME_DS_DETALHE_MERCADORIA")]
		[StringLength(255)]
		public string DescricaoDetalheMercadoria { get; set; }

		[Column("UME_ID")]
		public int IdUnidadeMedida { get; set; }

		[Column("UME_CO")]
		public int CodigoUnidadeMedida { get; set; }

		[Column("UME_DS")]
		[StringLength(40)]
		public string DescricaoUnidadeMedida { get; set; }

		[Column("UME_SG")]
		[StringLength(5)]
		public string SiglaUnidadeMedida { get; set; }
	}
}