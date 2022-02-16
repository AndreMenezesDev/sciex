using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_USUARIO_SETOR")]
    public partial class UsuarioInternoSetorEntity
    {
        [Column("USE_CO_SETOR")]
        public int? CodigoSetor { get; set; }

        [Column("USE_DS_SETOR")]
        [StringLength(200)]
        public string DescricaoSetor { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        [Key]
        [Column("USE_ID")]
        public int IdUsuarioInternoSetor { get; set; }

        public virtual ICollection<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; set; }

        [Column("USE_SG")]
        [StringLength(20)]
        public string SiglaSetor { get; set; }

        [Column("USE_ST_SETOR")]
        public int? Status { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public virtual ICollection<UsuarioInternoEntity> UsuarioInterno { get; set; }

        public UsuarioInternoSetorEntity()
        {
            MotivoSituacaoInscricao = new HashSet<MotivoSituacaoInscricaoEntity>();

            UsuarioInterno = new HashSet<UsuarioInternoEntity>();
        }
    }
}