using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_DI_ENTRADA")]
	public partial class DiEntradaEntity : BaseEntity
	{

		public virtual DiArquivoEntradaEntity DiArquivoEntrada { get; set; }
		public virtual ICollection<ErroProcessamentoEntity> ErroProcessamento { get; set; }

		public DiEntradaEntity()
		{
			ErroProcessamento = new HashSet<ErroProcessamentoEntity>();
		}

		[Key]
		[Column("DIE_ID")]
		public long Id { get; set; }

		[Column("DIE_NU_DI")]
		public string Numero { get; set; }

		[Column("DIE_QT_ADICAO")]
		public string QuantidadeAdicao { get; set; }

		[Column("die_dt_desembaraco")]
		public string DataDesembaraco { get; set; }

		[Column("DIE_DT_REGISTRO")]
		public string DataRegistro { get; set; }

		[Column("DIE_CO_DECLARACAO")]
		public string CodigoDeclaracao { get; set; }

		[Column("DIE_NU_CNPJ")]
		public string Cnpj { get; set; }

		[Column("DIE_CO_URF_ENTRADA_CARGA")]
		public string CodigoUrfEntradaCarga { get; set; }

		[Column("DIE_CO_URF_DESPACHO")]
		public string CodigoUrfDespacho { get; set; }

		[Column("DIE_CO_RECINTO_ALFANDEGA")]
		public string CodigoRecintoAlfandega { get; set; }

		[Column("DIE_CO_VIA_TRANSP_CARGA")]
		public string CodigoViaTransCarga { get; set; }

		[Column("DIE_TP_MULTIMODAL")]
		public string TipoMultimodal { get; set; }

		[Column("DIE_VL_TOTAL_MLE_DOLAR")]
		public string ValorTotalMleDolar { get; set; }

		[Column("DIE_CO_SETOR_ARMAZENA")]
		public string CodigoSetorArmazena { get; set; }

		[Column("DIE_ST")]
		public int? Situacao { get; set; }

		[Column("DIE_VL_TOTAL_MLE_MN")]
		public string ValorTotalMleMn { get; set; }

		[Column("DIE_DH_ENTRADA")]
		public DateTime DataEntrada { get; set; }

		[Column("DAR_ID")]
		[ForeignKey(nameof(DiArquivoEntrada))]
		public long IdDiArquivoEntrada { get; set; }

	}
}