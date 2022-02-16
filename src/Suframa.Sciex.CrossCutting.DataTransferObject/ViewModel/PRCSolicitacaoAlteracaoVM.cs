using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCSolicitacaoAlteracaoVM : PagedOptions
	{
		public int Id { get; set; }
		public int? NumeroSolicitacao { get; set; }
		public int? AnoSolicitacao { get; set; }
		public DateTime? DataInclusao { get; set; }
		public int? Status { get; set; }
		public string CpfResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public ProcessoExportacaoVM ProcessoVM { get; set; }
		public List<PRCSolicDetalheVM> ListaSolicDetalhe { get; set; }
		public int? IdProcesso { get; set; }
		public DateTime? DataAlteracao { get; set; }
		public string Cnpj { get; set; }
		public string RazaoSocial { get; set; }
		public decimal? AcrescimoSolicitacao { get; set; }


		// Complemento classe
		public string DataInclusaoFormatada { get; set; }
		public string DataAlteracaoFormatada { get; set; }
		public int QuantidaDeItens { get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public string DescricaoStatus { get; set; }
		public int NumeroProcesso { get; set; }
		public int AnoProcesso { get; set; }
		public string NumeroAnoSolicitacaoFormatado { get; set; }
		public string NumeroAnoProcessoFormatado { get; set; }
		public string AcrescimoSolicitacaoFormatado { get; set; }
		public int? IdAnalistaDesignado { get; set; }

		public int DefinidorGrid {get; set; }
		public string Descricao { get; set; }
		public int IdProcessoProduto { get; set; }
		public string MensagemErro { get; set; }
		public int IdSolicitacao { get; set; }
		public ParecerTecnicoVM dadosParecer { get; set; }

	}
}
