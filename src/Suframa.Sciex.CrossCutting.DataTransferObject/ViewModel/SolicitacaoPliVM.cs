using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPliVM : PagedOptions
	{
		public long IdSolicitacaoPli { get; set; }
		public string CnpjEmpresa { get; set; }
		public int idTipoAplicacaoPli { get; set; }
		public string NumeroLIReferencia { get; set; }
		public int? NumeroPEXPAM { get; set; }
		public short? NumeroAnoPEXPAM { get; set; }
		public string NumeroCPFRepresentanteLegal { get; set; }
		public string CodigoCNAE { get; set; }
		public int? InscricaoCadastral { get; set; }
		public int? CodigoSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public byte TipoDocumento { get; set; }
		public byte? TipoOrigem { get; set; }
		public string RazaoSocialEmpresa { get; set; }
		public short? CodigoPliAplicacao { get; set; }
		public string StatusIndicacaoPliExigencia { get; set; }
		public string NumeroPliImportador { get; set; }
		public long? IdEstruturaPropriaPli { get; set; }
		public byte StatusSolicitacao { get; set; }

		// Complemento de Classe
		public string DataValidacao { get; set; }
		public int QtdErrosPli { get; set; }
		public int QtdSucessoPli { get; set; }
		public List<ErroProcessamentoVM> ListaErros { get; set; }
		public string StatusSolicitacaoNome { get; set; }
		public DateTime DataInicioProcessamento { get; set; }
		public string NumeroPliSuframa { get; set; }

		public long? IdPLI { get; set; }
		public long? NumeroPLI { get; set; }
		public long? AnoPLI { get; set; }

	}
}
