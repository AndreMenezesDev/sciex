using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ALI_ARQUIVO_ENVIO")]
	public partial class AliArquivoEnvioEntity : BaseEntity
	{

		public virtual AliArquivoEntity AliArquivo { get; set; }
		public virtual ICollection<AliEntity> Ali { get; set; }
		public virtual ICollection<AliEntradaArquivoEntity> AliEntradaArquivo { get; set; }

		public AliArquivoEnvioEntity()
		{
			Ali = new HashSet<AliEntity>();
			AliEntradaArquivo = new HashSet<AliEntradaArquivoEntity>();
		}


		[Key]
		[Column("AAE_ID")]
		public long IdAliArquivoEnvio { get; set; }

		[Required]
		[StringLength(255)]
		[Column("AAE_DS_NOME_ARQUIVO")]
		public string NomeArquivo { get; set; }

		[Required]		
		[Column("AAE_DH_GERACAO_ARQUIVO")]
		public DateTime DataGeracao { get; set; }

		[Required]
		[Column("AAE_TP_ARQUIVO")]
		public byte TipoArquivo { get; set; }

		[Required]
		[Column("AAE_ST_ENVIO_SISCOMEX")]
		public byte CodigoStatusEnvioSiscomex { get; set; }

		[Required]
		[Column("AAE_QT_PLI")]
		public short QuantidadePLIs { get; set; }

		[Required]
		[Column("AAE_QT_ALI")]
		public short QuantidadeALIs { get; set; }

		[Required]
		[Column("AAE_VL_TOTAL_REAL")]
		public decimal ValorTotalReal { get; set; }

		[Required]
		[Column("AAE_VL_TOTAL_DOLAR")]
		public decimal ValorTotalDolar { get; set; }

		[Required]
		[Column("AAE_QT_TENTATIVA_ENVIO")]
		public byte TentativasEnvio { get; set; }


	}
}