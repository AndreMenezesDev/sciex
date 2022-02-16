using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class EstruturaPropriaLEEntityVM : PagedOptions
	{
		//ESTRUTURA LE
		public EstruturaPropriaPLIVM EstruturaPropriaPli { get; set; }
		public PagedItems<SolicitacaoLeInsumoVM> listaSolicitacaoLeInsumo { get; set; }
		public long IdEstruturaPropria { get; set; }
		public int IdSolicitacaoLeInsumo { get; set; }
		public decimal CodigoProduto { get; set; }
		public string DescricaoProduto { get; set; }
		public decimal CodigoTipoProduto { get; set; }
		public string DescricaoTipoProduto { get; set; }
		public string CodigoNCM { get; set; }
		public string DescricaoNCM { get; set; }
		public short CodigoUnidadeMedida { get; set; }
		public string DescricaoModelo { get; set; }
		public string CodigoModeloEmpresa { get; set; }
		public string DescricaoCentroCusto { get; set; }

		// Complemento de Classe
		public string DataValidacao { get; set; }
		public int QtdErrosPli { get; set; }
		public int QtdSucessoPli { get; set; }
		public List<ErroProcessamentoVM> ListaErros { get; set; }
		public string StatusSolicitacaoNome { get; set; }
		public int StatusSolicitacao { get; set; }
		public int SituacaoInsumo { get; set; }
		public string NumeroPliSuframa { get; set; }

		public long? IdPLI { get; set; }
		public long? NumeroPLI { get; set; }
		public long? AnoPLI { get; set; }

		//Estrutura Propria
		public string Empresa { get; set; }
		public int? NumeroProtocolo { get; set; }

		public string CentroDeCusto { get; set; }
		public string Modelo { get; set; }
		public string ModeloExportador { get; set; }
		public string Ncm { get; set; }
		public string Produto { get; set; }
		public string Tipo { get; set; }
		public string UnidadeMedida { get; set; }
	}

}

