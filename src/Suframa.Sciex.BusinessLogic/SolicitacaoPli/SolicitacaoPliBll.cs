using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.IO;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class SolicitacaoPliBll : ISolicitacaoPliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		private string CNPJ;

		public SolicitacaoPliBll(
			IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogado,
			IViewImportadorBll viewImportadorBll,
			IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_IUsuarioLogado = usuarioLogado;
			_IViewImportadorBll = viewImportadorBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ();

		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public string LeituraArquivoInserirDados()
		{
			try
			{
				List<EstruturaPropriaPliEntity> listaArquivosProprios =
					_uowSciex.QueryStackSciex.EstruturaPropriaPLI.Listar<EstruturaPropriaPliEntity>(
						o => (o.StatusProcessamentoArquivo == (byte)EnumEstruturaPropriaArquivoProcessamento.ENVIADO_A_SUFRAMA ||
							 o.StatusProcessamentoArquivo == (byte)EnumEstruturaPropriaArquivoProcessamento.EM_EXTRACAO)
							 && o.TipoArquivo == 1
						).OrderByDescending(o => o.StatusProcessamentoArquivo).ToList();


				StringBuilder SQL = new StringBuilder();
				StringBuilder SQLMercadoria = new StringBuilder();
				string SQLFabricanteFornecedor = string.Empty;
				string SQLFabricante = string.Empty;
				string tipoAplicacao = string.Empty;
				string codigoProduto = string.Empty;
				string codigoNCMMercadoria = string.Empty;

				if (listaArquivosProprios.Any())
				{
					foreach (EstruturaPropriaPliEntity item in listaArquivosProprios)
					{
						if (item.StatusProcessamentoArquivo == (byte)EnumEstruturaPropriaArquivoProcessamento.EM_EXTRACAO)
						{
							#region Exclusao de dados SOLIC
							SQL.AppendLine(@"declare @codigo_estrura bigint 
								declare @codigo_soli_pli bigint
								declare @codigo_soli_pli_mercadoria bigint

								begin
									set @codigo_estrura = " + item.IdEstruturaPropria.ToString() + @"

									while exists(select * from SCIEX_SOLIC_PLI where ESP_ID = @codigo_estrura)
									begin
										select top 1 @codigo_soli_pli = SPL_ID from SCIEX_SOLIC_PLI
										where ESP_ID = @codigo_estrura

										while exists(select * from SCIEX_SOLIC_PLI_MERCADORIA

														where SPL_ID = @codigo_soli_pli )
										begin
											select top 1 @codigo_soli_pli_mercadoria = SPM_ID from SCIEX_SOLIC_PLI_MERCADORIA
											where SPL_ID = @codigo_soli_pli

											delete from SCIEX_ERRO_PROCESSAMENTO where spl_id = @codigo_soli_pli 
											
											delete from SCIEX_SOLIC_PLI_DETALHE_MERCADORIA where SPM_ID = @codigo_soli_pli_mercadoria
											delete from SCIEX_SOLIC_FORNECEDOR_FABRICANTE where SPM_ID = @codigo_soli_pli_mercadoria
											delete from SCIEX_SOLIC_PLI_PROCESSO_ANUENTE where SPM_ID = @codigo_soli_pli_mercadoria

											delete from SCIEX_SOLIC_PLI_MERCADORIA where SPM_ID = @codigo_soli_pli_mercadoria

										end

										delete from SCIEX_SOLIC_PLI where SPL_ID = @codigo_soli_pli
									end
								end
								");

							_uowSciex.CommandStackSciex.Salvar(SQL.ToString());
							SQL = new StringBuilder();
							#endregion
						}

						int inscricaoCadastral = item.InscricaoCadastral.Value;

						ViewImportadorEntity viewImportador =
							_uowSciex.QueryStackSciex.ViewImportador.Selecionar(x => x.InscricaoCadastral == inscricaoCadastral);

						if (viewImportador != null && viewImportador.DescricaoSituacaoInscricao.ToUpper() == "ATIVA")
						{
							int qtdPli = 0;
							StringBuilder plis = new StringBuilder();
							string SQLDetalheMercadoria = string.Empty;

							#region SQLAtualizarEstruturaPropria

							DateTime dataAtual = DateTime.Now;
							string data = dataAtual.ToString("yyyy-MM-dd HH:mm:ss.fff");
							SQL.AppendLine(
								" UPDATE SCIEX_ESTRUTURA_PROPRIA SET ESP_DH_INICIO_PROCESSAMENTO = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
								" ESP_ST_PROCESSAMENTO_ARQUIVO = 2, ESP_DS_PENDENCIA_IMPORTADOR = NULL " +
								" WHERE ESP_ID = " + item.IdEstruturaPropria.ToString());

							_uowSciex.CommandStackSciex.Salvar(SQL.ToString());

							//limpar string
							SQL = new StringBuilder();

							#endregion

							#region Controle de execução

							ControleExecucaoServicoEntity objControle = new ControleExecucaoServicoEntity();
							objControle.DataHoraExecucaoInicio = GetDateTimeNowUtc();
							objControle.StatusExecucao = 0;
							objControle.IdListaServico = 16;
							objControle.MemoObjetoEnvio = "Tabela: SCIEX_ESTRUTURA_PROPRIA_ARQUIVO: " + item.IdEstruturaPropria.ToString();
							objControle.NomeCPFCNPJInteressado = item.NomeUsuarioEnvio;
							objControle.NumeroCPFCNPJInteressado = item.LoginUsuarioEnvio;
							_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(objControle);
							_uowSciex.CommandStackSciex.Save();


							//limpar string
							SQL = new StringBuilder();

							#endregion

							byte[] arquivo = item.EstruturaPropriaPliArquivo.Arquivo;
							StreamReader leitor = new StreamReader(new MemoryStream(arquivo));

							string linha;
							string registroAnterior = String.Empty;

							while ((linha = leitor.ReadLine()) != null)
							{
								if (linha.Length > 1)
								{
									string tipoRegistro = linha.Substring(0, 2);
									if (registroAnterior.Length == 0)
									{
										registroAnterior = tipoRegistro;
									}

									switch (tipoRegistro)
									{
										case "01":
											{
												if (registroAnterior == "03" || registroAnterior == "04" || registroAnterior == "05" || registroAnterior == "06" || registroAnterior == "07")
												{
													SQL.AppendLine(
														@"COMMIT 
													END TRY
													 BEGIN CATCH

														IF @@TRANCOUNT > 0--SE EXISTIREM TRANSAÇÕES, DESFAZE - LAS
														BEGIN
															ROLLBACK


															     SELECT
																	 ERROR_NUMBER() AS ErrorNumber
																	, ERROR_SEVERITY() AS ErrorSeverity
																	, ERROR_STATE() AS ErrorState
																	, ERROR_PROCEDURE() AS ErrorProcedure
																	, ERROR_LINE() AS ErrorLine
																	, ERROR_MESSAGE() AS ErrorMessage;
										
														END
													END CATCH "
													);

													qtdPli = qtdPli + 1;

													_uowSciex.CommandStackSciex.Salvar(SQL.ToString());

													SQL = new StringBuilder();

													SQL.AppendLine(
													  "DECLARE @CODIGO_PLI BIGINT " +
													  "DECLARE @CODIGO_PLI_MERCADORIA BIGINT " +
													  "DECLARE @CODIGO_PLI_DETALHEMERCADORIA BIGINT " +
													  "DECLARE @DESCRICAO_DETALHE_MERCADORIA VARCHAR(254) " +
													  " " +
													  "BEGIN TRY " +
													  "BEGIN TRANSACTION ");
												}
												else
												{
													SQL.AppendLine(
													  "DECLARE @CODIGO_PLI BIGINT " +
													  "DECLARE @CODIGO_PLI_MERCADORIA BIGINT " +
													  "DECLARE @CODIGO_PLI_DETALHEMERCADORIA BIGINT " +
													  "DECLARE @DESCRICAO_DETALHE_MERCADORIA VARCHAR(254) " +
													  " " +
													  "BEGIN TRY " +
													  "BEGIN TRANSACTION ");
												}

												tipoAplicacao = linha.Substring(265, 2).Trim();

												registroAnterior = "01";



												SQL.AppendLine(
													"INSERT INTO SCIEX_SOLIC_PLI ( " +
													" SPL_ST_SOLICITACAO," +
													" ESP_ID, " +
													" SPL_NU_CNPJ, " +
													" SPL_NU_PLI_IMPORTADOR, " +
													" SPL_NU_LI_REFERENCIA, " +
													" SPL_NU_PEXPAM, " +
													" SPL_NU_ANO_PEXPAM, " +
													" SPL_NU_CPF_REP_LEGAL_SISCOMEX, " +
													" SPL_CO_CNAE, " +
													" INS_CO, " +
													" SET_CO, " +
													" SET_DS, " +
													" SPL_TP_DOCUMENTO, " +
													" SPL_TP_ORIGEM, " +
													" IMP_DS_RAZAO_SOCIAL, " +
													" PAP_CO, SPL_ST_PLI_TECNOLOGIA_ASSISTIVA) " +
													" VALUES (" +
													" 1, " +
													item.IdEstruturaPropria.ToString() + ", " +
													"'" + linha.Substring(22, 14) + "', " + //CNPJ
													"'" + linha.Substring(2, 10) + "', " +//PLIIMPORTADOR
													"'" + linha.Substring(267, 10).TrimStart('0').Trim() + "', " +//LI REFERENCIA
													(linha.Substring(277, 8).Trim().Length > 0 ? linha.Substring(277, 8).Trim() : "0") + ", " + //NU_PEXPAM
													(linha.Substring(285, 4).Trim().Length > 0 ? linha.Substring(285, 4).Trim() : "0") + ", " + //ANO_PEXPAM
													"'" + linha.Substring(245, 11).Trim() + "', " + //NU_CPF_REPRESENTANTE_LEGAL
													linha.Substring(241, 4).Trim() + ", " +//CNAE
													linha.Substring(12, 9).Trim() + ", " +//INSCRICAO CADASTRAL
													"0," +//CODIGO SETOR
													"''," +//DESCRICAO SETOR
													linha.Substring(256, 1).Trim() + ", " +//TÌPO_DOCUMENTO
													"3, " +//TIPO_ORIGEM
													"'" + linha.Substring(39, 60).Trim() + "', " +//RAZAO SOCIAL
													(linha.Substring(265, 2).Trim().Length > 0 ? linha.Substring(265, 2).Trim() : "0") + ", " +
													item.StatusPLITecnologiaAssistiva +
													")"
												);

												SQL.AppendLine(" SET @CODIGO_PLI = @@IDENTITY ");

												SQL.AppendLine(
													"UPDATE SCIEX_CONTROLE_EXEC_SERVICO SET " +
													"CES_ST_EXECUCAO = 2, CES_ME_OBJETO_RETORNO = CES_ME_OBJETO_RETORNO + CAST(@CODIGO_PLI AS VARCHAR)+'; '" +
													"WHERE CES_ID = " + objControle.IdControleExecucaoServico.ToString()
													);

												plis.Append(linha.Substring(2, 10) + "; ");

												break;
											}
										case "09":
											{

												registroAnterior = "09";

												string codigoProdutoCompleto = linha.Substring(29, 11);
												string bemencomenda = string.Empty;
												string tipoMaterialUsado = string.Empty;

												codigoProduto = codigoProdutoCompleto;


												if (linha.Substring(100, 1) == "1" ||
													linha.Substring(100, 1) == "V" ||
													linha.Substring(100, 1) == "T" ||
													linha.Substring(100, 1) == "S")
												{
													bemencomenda = "1";
												}
												else
												{
													bemencomenda = "0";
												}

												if (linha.Substring(99, 1) == "1" ||
													linha.Substring(99, 1) == "V" ||
													linha.Substring(99, 1) == "T" ||
													linha.Substring(99, 1) == "S")
												{
													tipoMaterialUsado = "1";
												}
												else
												{
													tipoMaterialUsado = "0";
												}

												codigoNCMMercadoria = linha.Substring(21, 8);

												SQLMercadoria.AppendLine(
													"INSERT INTO SCIEX_SOLIC_PLI_MERCADORIA ( " +
													@"SPM_NU_PESO_LIQUIDO,
												SPM_QT_UNID_MEDIDA_ESTATISTICA,
												SPM_NU_COMUNICADO_COMPRA,												
												SPM_VL_CRA,												
												SPM_TP_BEM_ENCOMENDA,
												SPM_TP_MATERIAL_USADO,
												SPL_ID,
												SPR_CO_PRODUTO,
												SPR_CO_TP_PRODUTO,
												SPR_CO_MODELO_PRODUTO,
												MER_CO_NCM_MERCADORIA,
												SPM_VL_TOTAL_CONDICAO_VENDA,
												MOE_CO,
												INC_CO,
												RFB_CO_ENTRADA,
												PAI_CO_ORIGEM_MERCADORIA,	
												RFB_CO_DESPACHO,
												NLD_CO
												)
												VALUES (");

												SQLMercadoria.AppendLine(
													"CAST('" + linha.Substring(48, 10) + "." + linha.Substring(58, 5) + "' as numeric(15,5))," +
													"CAST('" + linha.Substring(63, 9) + "." + linha.Substring(72, 5) + "' as numeric(15,5)), " +
													"'" + linha.Substring(101, 13) + "', " +
													"CAST('" + linha.Substring(131, 2) + "." + linha.Substring(133, 2) + "' as numeric(4,2))," +
													"'" + bemencomenda + "', " +
													"'" + tipoMaterialUsado + "', " +
													"@CODIGO_PLI, " +
													codigoProdutoCompleto.Substring(0, 4).Trim() + ", " +
													(codigoProdutoCompleto.Substring(4, 3).Trim().Length > 0 ? codigoProdutoCompleto.Substring(4, 3).Trim() : "0") + ", " +
													(codigoProdutoCompleto.Substring(7, 4).Trim().Length > 0 ? codigoProdutoCompleto.Substring(7, 4).Trim() : "0") + ", " +
													linha.Substring(21, 8) + ", " +
													"CAST('" + linha.Substring(84, 13) + "." + linha.Substring(97, 2) + "' as numeric(15,2)), " +
													linha.Substring(78, 3) + ", " +
													"'" + linha.Substring(81, 3) + "', " + //CIF
													linha.Substring(114, 7) + ", " +
													linha.Substring(121, 3) + ", " +
													linha.Substring(124, 7) + ", " +
													(linha.Substring(40, 8).Trim().Length > 0 ? linha.Substring(40, 8).Trim() : "0") + ") "
													);

												SQLMercadoria.AppendLine(" SET @CODIGO_PLI_MERCADORIA = @@IDENTITY ");


												break;
											}
										case "08":
											{

												SQLFabricante = string.Empty;
												string tipoFornecedor = linha.Substring(220, 1);
												string paisFornecedor = linha.Substring(217, 3).Replace("'", "´").Trim();
												string paisFabricante = string.Empty;

												try
												{
													if (linha.Substring(398, 3).Length > 0)
													{
														paisFabricante = linha.Substring(398, 3).Replace("'", "´").TrimEnd();
													}
												}
												catch { }


												if (tipoFornecedor == "1")
												{
													paisFabricante = paisFornecedor;

													SQLFabricanteFornecedor = @"
														INSERT INTO SCIEX_SOLIC_FORNECEDOR_FABRICANTE
														   (SPM_ID
														   ,SFF_DS_FORNECEDOR
														   ,SFF_DS_LOGRADOURO_FORN
														   ,SFF_NU_FORN
														   ,SFF_DS_COMPLEMENTO_FORN
														   ,SFF_DS_CIDADE_FORN
														   ,SFF_DS_ESTADO_FORN
														   ,SFF_CO_PAIS_FORN
														   ,SFF_CO_AUSENCIA_FABRICANTE
														 )
														VALUES
														   (
															@CODIGO_PLI_MERCADORIA, 
															'" + linha.Substring(40, 60).Replace("'", "´").TrimEnd() + "'," +
																"'" + linha.Substring(100, 40).Replace("'", "´").TrimEnd() + "'," +
																"'" + linha.Substring(140, 6).Replace("'", "´").TrimEnd() + "'," +
																"'" + linha.Substring(146, 21).Replace("'", "´").TrimEnd() + "'," +
																"'" + linha.Substring(192, 25).Replace("'", "´").TrimEnd() + "'," +
																"'" + linha.Substring(167, 25).Replace("'", "´").TrimEnd() + "'," +
																"'" + linha.Substring(217, 3).Replace("'", "´").Trim() + "'," +
																"" + linha.Substring(220, 1) +
																")";
												}

												//RN06
												if (tipoFornecedor == "3")
												{
													SQLFabricante = @"
															UPDATE SCIEX_SOLIC_PLI_MERCADORIA 
															SET pai_co_origem_fabricante = " + paisFabricante +
															" WHERE SPM_ID = @CODIGO_PLI_MERCADORIA";
												}

												if (tipoFornecedor == "2")
												{
													SQLFabricanteFornecedor = @"
														INSERT INTO SCIEX_SOLIC_FORNECEDOR_FABRICANTE
														   (SPM_ID
														   ,SFF_DS_FORNECEDOR
														   ,SFF_DS_LOGRADOURO_FORN
														   ,SFF_NU_FORN
														   ,SFF_DS_COMPLEMENTO_FORN
														   ,SFF_DS_CIDADE_FORN
														   ,SFF_DS_ESTADO_FORN
														   ,SFF_CO_PAIS_FORN
														   ,SFF_CO_AUSENCIA_FABRICANTE
														   ,SFF_DS_FABRICANTE
														   ,SFF_DS_LOGRADOURO_FAB
														   ,SFF_NU_FAB
														   ,SFF_DS_COMPLEMENTO_FAB
														   ,SFF_DS_CIDADE_FAB
														   ,SFF_DS_ESTADO_FAB
														   ,SFF_CO_PAIS_FAB)
														VALUES
														   (
															@CODIGO_PLI_MERCADORIA, 
															'" + linha.Substring(40, 60).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(100, 40).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(140, 6).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(146, 21).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(192, 25).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(167, 25).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(217, 3).Replace("'", "´").Trim() + "'," +
																	"" + linha.Substring(220, 1) + "," +
																	"'" + linha.Substring(221, 60).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(281, 40).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(321, 6).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(327, 21).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(373, 25).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(348, 25).Replace("'", "´").TrimEnd() + "'," +
																	"'" + linha.Substring(398, 3).Replace("'", "´").TrimEnd() +
																	"')";
												}


												registroAnterior = "08";

												break;
											}
										case "10":
											{

												registroAnterior = "10";

												string numAtoDrawback = string.Empty;
												string numAgenciaSecex = string.Empty;

												try
												{
													numAtoDrawback = linha.Substring(62, linha.Length - 62);
												}
												catch
												{
													numAtoDrawback = "";
												}

												try
												{
													numAgenciaSecex = linha.Substring(57, 5);
												}
												catch
												{
													numAgenciaSecex = "";
												}

												SQLMercadoria.AppendLine(
												"UPDATE SCIEX_SOLIC_PLI_MERCADORIA SET " +
													"FLE_CO =" + (linha.Substring(45, 2).Trim().Length > 0 ? linha.Substring(45, 2) : "0") +
													",ALA_CO = " + (linha.Substring(41, 3).Trim().Length > 0 ? linha.Substring(41, 3) : "0") +
													",INF_CO =" + linha.Substring(53, 2) +
													",MOT_CO =" + linha.Substring(55, 2) +
													",MOP_CO =" + linha.Substring(48, 2) +
													",RTB_CO = " + linha.Substring(44, 1) +
													",SPM_NU_ATO_DRAWBACK = '" + numAtoDrawback + "'" +
													",SPM_NU_AGENCIA_SECEX='" + numAgenciaSecex + "' " +
													",SPM_TP_COBCAMBIAL=" + linha.Substring(47, 1) +
													",SPM_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO =" + linha.Substring(50, 3) +
													",SPM_TP_ACORDO_TARIFARIO = " + linha.Substring(40, 1) +
													"WHERE SPM_ID = @CODIGO_PLI_MERCADORIA");

												SQL.AppendLine(SQLMercadoria.ToString());
												SQL.AppendLine(SQLFabricanteFornecedor);
												SQL.AppendLine(SQLFabricante);

												SQLMercadoria = new StringBuilder();


												break;
											}
										case "03":
											{

												SQLDetalheMercadoria = string.Empty;
												registroAnterior = "03";

												string materiaPrima = string.Empty;

												try
												{
													materiaPrima = linha.Substring(390, linha.Length - 390);
												}
												catch
												{
													materiaPrima = "";
												}

												string SQLDetalhe = string.Empty;

												string detalhe = string.Empty;
												string part_number = string.Empty;
												string reffabricante = string.Empty;

												try
												{
													for (int i = 0; i < linha.Length; i++)
													{
														if (i >= 96 && i < 350)
														{
															detalhe = detalhe + linha[i];
														}
													}
												}
												catch { }

												try
												{
													for (int i = 0; i < linha.Length; i++)
													{
														if (i >= 370 && i < 390)
														{
															part_number = part_number + linha[i];
														}
													}
												}
												catch { }

												try
												{
													for (int i = 0; i < linha.Length; i++)
													{
														if (i >= 350 && i < 370)
														{
															reffabricante = reffabricante + linha[i];
														}
													}
												}
												catch { }



												SQLDetalhe =
													@"
												INSERT INTO SCIEX_SOLIC_PLI_DETALHE_MERCADORIA
											   (SPM_ID
											   ,SDM_DS_DETALHE
											   ,SDM_DS_PART_NUMBER
											   ,SDM_DS_REF_FABRICANTE
											   ,SDM_QT_UNID_COMERCIALIZADA
											   ,SDM_VL_UNITARIO_COND_VENDA
											   ,DME_CO_DETALHE_MERCADORIA
											   ,SDM_VL_CONDICAO_VENDA
											   ,UME_DS
											   ,SDM_DS_MAT_PRIMA_BASICA)
											   VALUES
											   (
												@CODIGO_PLI_MERCADORIA,
												'" + detalhe.TrimEnd().Replace("'", "'+char(39)+'") + "'," +
													"'" + part_number.TrimEnd() + "'," +
													"'" + reffabricante.TrimEnd() + "'," +
													"CAST('" + linha.Substring(44, 9) + "." + linha.Substring(53, 5) + "' as numeric(14,5)), " +
													"CAST('" + linha.Substring(78, 11) + "." + linha.Substring(89, 7) + "' as numeric(18,7)), " +
													"'" + linha.Substring(29, 4) + "'," +
													"CAST('" + linha.Substring(44, 9) + "." + linha.Substring(53, 5) + "' as numeric(14,5)) * CAST('" + linha.Substring(78, 11) + "." + linha.Substring(89, 7) + "' as numeric(18,7)), " +
													"'" + linha.Substring(58, 20) + "'," +
													"'" + materiaPrima.TrimEnd() + "'" +
													")";

												SQLDetalhe = SQLDetalhe + " SET @CODIGO_PLI_DETALHEMERCADORIA = @@IDENTITY";


												if (tipoAplicacao == "1") //tipo de aplicação industria
												{
													SQLDetalhe +=
														@" 
															SELECT
																@DESCRICAO_DETALHE_MERCADORIA = DME_DS_DETALHE_MERCADORIA
															FROM VW_SCIEX_DETALHE_MERCADORIA
															WHERE DME_CO_PRODUTO = " + codigoProduto.Substring(0, 4).Trim() +
																" AND dme_co_ncm_mercadoria = " + codigoNCMMercadoria +
																" AND dme_co_detalhe_mercadoria = " + linha.Substring(29, 4) +

															" UPDATE SCIEX_SOLIC_PLI_DETALHE_MERCADORIA SET SDM_DS_DETALHE = @DESCRICAO_DETALHE_MERCADORIA " +
															" WHERE SDM_ID = @CODIGO_PLI_DETALHEMERCADORIA ";
													;
												}


												SQL.AppendLine(SQLDetalhe);


												break;
											}
										case "06":
											{

												string SQLDetalhes = string.Empty;
												for (int i = 0; i < linha.Length; i++)
												{
													if (i >= 47)
													{
														SQLDetalhes = SQLDetalhes + linha[i];
													}
												}
												if (registroAnterior == tipoRegistro)
												{


													SQLDetalheMercadoria = SQLDetalheMercadoria + SQLDetalhes + " ";
												}
												else
												{
													SQLDetalheMercadoria = SQLDetalhes + " ";
												}

												registroAnterior = "06";

												string SQLDel =
													 "UPDATE SCIEX_SOLIC_PLI_DETALHE_MERCADORIA SET " +
													 "SDM_DS_COMPLEMENTO = '" + SQLDetalheMercadoria.Replace("'", "'+char(39)+'") + "'" +
													 "WHERE SDM_ID = @CODIGO_PLI_DETALHEMERCADORIA";

												SQL.AppendLine(SQLDel);


												break;
											}
										case "04":
											{

												SQLDetalheMercadoria = String.Empty;
												string SQLFF = string.Empty;

												SQLFF = @"
												INSERT INTO SCIEX_SOLIC_PLI_PROCESSO_ANUENTE (SPM_ID, SSP_NU_PROCESSO, SSP_SG_ORGAO_ANUENTE)  
                                                VALUES ( @CODIGO_PLI_MERCADORIA, " +
													"'" + linha.Substring(29, 20).TrimEnd() + "'," +
													"'" + linha.Substring(49, 10).TrimEnd() + "')";

												registroAnterior = "04";

												SQL.AppendLine(SQLFF);


												break;
											}
										case "05":
											{

												SQLDetalheMercadoria = String.Empty;

												string SQLNCMDestaque = string.Empty;
												SQLNCMDestaque =
													"UPDATE SCIEX_SOLIC_PLI_MERCADORIA SET " +
													" SPM_NU_NCM_DESTAQUE = '" + linha.Substring(29, 3) + "' " +
													"WHERE SPM_ID = @CODIGO_PLI_MERCADORIA";

												registroAnterior = "05";

												SQL.AppendLine(SQLNCMDestaque);

												break;
											}
										case "07":
											{

												SQLDetalheMercadoria = String.Empty;

												string SQLDetalhes = string.Empty;
												for (int i = 0; i < linha.Length; i++)
												{
													if (i >= 43)
													{
														SQLDetalhes = SQLDetalhes + linha[i];
													}
												}

												string SQLInformacoesComplementares = string.Empty;
												SQLInformacoesComplementares =
													"UPDATE SCIEX_SOLIC_PLI_MERCADORIA SET " +
													" SPM_DS_INFORMACAO_COMPLEMENTAR = '" + SQLDetalhes.Replace("'", "'+char(39)+'") + "' " +
													"WHERE SPM_ID = @CODIGO_PLI_MERCADORIA";

												registroAnterior = "07";

												SQL.AppendLine(SQLInformacoesComplementares);

												break;
											}
										default: break;
									}
								}
							}

							leitor.Close();

							SQL.AppendLine(
								@"COMMIT 
								END TRY
									BEGIN CATCH

									IF @@TRANCOUNT > 0--SE EXISTIREM TRANSAÇÕES, DESFAZE - LAS
									BEGIN
										ROLLBACK
																							
											SELECT
												ERROR_NUMBER() AS ErrorNumber
											, ERROR_SEVERITY() AS ErrorSeverity
											, ERROR_STATE() AS ErrorState
											, ERROR_PROCEDURE() AS ErrorProcedure
											, ERROR_LINE() AS ErrorLine
											, ERROR_MESSAGE() AS ErrorMessage;
										
									END
								END CATCH "
							);

							_uowSciex.CommandStackSciex.Salvar(SQL.ToString());
							qtdPli = qtdPli + 1;

							SQL = new StringBuilder();

							SQL.AppendLine(
								"UPDATE SCIEX_ESTRUTURA_PROPRIA SET " +
								" ESP_ST_PROCESSAMENTO_ARQUIVO = 3, ESP_QT_PLI_ARQUIVO = " + qtdPli.ToString() +
								//" , ESP_NU_LISTA_PLI = '" + plis.ToString() + "'" +
								" WHERE ESP_ID = " + item.IdEstruturaPropria.ToString());

							_uowSciex.CommandStackSciex.Salvar(SQL.ToString());

							SQL.AppendLine(
								" UPDATE SCIEX_CONTROLE_EXEC_SERVICO SET " +
								" CES_ST_EXECUCAO = 1, CES_DH_EXECUCAO_FIM= CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00'))" +
								" WHERE CES_ID = " + objControle.IdControleExecucaoServico.ToString()
								);

							_uowSciex.CommandStackSciex.Salvar(SQL.ToString());
						
						}
						else
						{
							SQL = new StringBuilder();
							SQL.AppendLine(
								"UPDATE SCIEX_ESTRUTURA_PROPRIA SET " +
								" ESP_DS_PENDENCIA_IMPORTADOR =  'SITUAÇÃO CADASTRAL DO IMPORTADOR NÃO ESTÁ ATIVA' " +
								" WHERE ESP_ID = " + item.IdEstruturaPropria.ToString());

							_uowSciex.CommandStackSciex.Salvar(SQL.ToString());
							SQL = new StringBuilder();

						}
					}
				}

				SQL = new StringBuilder();

				//executar a stored procedure

				SQL.Append("EXEC ST_VALIDACAO_ESTRUTURA_PROPRIA");
				_uowSciex.CommandStackSciex.Salvar(SQL.ToString());

				return "Serviço executado.";
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}
		}

		public IEnumerable<SolicitacaoPliVM> Listar(SolicitacaoPliVM solicitacaoPliVM)
		{
			var solicitacaopli = _uowSciex.QueryStackSciex.SolicitacaoPli.Listar<SolicitacaoPliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<SolicitacaoPliVM>>(solicitacaopli);
		}

		public SolicitacaoPliVM SalvarDoArquivo(SolicitacaoPliVM solicitacaoPliVM)
		{
			var solicitacaoPliEntity = AutoMapper.Mapper.Map<SolicitacaoPliEntity>(solicitacaoPliVM);
			_uowSciex.CommandStackSciex.SolicitacaoPli.Salvar(solicitacaoPliEntity);

			solicitacaoPliVM = AutoMapper.Mapper.Map<SolicitacaoPliVM>(solicitacaoPliEntity);
			return solicitacaoPliVM;
		}

		public PagedItems<SolicitacaoPliVM> ListarPaginado(SolicitacaoPliVM pagedFilter)
		{
			if (pagedFilter.Sort != null && pagedFilter.Sort.ToUpper() == "QTDERROSPLI")
			{
				pagedFilter.Sort = "";

				try
				{
					if (pagedFilter == null) { return new PagedItems<SolicitacaoPliVM>(); }
					var aladi = _uowSciex.QueryStackSciex.SolicitacaoPli.ListarPaginado<SolicitacaoPliVM>(o =>
						(
							(
								pagedFilter.IdSolicitacaoPli == -1 || o.IdSolicitacaoPli == pagedFilter.IdSolicitacaoPli
							)
							&&
							(
								pagedFilter.IdEstruturaPropriaPli == -1 || o.IdEstruturaPropriaPli == pagedFilter.IdEstruturaPropriaPli
							)
							&&
							(
								pagedFilter.StatusSolicitacao == 0 || o.StatusSolicitacao == pagedFilter.StatusSolicitacao
							)
						),
						pagedFilter);

					//foreach (SolicitacaoPliVM item in aladi.Items)
					//{
					//	try
					//	{
					//		PliEntity obj = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.NumeroPliImportador == item.NumeroPliImportador);

					//		item.NumeroPliSuframa =
					//			obj.Ano.ToString() + "/" + obj.NumeroPli.ToString("D6");
					//	}
					//	catch { }
					//}

					aladi.Items = aladi.Items.OrderBy(o => o.QtdErrosPli).ToList();

					return aladi;
				}
				catch (Exception ex)
				{
					//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

				}
				return new PagedItems<SolicitacaoPliVM>();
			}
			else
			{
				try
				{
					//var a = Convert.ToInt32("1") / Convert.ToInt32("0");
					if (pagedFilter == null) { return new PagedItems<SolicitacaoPliVM>(); }
					var aladi = _uowSciex.QueryStackSciex.SolicitacaoPli.ListarPaginado<SolicitacaoPliVM>(o =>
						(
							(
								pagedFilter.IdSolicitacaoPli == -1 || o.IdSolicitacaoPli == pagedFilter.IdSolicitacaoPli
							)
							&&
							(
								pagedFilter.IdEstruturaPropriaPli == -1 || o.IdEstruturaPropriaPli == pagedFilter.IdEstruturaPropriaPli
							)
							&&
							(
								pagedFilter.StatusSolicitacao == 0 || o.StatusSolicitacao == pagedFilter.StatusSolicitacao
							)
						),
						pagedFilter);

					return aladi;

				}

				catch (Exception ex)
				{

					//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

				}
			}

			return new PagedItems<SolicitacaoPliVM>();

		}
	}


}


