using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PRC_SOLIC_ALTERACAO")]
	public class PRCSolicitacaoAlteracaoEntity : BaseEntity
	{
		public virtual ProcessoEntity Processo { get; set; }	
		public virtual ICollection<PRCSolicDetalheEntity> ListaSolicDetalhe { get; set; }
		public virtual ICollection<ParecerTecnicoEntity> ListaParecerTecnico { get; set; }
		public virtual ICollection<PRCStatusEntity> ListaStatus { get; set; }
		public virtual ICollection<PRCInsumoEntity> ListaInsumos { get; set; }
		public virtual ICollection<PRCHistoricoInsumoEntity> ListaHistoricoInsumo { get; set; }


		public PRCSolicitacaoAlteracaoEntity()
		{
			ListaSolicDetalhe = new HashSet<PRCSolicDetalheEntity>();
			ListaStatus = new HashSet<PRCStatusEntity>();
			ListaInsumos = new HashSet<PRCInsumoEntity>();
			ListaHistoricoInsumo = new HashSet<PRCHistoricoInsumoEntity>();
			ListaParecerTecnico = new HashSet<ParecerTecnicoEntity>();
		}

		[Key]
		[Column("SOA_ID")]
		public int Id { get; set; }

		[ForeignKey(nameof(Processo))]
		[Column("PRC_ID")]
		public int? IdProcesso { get; set; }

		[Column("SOA_NU")]
		public int? NumeroSolicitacao { get; set; }

		[Column("SOA_NU_ANO")]
		public int? AnoSolicitacao { get; set; }

		[Column("SOA_ST")]
		public int? Status { get; set; }

		[Column("SOA_DT_INCLUSÃO")]
		public DateTime? DataInclusao { get; set; }
		
		[Column("soa_nu_cfp_responsavel")]
		public string CpfResponsavel { get; set; }

		[Column("soa_no_responsavel")]
		public string NomeResponsavel { get; set; }
		
		[Column("SOA_DT_ALTERACAO")]
		public DateTime? DataAlteracao { get; set; }

		[Column("SOA_NU_CNPJ")]
		public string Cnpj { get; set; }
		
		[Column("SOA_DS_RAZAO_SOCIAL")]
		public string RazaoSocial { get; set; }

		[Column("SOA_VL_TOTAL_ACRESCIMO")]
		public decimal? AcrescimoSolicitacao { get; set; }
	}
}
