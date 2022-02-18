using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class CertificadoRegistroVM : PRCStatusVM
	{
		#region Informações Empresa Exportadora
		public string RazaoSocial { get; set; }
		public int? InscricaoCadastral { get; set; }
		public string Cnpj { get; set; }
		// Concatenar Logradouro, Bairro, Estado e Uf
		public string Endereco { get; set; }
		public string CEP { get; set; }

		#endregion

		#region Dados Plano Aprovação
		public string NumeroPlano { get; set; }
		public string NumeroProcesso { get; set; }
		public string Modalidade { get; set; }
		public string DataDeferimento { get; set; }
		public string DataCancelamento { get; set; }
		public string DataValidade { get; set; }
		public string DataProrrogacao { get; set; }
		public string DataProrrogacaoEspecial { get; set; }
		#endregion

		#region Valores Aprovados
		public string ValorNacional { get; set; }
		public string ValorImportacaoFOB { get; set; }
		public string ValorImportacaoCFR { get; set; }
		public string ValorExportacao { get; set; }
		#endregion

		public string CodigoIdentificadorCRPE { get; set; }
		
		public string InsumosImportadosAprovados { get; set; }
		public List<PRCSolicitacaoAlteracaoVM> ListaAcrescimoSolicitacao { get; set; }
		public string TotalInsumosImportados { get; set; }

		public string ExportacoesRealizadasDolar { get; set; }
		public string ExportacoesRealizadasReal { get; set; }

	}
}
