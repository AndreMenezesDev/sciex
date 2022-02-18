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
	public class PlanoExportacaoVM : PagedOptions
	{

		public int IdPlanoExportacao { get; set; }
		public long NumeroPlano { get; set; }
		public int AnoPlano { get; set; }
		public int? NumeroInscricaoCadastral { get; set; }
		public string Cnpj { get; set; }
		public string RazaoSocial { get; set; }
		public string DescricaoInsumo { get; set; }
		public string TipoModalidade { get; set; }
		public string TipoExportacao { get; set; }
		public int Situacao { get; set; }
		public DateTime? DataEnvio { get; set; }
		public string DataEnvioFormatada { get; set; }
		public DateTime? DataCadastro { get; set; }
		public string DataCadastroFormatada { get; set; }
		public DateTime? DataStatus { get; set; }
		public string DataStatusFormatada { get; set; }
		public string CpfResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public string DescricaoJustificativaErro { get; set; }
		public int? NumeroProcesso { get; set; }
		public int? NumeroAnoProcesso { get; set; }
		public int? QtdFluxoMenor70porcento { get; set; }
		public string QtdFluxoMenor70porcentoString { get; set; }
		public int? QtdPerdaMaior2porcento { get; set; }
		public string QtdPerdaMaior2porcentoString { get; set; }

		public string NumeroAnoPlanoConcat { get; set; }
		public string NumeroAnoPlanoFormatado { get; set; }
		public string NumeroAnoProcessoFormatado { get; set; }
		public string TipoModalidadeString { get; set; }
		public string TipoExportacaoString { get; set; }
		public string SituacaoString { get; set; }
		public List<PEProdutoVM> ListaPEProdutos { get; set; }
		public List<PEProdutoComplementoVM> ListaPEProdutosComplemento { get; set; }
		public List<PEArquivoVM> ListaAnexos { get; set; }

		//complemento de classe
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public int? IdAnalistaDesignado { get; set; }
		public string Mensagem { get; set; }
	}

	public class ConsultarPlanoExportacaoVM : PagedOptions
	{
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public string NumeroPlano { get; set; }
		public int? StatusPlano { get; set; }
	}

	public class NovoPlanoExportacaoVM : ResultadoMensagemProcessamentoVM
	{
		public string Modalidade { get; set; }
		public string TipoExportacao { get; set; }
		public string NumeroProcesso { get; set; }

		public string AnoProcesso { get; set; }
		public string NomeAnexo { get; set; }
		public byte[] Anexo { get; set; }
		public bool IsCopia { get; set; }
		public int IdPlanoExportacao { get; set; }
	}

	public class ResultadoProcessamentoVM : ResultadoMensagemProcessamentoVM
	{
		public CamposNaoValidadosVM CamposNaoValidos { get; set; }
		public bool PossuiTodosRegistros { get; set; }
	}

	public class CamposNaoValidadosVM
	{
		public bool NaoExisteProduto { get; set; }
		public bool NaoExistePais { get; set; }
		public bool NaoExisteDue { get; set; }
		public bool NaoExisteInsumo { get; set; }
		public bool NaoExisteDetalhe { get; set; }
		public bool NaoExisteParidadeCambial { get; set; }
		public bool NaoExisteParidadeCambialEstrangeira { get; set; }
		public int? IdProduto { get; set; }
		public int? IdInsumo { get; set; }
		public bool IsNacional { get; set; }
	}

	public class ListarInsumosNacionalImportadosVM : PagedOptions
	{
		public bool isQuadroNacional { get; set; }
		public bool IsCorrecao { get; set; }
		public int? IdPEProduto { get; set; }
		public int? CodigoProdutoExportacao { get; set; }
		public List<int> ListaCodigoInsumosIncluidos { get; set; } = new List<int>();
	}

	public class DadosAprovacaoTodos
	{
		public List<int> ListaIdsParaAprovacao { get; set; }
	}

}
