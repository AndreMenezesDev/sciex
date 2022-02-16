using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ESTRUTURA_PROPRIA_LE")]
	public class EstruturaPropriaLEEntity : BaseEntity
	{
		public virtual EstruturaPropriaPliEntity EstruturaPropriaPli { get; set; }
		public virtual ICollection<SolicitacaoLeInsumoEntity> listaSolicitacaoLeInsumo { get; set; }
		public virtual ICollection<ErroProcessamentoEntity> listaErroProcessamento { get; set; }

		public EstruturaPropriaLEEntity()
		{
			listaSolicitacaoLeInsumo = new HashSet<SolicitacaoLeInsumoEntity>();
			listaErroProcessamento = new HashSet<ErroProcessamentoEntity>();
		}

		[Key]
		[Column("ESP_ID")]
		[ForeignKey(nameof(EstruturaPropriaPli))]
		public long IdEstruturaPropria { get; set; }

		[Required]
		[Column("ELE_CO_PRODUTO")]
		public decimal CodigoProduto { get; set; }

		[Required]
		[Column("ELE_DS_PRODUTO")]
		[StringLength(255)]
		public string DescricaoProduto { get; set; }

		[Required]
		[Column("ELE_CO_TIPO_PROD")]
		public decimal CodigoTipoProduto { get; set; }

		[Required]
		[Column("ELE_DS_TIPO_PROD")]
		[StringLength(255)]
		public string DescricaoTipoProduto { get; set; }


		[Required]
		[Column("ELE_CO_NCM")]
		[StringLength(8)]
		public string CodigoNCM { get; set; }

		[Required]
		[Column("ELE_DS_NCM")]
		[StringLength(120)]
		public string DescricaoNCM { get; set; }

		[Required]
		[Column("ELE_CO_UNID_MEDIDA")]
		public short CodigoUnidadeMedida { get; set; }

		[Required]
		[Column("ELE_DS_MODELO")]
		[StringLength(255)]
		public string DescricaoModelo { get; set; }

		[Column("ELE_CO_MODELO_EMPRESA")]
		[StringLength(30)]
		public string CodigoModeloEmpresa { get; set; }

		[Column("ELE_DS_CENTRO_CUSTO")]
		[StringLength(10)]
		public string DescricaoCentroCusto { get; set; }
	}
}