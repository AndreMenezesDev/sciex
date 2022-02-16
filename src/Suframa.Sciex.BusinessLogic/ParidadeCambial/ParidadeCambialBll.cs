using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.Database;
using System.Linq;
using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.Extension;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.CrossCutting.Resources;
using HandlebarsDotNet;
using Newtonsoft.Json;
using Suframa.Sciex.CrossCutting.Json;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public class ParidadeCambialBll : IParidadeCambialBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public ParidadeCambialBll(IUnitOfWorkSciex uowSciex, IUsuarioLogado usuarioLogado, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogado;
			_usuarioPssBll = usuarioPssBll;
		}

		private string MontarConsulta(ParidadeCambialPagedFilterVM pagedFilter)
		{
			String consultaMoeda = "";

			if (pagedFilter.IdMoeda > 0)
				consultaMoeda = " and pva.MOE_ID = " + pagedFilter.IdMoeda.ToString();

			var sql = @"Select pca.PCA_ID AS IdParidadeCambial," +
					  "       Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) AS DataParidade," +
					  "       Convert(Varchar(10), pca.PCA_DH_CADASTRO, 103) +' ' + Convert(Varchar(8), pca.PCA_DH_CADASTRO, 24) AS DataCadastro," +
					  "       Convert(Varchar(10), pca.PCA_DT_ARQUIVO, 103) AS DataArquivo," +
					  "       pca.PCA_NO_USUARIO AS NomeUsuario," +
					  "       pva.PVA_VL_PARIDADE AS Valor," +
					  "       CAST(moe.MOE_CO AS varchar) + ' - ' + moe.MOE_DS AS CodDscMoeda" +
					  "   From SCIEX_PARIDADE_CAMBIAL pca," +
					  "        SCIEX_PARIDADE_VALOR pva," +
					  "        SCIEX_MOEDA moe" +
					  //					  "   Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = " + dtParidade.ToShortDateString() + " And" +
					  "   Where Convert(Varchar(10), pca.PCA_DT_PARIDADE, 103) = '" + pagedFilter.DataParidade.ToShortDateString() + "' And" +
					  "         pva.PCA_ID = pca.PCA_ID and" +
					  "         moe.MOE_ID = pva.MOE_ID" + consultaMoeda;
			return sql;
		}

		private ParidadeCambialVM ValidarExisteParidade(DateTime dataParidade)
		{
			var paridadeCambialVM = _uowSciex.QueryStackSciex.GetParidadeCambial(dataParidade);

			if (paridadeCambialVM != null)
				return paridadeCambialVM;

			return null;
		}

		private List<String> ValidarDownloadArquivo(ref ParidadeCambialGenerator paridadeCambialGenerator)
		{
			try
			{
				paridadeCambialGenerator.Mensagem = "";
				List<String> listaParidade = new List<string>();
				string arquivo =
							paridadeCambialGenerator.DataOrigem.Year.ToString() +
							(paridadeCambialGenerator.DataOrigem.Month < 10 ? "0" + paridadeCambialGenerator.DataOrigem.Month.ToString() : paridadeCambialGenerator.DataOrigem.Month.ToString()) +
							(paridadeCambialGenerator.DataOrigem.Day < 10 ? "0" + paridadeCambialGenerator.DataOrigem.Day.ToString() : paridadeCambialGenerator.DataOrigem.Day.ToString()) + ".csv";

				string webResource = null;
				WebClient webClient = new WebClient();
				webResource = paridadeCambialGenerator.UrlPathArquivo + arquivo;
				byte[] data = webClient.DownloadData(webResource);
				CultureInfo myCIintl = new CultureInfo("pt-BR", false);
				MemoryStream ms = new MemoryStream(data);
				StreamReader sr = new StreamReader(ms, Encoding.GetEncoding(myCIintl.TextInfo.ANSICodePage));

				// inseri resultado na lista temporaria
				string linha = null;

				try
				{
					while ((linha = sr.ReadLine()) != null)
					{
						string[] coluna = linha.Split(';');
						string valor = "";
						for (int i = 0; i < coluna.Length; i++)
						{
							if (i < coluna.Length - 1)
								valor += coluna[i] + ";";
							else
								valor += coluna[i];
						}
						listaParidade.Add(valor);
					}
					bool isExiste = true;
					foreach (var item in listaParidade)
					{
						if (!item.Contains(";790;"))
						{
							isExiste = false;
						}
					}

					if (!isExiste)
					{
						var a = paridadeCambialGenerator.DataOrigem.ToShortDateString() + ";790;A;BRL;1,0;1,0;1,0;1,0";
						listaParidade.Add(a);
					}

					paridadeCambialGenerator.IndRetorno = 0;

					return listaParidade;
				}
				catch (Exception ex)
				{
					paridadeCambialGenerator.IndRetorno = 1;
					paridadeCambialGenerator.Mensagem = "Erro na leitura do arquivo";
				}
			}
			catch (Exception web)
			{
				paridadeCambialGenerator.IndRetorno = 1;
				if (web.Message == "Impossível conectar-se ao servidor remoto")
					paridadeCambialGenerator.Mensagem = "Não foi possível estabelecer uma conexão com o Banco Central";
			}

			return new List<string>();
		}

		private void SalvarParidadeValor(
					ParidadeCambialVM _ParidadeCambialVM,
					ParidadeCambialGenerator paridadeCambialGenerator,
					List<string> listaParidade
					)
		{
			if (listaParidade.Count > 0)
			{
				var moedaList = _uowSciex.QueryStackSciex.Moeda.Listar();

				string gerarSQL = "";

				foreach (var item in listaParidade)
				{
					string[] coluna = item.Split(';');

					string codigoMoeda = coluna[1];

					int? idMoeda = 0;

					try { idMoeda = moedaList.Where(o => o.CodigoMoeda.Equals(Convert.ToInt16(codigoMoeda))).FirstOrDefault().IdMoeda; } catch (Exception) { }

					//verifica se existe moeda
					if (idMoeda != null && idMoeda > 0)
					{
						decimal valorParidade = Convert.ToDecimal(coluna[5]);

						gerarSQL +=
							@"INSERT INTO SCIEX_PARIDADE_VALOR(PCA_ID, MOE_ID,PVA_VL_PARIDADE)
							VALUES (" + _ParidadeCambialVM.IdParidadeCambial.ToString() + "," +
							idMoeda.ToString() + "," +
							valorParidade.ToString().Replace(",", ".") + ")	";
					}
				}
				if (gerarSQL != "")
					SalvarParidadeValor(gerarSQL);
			}
		}

		private void SalvarParidadeValor(string sql)
		{
			_uowSciex.CommandStackSciex.SalvarParidadeCambial(sql);
		}

		private ParidadeCambialGenerator ConfigurarServicosParidadeCambialGenerator(ParidadeCambialGenerator paridadeCambialGenerator)
		{
			if (paridadeCambialGenerator != null)
			{
				var lista = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == 1);

				if (lista.Count > 0)
				{
					paridadeCambialGenerator.UrlPathArquivo = lista[0].Valor;
					paridadeCambialGenerator.NumeroTentativasDownload = Convert.ToInt32(lista[1].Valor.ToString());
					paridadeCambialGenerator.HoraMaximaDownload = Convert.ToDateTime(lista[2].Valor).TimeOfDay;
					paridadeCambialGenerator.EmailDestinatario = lista[3].Valor;
					paridadeCambialGenerator.TituloEmail = lista[4].Valor;

					paridadeCambialGenerator.DataParidade = DateTime.Now.AddDays(1);
					paridadeCambialGenerator.DataOrigem = DateTime.Now.AddDays(1);
				}
			}
			return paridadeCambialGenerator;
		}


		private ControleExecucaoServicoVM ConfigurarControleExecutacaoServico(ControleExecucaoServicoVM controleExecucaoServicoVM)
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));
			controleExecucaoServicoVM.DataHoraExecucaoInicio = manausTime;
			controleExecucaoServicoVM.IdListaServico = 1;
			controleExecucaoServicoVM.StatusExecucao = 0;
			controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
			controleExecucaoServicoVM.NomeCPFCNPJInteressado = "04407029000143";

			return controleExecucaoServicoVM;
		}


		private ParidadeCambialVM Salvar(ParidadeCambialVM paridadeCambialVM)
		{
			var entityParidadeCambial = Mapper.Map<ParidadeCambialEntity>(paridadeCambialVM);
			_uowSciex.CommandStackSciex.ParidadeCambial.Salvar(entityParidadeCambial);
			_uowSciex.CommandStackSciex.Save();
			return Mapper.Map<ParidadeCambialVM>(entityParidadeCambial);
		}


		private string GerarTemplate()
		{
			var res = Resources.paridadeCambial;
			var template = Handlebars.Compile(res);

			return template(template);
		}

		/// <summary>Carregar DTO da Tela 1 - Tela Listar Paridade Cambial</summary>
		/// <returns></returns>
		public PagedItems<ParidadeCambialDto> ListarPaginado(ParidadeCambialPagedFilterVM pagedFilter)
		{
			return _uowSciex.QueryStackSciex.ListarPaginadoSql<ParidadeCambialDto>(MontarConsulta(pagedFilter), pagedFilter);
		}

		public ParidadeCambialGenerator BaixarArquivoParidadeRemoto(ParidadeCambialGenerator paridadeCambialGenerator)
		{
			//instancia o controle de execucao do servico;
			var _controleExecucaoServicoVM = ConfigurarControleExecutacaoServico(new ControleExecucaoServicoVM());

			//configurar parametros de ações da durante a execução do download
			paridadeCambialGenerator = ConfigurarServicosParidadeCambialGenerator(paridadeCambialGenerator);

			// verifica se existe paridade cambial
			if (ValidarExisteParidade(paridadeCambialGenerator.DataParidade) == null)
			{
				// executa o numero de tentativas de download conforme valor do NumeroTentativasDownload
				for (int i = 0; i < paridadeCambialGenerator.NumeroTentativasDownload; i++)
				{
					// metodo que executar o downlaod do arquivo, caso exista retona uma lista de pararidade cambial
					var listaParidade = ValidarDownloadArquivo(ref paridadeCambialGenerator);
					if (listaParidade.Count > 0)
					{
						ParidadeCambialVM _ParidadeCambialVM = new ParidadeCambialVM();
						_ParidadeCambialVM.DataArquivo = paridadeCambialGenerator.DataOrigem;
						_ParidadeCambialVM.DataCadastro = DateTime.Now;
						_ParidadeCambialVM.DataParidade = DateTime.Now;
						_ParidadeCambialVM.NomeUsuario = "Administrador do Sistema - SUFRAMA";
						_ParidadeCambialVM.NumeroUsuario = "04407029000143";

						_ParidadeCambialVM = Salvar(_ParidadeCambialVM);

						SalvarParidadeValor(_ParidadeCambialVM, paridadeCambialGenerator, listaParidade);

						_controleExecucaoServicoVM.StatusExecucao = 1;
						_controleExecucaoServicoVM.MemoObjetoRetorno = listaParidade.ToJson();
						break;
					}
					else if (paridadeCambialGenerator.IndRetorno == 1)
					{
						paridadeCambialGenerator.DataOrigem = paridadeCambialGenerator.DataOrigem.AddDays(-1);
						_controleExecucaoServicoVM.StatusExecucao = 2;
						//break;
					}
				}
			}

			_controleExecucaoServicoVM.DataHoraExecucaoFim = DateTime.Now;
			var entityControleExecucaoServico = Mapper.Map<ControleExecucaoServicoEntity>(_controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(entityControleExecucaoServico);

			if (paridadeCambialGenerator.HoraMaximaDownload.Hours == DateTime.Now.Hour)
				EnviarEmail(paridadeCambialGenerator);

			_uowSciex.CommandStackSciex.Save();
			return paridadeCambialGenerator;
		}

		public ParidadeCambialGenerator BaixarArquivoParidadeEmail(ParidadeCambialGenerator paridadeCambialGenerator)
		{
			if (paridadeCambialGenerator != null)
			{
				//instancia o controle de execucao do servico
				var _controleExecucaoServicoVM = ConfigurarControleExecutacaoServico(new ControleExecucaoServicoVM());

				//configurar parametros de ações da durante a execução do download
				paridadeCambialGenerator = ConfigurarServicosParidadeCambialGenerator(paridadeCambialGenerator);

				// verifica se existe paridade cambial
				var dataToday = DateTime.Now;

				if (ValidarExisteParidade(dataToday) == null)
				{
					ParidadeCambialGenerator copy = paridadeCambialGenerator;
					copy.DataParidade = dataToday;
					_controleExecucaoServicoVM.StatusExecucao = 2;
					EnviarEmail(copy);
					paridadeCambialGenerator.Mensagem = "Paridade Cambial: Não existe data de paridade para a data: " + dataToday.ToShortDateString();
					_controleExecucaoServicoVM.MemoObjetoEnvio = GerarTemplate().ToJson();
				}

				paridadeCambialGenerator.DataParidade = dataToday.AddDays(1);
				if (ValidarExisteParidade(paridadeCambialGenerator.DataParidade) == null)
				{
					//paridadeCambialGenerator.DataOrigem = paridadeCambialGenerator.DataOrigem.AddDays(-1);
					_controleExecucaoServicoVM.StatusExecucao = 2;
					EnviarEmail(paridadeCambialGenerator);
					paridadeCambialGenerator.Mensagem = "Paridade Cambial: Não existe data de paridade para a data: "+paridadeCambialGenerator.DataParidade.ToShortDateString();
					_controleExecucaoServicoVM.MemoObjetoEnvio = GerarTemplate().ToJson();
				}

				var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));
				_controleExecucaoServicoVM.DataHoraExecucaoFim = manausTime;
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				var entityControleExecucaoServico = Mapper.Map<ControleExecucaoServicoEntity>(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(entityControleExecucaoServico);

				_uowSciex.CommandStackSciex.Save();
			}
			return paridadeCambialGenerator;
		}

		public ParidadeCambialGenerator Gerar(ParidadeCambialGenerator paridadeCambialGenerator)
		{
			paridadeCambialGenerator.Mensagem = "";

			var paridadeCambialVMNew = new ParidadeCambialVM();
			ParidadeCambialVM paridadeCambialVM;
			ParidadeCambialVM paridadeCambialVMOld;

			paridadeCambialGenerator.IndRetorno = 0;

			paridadeCambialVMOld = null;

			var lista = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == 1);

			paridadeCambialGenerator.UrlPathArquivo = lista[0].Valor;

			try
			{
				List<MoedaEntity> moedaList = null;
				List<String> listaParidade = new List<String>();

				// configura valores para inserção da paridade

				DateTime? dataParidadeAux = null;
				ParidadeCambialVM _ParidadeCambialVM = null;

				if (paridadeCambialGenerator.TipoGeracao == 1)
				{
					listaParidade = ValidarDownloadArquivo(ref paridadeCambialGenerator);
					if (listaParidade.Count == 0)
					{
						paridadeCambialGenerator.IndRetorno = 1;
						return paridadeCambialGenerator;
					}
				}
				else if (paridadeCambialGenerator.TipoGeracao == 2)
				{
					var paridadeCambialVMTMP = ValidarExisteParidade(paridadeCambialGenerator.DataOrigem);

					if (paridadeCambialVMTMP == null)
					{
						paridadeCambialGenerator.DataParidadeProxima = null;
						paridadeCambialGenerator.IndRetorno = 1;
						return paridadeCambialGenerator;
					}
				}

				for (int i = 0; i < paridadeCambialGenerator.Dias; i++)
				{
					/*
					  RN 05 Data da Paridade: O sistema deve permitir a data atual se, não existir nenhum registro com esta data na base de dados, 
					  senão apresentar mensagem “Paridade já cadastrada para data escolhida".
					*/
					var dataAtual = System.DateTime.Now.ToString("dd/MM/yyyy");
					if (paridadeCambialGenerator.DataParidade.ToString("dd/MM/yyyy").Equals(dataAtual))
					{
						var existeParidadeDataAtual = ValidarExisteParidade(paridadeCambialGenerator.DataParidade);
						if(existeParidadeDataAtual != null)
						{
							paridadeCambialGenerator.IndRetorno = 4;
							return paridadeCambialGenerator;
						}
					}

					var dataMaximaParidade = paridadeCambialGenerator.DataParidadeInicio.AddDays(paridadeCambialGenerator.Dias);
					if (paridadeCambialGenerator.DataParidade >= dataMaximaParidade)
					{
						paridadeCambialGenerator.DiaAtual = paridadeCambialGenerator.Dias;
						break;
					}

					//verifica se existe paridade cambial pela data da paridade
					var paridadeCambialVMTMP = ValidarExisteParidade(
						paridadeCambialGenerator.DataParidade > paridadeCambialGenerator.DataParidadeProxima ?
							paridadeCambialGenerator.DataParidade : paridadeCambialGenerator.DataParidadeProxima != null ?
							paridadeCambialGenerator.DataParidadeProxima.Value : paridadeCambialGenerator.DataParidade);

					if (paridadeCambialVMTMP == null)
					{
						//if (paridadeCambialGenerator.TipoGeracao == 2)
						//{
						//	if (i == 0)
						//	{
						//		paridadeCambialGenerator.DataParidadeProxima = null;
						//		paridadeCambialGenerator.IndRetorno = 1;
						//		return paridadeCambialGenerator; 
						//	}
						//}

						// caso nao exista a paridade adicionar
						if (paridadeCambialVMTMP == null)
							paridadeCambialGenerator.AdicionaParidade = true;
						// variavel de controle para caso haja sobreposição de dados
						paridadeCambialGenerator.IndSobrepor = 0;
						if (paridadeCambialGenerator.DataParidadeProxima != null)
						{
							paridadeCambialGenerator.DataParidade = paridadeCambialGenerator.DataParidade > paridadeCambialGenerator.DataParidadeProxima ?
							paridadeCambialGenerator.DataParidade : paridadeCambialGenerator.DataParidadeProxima != null ?
							paridadeCambialGenerator.DataParidadeProxima.Value : paridadeCambialGenerator.DataParidade;
							paridadeCambialGenerator.DataParidadeProxima = null;
						}
					}
					else if (paridadeCambialVMTMP != null && paridadeCambialGenerator.IndSobrepor == 0)
					{
						paridadeCambialGenerator.DataParidadeProxima = paridadeCambialGenerator.DataParidade;
						paridadeCambialGenerator.IndRetorno = 3; // JÁ existe Paridade Cambial para a data de origem informada
						return paridadeCambialGenerator;
					}

					if (paridadeCambialGenerator.DataParidade != dataParidadeAux)
					{
						if (paridadeCambialGenerator.AdicionaParidade)
						{
							if (paridadeCambialVMTMP != null)
							{
								if (paridadeCambialGenerator.TipoGeracao == 2)
								{
									var origemParidade = ValidarExisteParidade(paridadeCambialGenerator.DataOrigem);

									paridadeCambialGenerator.IndSobrepor = 0;
									dataParidadeAux = paridadeCambialGenerator.DataParidade;
									_ParidadeCambialVM = new ParidadeCambialVM();
									_ParidadeCambialVM.DataArquivo = origemParidade.DataArquivo;
									_ParidadeCambialVM.DataCadastro = DateTime.Now;
									_ParidadeCambialVM.DataParidade = paridadeCambialGenerator.DataParidadeProxima != null ? paridadeCambialGenerator.DataParidadeProxima : paridadeCambialGenerator.DataParidade;
									_ParidadeCambialVM.NomeUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
									_ParidadeCambialVM.NumeroUsuario = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();
									paridadeCambialGenerator.DataParidade = paridadeCambialGenerator.DataParidadeProxima.Value;
									_ParidadeCambialVM = Salvar(_ParidadeCambialVM);
									paridadeCambialGenerator.IdParidadeCambialUltimaCopia = _ParidadeCambialVM.IdParidadeCambial;

									_uowSciex.CommandStackSciex.CopiarParidadeCambialValor((int)_ParidadeCambialVM.IdParidadeCambial,
									(int)origemParidade.IdParidadeCambial);
								}

								_uowSciex.CommandStackSciex.ExcluirParidadeCambial((int)paridadeCambialVMTMP.IdParidadeCambial);
							}
							else if (paridadeCambialVMTMP == null && paridadeCambialGenerator.TipoGeracao == 2)
							{
								var origemParidade = ValidarExisteParidade(paridadeCambialGenerator.DataOrigem);
								dataParidadeAux = paridadeCambialGenerator.DataParidade;
								_ParidadeCambialVM = new ParidadeCambialVM();
								_ParidadeCambialVM.DataArquivo = origemParidade.DataArquivo;
								_ParidadeCambialVM.DataCadastro = DateTime.Now;
								_ParidadeCambialVM.DataParidade = paridadeCambialGenerator.DataParidade;
								_ParidadeCambialVM.NomeUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
								_ParidadeCambialVM.NumeroUsuario = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

								_ParidadeCambialVM = Salvar(_ParidadeCambialVM);

								_uowSciex.CommandStackSciex.CopiarParidadeCambialValor((int)_ParidadeCambialVM.IdParidadeCambial,
								(int)origemParidade.IdParidadeCambial);
							}

							if (paridadeCambialGenerator.TipoGeracao == 1)
							{
								paridadeCambialGenerator.IndSobrepor = 0;
								dataParidadeAux = paridadeCambialGenerator.DataParidade;
								_ParidadeCambialVM = new ParidadeCambialVM();
								_ParidadeCambialVM.DataArquivo = paridadeCambialGenerator.DataOrigem;
								_ParidadeCambialVM.DataCadastro = DateTime.Now;
								_ParidadeCambialVM.DataParidade = paridadeCambialGenerator.DataParidadeProxima.HasValue && paridadeCambialGenerator.DataParidade > paridadeCambialGenerator.DataParidadeProxima ?
									paridadeCambialGenerator.DataParidade : paridadeCambialGenerator.DataParidadeProxima != null ?
									paridadeCambialGenerator.DataParidadeProxima.Value : paridadeCambialGenerator.DataParidade;

								_ParidadeCambialVM.NomeUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
								_ParidadeCambialVM.NumeroUsuario = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

								_ParidadeCambialVM = Salvar(_ParidadeCambialVM);
								if (paridadeCambialGenerator.DataParidadeProxima != null)
									paridadeCambialGenerator.DataParidade = paridadeCambialGenerator.DataParidadeProxima.Value;
								//paridadeCambialGenerator.DataParidadeProxima = null;

								if (paridadeCambialGenerator.ParidadeCambialVM == null)
									paridadeCambialGenerator.ParidadeCambialVM = new ParidadeCambialVM();
								paridadeCambialGenerator.ParidadeCambialVM = _ParidadeCambialVM;
								if (!paridadeCambialGenerator.ListaParidadeCambialAdd.Contains(paridadeCambialGenerator.ParidadeCambialVM))
									paridadeCambialGenerator.ListaParidadeCambialAdd.Add(paridadeCambialGenerator.ParidadeCambialVM);
							}
						}
					}

					if (paridadeCambialGenerator.DataParidadeProxima != null)
						paridadeCambialGenerator.DataParidade = paridadeCambialGenerator.DataParidadeProxima.Value.AddDays(1);
					else if (paridadeCambialGenerator.Dias > 1)
					{
						paridadeCambialGenerator.DataParidade = paridadeCambialGenerator.DataParidade.AddDays(1);
						paridadeCambialGenerator.DataParidadeProxima = paridadeCambialGenerator.DataParidade;
					}
					paridadeCambialGenerator.IndSobrepor = 0;
				}
				//“Paridade já cadastrada para data escolhida”
				if (paridadeCambialGenerator.ListaParidadeCambialAdd.Count > 0)
				{
					if (paridadeCambialGenerator.TipoGeracao == 1) // '' arquivo
					{
						string gerarSQL = "";
						moedaList = _uowSciex.QueryStackSciex.Moeda.Listar();
						foreach (var item in listaParidade)
						{
							string[] coluna = item.Split(';');

							string codigoMoeda = coluna[1];

							int? idMoeda = 0;

							try { idMoeda = moedaList.Where(o => o.CodigoMoeda.Equals(Convert.ToInt16(codigoMoeda)))?.FirstOrDefault()?.IdMoeda ?? null; } catch (Exception) { }

							//verifica se existe moeda
							if (idMoeda != null && idMoeda > 0)
							{
								decimal valorParidade = Convert.ToDecimal(coluna[5]);

								gerarSQL +=
									@"INSERT INTO dbo.SCIEX_PARIDADE_VALOR(PCA_ID, MOE_ID,PVA_VL_PARIDADE)
									VALUES (" + paridadeCambialGenerator.ListaParidadeCambialAdd[0].IdParidadeCambial.ToString() + "," +
									idMoeda.ToString() + "," +
									valorParidade.ToString().Replace(",", ".") + ")	";
							}
						}

						if (gerarSQL != "")
						{
							SalvarParidadeValor(gerarSQL);
							paridadeCambialGenerator.IndRetorno = 2;
						}
					}

					if (paridadeCambialGenerator.TipoGeracao == 1)
					{
						foreach (var item in paridadeCambialGenerator.ListaParidadeCambialAdd)
						{
							// insere paridades apos a conclusao do download
							if (paridadeCambialGenerator.ListaParidadeCambialAdd[0].IdParidadeCambial != item.IdParidadeCambial)
							{
								_uowSciex.CommandStackSciex.CopiarParidadeCambialValor((int)item.IdParidadeCambial,
									(int)paridadeCambialGenerator.ListaParidadeCambialAdd[0].IdParidadeCambial);
							}
						}
					}
					paridadeCambialGenerator.IndRetorno = paridadeCambialGenerator.IndRetorno == 2 ? 2 : 0; // Não existe Arquivo de Paridade Cambial para a data de origem informada
				}

				return paridadeCambialGenerator;
			}
			catch (Exception ex)
			{
				paridadeCambialGenerator.IndRetorno = 1;
			}

			return paridadeCambialGenerator;
		}

		public void EnviarEmail(ParidadeCambialGenerator paridadeCambialGenerator)
		{
			var body = GerarTemplate();

			try
			{
				Email.Enviar(body, paridadeCambialGenerator.TituloEmail, paridadeCambialGenerator.EmailDestinatario, paridadeCambialGenerator);
			}
			catch (Exception ex)
			{
			}
		}
	}
}