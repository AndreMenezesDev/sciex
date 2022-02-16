using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_FORNECEDOR_FABRICANTE")]
	public class PliFornecedorFabricanteEntity : BaseEntity
	{
		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		
		[Key]	
		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long IdPliMercadoria { get; set; }

		[StringLength(60)]
        [Column("PFF_DS_FORNECEDOR")]
        public string DescricaoFornecedor { get; set; }

		[StringLength(40)]
		[Column("PFF_DS_LOGRADOURO_FORN")]
        public string DescricaoLogradouroFornecedor { get; set; }

		[StringLength(6)]
		[Column("PFF_NU_FORN")]
        public string NumeroFornecedor { get; set; }

		[StringLength(21)]
		[Column("PFF_DS_COMPLEMENTO_FORN")]
		public string DescricaoComplementoFornecedor { get; set; }

		[StringLength(25)]
		[Column("PFF_DS_CIDADE_FORN")]
		public string DescricaoCidadeFornecedor { get; set; }

		[StringLength(25)]
		[Column("PFF_DS_ESTADO_FORN")]
		public string DescricaoEstadoFornecedor { get; set; }

		[StringLength(3)]
		[Column("PFF_CO_PAIS_FORN")]
		public string CodigoPaisFornecedor { get; set; }

		[StringLength(50)]
		[Column("PFF_DS_PAIS_FORN")]
		public string DescricaoPaisFornecedor { get; set; }

		[Column("PFF_CO_AUSENCIA_FABRICANTE")]
		public byte? CodigoAusenciaFabricante { get; set; }

		[StringLength(60)]
		[Column("PFF_DS_FABRICANTE")]
		public string DescricaoFabricante { get; set; }

		[StringLength(40)]
		[Column("PFF_DS_LOGRADOURO_FAB")]
		public string DescricaoLogradouroFabricante { get; set; }

		[StringLength(6)]
		[Column("PFF_NU_FAB")]
		public string NumeroFabricante { get; set; }

		[StringLength(21)]
		[Column("PFF_DS_COMPLEMENTO_FAB")]
		public string DescricaoComplementoFabricante { get; set; }

		[StringLength(25)]
		[Column("PFF_DS_CIDADE_FAB")]
		public string DescricaoCidadeFabricante { get; set; }

		[StringLength(25)]
		[Column("PFF_DS_ESTADO_FAB")]
		public string DescricaoEstadoFabricante { get; set; }

		[StringLength(3)]
		[Column("PFF_CO_PAIS_FAB")]
		public string CodigoPaisFabricante { get; set; }

		[StringLength(50)]
		[Column("PFF_DS_PAIS_FAB")]
		public string DescricaoPaisFabricante { get; set; }


	}
}