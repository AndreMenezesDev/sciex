using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LISTA_SERVICO")]
	public partial class ListaServicoEntity : BaseEntity
	{

		[Key]
		[Column("LSE_ID")]
		public int IdListaServico { get; set; }

		[Required]
		[StringLength(120)]
		[Column("LSE_DS")]
		public string Descricao { get; set; }


		
	}
}
