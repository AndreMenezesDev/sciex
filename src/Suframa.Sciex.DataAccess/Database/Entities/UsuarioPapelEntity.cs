using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_USUARIO_PAPEL")]
    public partial class UsuarioPapelEntity : BaseEntity
    {
        [Column("PAP_ID")]
        [ForeignKey(nameof(Papel))]
        public int IdPapel { get; set; }

        [Column("USI_ID")]
        [ForeignKey(nameof(UsuarioInterno))]
        public int IdUsuarioInterno { get; set; }

        [Key]
        [Column("USP_ID")]
        public int IdUsuarioPapel { get; set; }

        [Column("USP_ST")]
        public bool IsAtivo { get; set; }

        public virtual PapelEntity Papel { get; set; }

        public virtual UsuarioInternoEntity UsuarioInterno { get; set; }
    }
}