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
	public class ProcessoExportacaoVM : PagedOptions
	{
		#region CampoTabela
		public int IdProcesso { get; set; }
		public int? NumeroProcesso { get; set; }
		public int? AnoProcesso { get; set; }
		public int? InscricaoSuframa { get; set; }
		public string RazaoSocial { get; set; }
		public string TipoModalidade { get; set; }
		public string TipoStatus { get; set; }
		public DateTime? DataValidade { get; set; }
		public decimal? ValorPremio { get; set; }
		public decimal? ValorPercentualIndImportado { get; set; }
		public decimal? ValorPercentualIndNacional { get; set; }
		public string Cnpj { get; set; }
		public List<PRCStatusVM> ListaStatus { get; set; }
		public List<PRCProdutoVM> ListaProduto { get; set; }
		#endregion

		#region Complemento
		public string NumeroAnoProcessoFormatado { get; set; }
		public string NumeroAnoPlanoFormatado { get; set; }
		public string DataStatusFormatada { get; set; }
		public string TipoModalidadeString { get; set; }
		public string TipoStatusString { get; set; }
		public string DataValidadeFormatada { get; set; }
		public InsumosAprovadosVM InsumosAprovados { get; set; } = new InsumosAprovadosVM();
		public SaldosVM Saldos { get; set; } = new SaldosVM();
		public bool ProrrogarExibir { get; set; }
		public bool ExibirProrrogacao { get; set; }
		public bool ExibirAlteracao { get; set; }
		public string DataValidadeProrrogada { get; set; }
		public string DataValidadeProrrogadaFormatada { get; set; }
		public bool JaPossuiProrrogacao { get; set; }
		public bool prorrogacaoAndamento { get; set; }

		#endregion
	}

	public class SaldosVM
	{
		public decimal? Nacionais { get; set; } = 0;
		public decimal? Importados { get; set; } = 0;
		public decimal? Cancelados { get; set; } = 0;
		public decimal? ValorAdicionalFrete { get; set; } = 0;
		public decimal? ValorAdicional { get; set; } = 0;
	}

	public class InsumosAprovadosVM
	{
		public decimal? Nacionais { get; set; } = 0;
		public decimal? ImportadosFOB { get; set; } = 0;
		public decimal? Frete { get; set; } = 0;
		public decimal? TotalFOB { get; set; } = 0;
	}

	public class ConsultarProcessoExportacaoVM : PagedOptions
	{
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public string NumeroPlano { get; set; }
		public string InscricaoCadastral { get; set; }
		public string RazaoSocial { get; set; }
		public string NumeroAnoConcatProcesso { get; set; }
		public string NumeroAnoConcatPlano { get; set; }
		public string TipoModalidade { get; set; }
		public string StatusPlano { get; set; }
	}

	public class ListarProcessoInsumosNacionalImportadosVM : PagedOptions
	{
		public bool isQuadroNacional { get; set; }
		public int? IdProcessoProduto { get; set; }
		public int? IdProcesso { get; set; }
		public int? IdProduto { get; set; }
		public int? IdNcm { get; set; }
		public int? IdProcessoInsumo { get; set; }
		public int? CodigoProdutoExportacao { get; set; }
		public int? CodigoInsumo { get; set; }
		public string CodigoNCM { get; set; }
		public List<int> ListaCodigoInsumosIncluidos { get; set; } = new List<int>();
		public int? TipoStatusAnalise { get; set; }
		public int? TipoAlteracao { get; set; }
		public int? IdSolicitacaoAnalise { get; set; }
		public bool ExisteSolicAlteracaoEmAnalise { get; set; }
	}


}
