using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_PRJ_DETALHE_MERCADORIA")]
    public partial class ViewDetalheMercadoriaEntity : BaseEntity
    {
		[Key]
		[Column("DME_ID")]
		public int IdDetalheMercadoria { get; set; }

		[Required]
		[Column("DME_CO_PRODUTO")]
        public short CodigoProduto { get; set; }

		[Required]
		[StringLength(8)]
		[Column("DME_CO_NCM_MERCADORIA")]
		public string CodigoNCMMercadoria { get; set; }

		[Required]
		[Column("DME_CO_DETALHE_MERCADORIA")]
		public int CodigoDetalheMercadoria { get; set; }

		[Required]
		[StringLength(255)]
		[Column("DME_DS_DETALHE_MERCADORIA")]
		public string Descricao { get; set; }

		[Required]
		[Column("DME_ST_DETALHE_MERCADORIA")]
		public short StatusDetalheMercadoria { get; set; }

		[Required]
		[Column("DME_ST_ITEM_CRITICO")]
		public short StatusItemCritico { get; set; }

	}
}