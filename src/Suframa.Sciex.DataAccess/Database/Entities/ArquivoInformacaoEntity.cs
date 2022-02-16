using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_CADSUF_ARQUIVO")]
    public partial class ArquivoInformacaoEntity : BaseEntity, IData
    {
        [Column("ARQ_DT_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("ARQ_DT_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [Key]
        [Column("ARQ_ID")]
        public int IdArquivo { get; set; }

        [StringLength(256)]
        [Column("ARQ_NO")]
        public string Nome { get; set; }

        [Column("ARQ_NU_TAMANHO")]
        public double? Tamanho { get; set; }

        [StringLength(20)]
        [Column("ARQ_TP")]
        public string Tipo { get; set; }
    }
}