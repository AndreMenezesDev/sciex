using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_QUADRO_PESSOAL")]
	public partial class QuadroPessoalEntity : BaseEntity
	{
		/// <summary>Alterado de Arquivo Entity para n?o carregar o bin?rio</summary>
		public virtual ArquivoInformacaoEntity Arquivo { get; set; }

		[Column("QPE_CO_CPF")]
		[StringLength(11)]
		public string Cpf { get; set; }

		[Column("ARQ_ID")]
		[ForeignKey(nameof(Arquivo))]
		public int? IdArquivo { get; set; }

		[Column("PJU_ID")]
		[ForeignKey(nameof(PessoaJuridica))]
		public int IdPessoaJuridica { get; set; }

		[Key]
		[Column("QPE_ID")]
		public int IdQuadroPessoal { get; set; }

		[Required]
		[StringLength(100)]
		[Column("QPE_NO")]
		public string Nome { get; set; }

		[StringLength(100)]
		[Column("QPE_NO_SOCIAL")]
		public string NomeSocial { get; set; }

		public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }
	}
}