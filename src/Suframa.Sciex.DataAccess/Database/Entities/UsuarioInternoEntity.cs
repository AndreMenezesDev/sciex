using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_USUARIO_INTERNO")]
	public partial class UsuarioInternoEntity : BaseEntity
	{
		[Column("USI_CO_ORIGEM")]
		public int? CodigoOrigem { get; set; }

		[Column("USI_CO_CPF")]
		[StringLength(11)]
		public string Cpf { get; set; }

		[Column("UND_ID")]
		[ForeignKey(nameof(UnidadeCadastradora))]
		public int? IdUnidadeCadastradora { get; set; }

		[Key]
		[Column("USI_ID")]
		public int IdUsuarioInterno { get; set; }

		[Column("USE_ID")]
		[ForeignKey(nameof(UsuarioInternoSetor))]
		public int? IdUsuarioInternoSetor { get; set; }

		[Column("USI_DS_LOGIN")]
		[StringLength(20)]
		public string Login { get; set; }

		[Column("USI_NO")]
		[StringLength(80)]
		public string Nome { get; set; }

		public virtual ICollection<ParametroAnalistaEntity> ParametroAnalista { get; set; }

		public virtual ICollection<ProtocoloEntity> Protocolo { get; set; }

		public virtual ICollection<RecursoEntity> Recurso { get; set; }

		[Column("USI_DS_SETOR")]
		[StringLength(20)]
		public string Setor { get; set; }

		public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

		public virtual ICollection<UsuarioPapelEntity> UsuarioInternoPapel { get; set; }

		public virtual UsuarioInternoSetorEntity UsuarioInternoSetor { get; set; }

		public virtual ICollection<WorkflowProtocoloEntity> WorkflowProtocolo { get; set; }

		public UsuarioInternoEntity()
		{
			ParametroAnalista = new HashSet<ParametroAnalistaEntity>();

			Protocolo = new HashSet<ProtocoloEntity>();

			WorkflowProtocolo = new HashSet<WorkflowProtocoloEntity>();

			UsuarioInternoPapel = new HashSet<UsuarioPapelEntity>();

			Recurso = new HashSet<RecursoEntity>();
		}
	}
}