using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCSolicDetalheVM : PagedOptions
	{
		public int Id { get; set; }
		public int Status { get; set; }
		public string DescricaoDe { get; set; }
		public string DescricaoPara { get; set; }
		public int IdInsumo { get; set; }
		public int? IdDetalheInsumo { get; set; }
		public int? IdSolicitacaoAlteracao { get; set; }
		public int IdTipoSolicitacao { get; set; }
		public string Justificativa { get; set; }
		public PRCSolicitacaoAlteracaoVM SolicitacaoAlteracao { get; set; }
		public virtual TipoSolicAlteracaoVM TipoSolicAlteracao { get; set; }


		//complementos
		public int CodigoDetalheInsumo { get; set; }
		public int CodigoInsumo { get; set; }
		public string DescricaoStatus { get; set; }
		public string DescricaoInsumo { get; set; }
		public string DescricaoTipoAlteracao { get; set; }
		public int Aprovacao { get; set; }
		public bool IsTransferenciaOuInsumoNovo { get; set; }
	}
}
