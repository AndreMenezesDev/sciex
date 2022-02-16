using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class ConsultarPliBll : IConsultarPliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IPliHistoricoBll _pliHistoricoBll;
		private readonly IPliBll _pliBll;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public ConsultarPliBll(IUnitOfWorkSciex uowSciex, IUsuarioLogado usuarioLogado,
			IPliHistoricoBll pliHistoricoBll, IPliBll pliBll, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogado;
			_pliHistoricoBll = pliHistoricoBll;
			_pliBll = pliBll;
			_usuarioPssBll = usuarioPssBll;
		}

		public PagedItems<PliVM> ListarPaginado(PliVM pagedFilter)
		{
			//StatusPli Entregue à SUFRAMA
			if (pagedFilter.StatusPliSelecionado == 2)
			{
				try
				{
					if (pagedFilter == null) { return new PagedItems<PliVM>(); }

					var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
					var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)
							)
							&&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							) 
							&&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							)
							&&
							(
								//Entregue à SUFRAMA
								(o.StatusPli == (byte)EnumPliStatus.ENTREGUE) ||
								o.StatusPli == (byte)EnumPliStatus.AGUARDANDO_GERAÇÃO_DE_DÉBITO

							)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
							)

						),
						pagedFilter);
					
					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}
			//StatusPli Débito Gerado
			if (pagedFilter.StatusPliSelecionado == 3)
			{
				try
				{
					if (pagedFilter == null) { return new PagedItems<PliVM>(); }

					var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
					var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)

							)
							&&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							) &&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							) &&
							(
								//Débito Gerado
								(o.StatusPli == (byte)EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL && o.TaxaPli.StatusTaxa == 3 ||
								o.StatusPli == (byte)EnumPliStatus.AGUARDANDO_PROCESSAMENTO && o.TaxaPli.StatusTaxa == 3)

							)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
							)

						),
						pagedFilter);


					
					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}
			//Processado Suframa
			if (pagedFilter.StatusPliSelecionado == 4)
			{
				try
				{
					if (pagedFilter == null) { return new PagedItems<PliVM>(); }

					var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
					var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							) 
							&&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)

							)
							&&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							) &&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							) &&
							(
								//Processado pela SUFRAMA
								(
								   o.StatusPli == (byte)EnumPliStatus.PROCESSADO
								   && (o.StatusPliProcessamento == 1 || o.StatusPliProcessamento == 2 || o.StatusPliProcessamento == 3)
								   && (o.PLIMercadoria.Any(x => x.Ali.Status == (byte)EnumAliStatus.ALI_GERADA || x.Ali.Status == (byte)EnumAliStatus.ALI_INDEFERIDA_PELA_SUFRAMA))
								   && (o.PLIMercadoria.Count(x=>x.Ali.DataRespostaSISCOMEX != null) == 0)
								)
							)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
							)

						),
						pagedFilter);

				
					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}
			//StatusPli Enviado SISCOMEX
			if (pagedFilter.StatusPliSelecionado == 5)
			{
				try
				{
					if (pagedFilter == null) { return new PagedItems<PliVM>(); }

					var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
					var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)

							)
							&&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							)
							&&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							)
							&&
							(
								//Enviado ao SICOMEX
								(o.StatusPli == (byte)EnumPliStatus.PROCESSADO
								&& o.PLIMercadoria.Any(x=>x.Ali.AliArquivoEnvio.CodigoStatusEnvioSiscomex == 2 && x.Ali.Status == 2)
								)
							)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
							)

						),
						pagedFilter);



					foreach (var item in ConsultarPli.Items)
					{

						// RN - verificar Status Enviado ao Siscomex

						item.EnviadoAoSiscomex = 1;

						// Se o STATUS da Ali >= 2 (Enviada ao SISCOMEX)
						// e STATUS do PLI Processamento = 1 
						// e STATUS do PLI >= 25

						var mercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => o.Ali.Status >= (byte)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX
							&& (o.Pli.StatusPliProcessamento == 1 || o.Pli.StatusPliProcessamento == 2) && o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.Ali.AliArquivoEnvio.CodigoStatusEnvioSiscomex == 2
							&& o.IdPLI == item.IdPLI
						);

						if (mercadoria.Any())
							item.EnviadoAoSiscomex = 2;
						else
						{

							// Se o STATUS da Ali >= 2 (Enviada ao SISCOMEX)
							// e STATUS do PLI Processamento = 2 
							// e STATUS do PLI >= 25

							var mercadoria1 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => o.Ali.Status >= (byte)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX
							&& (o.Pli.StatusPliProcessamento == 1 || o.Pli.StatusPliProcessamento == 2) && o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.IdPLI == item.IdPLI && o.Ali.AliArquivoEnvio.CodigoStatusEnvioSiscomex == 3
							);

							if (mercadoria1.Any())
								item.EnviadoAoSiscomex = 3;
						}

					}

					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}
			//StatusPli Recebido SISCOMEX
			if (pagedFilter.StatusPliSelecionado == 6)
			{
				try
				{
					if (pagedFilter == null) { return new PagedItems<PliVM>(); }

					var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
					var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)

							)
							&&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							)
							&&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							)
							&&
							(
							//Respondido SISCOMEX
							/*o.StatusPli == pagedFilter.StatusPli && 
								o.StatusPli == (byte)EnumPliStatus.PROCESSADO && 
								o.PLIMercadoria.Any(x => 
									x.Ali.DataRespostaSISCOMEX == null
								)	*/

							o.StatusPli == pagedFilter.StatusPli &&
								o.StatusPli == (byte)EnumPliStatus.PROCESSADO &&
								
								o.PLIMercadoria.Any(
										x => x.Ali.DataRespostaSISCOMEX != null) &&

								! o.PLIMercadoria.Any(x => 										
										x.Li.IdDI != null
								)
							)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
							)

						),
						pagedFilter);

					/*StringBuilder sqlQuery = new StringBuilder().Append("select distinct "
						+ " C.PLI_ID  as IdPLI, PAP_ID as IdPLIAplicacao,PLI_NU  as NumeroPli,PLI_NU_ANO  as  Ano,PLI_NU_CNPJ as Cnpj,INS_CO as InscricaoCadastral,SET_CO as CodigoSetor," +
						" SET_DS  as  DescricaoSetor,PLI_TP_DOCUMENTO as  TipoDocumento,PLI_ST_ANALISE_VISUAL as  StatusAnaliseVisual,PLI_ST_DISTRIBUICAO  as  StatusDistribuicao,PLI_VL_TCIF  as  ValorTCIF," +
						" PLI_VL_TCIF_ITENS  as  ValorTECIFItens,PLI_NU_DEBITO  as  Debito,PLI_NU_DEBITO_ANO  as  DebitoAno,PLI_DH_DEBITO_PGTO  as  DataDebitoPagamento,PLI_DH_DEBITO_GERACAO  as  DataDebitoGeracao," +
						" PLI_NU_LI_REFERENCIA as  NumeroLIReferencia,PLI_NU_DI_REFERENCIA as  NumeroDIReferencia,PLI_NU_PEXPAM as  NumeroPEXPAM,PLI_NU_ANO_PEXPAM as  AnoPEXPAM," +
						" PLI_NU_LOTE_PEXPAM  as  LotePEXPAM,PLI_ME_ALI_ARQUIVO  as  MEALIArquivo,PLI_TP_ORIGEM  as  TipoOrigem,PLI_NU_RESPONSAVEL_CADASTRO  as  NumeroResponsavelRegistro," +
						" PLI_NO_RESPONSAVEL_CADASTRO  as  NomeResponsavelRegistro,PLI_DH_CADASTRO as DataCadastro,PLI_DH_ENVIO as  DataEnvioPli,PLI_NU_CPF_REP_LEGAL_SISCOMEX as  NumCPFRepLegalSISCO," +
						" PLI_CO_CNAE as  CodigoCNAE,PLI_VL_TOTAL_CONDICAO_VENDA  as  ValorTotalCondicaoVenda,PLI_VL_TOTAL_CONDICAO_VENDA_REAL  as  ValorTotalCondicaoVendaReal," +
						" PLI_VL_TOTAL_CONDICAO_VENDA_DOLAR  as  ValorTotalCondicaoVendaDolar,PLI_COL_ROW_VERSION  as RowVersion,IMP_DS_RAZAO_SOCIAL  as  RazaoSocial,PLI_ST_PLI  as  StatusPli," +
						" PLI_ST_PROCESSAMENTO as  StatusPliProcessamento,SPL_NU_PLI_IMPORTADOR  as  NumeroPliImportador,SPL_ST_PLI_TECNOLOGIA_ASSISTIVA  as  StatusPliTecnologiaAssistiva," +
						" SPL_ST_INDICACAO_PLI_EXIGENCIA  as  StatusIndicacaoPliExigencia,ESP_ID as IdEstruturaPropria"
						+ " from sciex_li a, sciex_pli_mercadoria b, sciex_pli c, SCIEX_ALI d"
						+ " where a.pme_id = b.pme_id"
						+ " and b.pli_id = c.pli_id"
						+ " and b.pme_id = d.pme_id"
						+ " and pli_st_pli = 25"
						+ " and d.ALI_DH_RESPOSTA_SISCOMEX is not null"
						+ " and not exists(select * from sciex_li e where a.pme_id = e.pme_id and di_id is not null)");

					if (pagedFilter.NumeroPli > 0)
					{
						sqlQuery.Append(" and pli_nu = " + pagedFilter.NumeroPli);
					}

					if (!string.IsNullOrEmpty(pagedFilter.RazaoSocial))
					{
						sqlQuery.Append(" and IMP_DS_RAZAO_SOCIAL like '%" + pagedFilter.RazaoSocial + "%'");
					}

					if (pagedFilter.Ano > 0)
					{
						sqlQuery.Append(" and PLI_NU_ANO = " + pagedFilter.Ano);
					}

					if (pagedFilter.InscricaoCadastral != null)
					{
						sqlQuery.Append(" and INS_CO = " + pagedFilter.InscricaoCadastral);
					}

					if (pagedFilter.DataInicio != null)
					{						
						sqlQuery.Append(" and CONVERT(datetime, PLI_DH_ENVIO, 103) between convert(datetime, '" + pagedFilter.DataInicio + "', 103) " +
							"and convert(datetime, '" + pagedFilter.DataFim + "', 103)");
					}

					var listaEntity = _uowSciex.QueryStackSciex.ListarPaginadoSql<PliEntity>(sqlQuery.ToString(), pagedFilter);

					PagedItems<PliVM> ConsultarPli = new PagedItems<PliVM>();

					IList<PliVM> listaPli = new List<PliVM>();

					foreach (var item in listaEntity.Items)
					{
						listaPli.Add(ConverterEntityToVM(item));
					}

					ConsultarPli.Items = new List<PliVM>();
					ConsultarPli.Items = listaPli;*/

					foreach (var item in ConsultarPli.Items)
					{						

						// RN - verificar Status Respondido pelo Siscomex
						item.TemALIIndeferida = false;

						item.TemDiDesembaracada = 0;
						item.EnviadoAoSiscomex = 2;
						item.RespondidoPeloSiscomex = 2;

						var mercadoria2 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => (o.Pli.StatusPliProcessamento == 1 || o.Pli.StatusPliProcessamento == 2)
							&& o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.IdPLI == item.IdPLI
							&& o.Ali.DataRespostaSISCOMEX != null
						);

						if (mercadoria2.Any())
						{
							item.EnviadoAoSiscomex = 2;
							item.RespondidoPeloSiscomex = 2;

							if (mercadoria2.Any(x => x.Ali.Status == (byte)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX))
							{
								item.TemALIIndeferida = true;
							}
						}
						else
						{
							item.EnviadoAoSiscomex = 2;
							item.RespondidoPeloSiscomex = 1;
						}

					}

					//	// Se o STATUS da Ali >= 3 (ALI Deferida)
					//	// e STATUS do PLI Processamento = 1 
					//	// e STATUS do PLI >= 25

					//	var mercadoria2 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
					//		o => o.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA
					//		&& o.Pli.StatusPliProcessamento == 1 && o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
					//		&& o.Ali.DataRespostaSISCOMEX != null 
					//		&& o.IdPLI == item.IdPLI
					//	);

					//	if (mercadoria2.Any())
					//	{
					//		item.RespondidoPeloSiscomex = 2;
					//		item.EnviadoAoSiscomex = 2;
					//	}
					//	else
					//	{

					//		// Se o STATUS da Ali >= 4 (ALI Indeferida pela SUFRAMA)
					//		// e STATUS do PLI Processamento = 2
					//		// e Data de Resposta do SISCOMEX != null 
					//		// e STATUS Pli >= 25

					//		var mercadoria3 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
					//		o => o.Ali.Status > (byte)EnumAliStatus.ALI_INDEFERIDA_PELA_SUFRAMA
					//		&& o.Pli.StatusPliProcessamento == 2
					//		&& o.Ali.DataRespostaSISCOMEX != null && o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO && o.IdPLI == item.IdPLI
					//		);

					//		if (mercadoria3.Any())
					//		{
					//			item.RespondidoPeloSiscomex = 3;
					//			item.EnviadoAoSiscomex = 2;
					//		}
					//	}
					//}

					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}
			//StatusPli Recebido SISCOMEX
			if (pagedFilter.StatusPliSelecionado == 7)
			{
				try
				{					
					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)

							)
							&&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							)
							&&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							)
							&&
							(
								//Respondido SISCOMEX
								(
								//o.StatusPli == pagedFilter.StatusPli && 
								o.StatusPli == (byte)EnumPliStatus.PROCESSADO
								&& 
								o.PLIMercadoria.Any(
									x => x.Li.Status == (byte)EnumLiStatus.LI_UTILIZADA)
								)
							)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= pagedFilter.DataInicio && o.DataEnvioPli <= pagedFilter.DataFim)
							)

						),
						pagedFilter);

					foreach (var item in ConsultarPli.Items)
					{
						// RN - verificar Status Respondido pelo Siscomex
						item.TemALIIndeferida = false;

						item.EnviadoAoSiscomex = 2;
						item.RespondidoPeloSiscomex = 2;

						var mercadoria2 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => (o.Pli.StatusPliProcessamento == 1 || o.Pli.StatusPliProcessamento == 2)
							&& o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.IdPLI == item.IdPLI
							&& o.Ali.DataRespostaSISCOMEX != null
						);

						if (mercadoria2.Any())
						{
							item.EnviadoAoSiscomex = 2;
							item.RespondidoPeloSiscomex = 2;

							if (mercadoria2.Any(x => x.Ali.Status == (byte)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX))
							{
								item.TemALIIndeferida = true;
							}
						}
						else
						{
							item.EnviadoAoSiscomex = 2;
							item.RespondidoPeloSiscomex = 1;
						}

						var mercadoria3 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => o.IdPLI == item.IdPLI
							&& o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.Li.Status == 7
						);
						if (mercadoria3.Any())
						{
							item.TemDiDesembaracada = 1;
						}
						else
						{
							item.TemDiDesembaracada = 0;
						}
					}

					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}
			else
			{
				try
				{
					if (pagedFilter == null) { return new PagedItems<PliVM>(); }

					var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
					var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

					var ConsultarPli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
							) &&
							(
								string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial)

							) &&
							(
								pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
							) &&
							(
								pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
							) &&
								//Entregue
								(o.StatusPli >= (byte)EnumPliStatus.ENTREGUE)
							&&
							(
								pagedFilter.DataInicio == null || (o.DataEnvioPli >= dataInicio && o.DataEnvioPli <= dataFim)
							)

						),
						pagedFilter);



					foreach (var item in ConsultarPli.Items)
					{
						item.TemALIIndeferida = false;
						// RN - verificar Status Enviado ao Siscomex
						item.EnviadoAoSiscomex = 1;

						// Se o STATUS da Ali >= 2 (Enviada ao SISCOMEX)
						// e STATUS do PLI Processamento = 1 
						// e STATUS do PLI >= 25
						var mercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => o.Ali.Status >= (byte)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX
							&& (o.Pli.StatusPliProcessamento == 1 || o.Pli.StatusPliProcessamento == 2) && o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.Ali.AliArquivoEnvio.CodigoStatusEnvioSiscomex == 2
							&& o.IdPLI == item.IdPLI
						);

						if (mercadoria.Any())
							item.EnviadoAoSiscomex = 2;
						else
						{
							// Se o STATUS da Ali >= 2 (Enviada ao SISCOMEX)
							// e STATUS do PLI Processamento = 2 
							// e STATUS do PLI >= 25							
						}


						// RN - verificar Status Respondido pelo Siscomex
						item.RespondidoPeloSiscomex = 1;

						// Se o STATUS da Ali >= 3 (ALI Deferida)
						// e STATUS do PLI Processamento = 1 
						// e STATUS do PLI >= 25

						var mercadoria2 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => (o.Pli.StatusPliProcessamento == 1 || o.Pli.StatusPliProcessamento == 2)
							&& o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.IdPLI == item.IdPLI
							&& o.Ali.DataRespostaSISCOMEX != null
						);

						if (mercadoria2.Any())
						{
							item.RespondidoPeloSiscomex = 2;

							if (mercadoria2.Any(x => x.Ali.Status == (byte)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX))
							{
								item.TemALIIndeferida = true;
							}
						}
						else
						{
							item.RespondidoPeloSiscomex = 1;
						}

						var mercadoria3 = _uowSciex.QueryStackSciex.PliMercadoria.Listar(
							o => o.IdPLI == item.IdPLI
							&& o.Pli.StatusPli >= (byte)EnumPliStatus.PROCESSADO
							&& o.Li.Status == 7
						);
						if (mercadoria3.Any())
						{
							item.TemDiDesembaracada = 1;
						}
						else
						{
							item.TemDiDesembaracada = 0;
						}

					}

					return ConsultarPli;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema ConsultarPli: Nenhum registro encontrado.");

				}
			}

			return new PagedItems<PliVM>();

		}

		private PliVM ConverterEntityToVM(PliEntity item)
		{
			var pliMercadorias = _uowSciex.QueryStackSciex.PliMercadoria.Listar(x => x.IdPLI == item.IdPLI);

			var pliAplicacao = _uowSciex.QueryStackSciex.PliAplicacao.Listar(x => x.IdPliAplicacao == item.IdPLIAplicacao);

			var pliAnaliseVisual = _uowSciex.QueryStackSciex.PliAnaliseVisual.Listar(x => x.IdPLI == item.IdPLI);

			var taxaPlis = _uowSciex.QueryStackSciex.TaxaPli.Listar(x => x.IdPli == item.IdPLI);

			var erroProcessamento = _uowSciex.QueryStackSciex.ErroProcessamento.Listar(x => x.IdPli == item.IdPLI);



			var vm = new PliVM
			{
				NumeroPliConcatenado = item.Ano + "/" + item.NumeroPli.ToString("d6"),
				IdPliMercadoria = (pliMercadorias.FirstOrDefault() != null) ? pliMercadorias.FirstOrDefault().IdPliMercadoria : 0,
				NumeroALIReferencia = pliMercadorias.FirstOrDefault().Ali == null ? " " : pliMercadorias.FirstOrDefault().Ali.NumeroAli.ToString(),
				DescricaoAplicacao = pliAplicacao.FirstOrDefault().Descricao,
				DescricaoTipoDocumento = item.TipoDocumento == 1 ? EnumPliTipoDocumento.NORMAL.ToString() : (item.TipoDocumento == 2) ? EnumPliTipoDocumento.SUBSTITUTIVO.ToString() : "",
				CodigoPLIStatus = (int)item.StatusPli,
				DescricaoStatus = ((EnumPliStatus)item.StatusPli).ToString().Replace("_", " "),
				CodigoPliAplicao = (item.PliAplicacao != null) ? item.PliAplicacao.Codigo : 0,
				IsMercadorias = pliMercadorias.Any() && item.StatusPli == (byte)EnumPliStatus.EM_ELABORAÇÃO,
				DescricaoSetor = item.DescricaoSetor,
				CodigoSetor = (item.CodigoSetor == null) ? 0 : (int)item.CodigoSetor,
				NumCPFRepLegalSISCO = item.NumCPFRepLegalSISCO.CnpjCpfFormat(),
				Cnpj = item.Cnpj.CnpjCpfFormat(),
				QuantidadeMercadorias = pliMercadorias.Count,
				ValorTotalDolarMercadorias = pliMercadorias.Any() ? String.Format("{0:n2}", pliMercadorias.Sum(o => o.ValorTotalCondicaoVendaDolar)) : "0,00",
				ValorTotalRealMercadorias = pliMercadorias.Any() ? String.Format("{0:n2}", pliMercadorias.Sum(o => o.ValorTotalCondicaoVendaReal)) : "0,00",
				DataPliFormatada = item.DataCadastro.ToShortDateString(),
				DataEnvioPliFormatada = item.DataEnvioPli == null ? "" : item.DataEnvioPli.Value.ToShortDateString(),
				StatusTaxa = (taxaPlis.FirstOrDefault() != null) ? taxaPlis.FirstOrDefault().StatusTaxa : 0,
				QuantidadeErroProcessamento = erroProcessamento != null ? erroProcessamento.Count : 0,
				StatusALI = pliMercadorias.Count() > 1 ? pliMercadorias.Max(o => o.Ali.Status) : pliMercadorias.FirstOrDefault().Ali.Status,
				DataProcessamento = pliMercadorias.FirstOrDefault().Ali == null ? null : pliMercadorias.FirstOrDefault().Ali.DataCadastro.ToShortDateString(),
				AnaliseVisualStatus = pliAnaliseVisual.FirstOrDefault() == null ? null : pliAnaliseVisual.FirstOrDefault().StatusAnalise,
				AnaliseVisualStatusFormatado = pliAnaliseVisual.FirstOrDefault() == null ? null : pliAnaliseVisual.FirstOrDefault().StatusAnalise == 2 ? "EM ANÁLISE VISUAL" : pliAnaliseVisual.FirstOrDefault().StatusAnalise == 9 ? "ANÁLISE VISUAL PENDENTE" : " - ",
				DescricaoMotivo = pliAnaliseVisual.FirstOrDefault() == null ? null : pliAnaliseVisual.FirstOrDefault().DescricaoMotivo,
				DescricaoResposta = pliAnaliseVisual.FirstOrDefault() == null ? null : pliAnaliseVisual.FirstOrDefault().DescricaoResposta,
				AnalistaDesignado = pliAnaliseVisual.FirstOrDefault() == null ? null : String.IsNullOrEmpty(pliAnaliseVisual.FirstOrDefault().NomeAnalista) ? " - " : pliAnaliseVisual.FirstOrDefault().NomeAnalista,
				Ano = item.Ano,
				AnoPEXPAM = item.AnoPEXPAM,
				CodigoCNAE = item.CodigoCNAE,
				AnoPliSubstitutivo = null,
				Anexo = null,
				Bairro = null,
				CEP = null,
				
			};

			return vm;
		}

		public void RegrasSalvar(PliVM ConsultarPli)
		{

			PliHistoricoVM _PliHistoricoVM = new PliHistoricoVM();

			foreach (var itemLista in ConsultarPli.ListaPli)
			{

				// altera o status do pli
				var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == itemLista);
				var plivm = AutoMapper.Mapper.Map<PliVM>(pli);

				plivm.StatusPli = (byte)EnumPliStatus.AGUARDANDO_PROCESSAMENTO;
				plivm.StatusPliProcessamento = null;

				#region EXCLUIR ERRO PROCESSAMENTO
				string SQL = string.Empty;
				foreach (var item in pli.PLIMercadoria)
				{
					foreach (var itemD in item.PliDetalheMercadoria)
					{
						SQL = "DELETE FROM SCIEX_ERRO_PROCESSAMENTO WHERE EPR_ID_MERC_DETALHE = " + itemD.IdPliDetalheMercadoria.ToString();
						_uowSciex.CommandStackSciex.Salvar(SQL);
					}

					SQL = "DELETE FROM SCIEX_ERRO_PROCESSAMENTO WHERE EPR_ID_MERC_DETALHE = " + item.IdPliMercadoria.ToString();
					_uowSciex.CommandStackSciex.Salvar(SQL);
				}

				SQL = "DELETE FROM SCIEX_ERRO_PROCESSAMENTO WHERE PLI_ID = " + plivm.IdPLI.ToString();
				_uowSciex.CommandStackSciex.Salvar(SQL);
				#endregion

				_pliBll.Salvar(plivm);

				// salvar o histórico do pli
				_PliHistoricoVM.IdPLI = itemLista;
				_PliHistoricoVM.Observacao = ConsultarPli.Observacao;
				_PliHistoricoVM.DataEvento = DateTime.Now;
				_PliHistoricoVM.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuNomeUsuario;
				_PliHistoricoVM.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
				_PliHistoricoVM.StatusPli = (byte)EnumPliStatus.AGUARDANDO_PROCESSAMENTO;
				_PliHistoricoVM.DescricaoStatusPli = "PLI ENCAMINHADO PARA REPROCESSAMENTO";
				_pliHistoricoBll.Salvar(_PliHistoricoVM);

			}

		}

		public void Salvar(PliVM PliVM)
		{
			RegrasSalvar(PliVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public PliVM Selecionar(long? idPli)
		{
			var PliVM = new PliVM();

			if (!idPli.HasValue) { return PliVM; }

			var Pli = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == idPli);

			PliVM = AutoMapper.Mapper.Map<PliVM>(Pli);

			return PliVM;
		}

	}
}