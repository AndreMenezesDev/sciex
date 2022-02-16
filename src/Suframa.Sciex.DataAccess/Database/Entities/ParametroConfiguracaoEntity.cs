using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PARAMETRO_CONFIGURACAO")]
	public partial class ParametroConfiguracaoEntity : BaseEntity
	{

		public virtual ListaServicoEntity ListaServico { get; set; }

		[Key]
		[Column("PCO_ID")]
		public int IdParametroConfiguracao { get; set; }

		[Required]
		[ForeignKey(nameof(ListaServico))]
		[Column("LSE_ID")]
		public int IdListaServico { get; set; }

		[Required]
		[StringLength(120)]
		[Column("PCO_DS_PARAMETRO")]
		public string Descricao { get; set; }

		[Required]
		[StringLength(120)]
		[Column("PCO_VL_PARAMETRO")]
		public string Valor { get; set; }

		
	}
}
