using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_QUADRO_PESSOAL_APROVADO")]
	public partial class VWQuadroPessoalAprovadoEntity : BaseEntity
	{
		/// <summary>Alterado de Arquivo Entity para não carregar o binário</summary>
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

		public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }
	}
}