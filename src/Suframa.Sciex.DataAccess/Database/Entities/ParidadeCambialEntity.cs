using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PARIDADE_CAMBIAL")]
	public class ParidadeCambialEntity : BaseEntity
	{
		public virtual ICollection<ParidadeValorEntity> ParidadeValor { get; set; }

		public ParidadeCambialEntity(){

			ParidadeValor = new HashSet<ParidadeValorEntity>();
		}

		[Key]
		[Column("PCA_ID")]
		public int IdParidadeCambial { get; set; }

		[Required]
		[Column("PCA_DT_PARIDADE")]
		public DateTime DataParidade { get; set; }

		[Required]
		[Column("PCA_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }

		[Required]
		[Column("PCA_DT_ARQUIVO")]
		public DateTime DataArquivo { get; set; }

		[Required]
		[StringLength(14)]
        [Column("PCA_NU_USUARIO")]
        public string NumeroUsuario { get; set; }

		[Required]
		[StringLength(120)]
		[Column("PCA_NO_USUARIO")]
		public string NomeUsuario { get; set; }
    }
}