using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suframa.Sciex.CrossCutting.Validation;

namespace Suframa.Sciex.BusinessLogic
{
	public class CertificadoRegistroSuframaBll : ICertificadoRegistroSuframaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public CertificadoRegistroSuframaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}


		public CertificadoRegistroVM CarregarDadosCertificado(int IdProcesso)
		{
			var retDados = new CertificadoRegistroVM();

			var dadosProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(p => p.IdProcesso == IdProcesso);

			if (dadosProcesso != null)
			{
				retDados.RazaoSocial = dadosProcesso.RazaoSocial;
				retDados.InscricaoCadastral = dadosProcesso.InscricaoSuframa;
				retDados.Cnpj = dadosProcesso.Cnpj.CnpjCpfFormat();

				retDados.NumeroProcesso = dadosProcesso.AnoProcesso.ToString()+"/"+
										  dadosProcesso.NumeroProcesso.ToString().PadLeft(4, '0');

				retDados.Modalidade = dadosProcesso.TipoModalidade == "S" ? "SUSPENSÃO":"";

				retDados.DataValidade = dadosProcesso.DataValidade != null ? Convert.ToDateTime(dadosProcesso.DataValidade).ToShortDateString() : "-";
			}

			if (dadosProcesso.ListaStatus.Count > 0)
			{
				foreach (var itemStatus in dadosProcesso.ListaStatus)
				{
					retDados.NumeroPlano = itemStatus.AnoPlano.ToString() + "/" + 
										   itemStatus.NumeroPlano.ToString().PadLeft(5,'0');


					retDados.DataDeferimento = itemStatus.Data != null ? Convert.ToDateTime(itemStatus.Data).ToShortDateString() : "-";
				}
			}

			if (dadosProcesso.ListaProduto.Count > 0)
			{
				// Insumos
				foreach (var itemProduto in dadosProcesso.ListaProduto)
				{
					var dadosInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Listar(r =>
													r.IdPrcProduto == itemProduto.IdProduto);

					if (dadosInsumo.Count > 0)
					{
						retDados.ValorNacional = this.FormatarMaskValor(dadosInsumo.Sum(s => s.ValorNacionalAprovado).ToString());
						retDados.ValorImportacaoFOB = this.FormatarMaskValor(dadosInsumo.Sum(s => s.ValorDolarFOBAprovado).ToString());
						retDados.ValorImportacaoCFR = this.FormatarMaskValor(dadosInsumo.Sum(s => s.ValorDolarCFRAprovado).ToString());
					}
				}

				// Produto
				retDados.ValorExportacao = this.FormatarMaskValor(dadosProcesso.ListaProduto.Sum(s => s.ValorDolarAprovado).ToString());
			}

			var dadosImportador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(i => i.Cnpj == dadosProcesso.Cnpj);
			if (dadosImportador != null)
			{
				retDados.Endereco = dadosImportador.Endereco.ToString() + " " +
									dadosImportador.Numero.ToString() + " " +
									dadosImportador.Bairro.ToString() + " " +
									dadosImportador.Municipio.ToString() + " -  " +
									dadosImportador.UF.ToString();

				retDados.CEP = dadosImportador.CEP.ToString().CepFormat();
			}


			// Codigo Identificador
			var vStringCodigo = retDados.InscricaoCadastral.ToString() + 
								retDados.NumeroPlano.ToString().Replace("/","") +
								retDados.NumeroProcesso.ToString().Replace("/","");

			var vCodigo = vStringCodigo.Replace("0", "").Replace("2", "").Replace("3", "");

			vCodigo = vCodigo.Replace("1", "0");

			retDados.CodigoIdentificadorCRPE = new string(vCodigo.Reverse().ToArray());
			//

			return retDados;
		}

		private string FormatarMaskValor(string sValor)
		{
			return Convert.ToDecimal(sValor).ToString("N7");
		}
	}
}
