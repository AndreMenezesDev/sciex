using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_FORNECEDOR")]
	public partial class FornecedorEntity : BaseEntity
	{
		
		public virtual ICollection<ParametrosEntity> Parametros { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PliMercadoria { get; set; }

		[Key]
		[Column("FOR_ID")]
		public int IdFornecedor { get; set; }

		[Required]
		[StringLength(25)]
		[Column("FOR_DS_CIDADE")]
		public string Cidade { get; set; }

		[StringLength(3)]
		[Column("PAI_CO")]
		public string CodigoPais { get; set; }

		[StringLength(50)]
		[Column("PAI_DS")]
		public string DescricaoPais { get; set; }

		[StringLength(21)]
		[Column("FOR_DS_COMPLEMENTO")]
		public string Complemento { get; set; }

		[Required]
		[StringLength(25)]
		[Column("FOR_DS_ESTADO")]
		public string Estado { get; set; }

		[Required]
		[StringLength(40)]
		[Column("FOR_DS_LOGRADOURO")]
		public string Logradouro { get; set; }

		[Required]
		[StringLength(6)]
		[Column("FOR_NU")]
		public string Numero { get; set; }

		[Required]
		[StringLength(60)]
		[Column("FOR_DS_RAZAO_SOCIAL")]
		public string RazaoSocial { get; set; }

		[StringLength(14)]
		[Column("IMP_NU_CNPJ")]
		public string CNPJImportador { get; set; }

		[Column("FOR_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		[Required]
		[Column("FOR_CO")]
		public int Codigo { get; set; }

		public FornecedorEntity()
		{
			Parametros = new HashSet<ParametrosEntity>();
			PliMercadoria = new HashSet<PliMercadoriaEntity>();
		}
	}
}