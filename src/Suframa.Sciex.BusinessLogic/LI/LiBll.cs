using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Ftp;
using Suframa.Sciex.CrossCutting.Texto;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class LiBll : ILiBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IPliBll _pliBll;
		private readonly IUsuarioPssBll _usuarioPssBll;

		private string CNPJ { get; set; }

		public LiBll(
			IUnitOfWorkSciex uowSciex,
			IUnitOfWork uowCadsuf,
			IUsuarioLogado usuarioLogado,
			IViewImportadorBll viewImportadorBll,
			IComplementarPLIBll complementarPLIBll,
			IUsuarioInformacoesBll usuarioInformacoesBll,
			IPliBll pliBll,
			IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IUsuarioLogado = usuarioLogado;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_pliBll = pliBll;
			_usuarioPssBll = usuarioPssBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ();
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public IEnumerable<LiVM> Listar()
		{
			var li = _uowSciex.QueryStackSciex.Li.Listar<AliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<LiVM>>(li);
		}

		public IEnumerable<object> ListarChave(LiVM liVM)
		{
			var lista = _uowSciex.QueryStackSciex.Li
				.Listar().Where(o =>
						(liVM.IdPliMercadoria == -1 || o.IdPliMercadoria == liVM.IdPliMercadoria)
					)
				.OrderBy(o => o.DataCadastro)
				.Select(
					s => new
					{
						id = s.NumeroLi,
						text = s.DataCadastro
					});
			return lista;
		}

		public PagedItems<LiVM> ListarPaginado(LiVM pagedFilter)
		{
			if (pagedFilter == null || this.CNPJ == null) { return new PagedItems<LiVM>(); }

			var li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
				(
				   (
						pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
					) &&
					(
						pagedFilter.AnoPli == -1 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
					) &&
				   (
					pagedFilter.NumeroLi == null || o.NumeroLi == pagedFilter.NumeroLi
					) && (
						(o.PliMercadoria.Pli.Cnpj == this.CNPJ && _IUsuarioLogado.Usuario.CpfCnpj.Length == 14) ||
						(o.PliMercadoria.Pli.Cnpj == this.CNPJ && _IUsuarioLogado.Usuario.CpfCnpj == o.PliMercadoria.Pli.NumeroResponsavelRegistro)
					)
				),
				pagedFilter);
			return li;
		}

		public LiVM RegrasSalvar(LiVM liVM)
		{
			var entityLI = AutoMapper.Mapper.Map<LiEntity>(liVM);
			_uowSciex.CommandStackSciex.Li.Salvar(entityLI);
			_uowSciex.CommandStackSciex.Save();

			var _liVM = AutoMapper.Mapper.Map<LiVM>(entityLI);
			return _liVM;
		}

		public LiVM Selecionar(int? numeroLI)
		{
			var liVM = new LiVM();
			if (!numeroLI.HasValue) { return liVM; }

			var li = _uowSciex.QueryStackSciex.Li.Selecionar(x => x.NumeroLi == numeroLI);
			if (li == null) { return liVM; }

			liVM = AutoMapper.Mapper.Map<LiVM>(li);
			return liVM;
		}

		public LiVM SelecionarPorMercadoria(long idMercadoria)
		{
			var liVM = new LiVM();

			var li = _uowSciex.QueryStackSciex.Li.Selecionar(x => x.IdPliMercadoria == idMercadoria);
			if (li == null) { return liVM; }

			liVM = AutoMapper.Mapper.Map<LiVM>(li);
			return liVM;
		}

		public LiVM Visualizar(LiVM liVM)
		{
			var entity = _uowSciex.QueryStackSciex.Li.Selecionar(x => x.NumeroLi == liVM.NumeroLi);
			var retorno = AutoMapper.Mapper.Map<LiVM>(entity);
			return retorno;
		}

		public void Salvar(LiVM liVM)
		{
			if (!liVM.ListaSelecionados.Any())
			{
				RegrasSalvar(liVM);
			}
			else
			{
				RegrasGerarCopiaPliParaCancelamento(liVM);
			}
		}

		public LiVM RegrasGerarCopiaPliParaCancelamento(LiVM liVM)
		{
			_pliBll.CopiarPliParaCancelamentoLi(liVM.ListaSelecionados);
			return new LiVM();
		}

		public void Deletar(int id)
		{
			var li = _uowSciex.QueryStackSciex.Li.Selecionar(s => s.NumeroLi == id);
			if (li != null)
			{
				_uowSciex.CommandStackSciex.Li.Apagar(li.NumeroLi.Value);
			}
		}

		public static byte[] ReceberArquivo(string arquivo, string usuario, string senha)
		{
			try
			{
				// Get the object used to communicate with the server.
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(arquivo);
				request.Method = WebRequestMethods.Ftp.DownloadFile;

				// This example assumes the FTP site uses anonymous logon.
				request.Credentials = new NetworkCredential(usuario, senha);

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();

				Stream responseStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(responseStream);

				byte[] streamInByte;

				using (MemoryStream ms = new MemoryStream())
				{
					responseStream.CopyTo(ms);
					streamInByte = ms.ToArray();
				}

				response.Close();
				reader.Close();

				return streamInByte;
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public string GerarArquivoSimulacaoALICancelamento()
		{
			StringBuilder arquivo = new StringBuilder();

			#region cabeçalho do arquivo

			arquivo.Append("24");
			arquivo.Append("04407029000143");

			string dataInicio = "01/01/" + DateTime.Now.Year.ToString();
			string dataAtual = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
			TimeSpan date = Convert.ToDateTime(dataAtual) - Convert.ToDateTime(dataInicio);
			arquivo.Append((date.Days + 1).ToString("D3"));

			arquivo.Append(DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2"));
			arquivo.Append("02");
			arquivo.Append("02");
			arquivo.Append("02");
			arquivo.Append("850");
			arquivo.AppendLine();

			#endregion

			List<AliEntity> listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO
			&& o.PliMercadoria.Li.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO);

			if (listaALI.Count > 0)
			{
				#region gera as linhas do arquivo

				int contador = 1;
				foreach (var item in listaALI)
				{
					long codigoLI = item.PliMercadoria.Li.NumeroLi.Value;

					//LI DEFERIDA
					arquivo.Append("01");
					arquivo.Append(codigoLI.ToString().PadRight(10, ' '));
					arquivo.Append("CANCELAMENTO EFETUADO COM SUCESSO!");

					arquivo.AppendLine();

					contador++;
				}

				#endregion

				#region verifica se existe para salvar

				var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "FTP retorno cancelamento SISCOMEX" && o.IdListaServico == 13);

				string localAqruivoFTP = configuracoesFTP.Valor;
				string usuario = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "Usuario FTP SISCOMEX" && o.IdListaServico == 13)?.Valor; // ConfigurationManager.AppSettings["FTP_USUARIO"];
				string senha = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "Senha FTP SISCOMEX" && o.IdListaServico == 13)?.Valor; //ConfigurationManager.AppSettings["FTP_SENHA"];

				if (!Ftp.VerificarSeExisteArquivo(localAqruivoFTP, usuario, usuario))
				{
					Ftp.EnviarArquivo(localAqruivoFTP, usuario, senha, arquivo.ToString());
					return "Arquivo gerado com sucesso";
				}
				else
				{
					return "Arquivo existente na pasta no momento do envio";
				}
				#endregion

			}

			return "Serviço executado sem gerar arquivo";
		}
		//Estoria 30
		public void ReceberArquivoLiCancelamento()
		{
			bool arquivoExistente = false;
			int contador = 1;
			string linha;
			long codigoLiArquivoRetorno = 0;
			string nomeDoArquivo = "SERPROCA.FILE";

			//BUSCA NO BANCO O ENDEREÇO DO FTP SUFRAMA
			var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "FTP retorno cancelamento SISCOMEX" && o.IdListaServico == 13);

			string localAqruivoFTP = configuracoesFTP.Valor;
			string usuario = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "Usuario FTP SISCOMEX" && o.IdListaServico == 13)?.Valor; // ConfigurationManager.AppSettings["FTP_USUARIO"];
			string senha = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "Senha FTP SISCOMEX" && o.IdListaServico == 13)?.Valor;  // ConfigurationManager.AppSettings["FTP_SENHA"];

			StreamReader file = null;

			//VERIFICA SE O ARQUIVO EXISTE ARQUIVO NO FTP
			if (Ftp.VerificarSeExisteArquivo(localAqruivoFTP, usuario, senha))
			{
				arquivoExistente = false;
				file =
					new StreamReader(
						 new MemoryStream(Ftp.ReceberArquivo(localAqruivoFTP, usuario, senha)),
						 Encoding.Default);
			}
			else
			{
				var registro = _uowSciex.QueryStackSciex.LiArquivoRetorno.Listar(o => o.StatusLeituraArquivo == 0 && o.TipoArquivoLI == 2 && o.NomeArquivo == "SERPROCA.FILE").OrderBy(o => o.DataRecepcaoArquivo).FirstOrDefault();
				if (registro.LiArquivo.ArquivoLIRetorno != null)
				{
					arquivoExistente = true;
					file =
						new StreamReader(
							 new MemoryStream(registro.LiArquivo.ArquivoLIRetorno), Encoding.Default);

				}
			}
			#region Regra 2 para salvar o arquivo gerado no banco
			if (!arquivoExistente)
			{
				//RECUPERA O ARQUIVO
				byte[] arquivo = Ftp.ReceberArquivo(localAqruivoFTP, usuario, senha);
				LiArquivoRetornoEntity _liArquivoRetornoEntity = new LiArquivoRetornoEntity();
				_liArquivoRetornoEntity.LiArquivo = new LiArquivoEntity();
				_liArquivoRetornoEntity.NomeArquivo = nomeDoArquivo;
				_liArquivoRetornoEntity.StatusLeituraArquivo = 0;
				_liArquivoRetornoEntity.DataRecepcaoArquivo = DateTime.Now;
				_liArquivoRetornoEntity.QuantidadeErroLI = 0;
				_liArquivoRetornoEntity.QuantidadeLI = 0;
				_liArquivoRetornoEntity.QuantidadeLIDeferida = 0;
				_liArquivoRetornoEntity.QuantidadeLIIndeferida = 0;
				_liArquivoRetornoEntity.TipoArquivoLI = (byte)EnumLiTipoArquivo.LI_ARQUIVO_CANCELAMENTO;
				_liArquivoRetornoEntity.LiArquivo.ArquivoLIRetorno = arquivo;
				_uowSciex.CommandStackSciex.LiArquivoRetorno.Salvar(_liArquivoRetornoEntity);
				_uowSciex.CommandStackSciex.Save();
				codigoLiArquivoRetorno = _liArquivoRetornoEntity.IdLiArquivoRetorno;


				//RN03
				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.IdListaServico = 13; //SALVAR ARQUIVO DE LI-CANCELAMENTO
				_controleExecucaoServicoVM.MemoObjetoEnvio = localAqruivoFTP;
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
				_controleExecucaoServicoVM.StatusExecucao = 1;
				_controleExecucaoServicoVM.MemoObjetoRetorno = arquivo.ToString();
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();

				//exclui o arquivo do FTP
				Ftp.DeleteFile(localAqruivoFTP, usuario, senha);
			}

			#endregion

			if (file != null)
			{
				StringBuilder SQLExecutar = new StringBuilder();

				//REGISTRA NO CONTROLE DE EXECUÇÃO DE SERVIÇOS
				SQLExecutar.AppendLine(
				"DECLARE @CONTROLE_SERVICO_COD INT " +

				"BEGIN TRY " +
				"BEGIN TRANSACTION; " +

				" INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO " +
				"(CES_DH_EXECUCAO_INICIO, CES_ST_EXECUCAO, CES_ME_OBJETO_ENVIO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID)" +
				"VALUES (" +
				"CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), 0,'" + localAqruivoFTP + "' ,'04407029000143','Administrador do Sistema – SUFRAMA', 13" +
				") " +

				"SET @CONTROLE_SERVICO_COD = @@identity"

				);

				int qtdLI = 0;
				int qtdLIDeferida = 0;
				int qtdLIIndeferida = 0;
				int qtdErros = 0;

				while ((linha = file.ReadLine()) != null)
				{
					//cabeçalho
					if (contador == 1)
					{
						Console.WriteLine(linha);
					}
					else
					{
						if (linha.Length > 1)
						{
							qtdLI = qtdLI + 1;
							string tipoRegistro = linha.Substring(0, 2);
							string numeroLI = string.Empty;
							string mensagemErro = string.Empty;

							switch (tipoRegistro)
							{
								case "01": //SIGNIFICA QUE O CANCELAMENTO FOI ACEITA PELO SISCOMEX, DEVENDO SER FINALIZADA
									{
										qtdLIDeferida = qtdLIDeferida + 1;
										numeroLI = linha.Substring(2, 10).Trim();
										mensagemErro = linha.Substring(12, linha.Length - 17);
										long numeroLIPesquisa = Convert.ToInt64(numeroLI);

										var listarLi = _uowSciex.QueryStackSciex.Li.Listar(o => o.NumeroLi == numeroLIPesquisa);

										if (listarLi.Count > 0)
										{
											foreach (var item in listarLi)
											{
												{
													//ATUALIZA O STATUS DA LI
													SQLExecutar.AppendLine(
														" UPDATE SCIEX_LI SET " +
															"  LI_ST = " + (int)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR +
															" ,LI_DH_CANCELAMENTO = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
														" WHERE LI_NU = " + numeroLI.ToString()
														);

													//ALTERA O STATUS DA ALI
													SQLExecutar.AppendLine(
														" UPDATE SCIEX_ALI SET " +
															"  ALI_ST = " + (int)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR +
														" WHERE PME_ID = " + item.PliMercadoria.IdPliMercadoria.ToString()
														);

													//REGISTRA HISTÓRICO RN 13
													SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
														"(PME_ID, AHI_DH_OPERACAO, AHI_ST_ALI_ANTERIOR, AHI_ST_LI_ANTERIOR, " +
														"AHI_NU_CPFCNPJ_RESPONSAVEL, AHI_NO_RESPONSAVEL, AHI_DS_OBSERVACAO) " +
														"VALUES (" + item.IdPliMercadoria.ToString() + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
														(byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO + "," +
														(byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO + "," +
														"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + "," +
														"'LI CANCELADA PELO IMPORTADOR')"
														);

													//RN 06 PARA LI SUBSTITUTIVA
													if (item.TipoLi == 2)
													{
														var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == item.PliMercadoria.IdPliMercadoria);
														if(ali.NumeroAtivaOrigem == 0)
														{
															var substituta = _uowSciex.QueryStackSciex.LiSubstituida.Selecionar(o=>o.IdLiSubstituta == item.IdPliMercadoria);
															substituta.Status = 2;

															_uowSciex.CommandStackSciex.LiSubstituida.Salvar(substituta);
															_uowSciex.CommandStackSciex.Save();


															var attAli = _uowSciex.QueryStackSciex.Ali.Selecionar(o=>o.IdPliMercadoria == substituta.IdLiOrigem);
															attAli.Status = 3;
															_uowSciex.CommandStackSciex.Ali.Salvar(attAli);
															_uowSciex.CommandStackSciex.Save();

															var attLi = _uowSciex.QueryStackSciex.Li.Selecionar(o=>o.IdPliMercadoria == substituta.IdLiOrigem);
															attLi.Status = 1;

															_uowSciex.CommandStackSciex.Li.Salvar(attLi);
															_uowSciex.CommandStackSciex.Save();
														}
													}
													//


													//DESFAZER O LANÇAMENTO ANTERIOR RN 11
													LancamentoEntity _lancamentoEntity = new LancamentoEntity();
													_lancamentoEntity.IdCodigoLancamento = 103; //Cancelamento de ALI
													_lancamentoEntity.Observacao = "LI CANCELADA PELO IMPORTADOR";
													_lancamentoEntity.IdPliMercadoria = item.IdPliMercadoria;

													var _viewImportador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == item.PliMercadoria.Pli.Cnpj);
													if (_viewImportador != null)
													{
														_lancamentoEntity.CodigoUnidadeCadastradora = (int)_viewImportador.CodigoUnidadeCadastradora;
													}

													_lancamentoEntity.DataCadastro = DateTime.Now;
													_lancamentoEntity.NumeroResponsavel = "04407029000143";
													_lancamentoEntity.IdPli = item.PliMercadoria.IdPLI;

													_uowSciex.CommandStackSciex.Lancamento.Salvar(_lancamentoEntity);
													_uowSciex.CommandStackSciex.Save();

													//ATUALIZAR COTA E CRÉDITO
													/**FALTA FAZER..**/
												}
											}
										}

										break;

									}

								case "99": //O CANCELAMENTO FOI INDEFERIDO PELO SISCOMEX
									{
										qtdLIIndeferida = qtdLIIndeferida + 1;
										numeroLI = linha.Substring(2, 10).Trim();
										mensagemErro = linha.Substring(12, linha.Length - 17);
										long numeroLIPesquisa = Convert.ToInt64(numeroLI);

										var listarLi = _uowSciex.QueryStackSciex.Li.Listar(o => o.NumeroLi == numeroLIPesquisa);

										if (listarLi.Count > 0)
										{
											foreach (var item in listarLi)
											{
												{
													//ATUALIZA LI
													SQLExecutar.AppendLine(
														" UPDATE SCIEX_LI SET " +
															"  LI_ST = " + (int)EnumLiStatus.LI_DEFERIDA +
														" WHERE LI_NU = " + numeroLI.ToString()
														);

													//ATUALIZA ALI
													SQLExecutar.AppendLine(
														" UPDATE SCIEX_ALI SET " +
															"  ALI_ST = " + (int)EnumAliStatus.ALI_DEFERIDA +
														" WHERE PME_ID = " + item.PliMercadoria.IdPliMercadoria.ToString()
														);

													//REGISTRA HISTÓRICO
													SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
														"(PME_ID, AHI_DH_OPERACAO, AHI_ST_ALI_ANTERIOR, AHI_ST_LI_ANTERIOR, " +
														"AHI_NU_CPFCNPJ_RESPONSAVEL, AHI_NO_RESPONSAVEL, AHI_DS_OBSERVACAO) " +
														"VALUES (" + item.IdPliMercadoria.ToString() + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
														(byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO + "," +
														(byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO + "," +
														"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + "," +
														"'LI SOLICITADA PARA CANCELAMENTO NÃO FOI ACEITA PELO SISCOMEX')"
														);
												}
											}
										}

										break;
									}
								case "97": //CANCELAMENTO DA LI APRESENTOU UM ERRO NA SUA ESTRUTURA E FOI INDEFERIDA PELO SISCOMEX
									{
										#region RN08
										qtdErros = qtdErros + 1;
										numeroLI = linha.Substring(2, 10).Trim();
										mensagemErro = linha.Substring(12, linha.Length - 17);
										long numeroLIPesquisa = Convert.ToInt64(numeroLI);

										var listarLi = _uowSciex.QueryStackSciex.Li.Listar(o => o.NumeroLi == numeroLIPesquisa);

										//NECESSITA TER LI PARA REALIZAR O CANCELAMENTO
										if (listarLi.Count > 0)
										{
											foreach (var item in listarLi)
											{
												{
													//ATUALIZA LI
													SQLExecutar.AppendLine(
														" UPDATE SCIEX_LI SET " +
															"  LI_ST = " + (int)EnumLiStatus.LI_DEFERIDA +
														" WHERE LI_NU = " + numeroLI.ToString()
														);

													//MANTÉM ALI
													SQLExecutar.AppendLine(
														" UPDATE SCIEX_ALI SET " +
															"  ALI_ST = " + (int)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO +
														" WHERE PME_ID = " + item.PliMercadoria.IdPliMercadoria.ToString()
														);

													//REGISTRA HISTÓRICO
													SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
														"(PME_ID, " +
														"AHI_DH_OPERACAO, " +
														"AHI_ST_ALI_ANTERIOR, " +
														"AHI_ST_LI_ANTERIOR, " +
														"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
														"AHI_NO_RESPONSAVEL, " +
														"AHI_DS_OBSERVACAO) " +
														"VALUES (" + item.IdPliMercadoria.ToString() + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
														(byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO + "," +
														(byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO + "," +
														"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + "," +
														"'LI NÃO DEFERIDA DEVIDO A PROBLEMAS ESTRUTURAIS')"
														);
												}
											}
										}
									}

									break;
								#endregion

								default:
									break;
							}
						}
					}
					contador++;
				}
				file.Close();

				#region REGRA DE NEGOCIO 08
				SQLExecutar.AppendLine(
					"UPDATE SCIEX_LI_ARQUIVO_RETORNO SET " +
					   "LAR_QT_LI = " + qtdLI.ToString() +
					   ",LAR_QT_LI_DEFERIDA = " + qtdLIDeferida.ToString() +
					   ",LAR_QT_LI_INDEFERIDA = " + qtdLIIndeferida.ToString() +
					   ",LAR_QT_LI_ERRO = " + qtdErros.ToString() +
					" WHERE LAR_ID =" + codigoLiArquivoRetorno
				);
				#endregion

				#region fimRegistroControleExecucao
				SQLExecutar.AppendLine(
				"					UPDATE SCIEX_CONTROLE_EXEC_SERVICO " +
				"					SET " +
				"						CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
				"						CES_ST_EXECUCAO = 2, " +
				"						CES_ME_OBJETO_RETORNO = 'Arquivo de LI (" + codigoLiArquivoRetorno.ToString() + " lido com sucesso' " +
				"					WHERE CES_ID = @CONTROLE_SERVICO_COD " +
					"COMMIT; " +
				"END TRY   " +
				"BEGIN CATCH " +
				"	IF @@TRANCOUNT > 0 " +
				"	BEGIN " +
				"		ROLLBACK " +
				"		SELECT " +
				"			 ERROR_NUMBER() AS ErrorNumber " +
				"			, ERROR_SEVERITY() AS ErrorSeverity " +
				"			, ERROR_STATE() AS ErrorState " +
				"			, ERROR_PROCEDURE() AS ErrorProcedure " +
				"			, ERROR_LINE() AS ErrorLine " +
				"			, ERROR_MESSAGE() AS ErrorMessage; " +
				"				BEGIN TRANSACTION " +
				"					UPDATE SCIEX_CONTROLE_EXEC_SERVICO " +
				"					SET " +
				"						CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
				"						CES_ST_EXECUCAO = 2, " +
				"						CES_ME_OBJETO_RETORNO = ERROR_MESSAGE() " +
				"					WHERE CES_ID = @CONTROLE_SERVICO_COD " +
				"				COMMIT " +
				"	END " +
				"END CATCH ");

				#endregion

				string sql = SQLExecutar.ToString();
				_uowSciex.CommandStackSciex.Salvar(sql);
			}
		}

		public void LerAquivoLI(string path)
		{
			string linha;
			long codigoLiArquivoRetorno = 0;
			string nomeDoArquivo = "SERPRO.FILE";
			StreamReader file = null;

			#region RN01
			var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == (int)EnumListaServico.SalvarArquivoLiNormal);

			// Alterado para atender a solicitacao dos analistas de sistemas.
			var configuracoesUsuario = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(x => x.IdParametroConfiguracao ==  14); // ConfigurationManager.AppSettings["FTP_USUARIO"];
			var configuracoesSenha = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(x => x.IdParametroConfiguracao == 15); //ConfigurationManager.AppSettings["FTP_SENHA"];

			string localArquivoFTP = configuracoesFTP.FirstOrDefault().Valor;
			string usuario = configuracoesUsuario.Valor;
			string senha = configuracoesSenha.Valor;
			#endregion

			#region RN02
			if (Ftp.VerificarSeExisteArquivo(localArquivoFTP, usuario, senha))
			{
				file =
					new StreamReader(
						 new MemoryStream(Ftp.ReceberArquivo(localArquivoFTP, usuario, senha)),
						 Encoding.Default);
			}
			else
			{
				//Registra o início da execução do salar Arquivo de resposta
				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.StatusExecucao = 0;
				_controleExecucaoServicoVM.MemoObjetoEnvio = "FTP: " + localArquivoFTP;
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoVM.IdListaServico = 12; //SALVAR ARQUIVO DE LI - NORMAL

				//Registra Fim de Execução
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.StatusExecucao = 2;
				_controleExecucaoServicoVM.MemoObjetoRetorno = "NÃO FOI POSSÍVEL ESTABELECER CONEXÃO COM O FTP " + localArquivoFTP;
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();

			}
			#endregion

			if (file != null)
			{
				//Comunicação com o FTP para receber o arquivo
				byte[] arquivo = Ftp.ReceberArquivo(localArquivoFTP, usuario, senha);

				//Salva o arquivo na tabela retorno arquivo
				LiArquivoRetornoEntity _liArquivoRetornoEntity = new LiArquivoRetornoEntity();
				_liArquivoRetornoEntity.NomeArquivo = nomeDoArquivo;
				_liArquivoRetornoEntity.StatusLeituraArquivo = 0;
				_liArquivoRetornoEntity.DataRecepcaoArquivo = DateTime.Now;
				_liArquivoRetornoEntity.QuantidadeErroLI = 0;
				_liArquivoRetornoEntity.QuantidadeLI = 0;
				_liArquivoRetornoEntity.QuantidadeLIDeferida = 0;
				_liArquivoRetornoEntity.QuantidadeLIIndeferida = 0;
				_liArquivoRetornoEntity.TipoArquivoLI = (byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL;
				//Salva também na tabela li arquivo
				_liArquivoRetornoEntity.LiArquivo = new LiArquivoEntity();
				_liArquivoRetornoEntity.LiArquivo.ArquivoLIRetorno = arquivo;
				_uowSciex.CommandStackSciex.LiArquivoRetorno.Salvar(_liArquivoRetornoEntity);
				_uowSciex.CommandStackSciex.Save();

				codigoLiArquivoRetorno = _liArquivoRetornoEntity.IdLiArquivoRetorno;

				//Finaliza o serviço de salvar o arquivo LI
				//Inicia o serviço de salvar o arquivo LI
				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.IdListaServico = 12; // SALVAR ARQUIVO DE LI-NORMAL
				_controleExecucaoServicoVM.MemoObjetoEnvio = localArquivoFTP;
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.StatusExecucao = 1;
				_controleExecucaoServicoVM.MemoObjetoRetorno = "Tabela: SCIEX_LI_ARQUIVO_RETORNO, Campo LAR_ID:" + _liArquivoRetornoEntity.IdLiArquivoRetorno;
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";

				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();

				//exclui o arquivo do FTP
				//Ftp.DeleteFile(localArquivoFTP, usuario, senha);

				#region RN04
				var li_arquivo_retorno = _uowSciex.QueryStackSciex.LiArquivoRetorno.Listar(o => o.StatusLeituraArquivo == 0 && o.TipoArquivoLI == 1);
				#endregion

				StringBuilder SQLExecutar = new StringBuilder();

				if (li_arquivo_retorno.Count > 0)
				{
					foreach (var item in li_arquivo_retorno)
					{
						int contador = 0;

						int qtdLI = 0;
						int qtdLIDeferida = 0;
						int qtdLIIndeferida = 0;
						int qtdErros = 0;
						byte statusALIAnterior;
						byte statusLIAnterior;

						file = new StreamReader(new MemoryStream(item.LiArquivo.ArquivoLIRetorno));


						while ((linha = file.ReadLine()) != null)
						{

							//cabeçalho       
							if (contador == 0)
							{
								Console.WriteLine(linha);
							}
							else
							{
								if (linha.Length > 1)
								{
									qtdLI = qtdLI + 1;

									string tipoRegistro = linha.Substring(0, 2);

									string numeroALI = string.Empty;
									string numeroLIProtocolada = string.Empty;
									string numeroLI = string.Empty;
									string codigoStatusDiagnostico = string.Empty;
									string dataGeracaoDiagnostico = string.Empty;
									string mensagemErro = string.Empty;


									#region REGRA DE NEGOCIO 04
									switch (tipoRegistro)
									{
										case "01": //ALI foi autorizada pelo SISCOMEX (DEFERIDA)
											{
												qtdLIDeferida = qtdLIDeferida + 1;

												numeroALI = linha.Substring(2, 15).Trim();
												numeroLIProtocolada = linha.Substring(17, 10);
												numeroLI = linha.Substring(27, 10);
												dataGeracaoDiagnostico = linha.Substring(37, 8);
												long numeroALIPesquisa = Convert.ToInt64(numeroALI);

												var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.PLIMercadoria.Any(x => x.Ali.NumeroAli == numeroALIPesquisa));
												var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);

												if (ali != null)
												{
													var listaLI = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

													string ano = dataGeracaoDiagnostico.Substring(0, 4);
													string mes = dataGeracaoDiagnostico.Substring(4, 2);
													string dia = dataGeracaoDiagnostico.Substring(6, 2);


													//Inicia o controle de execução de serviço
													ControleExecucaoServicoEntity _controleExecucaoServico = new ControleExecucaoServicoEntity();
													_controleExecucaoServico.DataHoraExecucaoInicio = GetDateTimeNowUtc();
													_controleExecucaoServico.StatusExecucao = 0;
													_controleExecucaoServico.IdListaServico = 14;
													_controleExecucaoServico.NomeCPFCNPJInteressado = pli.RazaoSocial;
													_controleExecucaoServico.NumeroCPFCNPJInteressado = pli.Cnpj;

													#region RN06

													if (listaLI != null && listaLI.IdPliMercadoria > 0)
													{
														SQLExecutar.AppendLine(
															"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
															"(" +
															"	PME_ID," +
															"	LAR_ID," +
															"	LI_NU," +
															"	LI_NU_LI_PROTOCOLADA," +
															"	LI_DT_GERACAO," +
															"	LI_DS_MENSAGEM," +
															"	LI_NU_DIAGNOSTICO_ERRO," +
															"	LI_ST," +
															"	LHE_TP_ERRO," +
															"	LI_DH_CADASTRO," +
															"	LI_DH_CANCELAMENTO" +
															")" +
															"" +
															"SELECT " +
															"	PME_ID," +
															"	LAR_ID," +
															"	LI_NU, " +
															"	LI_NU_LI_PROTOCOLADA, " +
															"	LI_DT_GERACAO, " +
															"	LI_DS_MENSAGEM, " +
															"	LI_NU_DIAGNOSTICO_ERRO, " +
															"	LI_ST, " +
															"	1, " +
															"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
															"	LI_DH_CANCELAMENTO " +
															"FROM SCIEX_LI WHERE PME_ID = " + listaLI.IdPliMercadoria
															);

														//var liHistorico = _uowSciex.QueryStackSciex.Li.Selecionar(o=>o.IdPliMercadoria == listaLI.IdPliMercadoria);
														//LiHistoricoErroEntity historicoErro = new LiHistoricoErroEntity();
														//historicoErro.IdPliMercadoria = liHistorico.IdPliMercadoria;
														//historicoErro.IdLIArquivoRetorno = liHistorico.IdLiArquivoRetorno;
														//historicoErro.NumeroLI = liHistorico.NumeroLi;
														//historicoErro.NumeroLIProtocolo = liHistorico.NumeroProtocoloLI;
														//historicoErro.DataGeração = liHistorico.DataGeracaoLI;
														//historicoErro.MensagemErro = liHistorico.MensagemErroLI;
														//historicoErro.CodigoDiagnosticoErro = liHistorico.CodigoErroDiagnostico;
														//historicoErro.Status = liHistorico.Status;
														//historicoErro.TipoErroLiH = 1;
														//historicoErro.DataCadastro = liHistorico.DataCadastro;
														//historicoErro.DataCancelamento = liHistorico.DataCancelamento;

														//_uowSciex.CommandStackSciex.LiHistoricoErro.Salvar(historicoErro, true);
														//_uowSciex.CommandStackSciex.Save();

														//Busca o status da LI Anterior
														statusLIAnterior = listaLI.Status;

														//Busca o status da ALI Anterior
														statusALIAnterior = ali.Status;

														//Registra Histórico

														if (listaLI.MensagemErroLI != null)
														{
															SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
																"(PME_ID, " +
																"AHI_DH_OPERACAO, " +
																"AHI_ST_ALI_ANTERIOR, " +
																"AHI_ST_LI_ANTERIOR, " +
																"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
																"AHI_NO_RESPONSAVEL, " +
																"AHI_DS_OBSERVACAO) " +

																"VALUES (" + ali.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																(byte)statusALIAnterior + "," +
																(byte)statusLIAnterior + "," +
																"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" +
																"," + "'" + listaLI.MensagemErroLI + "')");

															//AliHistoricoEntity aliHistorico = new AliHistoricoEntity();
															//aliHistorico.IdPliMercadoria = ali.IdPliMercadoria;
															//aliHistorico.DataOperacao = DateTime.Now;
															//aliHistorico.StatusAliAnterior = (byte)statusALIAnterior;
															//aliHistorico.StatusLiAnterior = (byte)statusLIAnterior;
															//aliHistorico.CPFCNPJResponsavel = "04407029000143";
															//aliHistorico.NomeResponsavel = "Administrador do Sistema - SUFRAMA";
															//aliHistorico.Observacao = listaLI.MensagemErroLI;

															//_uowSciex.CommandStackSciex.AliHistorico.Salvar(aliHistorico);
															//_uowSciex.CommandStackSciex.Save();

														}

														SQLExecutar.AppendLine(
															"UPDATE SCIEX_LI SET " +
																"LAR_ID = " + item.IdLiArquivoRetorno +
																",LI_NU = " + numeroLI +
																",LI_ST = 1" +
																",LI_TP = " + ali.TipoAli +
																",LI_DH_CADASTRO = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
																",LI_NU_LI_PROTOCOLADA = " + numeroLIProtocolada +
																",LI_DT_GERACAO = '" + ano + "-" + mes + "-" + dia + "'" +
																",LI_DS_MENSAGEM = NULL " +
																",LI_NU_DIAGNOSTICO_ERRO = NULL " +
																",LI_DH_CANCELAMENTO = NULL " +
																" WHERE PME_ID = " + listaLI.IdPliMercadoria
															);



														//atualizar a ALI
														SQLExecutar.AppendLine(
															" UPDATE SCIEX_ALI SET " +
															 "  ALI_ST = " + (int)EnumAliStatus.ALI_DEFERIDA +
															"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
															" WHERE ALI_NU = " + ali.NumeroAli.ToString()
															);

														//_uowSciex.CommandStackSciex.DetachEntries();
														//var attLi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.IdPliMercadoria == listaLI.IdPliMercadoria);
														//attLi.IdLiArquivoRetorno = item.IdLiArquivoRetorno;
														//attLi.NumeroLi = Convert.ToInt64(numeroLI);
														//attLi.Status = 1;
														//attLi.TipoLi = ali.TipoAli;
														//attLi.DataCadastro = DateTime.Now;
														//attLi.NumeroProtocoloLI = Convert.ToInt64(numeroLIProtocolada);
														//attLi.DataGeracaoLI = DateTime.Parse(ano + "-" + mes + "-" + dia);

														//_uowSciex.CommandStackSciex.Li.Salvar(attLi);
														//_uowSciex.CommandStackSciex.Save();

														//var attAli = _uowSciex.QueryStackSciex.Ali.Selecionar(o=>o.NumeroAli == ali.NumeroAli);
														//attAli.Status = (int)EnumAliStatus.ALI_DEFERIDA;
														//attAli.DataRespostaSISCOMEX = DateTime.Parse(ano + "-" + mes + "-" + dia);

														//_uowSciex.CommandStackSciex.Ali.Salvar(attAli);
														//_uowSciex.CommandStackSciex.Save();


														//Se LI Substitutiva, substituir Li de referencia RN14
														if (ali.TipoAli == 2)
														{
															//AtualizaLI
															var pliDeReferencia = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == pli.IdPLI);
															var numeroLiReferencia = Convert.ToInt64(pliDeReferencia.NumeroLIReferencia.Trim());
															var liDeReferencia = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLiReferencia);
															//liDeReferencia.Status = 6;
															//_uowSciex.CommandStackSciex.Li.Salvar(liDeReferencia);
															//_uowSciex.CommandStackSciex.Save();

															//ATUALIZA A LI
															SQLExecutar.AppendLine(
															"UPDATE SCIEX_LI SET " +
															" LI_ST = " + (int)EnumLiStatus.LI_SUBSTITUIDA +
															" WHERE  PME_ID = " + liDeReferencia.IdPliMercadoria.ToString());

															//var aliref = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == liDeReferencia.IdPliMercadoria);
															//aliref.Status = 9;
															//aliref.PliMercadoria = null;
															//_uowSciex.CommandStackSciex.Ali.Salvar(aliref);
															//_uowSciex.CommandStackSciex.Save();

															//ATUALIZA A ALI
															SQLExecutar.AppendLine(
																"UPDATE SCIEX_ALI SET " +
																"ALI_ST = " + (int)EnumAliStatus.ALI_SUBSTITUÍDA +
																" WHERE ALI_NU = " + liDeReferencia.IdPliMercadoria.ToString());

															var listLiOrigem = _uowSciex.QueryStackSciex.LiSubstituida.Listar(o => o.IdLiSubstituida == liDeReferencia.IdPliMercadoria || o.IdLiSubstituta == liDeReferencia.IdPliMercadoria);
															if (listLiOrigem == null || listLiOrigem.Count == 0)
															{
																//_uowSciex.CommandStackSciex.DetachEntries();
																//LiSubstituidaEntity liSubs = new LiSubstituidaEntity();
																//liSubs.IdLiSubstituida = liDeReferencia.IdPliMercadoria;
																//liSubs.IdLiSubstituta = ali.IdPliMercadoria;
																//liSubs.IdLiOrigem = liDeReferencia.IdPliMercadoria;
																//liSubs.NumeroLsu = 1;
																//liSubs.Status = 0; //Ativo
																//liSubs.DataOperacao = DateTime.Now;

																//_uowSciex.CommandStackSciex.LiSubstituida.Salvar(liSubs, true);
																//_uowSciex.CommandStackSciex.Save();

																SQLExecutar.AppendLine(
																"INSERT INTO SCIEX_LI_SUBSTITUIDA " +
																"( pme_id_substituida," +
																"  pme_id_substituta, " +
																"  pme_id_origem, " +
																"  lsu_nu, " +
																"  lsu_st, " +
																"  lsu_dh_operacao)" +
																"VALUES (" +
																	liDeReferencia.IdPliMercadoria.ToString() + "," +
																	ali.IdPliMercadoria.ToString() + "," +
																	liDeReferencia.IdPliMercadoria + "," +
																	1 + "," +
																	0 + "," +
																	" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
																") ");
															}
															else
															{
																//AlteraStatus do Substituido
																foreach (var item1 in listLiOrigem)
																{
																	//item1.Status = 1; //Substituido
																	//_uowSciex.CommandStackSciex.LiSubstituida.Salvar(item1);
																	//_uowSciex.CommandStackSciex.Save();

																	SQLExecutar.AppendLine(
																	"UPDATE SCIEX_LI_SUBSTITUIDA SET " +
																	"lsu_st = " + 1 +
																	" WHERE pme_id_substituida = " + item1.IdLiSubstituida);
																}
																var idOrigem = listLiOrigem.FirstOrDefault().IdLiOrigem;
																var count = _uowSciex.QueryStackSciex.LiSubstituida.Contar(o => o.IdLiOrigem == idOrigem) + 1;

																//_uowSciex.CommandStackSciex.DetachEntries();
																////Insere a nova substituicao
																//LiSubstituidaEntity liSubs = new LiSubstituidaEntity();
																//liSubs.IdLiSubstituida = liDeReferencia.IdPliMercadoria;
																//liSubs.IdLiSubstituta = ali.IdPliMercadoria;
																//liSubs.IdLiOrigem = listLiOrigem.FirstOrDefault().IdLiOrigem;
																//byte count = Convert.ToByte(listLiOrigem.Count + 1);
																//liSubs.NumeroLsu = count;
																//liSubs.Status = 0; //Ativo
																//liSubs.DataOperacao = DateTime.Now;

																//_uowSciex.CommandStackSciex.LiSubstituida.Salvar(liSubs);
																//_uowSciex.CommandStackSciex.Save();

																SQLExecutar.AppendLine(
																"INSERT INTO SCIEX_LI_SUBSTITUIDA " +
																"( pme_id_substituida," +
																"  pme_id_substituta, " +
																"  pme_id_origem, " +
																"  lsu_nu, " +
																"  lsu_st, " +
																"  lsu_dh_operacao) " +
																"VALUES (" +
																	liDeReferencia.IdPliMercadoria.ToString() + "," +
																	ali.IdPliMercadoria.ToString() + "," +
																	listLiOrigem.FirstOrDefault().IdLiOrigem + "," +
																	count + "," +
																	0 + "," +
																	" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
																") ");
															}
														}

														if (ali.TipoAli == 3)
														{
															var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == ali.IdPliMercadoria);
															var numeroLiRetificador = Convert.ToInt64(pliMercadoria.NumeroLiRetificador);
															var liRetificador = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLiRetificador);
															var aliRetificador = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == liRetificador.IdPliMercadoria);

															//Registra Histórico
															SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
																"(PME_ID, " +
																"AHI_DH_OPERACAO, " +
																"AHI_ST_ALI_ANTERIOR, " +
																"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
																"AHI_NO_RESPONSAVEL) " +
																"VALUES (" + aliRetificador.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																(byte)aliRetificador.Status + "," +
																"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + ")");

															//atualizar a ALI
															SQLExecutar.AppendLine(
																"UPDATE SCIEX_ALI SET " +
																"ALI_ST = " + (int)EnumAliStatus.ALI_RETIFICADA +
																" WHERE PME_ID = " + aliRetificador.IdPliMercadoria.ToString()
																);

															SQLExecutar.AppendLine(
															"UPDATE SCIEX_LI SET " +
															" LI_ST = " + (int)EnumLiStatus.LI_RETIFICADA +
															" WHERE  PME_ID = " + liRetificador.IdPliMercadoria.ToString());
														}


														//Finaliza o controle de execução
														_controleExecucaoServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
														_controleExecucaoServico.StatusExecucao = 1;
														_controleExecucaoServico.MemoObjetoRetorno = "Tabela: SCIEX_LI, Campo pme_id: " + ali.IdPliMercadoria; ;
														_controleExecucaoServico.MemoObjetoEnvio = "Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + item.IdLiArquivoRetorno;
														_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico);
														_uowSciex.CommandStackSciex.Save();

													}
													else
													{
														//INSERIR A LI
														SQLExecutar.AppendLine(
															"INSERT INTO SCIEX_LI " +
															"( PME_ID," +
															"  LAR_ID, " +
															"  LI_NU, " +
															"  LI_ST, " +
															"  LI_TP, " +
															"  LI_DH_CADASTRO, " +
															"  LI_NU_LI_PROTOCOLADA, " +
															"  LI_DT_GERACAO) " +
															"VALUES (" +
																ali.IdPliMercadoria.ToString() + "," +
																item.IdLiArquivoRetorno + "," +
																numeroLI + "," +
																(byte)EnumLiStatus.LI_DEFERIDA + "," +
																ali.TipoAli + "," +
																" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																numeroLIProtocolada + "," +
																"'" + ano + "-" + mes + "-" + dia + "'" +
															") ");

														//Busca o status da ALI Anterior
														statusALIAnterior = ali.Status;

														//Registra Histórico
														SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
															"(PME_ID, " +
															"AHI_DH_OPERACAO, " +
															"AHI_ST_ALI_ANTERIOR, " +
															"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
															"AHI_NO_RESPONSAVEL) " +
															"VALUES (" + ali.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
															(byte)statusALIAnterior + "," +
															"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + ")");

														//atualizar a ALI
														SQLExecutar.AppendLine(
															"UPDATE SCIEX_ALI SET " +
															"ALI_ST = " + (int)EnumAliStatus.ALI_DEFERIDA + "," +
															"ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
															" WHERE ALI_NU = " + ali.NumeroAli.ToString()
															);

														//Se LI Substitutiva, substituir Li de referencia RN14
														if (ali.TipoAli == 2)
														{
															var pliDeReferencia = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == pli.IdPLI);
															var numeroLiReferencia = Convert.ToInt64(pliDeReferencia.NumeroLIReferencia.Trim());
															var liDeReferencia = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLiReferencia);

															//liDeReferencia.Status = 6;
															//_uowSciex.CommandStackSciex.Li.Salvar(liDeReferencia);
															//_uowSciex.CommandStackSciex.Save();

															SQLExecutar.AppendLine(
															"UPDATE SCIEX_LI SET " +
															" LI_ST = " + (int)EnumLiStatus.LI_SUBSTITUIDA +
															" WHERE  PME_ID = " + liDeReferencia.IdPliMercadoria.ToString());

															//var aliref = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == liDeReferencia.IdPliMercadoria);
															//aliref.Status = 9;
															//aliref.PliMercadoria = null;
															//_uowSciex.CommandStackSciex.Ali.Salvar(aliref);
															//_uowSciex.CommandStackSciex.Save();

															SQLExecutar.AppendLine(
															"UPDATE SCIEX_ALI SET " +
															"ALI_ST = " + (int)EnumAliStatus.ALI_SUBSTITUÍDA +
															" WHERE PME_ID = " + liDeReferencia.IdPliMercadoria.ToString());

															var listLiOrigem = _uowSciex.QueryStackSciex.LiSubstituida.Listar(o => o.IdLiSubstituida == liDeReferencia.IdPliMercadoria || o.IdLiSubstituta == liDeReferencia.IdPliMercadoria);
															if (listLiOrigem == null || listLiOrigem.Count == 0)
															{
																SQLExecutar.AppendLine(
																"INSERT INTO SCIEX_LI_SUBSTITUIDA " +
																"( pme_id_substituida," +
																"  pme_id_substituta, " +
																"  pme_id_origem, " +
																"  lsu_nu, " +
																"  lsu_st, " +
																"  lsu_dh_operacao) " +
																"VALUES (" +
																	liDeReferencia.IdPliMercadoria.ToString() + "," +
																	ali.IdPliMercadoria.ToString() + "," +
																	liDeReferencia.IdPliMercadoria + "," +
																	1 + "," +
																	0 + "," +
																	" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
																") ");

															}
															else
															{
																//AlteraStatus do Substituido
																foreach (var item1 in listLiOrigem)
																{
																	//item1.Status = 1; //Substituido
																	//_uowSciex.CommandStackSciex.LiSubstituida.Salvar(item1);
																	//_uowSciex.CommandStackSciex.Save();

																	SQLExecutar.AppendLine(
																	"UPDATE SCIEX_LI_SUBSTITUIDA SET " +
																	"lsu_st = " + 1 +
																	" WHERE pme_id_substituida = " + item1.IdLiSubstituida);
																}
																var idOrigem = listLiOrigem.FirstOrDefault().IdLiOrigem;
																var count = _uowSciex.QueryStackSciex.LiSubstituida.Contar(o => o.IdLiOrigem == idOrigem) + 1;
																

																//byte count = Convert.ToByte(listLiOrigem.Count + 1);

																//_uowSciex.CommandStackSciex.DetachEntries();
																//Insere a nova substituicao
																//LiSubstituidaEntity liSubs = new LiSubstituidaEntity();
																//liSubs.IdLiSubstituida = liDeReferencia.IdPliMercadoria;
																//liSubs.IdLiSubstituta = ali.IdPliMercadoria;
																//liSubs.IdLiOrigem = listLiOrigem.FirstOrDefault().IdLiOrigem;
																////byte count = Convert.ToByte(listLiOrigem.Count + 1);
																//liSubs.NumeroLsu = count;
																//liSubs.Status = 0; //Ativo
																//liSubs.DataOperacao = DateTime.Now;

																//_uowSciex.CommandStackSciex.LiSubstituida.Salvar(liSubs);
																//_uowSciex.CommandStackSciex.Save();

																SQLExecutar.AppendLine(
																"INSERT INTO SCIEX_LI_SUBSTITUIDA " +
																"( pme_id_substituida," +
																"  pme_id_substituta, " +
																"  pme_id_origem, " +
																"  lsu_nu, " +
																"  lsu_st, " +
																"  lsu_dh_operacao) " +
																"VALUES (" +
																	liDeReferencia.IdPliMercadoria.ToString() + "," +
																	ali.IdPliMercadoria.ToString() + "," +
																	listLiOrigem.FirstOrDefault().IdLiOrigem + "," +
																	count + "," +
																	0 + "," +
																	" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
																") ");
															}
														}

														if (ali.TipoAli == 3)
														{
															var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == ali.IdPliMercadoria);
															var numeroLiRetificador = Convert.ToInt64(pliMercadoria.NumeroLiRetificador);
															var liRetificador = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLiRetificador);
															var aliRetificador = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == liRetificador.IdPliMercadoria);

															//Registra Histórico
															SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
																"(PME_ID, " +
																"AHI_DH_OPERACAO, " +
																"AHI_ST_ALI_ANTERIOR, " +
																"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
																"AHI_NO_RESPONSAVEL) " +
																"VALUES (" + aliRetificador.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																(byte)aliRetificador.Status + "," +
																"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + ")");

															//atualizar a ALI
															SQLExecutar.AppendLine(
																"UPDATE SCIEX_ALI SET " +
																"ALI_ST = " + (int)EnumAliStatus.ALI_RETIFICADA +
																" WHERE PME_ID = " + aliRetificador.IdPliMercadoria.ToString()
																);

															SQLExecutar.AppendLine(
															"UPDATE SCIEX_LI SET " +
															" LI_ST = " + (int)EnumLiStatus.LI_RETIFICADA +
															" WHERE  PME_ID = " + liRetificador.IdPliMercadoria.ToString());
														}

														//Finaliza o controle de execução
														_controleExecucaoServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
														_controleExecucaoServico.StatusExecucao = 1;
														_controleExecucaoServico.MemoObjetoEnvio = "Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + item.IdLiArquivoRetorno;
														_controleExecucaoServico.MemoObjetoRetorno = "Tabela: SCIEX_LI, Campo pme_id: " + ali.IdPliMercadoria;
														_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico);
														_uowSciex.CommandStackSciex.Save();

													}

													#endregion
												}
												break;
											}
										case "99": //LI COM ERRO
											{
												qtdLIIndeferida = qtdLIIndeferida + 1;
												numeroALI = linha.Substring(2, 15).Trim();
												numeroLIProtocolada = linha.Substring(17, 10);
												codigoStatusDiagnostico = linha.Substring(27, 1);
												dataGeracaoDiagnostico = linha.Substring(28, 8);


												mensagemErro = linha.Substring(36, linha.Length - 36);

												long numeroALIPesquisa = Convert.ToInt64(numeroALI);

												var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.PLIMercadoria.Any(x => x.Ali.NumeroAli == numeroALIPesquisa));
												var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);


												//Inicia o controle de Execução
												ControleExecucaoServicoEntity _controleExecucaoServico = new ControleExecucaoServicoEntity();
												_controleExecucaoServico.DataHoraExecucaoInicio = GetDateTimeNowUtc();
												_controleExecucaoServico.StatusExecucao = 0;
												_controleExecucaoServico.IdListaServico = 14;
												_controleExecucaoServico.NomeCPFCNPJInteressado = pli.RazaoSocial;
												_controleExecucaoServico.NumeroCPFCNPJInteressado = pli.Cnpj;


												if (ali != null)
												{
													var listaLI = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

													string ano = dataGeracaoDiagnostico.Substring(0, 4);
													string mes = dataGeracaoDiagnostico.Substring(4, 2);
													string dia = dataGeracaoDiagnostico.Substring(6, 2);

													#region REGRA DE NEGOCIO¨06

													if (listaLI != null) //Caso a ALI já possua uma LI associada
													{
														//Insere Tabela de li_historico
														SQLExecutar.AppendLine(
															"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
															"(" +
															"	PME_ID," +
															"	LAR_ID," +
															"	LI_NU," +
															"	LI_NU_LI_PROTOCOLADA," +
															"	LI_DT_GERACAO," +
															"	LI_DS_MENSAGEM," +
															"	LI_NU_DIAGNOSTICO_ERRO," +
															"	LI_ST," +
															"	LHE_TP_ERRO," +
															"	LI_DH_CADASTRO," +
															"	LI_DH_CANCELAMENTO" +
															") " +
															"" +
															"SELECT " +
															"	PME_ID," +
															"	LAR_ID," +
															"	LI_NU, " +
															"	LI_NU_LI_PROTOCOLADA, " +
															"	LI_DT_GERACAO, " +
															"	LI_DS_MENSAGEM, " +
															"	LI_NU_DIAGNOSTICO_ERRO, " +
															"	LI_ST, " +
															"	1, " +
															"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), " +
															"	LI_DH_CANCELAMENTO " +
															"FROM SCIEX_LI WHERE PME_ID = " + listaLI.IdPliMercadoria
															);


														//Busca o status da LI Anterior
														statusLIAnterior = listaLI.Status;

														//Busca o status da ALI Anterior
														statusALIAnterior = ali.Status;

														//Registra Histórico

														if (listaLI != null && listaLI.MensagemErroLI != null)
														{
															SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
																"(PME_ID, " +
																"AHI_DH_OPERACAO, " +
																"AHI_ST_ALI_ANTERIOR, " +
																"AHI_ST_LI_ANTERIOR, " +
																"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
																"AHI_NO_RESPONSAVEL, " +
																"AHI_DS_OBSERVACAO) " +

																"VALUES (" + ali.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00'))," +
																(byte)statusALIAnterior + "," +
																(byte)statusLIAnterior + "," +
																"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" +
																"," + "'" + listaLI.MensagemErroLI + "')");
														}
														//else
														//{
														//	SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
														//	"(PME_ID, " +
														//	"AHI_DH_OPERACAO, " +
														//	"AHI_ST_ALI_ANTERIOR, " +
														//	"AHI_ST_LI_ANTERIOR, " +
														//	"AHI_NU_CPF CNPJ_RESPONSAVEL, " +
														//	"AHI_NO_RESPONSAVEL," +
														//	"AHI_DS_OBSERVACAO)" +

														//	"VALUES (" + ali.IdPliMercadoria + ",CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00'))," +
														//	(byte)statusALIAnterior + "," +
														//	(byte)statusLIAnterior + "," +
														//	"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + "," + listaLI.MensagemErroLI + ")" );

														//}
														//Atualiza LI
														SQLExecutar.AppendLine(
															"UPDATE SCIEX_LI SET " +
																"LI_NU = NULL" +
																",LI_NU_LI_PROTOCOLADA = " + numeroLIProtocolada +
																",LI_DT_GERACAO = '" + ano + "-" + mes + "-" + dia + "'" +
																",LI_ST = " + (byte)EnumLiStatus.LI_INDEFERIDA +
																",LI_TP = " + (byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL +
																",LI_DH_CADASTRO = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
																",LI_NU_DIAGNOSTICO_ERRO = " + codigoStatusDiagnostico +
																",LI_DS_MENSAGEM =  '" + mensagemErro + "'" +
																",LAR_ID =  '" + item.IdLiArquivoRetorno + "'" +
																" WHERE PME_ID = " + listaLI.IdPliMercadoria
															);

														//Atualizar o Status da ALI
														SQLExecutar.AppendLine(
															" UPDATE SCIEX_ALI SET " +
															"  ALI_ST = " + (int)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX +
															"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
															" WHERE ALI_NU = " + ali.NumeroAli.ToString()
															);


														//Registra novo lançamento
														try
														{
															LancamentoEntity _lancamentoEntity = new LancamentoEntity();
															_lancamentoEntity.IdCodigoLancamento = 102; //Indeferimento
															_lancamentoEntity.Observacao = "ALI INDEFERIDA";
															_lancamentoEntity.IdPliMercadoria = ali.IdPliMercadoria;

															var _viewImportador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == ali.PliMercadoria.Pli.Cnpj);
															if (_viewImportador != null)
															{
																_lancamentoEntity.CodigoUnidadeCadastradora = (int)_viewImportador.CodigoUnidadeCadastradora;
															}
															_lancamentoEntity.IdPli = ali.PliMercadoria.IdPLI;
															_lancamentoEntity.DataCadastro = DateTime.Now;
															_lancamentoEntity.NumeroResponsavel = "04407029000143";

															_uowSciex.CommandStackSciex.Lancamento.Salvar(_lancamentoEntity);
															_uowSciex.CommandStackSciex.Save();
														}
														catch { }


														//Finaliza o controle de execução
														_controleExecucaoServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
														_controleExecucaoServico.StatusExecucao = 1;
														_controleExecucaoServico.MemoObjetoRetorno = "Tabela: SCIEX_LI, Campo pme_id: " + ali.IdPliMercadoria; ;
														_controleExecucaoServico.MemoObjetoEnvio = "Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + item.IdLiArquivoRetorno;
														_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico);
														_uowSciex.CommandStackSciex.Save();

														//Caso a ALI seja de Industrialização, devolver o saldo consumido pela empresa na aprovação do PLI
														// FALTA A REGRA RN12 - COTA CRÉDITO

													}
													else //Caso a ALI não possua uma LI associada
													{
														if (!mensagemErro.Contains("LICENCIAMENTO COM ERRO – NAO PROCESSADO") && !mensagemErro.Contains("LICENCIAMENTO COM ERRO - NAO PROCESSADO"))
														{
															//O sistema deverá criar um registro da LI
															SQLExecutar.AppendLine(
																"INSERT INTO SCIEX_LI " +
																"( PME_ID," +
																"  LAR_ID," +
																"  LI_NU_LI_PROTOCOLADA, " +
																"  LI_NU_DIAGNOSTICO_ERRO, " +
																"  LI_DS_MENSAGEM, " +
																"  LI_ST, " +
																"  LI_TP, " +
																"  LI_DH_CADASTRO, " +
																"  LI_DT_GERACAO) " +
																"VALUES (" +
																	ali.IdPliMercadoria.ToString() + "," +
																	item.IdLiArquivoRetorno + "," +
																	numeroLIProtocolada + "," +
																	codigoStatusDiagnostico + "," +
																	"'" + mensagemErro + "'," +
																	(byte)EnumLiStatus.LI_INDEFERIDA + "," +
																	(byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL + "," +
																	" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																	"'" + ano + "-" + mes + "-" + dia + "'" +
																") ");

															//Busca o status da ALI Anterior
															statusALIAnterior = ali.Status;

															//Registra Histórico
															SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
																"(PME_ID, " +
																"AHI_DH_OPERACAO, " +
																"AHI_ST_ALI_ANTERIOR, " +
																"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
																"AHI_NO_RESPONSAVEL) " +

																"VALUES (" + ali.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																(byte)statusALIAnterior + "," +
																"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + ")");


															//Atualizar a ALI
															SQLExecutar.AppendLine(
																" UPDATE SCIEX_ALI SET " +
																 "ALI_ST = " + (byte)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX +
																",ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
																" WHERE ALI_NU = " + ali.NumeroAli.ToString()
																);


															//Registra novo lançamento
															LancamentoEntity _lancamentoEntity = new LancamentoEntity();
															_lancamentoEntity.IdCodigoLancamento = 102; //Indeferimento
															_lancamentoEntity.Observacao = "ALI INDEFERIDA";

															var _viewImportador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == ali.PliMercadoria.Pli.Cnpj);
															if (_viewImportador != null)
															{
																_lancamentoEntity.CodigoUnidadeCadastradora = (int)_viewImportador.CodigoUnidadeCadastradora;
															}

															_lancamentoEntity.DataCadastro = DateTime.Now;
															_lancamentoEntity.NumeroResponsavel = "04407029000143";

															_lancamentoEntity.IdPliMercadoria = ali.IdPliMercadoria;
															_lancamentoEntity.IdPli = ali.PliMercadoria.IdPLI;
															_uowSciex.CommandStackSciex.Lancamento.Salvar(_lancamentoEntity);


															//Finaliza o controle de execução
															_controleExecucaoServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
															_controleExecucaoServico.StatusExecucao = 1;
															_controleExecucaoServico.MemoObjetoEnvio = "Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + item.IdLiArquivoRetorno;
															_controleExecucaoServico.MemoObjetoRetorno = "Tabela: SCIEX_LI, Campo pme_id: " + ali.IdPliMercadoria;
															_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico);
															_uowSciex.CommandStackSciex.Save();


															//Caso a ALI seja de Industrialização, devolver o saldo consumido pela empresa na aprovação do PLI
															// FALTA A REGRA RN12 - COTA CRÉDITO

														}
														else
														{
															//INSERIR A LI
															SQLExecutar.AppendLine(
																"INSERT INTO SCIEX_LI " +
																"( PME_ID," +
																"  LAR_ID, " +
																"  LI_NU_LI_PROTOCOLADA, " +
																"  LI_NU_DIAGNOSTICO_ERRO, " +
																"  LI_DS_MENSAGEM, " +
																"  LI_ST, " +
																"  LI_TP, " +
																"  LI_DH_CADASTRO, " +
																"  LI_DT_GERACAO) " +
																"VALUES (" +
																	ali.IdPliMercadoria.ToString() + "," +
																	+item.IdLiArquivoRetorno + "," +
																	numeroLIProtocolada + "," +
																	codigoStatusDiagnostico + "," +
																	"'" + mensagemErro + "'," +
																	(byte)EnumLiStatus.LI_INDEFERIDA + "," +
																	(byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL + "," +
																	" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																	"'" + ano + "-" + mes + "-" + dia + "'" +
																") ");


															//Busca o status da ALI Anterior
															statusALIAnterior = ali.Status;


															//Registra Histórico
															SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
																"(PME_ID, " +
																"AHI_DH_OPERACAO, " +
																"AHI_ST_ALI_ANTERIOR, " +
																"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
																"AHI_NO_RESPONSAVEL) " +

																"VALUES (" + ali.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
																(byte)statusALIAnterior + "," +
																"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + ")");


															//atualizar a ALI
															SQLExecutar.AppendLine(
																" UPDATE SCIEX_ALI SET " +
																"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
																"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
																" WHERE ALI_NU = " + ali.NumeroAli.ToString()
																);


															//Finaliza o controle de execução
															_controleExecucaoServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
															_controleExecucaoServico.StatusExecucao = 1;
															_controleExecucaoServico.MemoObjetoEnvio = "Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + item.IdLiArquivoRetorno;
															_controleExecucaoServico.MemoObjetoRetorno = "Tabela: SCIEX_LI, Campo pme_id: " + ali.IdPliMercadoria; ;

															_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico);
															_uowSciex.CommandStackSciex.Save();


														}
													}

													#endregion
												}
												break;
											}
										case "98": //ERRO DE ESTRUTURA DO ARQUIVO
											{
												qtdErros = qtdErros + 1;

												numeroALI = linha.Substring(2, 15).Trim();
												mensagemErro = linha.Substring(17, linha.Length - 17);

												long numeroALIPesquisa = Convert.ToInt64(numeroALI);

												var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.PLIMercadoria.Any(x => x.Ali.NumeroAli == numeroALIPesquisa));
												var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);


												//Inicia o controle de execução de serviço
												ControleExecucaoServicoEntity _controleExecucaoServico = new ControleExecucaoServicoEntity();
												_controleExecucaoServico.DataHoraExecucaoInicio = GetDateTimeNowUtc();
												_controleExecucaoServico.StatusExecucao = 0;
												_controleExecucaoServico.IdListaServico = 14;
												_controleExecucaoServico.NomeCPFCNPJInteressado = pli.RazaoSocial;
												_controleExecucaoServico.NumeroCPFCNPJInteressado = pli.Cnpj;


												if (ali != null)
												{
													var listaLI = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

													#region REGRA DE NEGOCIO 08

													if (listaLI == null)
													{
														//INSERIR A LI
														SQLExecutar.AppendLine(
															"INSERT INTO SCIEX_LI " +
															"( PME_ID," +
															"  LAR_ID, " +
															"  LI_DS_MENSAGEM, " +
															"  LI_ST, " +
															"  LI_TP, " +
															"  LI_DH_CADASTRO) " +
															"VALUES (" +
																ali.IdPliMercadoria.ToString() + "," +
																item.IdLiArquivoRetorno + "," +
																"'" + mensagemErro + "'," +
																(byte)EnumLiStatus.LI_INDEFERIDA + "," +
																(byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL + "," +
																" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
															") ");

														//Busca o status da ALI Anterior
														statusALIAnterior = ali.Status;

														//Registra Histórico
														SQLExecutar.AppendLine("INSERT INTO SCIEX_ALI_HISTORICO " +
															"(PME_ID, " +
															"AHI_DH_OPERACAO, " +
															"AHI_ST_ALI_ANTERIOR, " +
															"AHI_NU_CPFCNPJ_RESPONSAVEL, " +
															"AHI_NO_RESPONSAVEL)" +

															"VALUES (" + ali.IdPliMercadoria + ", CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) ," +
															(byte)statusALIAnterior + "," +
															"'04407029000143'" + "," + "'Administrador do Sistema - SUFRAMA'" + ")");


														//atualizar a ALI
														SQLExecutar.AppendLine(
															" UPDATE SCIEX_ALI SET " +
																"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
																", AAE_ID = null " +
																", AAE_DS_NOME_ARQUIVO = '' " +
																", ALI_DH_RESPOSTA_SISCOMEX = null" + //Dúvidas
															" WHERE ALI_NU = " + ali.NumeroAli.ToString()
															);


														//Finaliza o controle de execução
														_controleExecucaoServico.DataHoraExecucaoFim = GetDateTimeNowUtc();
														_controleExecucaoServico.StatusExecucao = 1;
														_controleExecucaoServico.MemoObjetoEnvio = "Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + item.IdLiArquivoRetorno;
														_controleExecucaoServico.MemoObjetoRetorno = "Tabela: SCIEX_LI, Campo pme_id: " + ali.IdPliMercadoria;
														_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServico);
														_uowSciex.CommandStackSciex.Save();
													}

													#endregion
												}
												break;
											}
										default:
											break;
									}
									#endregion
								}
							}
							if (!String.IsNullOrEmpty(SQLExecutar.ToString().Trim()))
							{
								string sql1 = SQLExecutar.ToString();
								_uowSciex.CommandStackSciex.Salvar(sql1);

								SQLExecutar = new StringBuilder();
							}

							contador++;
						}

						//Salva os arquivo
						file.Close();

						SQLExecutar = new StringBuilder();

						#region REGRA DE NEGOCIO 08
						SQLExecutar.AppendLine(
							"UPDATE SCIEX_LI_ARQUIVO_RETORNO SET " +
							   "LAR_QT_LI = " + qtdLI.ToString() +
							   ",LAR_ST_LEITURA = " + 1 +
							   ",LAR_QT_LI_DEFERIDA = " + qtdLIDeferida.ToString() +
							   ",LAR_QT_LI_INDEFERIDA = " + qtdLIIndeferida.ToString() +
							   ",LAR_QT_LI_ERRO = " + qtdErros.ToString() +
							" WHERE LAR_ID =" + item.IdLiArquivoRetorno
						);
						#endregion

						string sql = SQLExecutar.ToString();
						_uowSciex.CommandStackSciex.Salvar(sql);


					}
				}
			}
			else
			{
				#region RN03
				//Registra o início da execução
				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.MemoObjetoEnvio = "FTP: " + localArquivoFTP;
				_controleExecucaoServicoVM.StatusExecucao = 0;
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoVM.IdListaServico = 12; //SALVAR ARQUIVO DE LI - NORMAL

				//Registra Fim de Execução
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.StatusExecucao = 1;
				_controleExecucaoServicoVM.MemoObjetoRetorno = "NENHUM ARQUIVO DISPONIBILIZADO";
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();
				#endregion
			}
		}

		public string RegistroControleExecucaoServicoInicio(string CNPJ, string RazaoSocial, long idLiArquivo, long idPiMercadoria)
		{
			//INÍCIO DO SERVIÇO DE LER ARQUIVO
			string SQL = @" INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO " +
			"(CES_DH_EXECUCAO_INICIO, CES_ST_EXECUCAO, CES_ME_OBJETO_ENVIO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID, CES_DH_EXECUCAO_FIM, CES_ST_EXECUCAO, CES_ME_OBJETO_RETORNO )" +
			"VALUES (" +
			" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , 0, 'Tabela: SCIEX_LI_ARQUIVO, Campo lar_id: " + idLiArquivo + "'"
			+ "," + "'" + CNPJ + "'"
			+ "," + "'" + RazaoSocial + "'"
			+ "," + "14" + " CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " + "," + "1" + "," + "Tabela: SCIEX_LI, Campo pme_id: " + idPiMercadoria + ")";

			return SQL;
		}

		public string GerarArquivoSimulacaoLI()
		{
			try
			{
				var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == (int)EnumListaServico.SalvarArquivoLiNormal);

				// Alterado para atender a solicitacao dos analistas de sistemas.
				var configuracoesUsuario = configuracoesFTP.ElementAt(1).Valor; // ConfigurationManager.AppSettings["FTP_USUARIO"];
				var configuracoesSenha = configuracoesFTP.ElementAt(2).Valor; //ConfigurationManager.AppSettings["FTP_SENHA"];

				var retorno = _uowSciex.CommandStackSciex.Salvar("SELECT MAX(ISNULL(LI_NU, 0)) + 1 FROM SCIEX_LI");
				List<AliEntity> listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.Status == (int)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX);

				if (retorno == 0) retorno = 1;

				if (listaALI.Count > 0)
				{
					StringBuilder arquivo = new StringBuilder();

					arquivo.Append("24");
					arquivo.Append("04407029000143");

					string dataInicio = "01/01/" + DateTime.Now.Year.ToString();
					string dataAtual = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
					TimeSpan date = Convert.ToDateTime(dataAtual) - Convert.ToDateTime(dataInicio);
					arquivo.Append((date.Days + 1).ToString("D3"));

					arquivo.Append(DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2"));
					arquivo.Append("02");
					arquivo.Append("02");
					arquivo.Append("02");
					arquivo.Append("850");
					arquivo.AppendLine();

					long codigoLIProtocolada = retorno;
					long codigoLI = retorno;

					int contador = 1;
					foreach (AliEntity item in listaALI)
					{
						if (contador % 6 == 0)
						{
							arquivo.Append("99");
							arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
							arquivo.Append(codigoLIProtocolada.ToString("D10"));
							arquivo.Append("2");
							arquivo.Append(DateTime.Now.Year.ToString("D4") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));
							arquivo.Append("NUMERO DO DESTAQUE DA NCM PARA ANUENCIA - NAO INFORMADO");
							codigoLIProtocolada++;
						}
						else

						if (contador % 10 == 0)
						{
							arquivo.Append("99");
							arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
							arquivo.Append(codigoLIProtocolada.ToString("D10"));
							arquivo.Append("2");
							arquivo.Append(DateTime.Now.Year.ToString("D4") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));
							arquivo.Append("DECLARACAO NAO ACEITA - USUARIO NAO CADASTRADO COMO REPRESENTANTE LEGAL DO IMPORTADOR PARA AS ATIVIDADES DE IMPORTACAO");
							codigoLIProtocolada++;
						}
						else
						{
							arquivo.Append("01");
							arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
							arquivo.Append(codigoLIProtocolada.ToString("D10"));
							arquivo.Append(codigoLI.ToString("D10"));
							arquivo.Append(DateTime.Now.Year.ToString("D4") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));
							codigoLIProtocolada++;
							codigoLI++;
						}

						arquivo.AppendLine();
						contador++;
					}

					if (!Ftp.VerificarSeExisteArquivo(configuracoesFTP.FirstOrDefault().Valor, configuracoesUsuario, configuracoesSenha))
					{
						Ftp.EnviarArquivo(configuracoesFTP.FirstOrDefault().Valor, configuracoesUsuario, configuracoesSenha, arquivo.ToString());
						return "Arquivo enviado com sucesso.";
					}
					else
					{
						return "Arquivo existente na pasta no momento do envio";
					}

				}
				return "Sem LI para geração do arquivo";
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}

		}

	}
}

