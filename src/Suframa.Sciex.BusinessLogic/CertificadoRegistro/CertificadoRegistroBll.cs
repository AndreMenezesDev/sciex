using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suframa.Sciex.CrossCutting.Validation;
using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
	public class CertificadoRegistroBll : ICertificadoRegistroBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public CertificadoRegistroBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}


		public CertificadoRegistroVM CarregarDadosCertificado(int IdStatus)
		{
			var retDados = new CertificadoRegistroVM();

			var regStatus = _uowSciex.QueryStackSciex.PRCStatus
					.Selecionar(
					 o =>
					 o.IdStatus == IdStatus);

			var dadosProcesso = _uowSciex.QueryStackSciex.Processo
					.Selecionar(
					o =>
					o.IdProcesso == regStatus.IdProcesso);


			if (dadosProcesso != null)
			{
				retDados.Tipo = regStatus.Tipo;
				
				switch (regStatus.Tipo)
				{
					case "AP": //APROVAÇÃO
						retDados.RazaoSocial = dadosProcesso.RazaoSocial;
						retDados.InscricaoCadastral = dadosProcesso.InscricaoSuframa;
						retDados.Cnpj = dadosProcesso.Cnpj.CnpjCpfFormat();

						retDados.NumeroProcesso = dadosProcesso.NumeroProcesso.ToString().PadLeft(4, '0') + "/" +
												  dadosProcesso.AnoProcesso.ToString();

						retDados.Modalidade = dadosProcesso.TipoModalidade == "S" ? "SUSPENSÃO" : "";

						retDados.DataValidade = dadosProcesso.DataValidade != null ? Convert.ToDateTime(dadosProcesso.DataValidade).ToShortDateString() : "-";
						

						if (dadosProcesso.ListaStatus.Count > 0)
						{

							retDados.NumeroPlano =  dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null ?
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().NumeroPlano.ToString().PadLeft(5, '0') + "/" +
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().AnoPlano.ToString()
													: "-";


							retDados.DataDeferimento = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().Data).ToShortDateString()
								: "-";
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
											retDados.NumeroPlano.ToString().Replace("/", "") +
											retDados.NumeroProcesso.ToString().Replace("/", "");

						var vCodigo = vStringCodigo.Replace("0", "").Replace("2", "").Replace("3", "");

						vCodigo = vCodigo.Replace("1", "0");

						retDados.CodigoIdentificadorCRPE = new string(vCodigo.Reverse().ToArray());

					break;

					case "AL": //ALTERAÇÃO
						retDados.RazaoSocial = dadosProcesso.RazaoSocial;
						retDados.InscricaoCadastral = dadosProcesso.InscricaoSuframa;
						retDados.Cnpj = dadosProcesso.Cnpj.CnpjCpfFormat();

						retDados.NumeroProcesso = dadosProcesso.NumeroProcesso.ToString().PadLeft(4, '0') + "/" +
												   dadosProcesso.AnoProcesso.ToString();

						retDados.Modalidade = dadosProcesso.TipoModalidade == "S" ? "SUSPENSÃO" : "";

						retDados.DataValidade = dadosProcesso.DataValidade != null ? Convert.ToDateTime(dadosProcesso.DataValidade).ToShortDateString() : "-";


						if (dadosProcesso.ListaStatus.Count > 0)
						{
							retDados.NumeroPlano =  dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null ?
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().NumeroPlano.ToString().PadLeft(5, '0') + "/" +
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().AnoPlano.ToString()
													: "-";

							retDados.DataDeferimento = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().Data).ToShortDateString()
								: "-";
						}

						var dadosImportador2 = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(i => i.Cnpj == dadosProcesso.Cnpj);
						if (dadosImportador2 != null)
						{
							retDados.Endereco = dadosImportador2.Endereco.ToString() + " " +
												dadosImportador2.Numero.ToString() + " " +
												dadosImportador2.Bairro.ToString() + " " +
												dadosImportador2.Municipio.ToString() + " -  " +
												dadosImportador2.UF.ToString();

							retDados.CEP = dadosImportador2.CEP.ToString().CepFormat();
						}

						var listaInsumosImportadosAprovados = _uowSciex.QueryStackSciex.ParecerTecnico.Listar(o => o.IdProcesso == dadosProcesso.IdProcesso);
						if(listaInsumosImportadosAprovados != null)
						{

							retDados.InsumosImportadosAprovados = listaInsumosImportadosAprovados.Sum(o=>o.ValorInsumoImportadoFob).ToString();
						}
						
						
						var listaAcrescimoSolicitacao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.ListarGrafo(o => new PRCSolicitacaoAlteracaoVM()
						{

							IdProcesso = o.IdProcesso,
							AcrescimoSolicitacao = o.AcrescimoSolicitacao,
							NumeroSolicitacao = o.NumeroSolicitacao,
							AnoSolicitacao = o.AnoSolicitacao
							
						}, o => o.IdProcesso == dadosProcesso.IdProcesso);

						if (listaAcrescimoSolicitacao != null)
						{		
							retDados.ListaAcrescimoSolicitacao = listaAcrescimoSolicitacao.ToList();
							foreach (var item in retDados.ListaAcrescimoSolicitacao)
							{
								item.NumeroAnoSolicitacaoFormatado = Convert.ToInt32(item.NumeroSolicitacao).ToString("D3") + "/" + Convert.ToInt32(item.AnoSolicitacao).ToString();
								item.AcrescimoSolicitacaoFormatado = item.AcrescimoSolicitacao.ToString();
							}
						}

						retDados.TotalInsumosImportados = (listaInsumosImportadosAprovados.Sum(o => o.ValorInsumoImportadoFob) + listaAcrescimoSolicitacao.Sum(o => o.AcrescimoSolicitacao)).ToString();
						
					break;

					case "CA": //CANCELAMENTO
						retDados.RazaoSocial = dadosProcesso.RazaoSocial;
						retDados.InscricaoCadastral = dadosProcesso.InscricaoSuframa;
						retDados.Cnpj = dadosProcesso.Cnpj.CnpjCpfFormat();

						retDados.NumeroProcesso = dadosProcesso.NumeroProcesso.ToString().PadLeft(4, '0') + "/" +
												  dadosProcesso.AnoProcesso.ToString();

						retDados.Modalidade = dadosProcesso.TipoModalidade == "S" ? "SUSPENSÃO" : "";

						retDados.DataValidade = dadosProcesso.DataValidade != null ? Convert.ToDateTime(dadosProcesso.DataValidade).ToShortDateString() : "-";

						if (dadosProcesso.ListaStatus.Count > 0)
						{
							retDados.NumeroPlano =	dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null ?
								                    dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().NumeroPlano.ToString().PadLeft(5, '0') + "/" +
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().AnoPlano.ToString() 
													: "-";

							retDados.DataDeferimento = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().Data).ToShortDateString()
								: "-";

							retDados.DataCancelamento = dadosProcesso.ListaStatus.Where(o => o.Tipo == "CA").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "CA").FirstOrDefault().Data).ToShortDateString()
								: "-";
						}
						var dadosImportador3 = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(i => i.Cnpj == dadosProcesso.Cnpj);
						if (dadosImportador3 != null)
						{
							retDados.Endereco = dadosImportador3.Endereco.ToString() + " " +
												dadosImportador3.Numero.ToString() + " " +
												dadosImportador3.Bairro.ToString() + " " +
												dadosImportador3.Municipio.ToString() + " -  " +
												dadosImportador3.UF.ToString();

							retDados.CEP = dadosImportador3.CEP.ToString().CepFormat();
						}
						var vStringCodigo3 = retDados.InscricaoCadastral.ToString() +
											retDados.NumeroPlano.ToString().Replace("/", "") +
											retDados.NumeroProcesso.ToString().Replace("/", "");

						var vCodigo3 = vStringCodigo3.Replace("0", "").Replace("2", "").Replace("3", "");
						retDados.CodigoIdentificadorCRPE = new string(vCodigo3.Reverse().ToArray());

						break;
					case "PR": //PRORROGADO

						retDados.RazaoSocial = dadosProcesso.RazaoSocial;
						retDados.InscricaoCadastral = dadosProcesso.InscricaoSuframa;
						retDados.Cnpj = dadosProcesso.Cnpj.CnpjCpfFormat();

						retDados.NumeroProcesso = dadosProcesso.NumeroProcesso.ToString().PadLeft(4, '0') + "/" +
												  dadosProcesso.AnoProcesso.ToString();

						retDados.Modalidade = dadosProcesso.TipoModalidade == "S" ? "SUSPENSÃO" : "";

						var regStatusPR = _uowSciex.QueryStackSciex.PRCStatus.Selecionar(o => o.IdStatus == IdStatus && o.Tipo == "PR");
						retDados.DataValidade = regStatusPR.DataValidade != null ? Convert.ToDateTime(regStatusPR.DataValidade).ToShortDateString() : "-";

						if (dadosProcesso.ListaStatus.Count > 0)
						{
							retDados.NumeroPlano = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null ?
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().NumeroPlano.ToString().PadLeft(5, '0') + "/" +
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().AnoPlano.ToString()
													: "-";

							retDados.DataDeferimento = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().Data).ToShortDateString()
								: "-";

							retDados.DataProrrogacao = dadosProcesso.ListaStatus.Where(o => o.Tipo == "PR").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "PR").FirstOrDefault().Data).ToShortDateString()
								: "-";
						}
						var dadosImportador4 = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(i => i.Cnpj == dadosProcesso.Cnpj);
						if (dadosImportador4 != null)
						{
							retDados.Endereco = dadosImportador4.Endereco.ToString() + " " +
												dadosImportador4.Numero.ToString() + " " +
												dadosImportador4.Bairro.ToString() + " " +
												dadosImportador4.Municipio.ToString() + " -  " +
												dadosImportador4.UF.ToString();

							retDados.CEP = dadosImportador4.CEP.ToString().CepFormat();
						}
						var vStringCodigo4 = retDados.InscricaoCadastral.ToString() +
											retDados.NumeroPlano.ToString().Replace("/", "") +
											retDados.NumeroProcesso.ToString().Replace("/", "");

						var vCodigo4 = vStringCodigo4.Replace("0", "").Replace("2", "").Replace("3", "");
						retDados.CodigoIdentificadorCRPE = new string(vCodigo4.Reverse().ToArray());
						break;

					case "PE": //PRORROGADO EM CARATER ESPECIAL

						retDados.RazaoSocial = dadosProcesso.RazaoSocial;
						retDados.InscricaoCadastral = dadosProcesso.InscricaoSuframa;
						retDados.Cnpj = dadosProcesso.Cnpj.CnpjCpfFormat();
						retDados.NumeroProcesso = dadosProcesso.NumeroProcesso.ToString().PadLeft(4, '0') + "/" +
												  dadosProcesso.AnoProcesso.ToString();

						retDados.Modalidade = dadosProcesso.TipoModalidade == "S" ? "SUSPENSÃO" : "";

						var regStatusPE = _uowSciex.QueryStackSciex.PRCStatus.Selecionar( o => o.IdStatus == IdStatus && o.Tipo == "PE");
						retDados.DataValidade = regStatusPE.DataValidade != null ? Convert.ToDateTime(regStatusPE.DataValidade).ToShortDateString() : "-";

						if (dadosProcesso.ListaStatus.Count > 0)
						{
							retDados.NumeroPlano = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null ?
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().NumeroPlano.ToString().PadLeft(5, '0') + "/" +
													dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().AnoPlano.ToString()
													: "-";

							retDados.DataDeferimento = dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "AP").FirstOrDefault().Data).ToShortDateString()
								: "-";

							retDados.DataProrrogacaoEspecial = dadosProcesso.ListaStatus.Where(o => o.Tipo == "PE").FirstOrDefault() != null
								? Convert.ToDateTime(dadosProcesso.ListaStatus.Where(o => o.Tipo == "PE").FirstOrDefault().Data).ToShortDateString()
								: "-";
						}
						var dadosImportador5 = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(i => i.Cnpj == dadosProcesso.Cnpj);
						if (dadosImportador5 != null)
						{
							retDados.Endereco = dadosImportador5.Endereco.ToString() + " " +
												dadosImportador5.Numero.ToString() + " " +
												dadosImportador5.Bairro.ToString() + " " +
												dadosImportador5.Municipio.ToString() + " -  " +
												dadosImportador5.UF.ToString();

							retDados.CEP = dadosImportador5.CEP.ToString().CepFormat();
						}
						var vStringCodigo5 = retDados.InscricaoCadastral.ToString() +
											retDados.NumeroPlano.ToString().Replace("/", "") +
											retDados.NumeroProcesso.ToString().Replace("/", "");

						var vCodigo5 = vStringCodigo5.Replace("0", "").Replace("2", "").Replace("3", "");
						retDados.CodigoIdentificadorCRPE = new string(vCodigo5.Reverse().ToArray());
						break;

				}
				
				
			}

			//

			return retDados;
		}

		public PagedItems<PRCStatusVM> ListarPaginado(PRCStatusVM pagedFilter)
		{
			

			//var lista = _uowSciex.QueryStackSciex.PRCStatus
			//		.ListarPaginado<PRCStatusVM>(
			//					o => o.IdProcesso == pagedFilter.IdProcesso, pagedFilter
			//				);
			var lista = _uowSciex.QueryStackSciex.PRCStatus
					.ListarPaginadoGrafo(o => new PRCStatusVM()
					{
						IdProcesso = o.IdProcesso,
						IdStatus = o.IdStatus,
						Tipo = o.Tipo
					},
					 o =>
					 o.IdProcesso == pagedFilter.IdProcesso
					 &&
					 (string.IsNullOrEmpty(pagedFilter.FiltroCertificado) || o.Tipo == pagedFilter.FiltroCertificado)
					 ,
					 pagedFilter
							);

			foreach(var item in lista.Items)
			{
				item.DescricaoTipo = item.Tipo == "AP" ? "Aprovação" : 
									 item.Tipo == "AL" ? "Alteração" :
								     item.Tipo == "CA" ? "Cancelamento" :
									 item.Tipo == "PR" ? "Prorrogado" :
									 item.Tipo == "PE" ? "Prorrogado em Carater Especial":
									 "-";
			}


			return lista;
		}

		private string FormatarMaskValor(string sValor)
		{
			return Convert.ToDecimal(sValor).ToString("N7");
		}
	}
}
