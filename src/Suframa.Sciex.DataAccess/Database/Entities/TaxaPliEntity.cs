using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_TAXA_PLI")]
	public partial class TaxaPliEntity : BaseEntity
	{
		public virtual PliAplicacaoEntity PliAplicacao { get; set; }
		public virtual PliEntity Pli { get; set; }
		public virtual TaxaFatoGeradorEntity TaxaFatoGerador { get; set; }		

		public virtual ICollection<TaxaPliMercadoriaEntity> TaxaPliMercadoria { get; set; }
		public virtual ICollection<TaxaPliDebitoEntity> TaxaPliDebito { get; set; }

		public TaxaPliEntity()
		{
			TaxaPliMercadoria = new HashSet<TaxaPliMercadoriaEntity>();
			TaxaPliDebito = new HashSet<TaxaPliDebitoEntity>();
		}


		[Key]
		[Column("PLI_ID")]
		[ForeignKey(nameof(Pli))]
		public long IdPli { get; set; }
		
		[Column("TFG_ID")]
		[ForeignKey(nameof(TaxaFatoGerador))]
		public int? IdTaxaFatoGerador { get; set; }

		[Column("TPL_VL_BASE_FATO_GERADOR")]
		public decimal? ValorBaseFatoGerador { get; set; }

		[Column("TPL_VL_PERC_LIMITADOR_PLI")]
		public decimal? ValorPercentualLimitadorPli { get; set; }

		[Column("TPL_DH_ENVIO_SAC")]
		public DateTime? DataEnvioSac { get; set; }

		[Column("TPL_CO_LOCALIDADE")]
		public int? CodigoLocalidade { get; set; }

		[Column("TPL_CD_MUNICIPIO")]
		public int? CodigoMunicipio { get; set; }

		[Column("TPL_NU_PROCESSO_PEXPAM")]
		public int? NumeroProcessoPEXPAM { get; set; }

		[Column("TPL_NU_ANO_PROCESSO_PEXPAM")]
		public short? AnoProcessoPEXPAM { get; set; }

		[Column("TPL_VL_TOTAL_MERCADORIA_REAIS")]
		public decimal? ValorTotalMercadoriaReais { get; set; }

		[Column("TPL_VL_TOTAL_CALC_LIMITADOR_PLI")]
		public decimal? ValorTotalCalculadoLimitadorPLI { get; set; }

		[Column("TPL_VL_PREVALENCIA_PLI")]
		public decimal? ValorPrevalenciaPLI { get; set; }

		[Column("TPL_VL_PERC_REDUCAO_PLI")]
		public decimal? ValorPercentualReducaoPLI { get; set; }

		[Column("TPL_VL_REDUCAO_PLI")]
		public decimal? ValorReducaoPLI { get; set; }

		[Column("TPL_VL_TCIF_PLI")]
		public decimal? ValorTCIFPLI { get; set; }

		[Column("TPL_VL_REDUCAO_BASE_PLI")]
		public decimal? ValorReducaoBasePLI { get; set; }

		[Column("TPL_VL_TOTAL_REDUCAO_ITENS")]
		public decimal? ValorTotalReducaoItens { get; set; }

		[Column("TPL_VL_TOTAL_REDUCAO_BASE_ITENS")]
		public decimal? ValorTotalReducaoBaseItens { get; set; }

		[Column("TPL_VL_GERAL_REDUCAO_TCIF")]
		public decimal? ValorGeralReducaoTCIF { get; set; }

		[Column("TPL_VL_GERAL_TCIF")]
		public decimal? ValorGeralTCIF { get; set; }

		[Column("TPL_VL_GERAL_REDUCAO_BASE")]
		public decimal? ValorGeralReducaoBase { get; set; }

		[Column("TPL_NU_CNPJ")]
		[StringLength(14)]
		public string CNPJ { get; set; }

		[Column("TPL_NU_SERVICO_SAC")]
		public short? NumeroServicoSAC { get; set; }

		[Column("PAP_ID")]
		[ForeignKey(nameof(PliAplicacao))]
		public int? IdPliAplicacao { get; set; }

		[Column("TGB_ID_ISENCAO")]
		public int? Isencao { get; set; }

		[Column("TGB_ID_REDUCAO")]
		public int? Reducao { get; set; }

		[Column("TPL_VL_TOTAL_TCIF_ITENS")]
		public decimal? ValorTotalTCIFItens { get; set; }

		[Required]
		[Column("TPL_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }

		[Required]
		[Column("TPL_ST_TAXA")]
		public int StatusTaxa { get; set; }

		[Column("TPL_DH_ENVIO_PLI")]
		public DateTime? DataEnvioPLI { get; set; }
	}
}