using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_ARQUIVO")]
    public partial class ArquivoEntity : BaseEntity
    {
        [Required]
        [Column("ARQ_BI")]
        public byte[] Arquivo { get; set; }

        public virtual ICollection<ConsultaPublicaEntity> ConsultaPublica { get; set; }

        public virtual ICollection<DiligenciaAnexosEntity> DiligenciaAnexos { get; set; }

        [Key]
        [Column("ARQ_ID")]
        public int IdArquivo { get; set; }

        [StringLength(256)]
        [Column("ARQ_NO")]
        public string Nome { get; set; }

        public virtual ICollection<QuadroPessoalEntity> QuadroPessoal { get; set; }

        public virtual ICollection<RecursoEntity> Recurso { get; set; }

        public virtual ICollection<RequerimentoDocumentoEntity> RequerimentoDocumento { get; set; }

        [Column("ARQ_NU_TAMANHO")]
        public double? Tamanho { get; set; }

        [StringLength(20)]
        [Column("ARQ_TP")]
        public string Tipo { get; set; }

        public ArquivoEntity()
        {
            QuadroPessoal = new HashSet<QuadroPessoalEntity>();
            RequerimentoDocumento = new HashSet<RequerimentoDocumentoEntity>();
            ConsultaPublica = new HashSet<ConsultaPublicaEntity>();
            Recurso = new HashSet<RecursoEntity>();
            DiligenciaAnexos = new HashSet<DiligenciaAnexosEntity>();
        }
    }
}