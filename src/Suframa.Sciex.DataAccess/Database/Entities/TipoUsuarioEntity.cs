using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_TIPO_USUARIO")]
    public class TipoUsuarioEntity : BaseEntity
    {
        public virtual ICollection<CredenciamentoEntity> Credenciamento { get; set; }

        [Required]
        [StringLength(200)]
        [Column("TUS_DES")]
        public string Descricao { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("TUS_ID")]
        public int IdTipoUsuario { get; set; }

        public virtual ICollection<TipoRequerimentoEntity> TipoRequerimento { get; set; }

        public TipoUsuarioEntity()
        {
            Credenciamento = new HashSet<CredenciamentoEntity>();
            TipoRequerimento = new HashSet<TipoRequerimentoEntity>();
        }
    }
}