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
	public class PRCInsumoVM : PagedOptions
	{
		public virtual PRCProdutoVM Produto { get; set; }
		public virtual PRCSolicitacaoAlteracaoVM PrcSolicitacaoAlteracao { get; set; }
		public virtual IList<PRCDetalheInsumoVM> ListaDetalheInsumos { get; set; }
		public virtual IList<PRCSolicDetalheVM> ListaSolicDetalhe { get; set; }

		public int IdInsumo { get; set; }
		public int IdPrcProduto { get; set; }
		public int IdTipoSolicitacao { get; set; }
		public int? IdPrcSolicitacaoAlteracao { get; set; }
		public int? CodigoInsumo { get; set; }
		public int? CodigoUnidade { get; set; }
		public string DescCodigoUnidade { get; set; }
		public string TipoInsumo { get; set; }
		public string DescricaoTipoInsumo { get; set; }
		public string CodigoNCM { get; set; }
		public decimal? ValorPercentualPerda { get; set; }
		public int? CodigoDetalhe { get; set; }
		public string DescricaoPartNumber { get; set; }
		public string DescricaoInsumo { get; set; }
		public string DescricaoEspecificacaoTecnica { get; set; }
		public decimal? ValorCoeficienteTecnico { get; set; }
		public decimal? ValorDolarAprovado { get; set; }
		public decimal? QuantidadeAprovado { get; set; }
		public decimal? ValorNacionalAprovado { get; set; }
		public decimal? ValorDolarFOBAprovado { get; set; }
		public decimal? ValorDolarCFRAprovado { get; set; }
		public decimal? ValorFreteAprovado { get; set; }
		public decimal? ValorDolarComp { get; set; }
		public decimal? QuantidadeComp { get; set; }
		public decimal? ValorDolarSaldo { get; set; }
		public decimal? QuantidadeSaldo { get; set; }
		public decimal? QuantidadeAdicional { get; set; }
		public decimal? ValorAdicional { get; set; }
		public decimal? ValorAdicionalFrete { get; set; }
		public int? StatusInsumo { get; set; }
		public int? StatusInsumoNovo { get; set; }
		public decimal? ValorDolarUnitario { get; set; }
		public decimal? ValorDolarUnitarioCrf { get; set; }

		#region Complemento
		public decimal ValoresTotais { get; set; }
		public decimal? QtdMaxInsumo { get; set; }
		public string DescricaoNCM { get; set; }
		public decimal PercentualPerda { get; set; }
		public decimal ValorPEInsumo { get; set; }
		public decimal SomatorioTotalPRCInsumo { get; set; }
		public string IsNovoRegistro { get; set; }
		public bool FlagIndicadoraExisteInsumoEmCopia { get; set; } = false;
		public string StatusAnalise { get; set; }

		#endregion

		public int? CodigoInsumoOrigem { get; set; }
		public int? CodigoInsumoDestino { get; set; }
		public string StatusSolicitacaoDetalhe { get; set; }


		public bool IsAprovarAnalise { get; set; }
		public string Mensagem { get; set; }
		public bool Sucesso { get; set; }
		public bool ExibirBotaoAprovarReprovar { get; set; }
		public bool TipoTransferencia { get; set; }

	}
}
