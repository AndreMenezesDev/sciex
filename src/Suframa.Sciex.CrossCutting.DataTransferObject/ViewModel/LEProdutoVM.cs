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
	public class LEProdutoVM : PagedOptions
	{
		public int IdLe { get; set; }
		public int InscricaoCadastral { get; set; }
		public string Cnpj { get; set; }
		public int CodigoProduto { get; set; }
		public int IdCodigoProdutoSuframa { get; set; }
		public int CodigoProdutoSuframa { get; set; }
		public string DescCodigoProdutoSuframa { get; set; }
		public int CodigoTipoProduto { get; set; }
		public string DescCodigoTipoProduto { get; set; }
		public int IdCodigoTipoProduto { get; set; }
		public int CodigoUnidadeMedida { get; set; }
		public string DescCodigoUnidadeMedida { get; set; }
		public int IdUnidadeMedida { get; set; }
		public string DescricaoModelo { get; set; }
		public int StatusLE { get; set; }
		public int StatusLEAlteracao { get; set; }
		public string StatusLEString { get; set; }
		public string StatusLEAlteracaoString { get; set; }
		public string DescStatusLE { get; set; }
		public string DescStatusLEAlteracao { get; set; }
		public DateTime? DataEnvio { get; set; }
		public string DataEnvioFormatada { get; set; }
		public DateTime? DataAprovacao { get; set; }
		public string CodigoModeloEmpresa { get; set; }
		public string DescricaoCentroCusto { get; set; }
		public string CpfResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public string CodigoNCM { get; set; }
		public string DescCodigoNCM { get; set; }
		public int IdCodigoNCM { get; set; }
		public DateTime? DataCadastro { get; set; }
		public string DataCadastroFormatada { get; set; }

		/* complemento da classe */

		public Boolean IsInsumos { get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public string Descricao { get; set; }
		public int Id { get; set; }
		public string Mensagem { get; set; }
		public string MensagemErro { get; set; }
		public int? IdAnalistaDesignado { get; set; }
		public string RazaoSocial { get; set; }
		public int QtdInsumo { get; set; }

		public bool isUsuarioInterno { get; set; }
		public bool isAprovarAnalise { get; set; }
	}

}
