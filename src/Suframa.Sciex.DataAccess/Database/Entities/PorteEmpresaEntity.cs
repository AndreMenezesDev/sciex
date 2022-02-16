using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PORTE_EMPRESA")]
    public partial class PorteEmpresaEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column("PEM_DS")]
        public string Descricao { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("PEM_ID")]
        public int IdPorteEmpresa { get; set; }

        public virtual ICollection<PessoaJuridicaEntity> PessoaJuridica { get; set; }

        public PorteEmpresaEntity()
        {
            PessoaJuridica = new HashSet<PessoaJuridicaEntity>();
        }
    }
}