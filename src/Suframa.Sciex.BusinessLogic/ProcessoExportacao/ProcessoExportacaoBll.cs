using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.BusinessLogic.Pss;

using NLog;

namespace Suframa.Sciex.BusinessLogic
{
	public class ProcessoExportacaoBll : IProcessoExportacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private static Logger logger = LogManager.GetCurrentClassLogger();


		private long _idPLiRetorno;

		public ProcessoExportacaoBll(
			IUnitOfWorkSciex uowSciex, 
			IUnitOfWork uowCadsuf,
			IUsuarioPssBll usuarioPssBll, 
			IUsuarioInformacoesBll usuarioInformacoesBll
			)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public PagedItems<ProcessoExportacaoVM> ListarPaginado(ConsultarProcessoExportacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			var dadosUsuarioLogado = _usuarioPssBll.ObterUsuarioLogado().Perfis;

			var listaPerfis = dadosUsuarioLogado.ToJson();

			logger.Info($"BUG.1504 - lista perfis = {listaPerfis}");

			string cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

			if (cnpj.Length == 11)
			{
				cnpj = null;
			}

			var dadosLogado = _usuarioPssBll.ObterUsuarioLogado().ToJson();

			logger.Info($"BUG.1504 - dadosLogado = {dadosLogado}");

			//if (dadosUsuarioLogado.Contains(EnumPerfil.Importador))
			//{
			//	cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCnpjRepresentanteLogado.CnpjCpfUnformat();
			//}

			logger.Info($"BUG.1504 - cnpj = {cnpj}");

			if (pagedFilter == null)
				return new PagedItems<ProcessoExportacaoVM>(); 
			

			var retornoConsulta = new PagedItems<ProcessoExportacaoVM>();

			long digitoPlano = 0;
			int anoPlano = 0;

			int digitoProcesso = 0;
			int anoProcesso = 0;


			if (!string.IsNullOrEmpty(pagedFilter.NumeroAnoConcatPlano))
			{
				digitoPlano = Convert.ToInt64(pagedFilter.NumeroAnoConcatPlano.Substring(0, 5));
				anoPlano = Convert.ToInt32(pagedFilter.NumeroAnoConcatPlano.Substring(6,4));
			}

			if (!string.IsNullOrEmpty(pagedFilter.NumeroAnoConcatProcesso))
			{
				digitoProcesso = Convert.ToInt32(pagedFilter.NumeroAnoConcatProcesso.Substring(0, 4));
				anoProcesso = Convert.ToInt32(pagedFilter.NumeroAnoConcatProcesso.Substring(5, 4));
			}

			string sort = null;
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("NumeroPlano"))
			{
				sort = "NumeroPlano";
				pagedFilter.Sort = null;
			}

			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DataStatus"))
			{
				sort = "DataStatus";
				pagedFilter.Sort = null;
			}

			retornoConsulta = _uowSciex.QueryStackSciex.Processo.ListarPaginadoGrafo(o => new ProcessoExportacaoVM()
			{
				IdProcesso = o.IdProcesso,
				NumeroProcesso = o.NumeroProcesso,
				AnoProcesso = o.AnoProcesso,
				InscricaoSuframa = o.InscricaoSuframa,
				RazaoSocial = o.RazaoSocial,
				TipoModalidade = o.TipoModalidade,
				TipoStatus = o.TipoStatus,
				DataValidade = o.DataValidade,
				ValorPremio = o.ValorPremio,
				ValorPercentualIndImportado = o.ValorPercentualIndImportado,
				ValorPercentualIndNacional = o.ValorPercentualIndNacional,
				Cnpj = o.Cnpj,
				ListaStatus = o.ListaStatus.Select(q => new PRCStatusVM()
				{
					IdStatus = q.IdStatus,
					IdProcesso = q.IdProcesso,
					Tipo = q.Tipo,
					Data = q.Data,
					DataValidade = q.DataValidade,
					CpfResponsavel = q.CpfResponsavel,
					NomeResponsavel = q.NomeResponsavel,
					NumeroPlano = q.NumeroPlano,
					AnoPlano = q.AnoPlano
				}
				).ToList()
			}
			,
			o =>
			(
				(string.IsNullOrEmpty(pagedFilter.InscricaoCadastral) || pagedFilter.InscricaoCadastral.Equals(o.InscricaoSuframa.ToString()))
				&&
				(string.IsNullOrEmpty(pagedFilter.RazaoSocial) || o.RazaoSocial.Contains(pagedFilter.RazaoSocial))
				&&
				(digitoProcesso == 0 || (digitoProcesso == o.NumeroProcesso && anoProcesso == o.AnoProcesso))
				&&
				(digitoPlano == 0 || o.ListaStatus.Any(w=> w.NumeroPlano == digitoPlano && w.AnoPlano == anoPlano))
				&&
				(string.IsNullOrEmpty(pagedFilter.TipoModalidade) || pagedFilter.TipoModalidade.Equals(o.TipoModalidade))
				&&
				(string.IsNullOrEmpty(pagedFilter.StatusPlano) || pagedFilter.StatusPlano.Equals(o.TipoStatus))
				&&
				(
				(pagedFilter.DataInicio == null) || o.ListaStatus.Any(w => o.TipoStatus.Equals(w.Tipo) && (dataInicio <= w.Data && w.Data <= dataFim))
				)
				&&
				(cnpj == null || o.Cnpj == cnpj)
			),
			pagedFilter);

			foreach (var registro in retornoConsulta.Items)
			{

				registro.NumeroAnoProcessoFormatado = Convert.ToInt32(registro.NumeroProcesso).ToString("D4") + "/" + registro.AnoProcesso;
				registro.TipoModalidadeString = registro.TipoModalidade == "S" ? "SUSPENSÃO"
																		: registro.TipoModalidade == "I" ? "ISENÇÃO"
																		: "-"
																		;

				
				registro.TipoStatusString = registro.TipoStatus.Equals("AP") ? "APROVADO" :
											registro.TipoStatus.Equals("CO") ? "COMPROVADO" :
											registro.TipoStatus.Equals("CA") ? "CANCELADO" :
											registro.TipoStatus.Equals("AL") ? "ALTERADO" :
											registro.TipoStatus.Equals("PR") ? "PRORROGADO" :
											registro.TipoStatus.Equals("PE") ? "PRORROGADO EM CARATER ESPECIAL" : "-";

				var ultimoStatus = registro.ListaStatus.Where(o => o.Tipo == registro.TipoStatus).FirstOrDefault();

				if (ultimoStatus != null)
				{
					registro.NumeroAnoPlanoFormatado = ultimoStatus.NumeroPlano != null ? Convert.ToInt32(ultimoStatus.NumeroPlano).ToString("D5") + "/" + ultimoStatus.AnoPlano : "-";
					registro.DataStatusFormatada = ultimoStatus.Data != null ? ((DateTime)(ultimoStatus.Data)).ToShortDateString() : "-";
				}
				else
				{
					registro.NumeroAnoPlanoFormatado = "-";
				}

				//validação botão prorrogação

				var dataHoje = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
				var regStatus = registro.ListaStatus.Where(o => o.Tipo == "PR" || o.Tipo == "PE").ToList();
				registro.prorrogacaoAndamento = _uowSciex.QueryStackSciex.ProcessoSolicProrrogacao.Listar(q => q.Status == 1 && q.IdProcesso == registro.IdProcesso).Any();


				if (registro.DataValidade >= dataHoje && regStatus.Count < 2 && !registro.prorrogacaoAndamento)
				{
					registro.ProrrogarExibir = true;
					registro.DataValidadeFormatada = Convert.ToDateTime(registro.DataValidade).ToShortDateString();
					registro.DataValidadeProrrogadaFormatada = Convert.ToDateTime(registro.DataValidade).AddDays(180).ToShortDateString();
				}else
				{
					registro.ProrrogarExibir = false;
				}

				registro.JaPossuiProrrogacao = _uowSciex.QueryStackSciex.PRCStatus.Listar(q => q.Tipo == "PR" && q.IdProcesso == registro.IdProcesso).Any();
			}

			if (!string.IsNullOrWhiteSpace(sort))
			{
				switch (sort)
				{
					case "NumeroPlano":
						if (pagedFilter.Reverse)
						{
							retornoConsulta.Items = retornoConsulta.Items.OrderBy(q => q.NumeroAnoPlanoFormatado).ThenBy(q => q.NumeroAnoPlanoFormatado).ToList();
						}
						else
						{
							retornoConsulta.Items = retornoConsulta.Items.OrderByDescending(q => q.NumeroAnoPlanoFormatado).ThenByDescending(q => q.NumeroAnoPlanoFormatado).ToList();
						}
						break;

					case "DataStatus":
						if (pagedFilter.Reverse)
						{
							retornoConsulta.Items = retornoConsulta.Items.OrderBy(q => q.DataStatusFormatada).ThenBy(q => q.DataStatusFormatada).ToList();
						}
						else
						{
							retornoConsulta.Items = retornoConsulta.Items.OrderByDescending(q => q.DataStatusFormatada).ThenByDescending(q => q.DataStatusFormatada).ToList();
						}
						break;
				}
				

			}

			return retornoConsulta;

		}
		
		public ProcessoExportacaoVM Selecionar(int idProcesso)
		{
			var pe = _uowSciex.QueryStackSciex.Processo.SelecionarGrafo(o => new ProcessoExportacaoVM()
			{
				IdProcesso = o.IdProcesso,
				NumeroProcesso = o.NumeroProcesso,
				AnoProcesso = o.AnoProcesso,
				InscricaoSuframa = o.InscricaoSuframa,
				RazaoSocial = o.RazaoSocial,
				TipoModalidade = o.TipoModalidade,
				TipoStatus = o.TipoStatus,
				DataValidade = o.DataValidade,
				ValorPremio = o.ValorPremio,
				ValorPercentualIndImportado = o.ValorPercentualIndImportado,
				ValorPercentualIndNacional = o.ValorPercentualIndNacional,
				Cnpj = o.Cnpj,
				ListaStatus = o.ListaStatus.Select(q => new PRCStatusVM()
				{
					IdStatus = q.IdStatus,
					IdProcesso = q.IdProcesso,
					Tipo = q.Tipo,
					Data = q.Data,
					DataValidade = q.DataValidade,
					CpfResponsavel = q.CpfResponsavel,
					NomeResponsavel = q.NomeResponsavel,
					NumeroPlano = q.NumeroPlano,
					AnoPlano = q.AnoPlano,
					DescricaoObservacao = q.DescricaoObservacao
				}
				).ToList(),

				ListaProduto = o.ListaProduto.Select(q => new PRCProdutoVM()
				{
					IdProduto = q.IdProduto,
					IdProcesso = q.IdProcesso,
					CodigoProdutoExportacao = q.CodigoProdutoExportacao,
					CodigoProdutoSuframa = q.CodigoProdutoSuframa,
					CodigoNCM = q.CodigoNCM,
					QuantidadeComprovado = q.QuantidadeComprovado==null || q.QuantidadeComprovado==0 ? 0: q.QuantidadeComprovado,
					ValorDolarComprovado = q.ValorDolarComprovado == null || q.ValorDolarComprovado == 0 ? 0 : q.ValorDolarComprovado,
					ValorNacionalComprovado = q.ValorNacionalComprovado == null || q.ValorNacionalComprovado == 0 ? 0 : q.ValorNacionalComprovado,

					TipoProduto = q.TipoProduto,
					DescricaoModelo = q.DescricaoModelo,
					QuantidadeAprovado = q.QuantidadeAprovado,
					CodigoUnidade = q.CodigoUnidade,
					ValorDolarAprovado = q.ValorDolarAprovado,
					ValorFluxoCaixa = q.ValorFluxoCaixa,
					ListaInsumos = q.ListaInsumos.Select(w => new PRCInsumoVM()
					{
						IdInsumo = w.IdInsumo,
						IdPrcProduto = w.IdPrcProduto,
						CodigoInsumo = w.CodigoInsumo,
						CodigoUnidade = w.CodigoUnidade,
						TipoInsumo = w.TipoInsumo,
						CodigoNCM = w.CodigoNCM,
						ValorPercentualPerda = w.ValorPercentualPerda,
						CodigoDetalhe = w.CodigoDetalhe,
						DescricaoPartNumber = w.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
						ValorDolarAprovado = w.ValorDolarAprovado,
						QuantidadeAprovado = w.QuantidadeAprovado,
						ValorNacionalAprovado = w.ValorNacionalAprovado,
						ValorDolarFOBAprovado = w.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
						ValorAdicionalFrete = w.ValorAdicionalFrete,
						ValorFreteAprovado = w.ValorFreteAprovado,
						ValorDolarComp = w.ValorDolarComp,
						QuantidadeComp = w.QuantidadeComp,
						ValorAdicional = w.ValorAdicional,
						ValorDolarSaldo = w.ValorDolarSaldo,
						QuantidadeSaldo = w.QuantidadeSaldo,
					}).ToList()
				}).ToList()
			}
			, o => o.IdProcesso == idProcesso);

			pe.NumeroAnoProcessoFormatado = Convert.ToInt32(pe.NumeroProcesso).ToString("D4") + "/" + pe.AnoProcesso;

			var ultimoStatus = pe.ListaStatus.LastOrDefault();

			pe.NumeroAnoPlanoFormatado = ultimoStatus != null ? Convert.ToInt32(ultimoStatus.NumeroPlano).ToString("D5") + "/" + ultimoStatus.AnoPlano : "-";

			pe.DataValidadeFormatada = pe.DataValidade == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)pe.DataValidade).ToShortDateString();

			pe.TipoModalidadeString = pe.TipoModalidade == "S" ? "SUSPENSÃO"
																		: pe.TipoModalidade == "I" ? "ISENÇÃO"
																		: "-"
																		;

			pe.TipoStatusString =	pe.TipoStatus.Equals("AP") ? "APROVADO":
									pe.TipoStatus.Equals("CO") ? "COMPROVADO":
									pe.TipoStatus.Equals("CA") ? "CANCELADO":
									pe.TipoStatus.Equals("AL") ? "ALTERADO" :
									pe.TipoStatus.Equals("PR") ? "PRORROGADO":
									pe.TipoStatus.Equals("PE") ? "PRORROGADO EM CARATER ESPECIAL":"-";

			foreach (var produto in pe.ListaProduto)
			{

				var dadosPrj = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(x => x.CodigoNCM == produto.CodigoNCM 
																							   && x.CodigoTipoProduto == produto.TipoProduto 
																							   && x.CodigoProduto == produto.CodigoProdutoSuframa
																							   && x.InscricaoCadastral == pe.InscricaoSuframa)
																						.FirstOrDefault();

				if(dadosPrj != null)
				{
					produto.DescricaoNCM = dadosPrj.DescricaoNCM;
					produto.DescricaoProduto = dadosPrj.DescricaoProduto;
					produto.DescricaoTipoProduto = dadosPrj.DescricaoTipoProduto;					
				}

				var codigoUnidadeMedida = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == produto.CodigoUnidade);

				if (codigoUnidadeMedida != null)
				{
					produto.DescricaoUnidadeMedida = codigoUnidadeMedida.Descricao;
				}
				
				pe.InsumosAprovados.Nacionais += produto.ListaInsumos.Sum(q => q.ValorNacionalAprovado);
				pe.InsumosAprovados.ImportadosFOB += produto.ListaInsumos.Sum(q => q.ValorDolarAprovado);
				pe.InsumosAprovados.Frete += produto.ListaInsumos.Sum(q => q.ValorFreteAprovado);
				pe.InsumosAprovados.TotalFOB += produto.ListaInsumos.Sum(q => q.ValorNacionalAprovado + q.ValorDolarAprovado);

				pe.Saldos.Nacionais += produto.ListaInsumos.Sum(q => q.ValorNacionalAprovado);
				pe.Saldos.Importados += produto.ListaInsumos.Sum(q => q.ValorDolarSaldo);

				pe.Saldos.ValorAdicionalFrete += produto.ListaInsumos.Sum(q => q.ValorAdicionalFrete);
				pe.Saldos.ValorAdicional += produto.ListaInsumos.Sum(q => q.ValorAdicional);
			}			

			return pe;
		}

		public NovoPlanoExportacaoVM SalvarNovoPlano(NovoPlanoExportacaoVM vm)
		{		
			if (vm == null) { return new NovoPlanoExportacaoVM() { Resultado = false, Mensagem = "sem dados informado na vm: "+ vm}; }
			var retorno = new NovoPlanoExportacaoVM();

			try
			{
				var dadosUsuarioLogado = _usuarioPssBll.ObterUsuarioLogado().Perfis;
				string dadosUsuario = dadosUsuarioLogado.ToJson();

				string cnpj = null;

				if (dadosUsuarioLogado.Contains(EnumPerfil.Preposto))
				{
					cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCnpjRepresentanteLogado.CnpjUnformat();
				}
				else
				{
					cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjUnformat();
				}

				var anoCorrente = DateTime.Now.Year;
				var dadosEmpresa = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == cnpj);

				var sequencial = _uowSciex.QueryStackSciex.BuscarUltimoCodigoSeqPlanoExportacao(cnpj, anoCorrente);

				int statusElaboracao = 1;

				retorno.Mensagem = $"usuCpfCnpjEmpresaOuLogado: [{cnpj}] / dadosUsuarioLogado: [{dadosUsuario}] / dadosEmpresa: [{dadosEmpresa.ToJson()}] / sequencial: [{sequencial}]  / viewModel: [{vm.ToJson()}] /";
				var regNovoPlano = new PlanoExportacaoEntity()
				{
					//NumeroPlano = sequencial == 0 ? sequencial : sequencial + 1,
					NumeroPlano = sequencial,
					AnoPlano = anoCorrente,
					NumeroInscricaoCadastral = dadosEmpresa.InscricaoCadastral,
					Cnpj = cnpj,
					RazaoSocial = dadosEmpresa.RazaoSocial,
					TipoModalidade = vm.Modalidade.ToUpper(),
					TipoExportacao = vm.TipoExportacao.ToUpper(),
					Situacao = statusElaboracao,
					DataCadastro = GetDateTimeNowUtc()

				};

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(regNovoPlano);
				_uowSciex.CommandStackSciex.Save();

				var regNovoArquivo = new PEArquivoEntity()
				{
					IdPlanoExportacao = regNovoPlano.IdPlanoExportacao,
					Anexo = vm.Anexo
				};

				_uowSciex.CommandStackSciex.PEArquivo.Salvar(regNovoArquivo);
				_uowSciex.CommandStackSciex.Save();

				retorno.IdPlanoExportacao = regNovoPlano.IdPlanoExportacao;

			}
			catch (Exception e)
			{
				retorno.Resultado = false;
				retorno.Mensagem += $"Mensagem: [{e.Message}] / InnerException: [{e.InnerException}] / StackTrace: [{e.StackTrace}]";
				return retorno;
			}
			retorno.Resultado = true;
			return retorno;
		}

		public bool CopiarPlano(PlanoExportacaoVM vm)
		{
			if (vm == null || vm.IdPlanoExportacao == 0)
				return false;

			var dadosCopiaPlano = new NovoPlanoExportacaoVM()
			{
				Modalidade = vm.TipoModalidade.ToUpper(),
				TipoExportacao = vm.TipoExportacao.ToUpper()
			};

			var idPlanoExportacaoOrigem = vm.IdPlanoExportacao;

			var regAnexo = _uowSciex.QueryStackSciex.PEArquivo.Selecionar(q => q.IdPlanoExportacao == idPlanoExportacaoOrigem);

			dadosCopiaPlano.Anexo = regAnexo.Anexo;
			dadosCopiaPlano.IsCopia = true;

			var resultadoNovoRegistro = SalvarNovoPlano(dadosCopiaPlano);

			if (!resultadoNovoRegistro.Resultado)
			{
				return false;
			}
			else
			{
				var idPlanoExportacaoCopia = resultadoNovoRegistro.IdPlanoExportacao;

				var regCopia = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == idPlanoExportacaoCopia);

				var listaProdutoOrigem = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q => q.IdPlanoExportacao == idPlanoExportacaoOrigem);

				if (listaProdutoOrigem.Count > 0)
				{
					var listaProdutoCopia = new List<PEProdutoEntity>();

					CopiarRegistroProduto(listaProdutoOrigem, idPlanoExportacaoCopia);

					var listaIdProdutoCopia = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q => q.IdPlanoExportacao == idPlanoExportacaoCopia).Select(q => q.IdPEProduto).ToList();

				}

				
			}

			return true;
		}

		public PlanoExportacaoVM SalvarAnexo(PlanoExportacaoVM vm)
		{
			if (vm.ListaAnexos == null && vm.ListaAnexos.Count == 0) 
			{ 
				return new PlanoExportacaoVM(); 
			}

			var id = vm.ListaAnexos[0].IdPlanoExportacaoArquivo;
			var arq = _uowSciex.QueryStackSciex.PEArquivo.Selecionar(o => o.IdPlanoExportacaoArquivo == id);
			arq.Anexo = vm.ListaAnexos[0].Anexo;
			arq.NomeArquivo = vm.ListaAnexos[0].NomeArquivo;
			_uowSciex.CommandStackSciex.PEArquivo.Salvar(arq);
			_uowSciex.CommandStackSciex.Save();

			return vm;
		}

		private void CopiarRegistroProduto(List<PEProdutoEntity> listaProdutoOrigem, int idPlanoExportacaoCopia)
		{

			foreach (var regProdutoOrigem in listaProdutoOrigem)
			{
					var regProdCopia = new PEProdutoEntity()
				{
					IdPlanoExportacao = idPlanoExportacaoCopia,
					CodigoProdutoExportacao = regProdutoOrigem.CodigoProdutoExportacao,
					CodigoProdutoSuframa = regProdutoOrigem.CodigoProdutoSuframa,
					CodigoNCM = regProdutoOrigem.CodigoNCM,
					CodigoTipoProduto = regProdutoOrigem.CodigoTipoProduto,
					DescricaoModelo = regProdutoOrigem.DescricaoModelo,
					Qtd = regProdutoOrigem.Qtd,
					ValorDolar = regProdutoOrigem.ValorDolar,
					ValorFluxoCaixa = regProdutoOrigem.ValorFluxoCaixa,
					CodigoUnidade = regProdutoOrigem.CodigoUnidade
				};

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProdCopia);
				_uowSciex.CommandStackSciex.Save();


				var listaPaisProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(q => q.IdPEProduto == regProdutoOrigem.IdPEProduto);

				if (listaPaisProduto.Count > 0)
				{
					CopiarRegistroPaisProduto(listaPaisProduto, regProdCopia.IdPEProduto);
				}

				var listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == regProdutoOrigem.IdPEProduto);

				if (listaInsumos.Count > 0)
				{
					CopiarRegistroInsumos(listaInsumos, regProdCopia.IdPEProduto);
				}

			}

		}

		private void CopiarRegistroInsumos(List<PEInsumoEntity> listaInsumos, int idPEProduto)
		{
			foreach (var regInsumo in listaInsumos)
			{

				var regInsumoCopia = new PEInsumoEntity()
				{
					IdPEProduto = idPEProduto,
					CodigoDetalhe = regInsumo.CodigoDetalhe,
					CodigoInsumo = regInsumo.CodigoInsumo,
					CodigoNcm = regInsumo.CodigoNcm,
					CodigoUnidade = regInsumo.CodigoUnidade,
					DescricaoEspecificacaoTecnica = regInsumo.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = regInsumo.DescricaoInsumo,
					DescricaoPartNumber = regInsumo.DescricaoPartNumber,
					TipoInsumo = regInsumo.TipoInsumo,
					ValorCoeficienteTecnico = regInsumo.ValorCoeficienteTecnico,
					ValorDolar = regInsumo.ValorDolar,
					ValorPercentualPerda = regInsumo.ValorPercentualPerda
				};

				_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoCopia);
				_uowSciex.CommandStackSciex.Save();



				var listaDetalhesInsumoOrigem = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == regInsumo.IdPEInsumo);

				if (listaDetalhesInsumoOrigem.Count > 0)
				{
					
					foreach (var regDetalheInsumoOrigem in listaDetalhesInsumoOrigem)
					{

						var regDetalheInsumoCopia = new PEDetalheInsumoEntity()
						{
							IdPEInsumo = regInsumoCopia.IdPEInsumo,
							CodigoPais = regDetalheInsumoOrigem.CodigoPais,
							IdMoeda = regDetalheInsumoOrigem.IdMoeda,
							//Moeda = regDetalheInsumoOrigem.Moeda,
							NumeroSequencial = regDetalheInsumoOrigem.NumeroSequencial,
							Quantidade = regDetalheInsumoOrigem.Quantidade,
							ValorDolar = regDetalheInsumoOrigem.ValorDolar,
							ValorDolarCRF = regDetalheInsumoOrigem.ValorDolarCRF,
							ValorDolarFOB = regDetalheInsumoOrigem.ValorDolarFOB,
							ValorFrete = regDetalheInsumoOrigem.ValorFrete,
							ValorUnitario = regDetalheInsumoOrigem.ValorUnitario
						};

						//_uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(x => x.idP)
						//var pais = new PEProdutoPaisEntity();

						_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalheInsumoCopia);
						_uowSciex.CommandStackSciex.Save();
					}

				}
				
			}
		}

		private void CopiarRegistroPaisProduto(List<PEProdutoPaisEntity> listaPaisProduto, int idPEProdutoCopia)
		{
			foreach (var regPaisProduto in listaPaisProduto)
			{
				var regPaisProdutoCopia = new PEProdutoPaisEntity()
				{
					IdPEProduto = idPEProdutoCopia,
					CodigoPais = regPaisProduto.CodigoPais,
					//PlanoExportacaoProduto = regPaisProduto.PlanoExportacaoProduto,
					Quantidade = regPaisProduto.Quantidade,
					ValorDolar = regPaisProduto.ValorDolar
				};

				_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPaisProdutoCopia);
				_uowSciex.CommandStackSciex.Save();
			}
		}

		public ResultadoProcessamentoVM EntregarPlano(PlanoExportacaoVM vm)
		{
			if (vm == null || vm.IdPlanoExportacao == 0)
				return new ResultadoProcessamentoVM { Resultado = false , Mensagem = "Faltando dados" };

			var retorno = new ResultadoProcessamentoVM()
			{
				Resultado = true
			};

			try
			{
				var regPlano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == vm.IdPlanoExportacao);

				regPlano.Situacao = (int)EnumSituacaoPlanoExportacao.ENTREGUE;
				regPlano.DataEnvio = GetDateTimeNowUtc();

				CalcularValoresImportados(vm.IdPlanoExportacao);

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(regPlano);
				_uowSciex.CommandStackSciex.Save();
			}
			catch (Exception e)
			{
				retorno.Resultado = false;
				retorno.Mensagem = e.Message; //$"Falha ao processar - MensagemErro: {e.Message} / StackTrace: {e.StackTrace} / InnerException: {e.InnerException}";
				return retorno;
			}

			return retorno;
		}

		private void CalcularValoresImportados(int idPlanoExportacao)
		{
			var listaProdutos = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).ToList();

			if (listaProdutos.Count > 0)
			{
				decimal fatorConvEmDolar = 0;
				decimal somatorioValorDolar = 0;
				decimal somatorioValorDolarFOB = 0;

				try
				{
					foreach (var regProduto in listaProdutos)
					{
						_uowSciex.CommandStackSciex.DetachEntries();

						var listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == regProduto.IdPEProduto).ToList();

						if (listaInsumos.Count > 0)
						{
							foreach (var regInsumo in listaInsumos)
							{
								_uowSciex.CommandStackSciex.DetachEntries();

								var listaDetalhesInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == regInsumo.IdPEInsumo).ToList();

								if (listaDetalhesInsumo.Count > 0)
								{
									foreach (var regDetalheInsumo in listaDetalhesInsumo)
									{
										_uowSciex.CommandStackSciex.DetachEntries();

										decimal valorInsDolar = 0;

										if (regDetalheInsumo.Moeda.CodigoMoeda != 220)
										{
											CalcularFatorConversao(regDetalheInsumo.IdMoeda, ref fatorConvEmDolar);

											valorInsDolar = regDetalheInsumo.ValorUnitario * regDetalheInsumo.Quantidade * fatorConvEmDolar;
										}
										else
										{
											valorInsDolar = regDetalheInsumo.ValorUnitario * regDetalheInsumo.Quantidade;
										}


										regDetalheInsumo.ValorDolar = valorInsDolar + (regDetalheInsumo.ValorFrete != null ? regDetalheInsumo.ValorFrete : 0);
										regDetalheInsumo.ValorDolarFOB = valorInsDolar;
										regDetalheInsumo.ValorDolarCRF = valorInsDolar + (regDetalheInsumo.ValorFrete != null ? regDetalheInsumo.ValorFrete : 0);

										_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalheInsumo);
										_uowSciex.CommandStackSciex.Save();

										somatorioValorDolar += regDetalheInsumo.ValorDolar != null ? (decimal)regDetalheInsumo.ValorDolar : 0;
										somatorioValorDolarFOB += regDetalheInsumo.ValorDolarFOB != null ? (decimal)regDetalheInsumo.ValorDolarFOB : 0;

										_uowSciex.CommandStackSciex.DetachEntries();
									}

									var regInsumoAtualizado = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == regInsumo.IdPEInsumo);

									regInsumoAtualizado.ValorDolar = somatorioValorDolar;

									_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAtualizado);
									_uowSciex.CommandStackSciex.Save();

									#region CalculoFluxoCaixa
									var valorTotInsFOB = somatorioValorDolarFOB;
									var valorProduto = regProduto.ValorDolar;

									var fluxoCaixa = (1 - valorTotInsFOB / valorProduto) * 100;

									var regProdutoAtualizado = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == regProduto.IdPEProduto);
									regProdutoAtualizado.ValorFluxoCaixa = fluxoCaixa;
									#endregion


									_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProdutoAtualizado);
									_uowSciex.CommandStackSciex.Save();

								}

								_uowSciex.CommandStackSciex.DetachEntries();
							}
						}

						_uowSciex.CommandStackSciex.DetachEntries();
					}
				}
				catch (Exception e)
				{
					throw new Exception("A soma dos valores resultou em um valor acima do permitido. Por favor, revise os valores informados");
				}
			}
		}

		private void CalcularFatorConversao(int? idMoeda, ref decimal fatorConvEmDolar)
		{
			decimal fatorMoedaEstrangeira = 0;
			decimal fatorMoedaDolar = 0;
			var dataHoje = DateTime.Now.Date;

			fatorMoedaEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == idMoeda && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			int codigoDolar = 220;
			fatorMoedaDolar = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.CodigoMoeda == codigoDolar && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			fatorConvEmDolar = fatorMoedaEstrangeira / fatorMoedaDolar;
		}

		public ResultadoProcessamentoVM ValidarPlano(int idPlanoExportacao, ResultadoProcessamentoVM retorno)
		{
			var listaProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).ToList();

			try
			{
				if (listaProduto.Count == 0)
				{
					if (retorno.CamposNaoValidos == null)
						retorno.CamposNaoValidos = new CamposNaoValidadosVM();

					retorno.CamposNaoValidos.NaoExisteProduto = true;
				}
				else
				{
					ValidarPaisPorProduto(ref retorno, listaProduto);
				}
			}
			catch (Exception e)
			{
				retorno.Resultado = false;
				retorno.Mensagem = $"Falha ao processar - MensagemErro: {e.Message} / StackTrace: {e.StackTrace} / InnerException: {e.InnerException}";
				return retorno;
			}

			

			return retorno;
		}
		

		private void ValidarPaisPorProduto(ref ResultadoProcessamentoVM retorno, List<PEProdutoEntity> listaProduto)
		{
			foreach (var regProduto in listaProduto)
			{

				if (regProduto.ListaPEProdutoPais.Count == 0)
				{
					if (retorno.CamposNaoValidos == null)
						retorno.CamposNaoValidos = new CamposNaoValidadosVM();

					retorno.CamposNaoValidos.NaoExistePais = true;
					retorno.CamposNaoValidos.IdProduto = regProduto.IdPEProduto;
				}


				if (regProduto.ListaPEInsumo.Count == 0)
				{
					if (retorno.CamposNaoValidos == null)
						retorno.CamposNaoValidos = new CamposNaoValidadosVM();

					retorno.CamposNaoValidos.NaoExisteInsumo = true;
					retorno.CamposNaoValidos.IdProduto = regProduto.IdPEProduto;
				}
				else
				{
					ValidarDetalhesInsumo(ref retorno, regProduto.ListaPEInsumo);

					if (retorno.CamposNaoValidos != null
						&&
						(retorno.CamposNaoValidos.NaoExisteParidadeCambial || retorno.CamposNaoValidos.NaoExisteParidadeCambialEstrangeira)
						)
						break;
				}

			}
		}

		private void ValidarDetalhesInsumo(ref ResultadoProcessamentoVM retorno, ICollection<PEInsumoEntity> listaInsumo)
		{
			foreach (var regInsumo in listaInsumo)
			{
				//var listaDetalhe = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == regInsumo.IdPEInsumo).ToList();

				if (regInsumo.ListaPEDetalheInsumo.Count == 0)
				{
					if (retorno.CamposNaoValidos == null)
						retorno.CamposNaoValidos = new CamposNaoValidadosVM();

					retorno.CamposNaoValidos.NaoExisteDetalhe = true;
					retorno.CamposNaoValidos.IdInsumo = regInsumo.IdPEInsumo;

					if (regInsumo.TipoInsumo.Equals("N") || regInsumo.TipoInsumo.Equals("R"))
					{
						retorno.CamposNaoValidos.IsNacional = true;
						break;
					}
					else
					{
						retorno.CamposNaoValidos.IsNacional = false;
						break;
					}
					
				}
				else
				{
					ValidarParidadeCambial(ref retorno, regInsumo.ListaPEDetalheInsumo);

					if (retorno.CamposNaoValidos != null 
						&& 
						(
						retorno.CamposNaoValidos.NaoExisteParidadeCambial || retorno.CamposNaoValidos.NaoExisteParidadeCambialEstrangeira)
						)
						break;
				}
			}
		}
		private void ValidarParidadeCambial(ref ResultadoProcessamentoVM retorno, ICollection<PEDetalheInsumoEntity> listaPEDetalheInsumo)
		{
			var dataHoje = DateTime.Now.Date;
			foreach (var regDetalheInsumo in listaPEDetalheInsumo)
			{
				ParidadeValorEntity regParidadeCambial = null;
				ParidadeValorEntity regParidadeCambialEstrangeira = null;

				int codigoMoedaDolar = 220;
				if (regDetalheInsumo.IdMoeda != codigoMoedaDolar)
				{
					regParidadeCambial = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.CodigoMoeda == codigoMoedaDolar && q.ParidadeCambial.DataParidade == dataHoje);
					regParidadeCambialEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == regDetalheInsumo.IdMoeda && q.ParidadeCambial.DataParidade == dataHoje);

					if (regParidadeCambial == null || regParidadeCambialEstrangeira == null)
					{
						if (regParidadeCambial == null)
						{
							if (retorno.CamposNaoValidos == null)
								retorno.CamposNaoValidos = new CamposNaoValidadosVM();

							retorno.CamposNaoValidos.NaoExisteParidadeCambial = true;
							break;
						}
						else
						{
							if (retorno.CamposNaoValidos == null)
								retorno.CamposNaoValidos = new CamposNaoValidadosVM();

							retorno.CamposNaoValidos.NaoExisteParidadeCambialEstrangeira = true;
							break;
						}

						
					}
				}
				else
				{
					regParidadeCambial = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == codigoMoedaDolar && q.ParidadeCambial.DataParidade == dataHoje);

					if (regParidadeCambial == null)
					{
						if (retorno.CamposNaoValidos == null)
							retorno.CamposNaoValidos = new CamposNaoValidadosVM();

						retorno.CamposNaoValidos.NaoExisteParidadeCambial = true;
						break;
					}
				}

				
			}
		}

		public bool DeletarPlano(int idPlanoExportacao)
		{
			if (idPlanoExportacao == 0)
				return false;

			var listaIdAnexo = _uowSciex.QueryStackSciex.PEArquivo.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).Select(q=> q.IdPlanoExportacaoArquivo).ToList();

			if (listaIdAnexo.Count > 0)
			{
				foreach (var idAnexo in listaIdAnexo)
				{
					_uowSciex.CommandStackSciex.PEArquivo.Apagar(idAnexo);
				}
				_uowSciex.CommandStackSciex.Save();
			}

			var listaIdProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).Select(q => q.IdPEProduto).ToList();

			if (listaIdProduto.Count > 0)
			{
				foreach (var idProduto in listaIdProduto)
				{

					var listaIdInsumo = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == idProduto).Select(q => q.IdPEInsumo).ToList();

					if (listaIdInsumo.Count > 0)
					{
						foreach (var idInsumo in listaIdInsumo)
						{
							var listaIdDetalheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == idInsumo).Select(q => q.IdPEDetalheInsumo).ToList();

							if (listaIdDetalheInsumo.Count > 0)
							{
								foreach (var idDetalheInsumo in listaIdDetalheInsumo)
								{
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Apagar(idDetalheInsumo);
								}
								_uowSciex.CommandStackSciex.Save();
							}

							_uowSciex.CommandStackSciex.PEInsumo.Apagar(idInsumo);
							_uowSciex.CommandStackSciex.Save();
						}
					}

					var listaPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(q => q.IdPEProduto == idProduto).Select(q => q.IdPEProdutoPais).ToList();

					if(listaPais.Count > 0)
					{
						foreach (var idPais in listaPais)
						{
							_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Apagar(idPais);
						}
						_uowSciex.CommandStackSciex.Save();
					}


					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Apagar(idProduto);
					_uowSciex.CommandStackSciex.Save();
				}
			}

			_uowSciex.CommandStackSciex.PlanoExportacao.Apagar(idPlanoExportacao);
			_uowSciex.CommandStackSciex.Save();

			return true;
		}
	}
}