using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_MUNICIPIO")]
    public partial class MunicipioEntity : BaseEntity
    {
        public virtual ICollection<CepEntity> Cep { get; set; }

        [Column("MUN_CO", TypeName = "numeric")]
        public decimal Codigo { get; set; }

        [Column("MUN_CO_UF")]
        public int? CodigoUF { get; set; }

        [Required]
        [StringLength(100)]
        [Column("MUN_DS")]
        public string Descricao { get; set; }

        [Key]
        [Column("MUN_ID")]
        public int IdMunicipio { get; set; }

        [Required]
        [StringLength(2)]
        [Column("MUN_SG_UF")]
        public string SiglaUF { get; set; }

        public virtual ICollection<UnidadeCadastradoraEntity> UnidadeCadastradora { get; set; }

        public virtual ICollection<UnidadeSecundariaEntity> UnidadeSecundaria { get; set; }

        public MunicipioEntity()
        {
            Cep = new HashSet<CepEntity>();
            UnidadeCadastradora = new HashSet<UnidadeCadastradoraEntity>();
            UnidadeSecundaria = new HashSet<UnidadeSecundariaEntity>();
        }
    }
}