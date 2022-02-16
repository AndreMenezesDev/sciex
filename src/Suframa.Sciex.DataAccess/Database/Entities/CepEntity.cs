using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CEP")]
    public partial class CepEntity : BaseEntity, IData
    {
        [Required]
        [StringLength(100)]
        [Column("CEP_DS_BAIRRO")]
        public string Bairro { get; set; }

        [Column("CEP_CO", TypeName = "numeric")]
        public decimal Codigo { get; set; }

        [Column("CEP_DT_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("CEP_DT_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [Required]
        [StringLength(200)]
        [Column("CEP_DS_ENDERECO")]
        public string Endereco { get; set; }

        [Key]
        [Column("CEP_ID")]
        public int IdCep { get; set; }

        [Column("MUN_ID")]
        [ForeignKey(nameof(Municipio))]
        public int IdMunicipio { get; set; }

        [Required]
        [StringLength(100)]
        [Column("CEP_TP_LOGRADOURO")]
        public string Logradouro { get; set; }

        public virtual MunicipioEntity Municipio { get; set; }

        public virtual ICollection<PessoaFisicaEntity> PessoaFisica { get; set; }

        public virtual ICollection<PessoaJuridicaEntity> PessoaJuridica { get; set; }

        public CepEntity()
        {
            PessoaFisica = new HashSet<PessoaFisicaEntity>();
            PessoaJuridica = new HashSet<PessoaJuridicaEntity>();
        }
    }
}