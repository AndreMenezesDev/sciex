
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PE_INSUMO")]
	public partial class PEInsumoEntity : BaseEntity
	{
		public virtual PEProdutoEntity PlanoExportacaoProduto { get; set; }
		public virtual ICollection<PEDetalheInsumoEntity> ListaPEDetalheInsumo { get; set; }
		public PEInsumoEntity()
		{
			ListaPEDetalheInsumo = new HashSet<PEDetalheInsumoEntity>();
		}

		[Key]
		[Column("INS_ID")]
		public int IdPEInsumo { get; set; }

		[Column("PRO_ID")]
		[ForeignKey(nameof(PlanoExportacaoProduto))]
		public int? IdPEProduto { get; set; }

		[Column("INS_CO_INSUMO")]
		public int CodigoInsumo { get; set; }

		[Column("INS_CO_UNIDADE")]
		public int? CodigoUnidade { get; set; }

		[Column("INS_TP_INSUMO")]
		[StringLength(1)]
		public string TipoInsumo { get; set; }

		[Column("INS_CO_NCM")]
		[StringLength(8)]
		public string CodigoNcm { get; set; }

		[Column("INS_VL_PERCENTUAL_PERDA")]
		public decimal? ValorPercentualPerda { get; set; }

		[Column("INS_CO_DETALHE")]
		public int? CodigoDetalhe { get; set; }

		[Column("INS_DS_INSUMO")]
		[StringLength(255)]
		public string DescricaoInsumo { get; set; }

		[Column("INS_DS_PART_NUMBER")]
		[StringLength(20)]
		public string DescricaoPartNumber { get; set; }

		[Column("INS_DS_ESPECIFICACAO_TECNICA")]
		[StringLength(3723)]
		public string DescricaoEspecificacaoTecnica { get; set; }

		[Column("INS_VL_COEFICIENTE_TECNICO")]
		public decimal? ValorCoeficienteTecnico { get; set; }

		[Column("INS_VL_DOLAR")]
		public decimal? ValorDolar { get; set; }

		[Column("INS_ST_ANALISE")]
		public int? SituacaoAnalise { get; set; }

		[StringLength(1000)]
		[Column("INS_DS_JUSTIFICATIVA_ERRO")]
		public string DescricaoJustificativaErro { get; set; }

	}
}