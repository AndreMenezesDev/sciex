using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCDetalheInsumoVM : PagedOptions
	{
		public virtual PRCInsumoVM PrcInsumo { get; set; }
		public virtual MoedaVM Moeda { get; set; }

		public int IdDetalheInsumo { get; set; }
		public int IdPrcInsumo { get; set; }
		public int? IdMoeda { get; set; }
		public int? NumeroSequencial { get; set; }
		public int? CodigoPais { get; set; }
		public decimal Quantidade { get; set; }
		public decimal? ValorUnitario { get; set; }
		public decimal? ValorFrete { get; set; }
		public decimal? ValorDolar { get; set; }
		public decimal? ValorUnitarioCFR { get; set; }
		public decimal? ValorDolarCFR { get; set; }
		public decimal? ValorDolarFOB { get; set; }		

		#region Complemento 
		public string DescricaoPais { get; set; }
		public string StatusAnalise { get; set; }
		public string CodigoDescricaoMoeda { get; set; }
		public decimal? ValorTotal { get; set; }
		public bool ExisteSolicitacaoDeAlteracao { get; set; }
		public bool ExisteInsumoJaSolicitadoAlteracao { get; set; }
		public decimal QuantidadeMaxima { get; set; }
		
		#region FLAG INDICADORES SOLIC DETALHE
		public bool FlagExisteAlteracaoPais { get; set; }
		public bool FlagExisteAlteracaoMoeda { get; set; }
		public bool FlagExisteAlteracaoQuantidade { get; set; }
		public bool FlagExisteAlteracaoValorUnitario { get; set; }
		public bool FlagExisteAlteracaoValorFrete { get; set; }

		public int? IdSolicDetalhePais { get; set; }
		public int? IdSolicDetalheMoeda { get; set; }
		public int? IdSolicDetalheQuantidade { get; set; }
		public int? IdSolicDetalheVlrUnitario { get; set; }
		public int? IdSolicDetalheVlrFrete { get; set; }
		
		public decimal? Paridade { get; set; }
		#endregion

		#endregion

	}


}
