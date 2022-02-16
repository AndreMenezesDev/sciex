using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_LISTA_DOCUMENTO")]
    public partial class ListaDocumentoEntity : BaseEntity
    {
        [Key]
        [Column("LDO_ID")]
        public int IdListaDocumento { get; set; }

        [Column("TDO_ID")]
        [ForeignKey(nameof(TipoDocumento))]
        public int IdTipoDocumento { get; set; }

        [Column("TRE_ID")]
        [ForeignKey(nameof(TipoRequerimento))]
        public int IdTipoRequerimento { get; set; }

        public virtual TipoDocumentoEntity TipoDocumento { get; set; }

        public virtual TipoRequerimentoEntity TipoRequerimento { get; set; }
    }
}