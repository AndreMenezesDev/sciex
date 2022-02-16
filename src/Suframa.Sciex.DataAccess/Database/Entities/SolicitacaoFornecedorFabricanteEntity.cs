using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_FORNECEDOR_FABRICANTE")]
	public partial class SolicitacaoFornecedorFabricanteEntity : BaseEntity
	{

		public virtual SolicitacaoPliMercadoriaEntity SolicitacaoPliMercadoria { get; set; }

		[Key]
		[Column("SFF_ID")]
		public long IdSolicitacaoFornecedorFabricante { get; set; }
		
		[Column("SPM_ID")]
		[ForeignKey(nameof(SolicitacaoPliMercadoria))]
		public long? IdSolicitacaoPliMercadoria { get; set; }
		
		[StringLength(60)]
		[Column("SFF_DS_FORNECEDOR")]
		public string DescricaoFornecedor { get; set; }

		[StringLength(40)]
		[Column("SFF_DS_LOGRADOURO_FORN")]
		public string LogradouroFornecedor { get; set; }

		[StringLength(6)]
		[Column("SFF_NU_FORN")]
		public string NumeroFornecedor { get; set; }

		[StringLength(21)]
		[Column("SFF_DS_COMPLEMENTO_FORN")]
		public string ComplementoFornecedor { get; set; }

		[StringLength(25)]
		[Column("SFF_DS_CIDADE_FORN")]
		public string CidadeFornecedor { get; set; }

		[StringLength(25)]
		[Column("SFF_DS_ESTADO_FORN")]
		public string EstadoFornecedor { get; set; }

		[StringLength(3)]
		[Column("SFF_CO_PAIS_FORN")]
		public string CodigoPaisFornecedor { get; set; }

		[Column("SFF_CO_AUSENCIA_FABRICANTE")]
		public byte? CodigoAusenciaFabricante { get; set; }

		[StringLength(60)]
		[Column("SFF_DS_FABRICANTE")]
		public string DescricaoFabricante { get; set; }

		[StringLength(40)]
		[Column("SFF_DS_LOGRADOURO_FAB")]
		public string LogradouroFabricante { get; set; }

		[StringLength(6)]
		[Column("SFF_NU_FAB")]
		public string NumeroFabricante { get; set; }

		[StringLength(21)]
		[Column("SFF_DS_COMPLEMENTO_FAB")]
		public string ComplementoFabricante { get; set; }

		[StringLength(25)]
		[Column("SFF_DS_CIDADE_FAB")]
		public string CidadeFabricante { get; set; }

		[StringLength(25)]
		[Column("SFF_DS_ESTADO_FAB")]
		public string EstadoFabricante { get; set; }

		[StringLength(3)]
		[Column("SFF_CO_PAIS_FAB")]
		public string CodigoPaisFabricante { get; set; }

	}
}