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
using Suframa.Sciex.BusinessLogic.Pss;
using System.Text;
using System.Web.UI;
using Suframa.Sciex.CrossCutting.Mensagens;
using Suframa.Sciex.CrossCutting.Compressor;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Suframa.Sciex.BusinessLogic
{
	public class PlanoExportacaoBll : IPlanoExportacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;


		private long _idPLiRetorno;

		public PlanoExportacaoBll(
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
		public PagedItems<PlanoExportacaoVM> ListarPaginado(ConsultarPlanoExportacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			var dadosUsuarioLogado = _usuarioPssBll.ObterUsuarioLogado().Perfis;

			string cnpj = null;

			if (dadosUsuarioLogado.Contains(EnumPerfil.Preposto))
			{
				cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCnpjRepresentanteLogado.CnpjUnformat();
			}
			else
			{
				cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.CnpjUnformat();
			}

			if (pagedFilter == null) { return new PagedItems<PlanoExportacaoVM>(); }

			//var retornoConsulta = new PagedItems<PlanoExportacaoVM>();

			long digitoPlano = 0;
			int anoPlano = 0;

			if (!string.IsNullOrEmpty(pagedFilter.NumeroPlano))
			{
				digitoPlano = Convert.ToInt64(pagedFilter.NumeroPlano.Substring(0, 5));
				anoPlano = Convert.ToInt32(pagedFilter.NumeroPlano.Substring(6, 4));
			}


			var retornoConsulta = _uowSciex.QueryStackSciex.PlanoExportacao.ListarPaginadoGrafo(o => new PlanoExportacaoVM()
			{
				IdPlanoExportacao = o.IdPlanoExportacao,
				NumeroPlano = o.NumeroPlano,
				AnoPlano = o.AnoPlano,
				NumeroInscricaoCadastral = o.NumeroInscricaoCadastral,
				Cnpj = o.Cnpj,
				RazaoSocial = o.RazaoSocial,
				TipoModalidade = o.TipoModalidade,
				TipoExportacao = o.TipoExportacao,
				Situacao = o.Situacao,
				DataEnvio = o.DataEnvio,
				DataCadastro = o.DataCadastro,
				DataStatus = o.DataStatus,
				CpfResponsavel = o.CpfResponsavel,
				NomeResponsavel = o.NomeResponsavel,
				NumeroProcesso =o.NumeroProcesso,
				NumeroAnoProcesso =o.NumeroAnoProcesso!=null ? o.NumeroAnoProcesso :null,
				ListaPEProdutos = o.ListaPEProdutos.Select(q => new PEProdutoVM()
				{
					IdPEProduto = q.IdPEProduto,
					IdPlanoExportacao = q.IdPlanoExportacao,
					CodigoProdutoExportacao = q.CodigoProdutoExportacao,
					CodigoProdutoSuframa = q.CodigoProdutoSuframa,
					CodigoNCM = q.CodigoNCM,
					CodigoTipoProduto = q.CodigoTipoProduto,
					DescricaoModelo = q.DescricaoModelo,
					Qtd = q.Qtd,
					ValorDolar = q.ValorDolar,
					ValorFluxoCaixa = q.ValorFluxoCaixa,
					CodigoUnidade = q.CodigoUnidade
				}
				).ToList()
			}
			,
			o =>
			(
				(digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano)
				&&
				(pagedFilter.StatusPlano == null || o.Situacao == pagedFilter.StatusPlano)
				&&
				(
					(pagedFilter.DataInicio == null) || (dataInicio <= o.DataCadastro && o.DataCadastro <= dataFim)
				)
				&&
				(o.Cnpj == cnpj)
			),
			pagedFilter);

			foreach (var registro in retornoConsulta.Items)
			{
				registro.DataCadastroFormatada = registro.DataCadastro == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)registro.DataCadastro).ToShortDateString();
				registro.NumeroAnoPlanoFormatado = registro.NumeroPlano.ToString("D5") + "/" + registro.AnoPlano;
				registro.NumeroAnoProcessoFormatado = registro.NumeroProcesso != null 
															? ((int)(registro.NumeroProcesso)).ToString("D4") + "/" + registro.NumeroAnoProcesso
															: "-";
				registro.SituacaoString = registro.Situacao != null ? Enum.GetName(typeof(EnumSituacaoPlanoExportacao), registro.Situacao).Replace("_", " ") : "-";
				//registro.SituacaoString = registro.Situacao == 1 ? "EM ELABORAÇÃO"
				//								: registro.Situacao == 2 ? "ENTREGUE"
				//								: registro.Situacao == 3 ? "EM ANÁLISE"
				//								: "-";

				registro.TipoExportacaoString = registro.TipoExportacao == "AP" ? "APROVAÇÃO"
													: registro.TipoExportacao == "CO" ? "COMPROVAÇÃO"
													: "-";

				registro.TipoModalidadeString = registro.TipoModalidade == "S" ? "SUSPENSÃO"
													: "-";

				foreach (var item in registro.ListaPEProdutos)
				{
					var codProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == item.CodigoProdutoSuframa).FirstOrDefault();
					item.DescCodigoProdutoSuframa = codProdSuf.CodigoProduto.ToString("D4") + " | " + codProdSuf.DescricaoProduto;
					var codTipoProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == item.CodigoProdutoSuframa && o.CodigoTipoProduto == item.CodigoTipoProduto).FirstOrDefault();
					item.DescCodigoTipoProduto = codTipoProdSuf.CodigoTipoProduto.ToString("D3") + " | " + codTipoProdSuf.DescricaoTipoProduto;
					var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == item.CodigoUnidade);
					item.DescCodigoUnidade = undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao;
				}

			}

			return retornoConsulta;

		}

		public PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUEPaginado(PEProdutoVM pagedFilter)
		{
			if (pagedFilter == null || pagedFilter.IdPEProduto == 0) { return new PagedItems<PlanoExportacaoDUEComplementoVM>(); }
	
			var listaProdutoPaisEntity = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.ListarGrafo(o => new PEProdutoPaisVM()
			{
				IdPEProduto = o.IdPEProduto,
				IdPEProdutoPais = o.IdPEProdutoPais,
				CodigoPais = o.CodigoPais
			},
			o=>
			(
				o.IdPEProduto == pagedFilter.IdPEProduto
			)
			)
			.ToList();
			var idPEProduto = listaProdutoPaisEntity.Select(q => q.IdPEProduto).FirstOrDefault();
			var listaIdProdutoPais = listaProdutoPaisEntity.Select(q => (int?)q.IdPEProdutoPais).ToList();

			string sort = null;
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DescricaoPais"))
			{
				sort = "DescricaoPais";
				pagedFilter.Sort = null;
			}

			List<int?> StatusDue = new List<int?>()
			{
				null,
				(int)EnumSituacaoAnaliseDUE.APROVADO,
				(int)EnumSituacaoAnaliseDUE.REPROVADO,
				(int)EnumSituacaoAnaliseDUE.CORRIGIDO,
				(int)EnumSituacaoAnaliseDUE.NOVO
			};

			var listaDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.ListarPaginadoGrafo(q => new PlanoExportacaoDUEComplementoVM()
			{
				IdPEProduto = idPEProduto,
				IdPEProdutoPais = q.IdPEProdutoPais,
				IdDue = q.IdDue,
				CodigoPais = q.CodigoPais,
				Numero = q.Numero,
				DataAverbacao = q.DataAverbacao,
				Quantidade = q.Quantidade,
				ValorDolar = q.ValorDolar,
				SituacaoAnalise = q.SituacaoAnalise
			},
			q=> listaIdProdutoPais.Contains(q.IdPEProdutoPais)
			&&
			StatusDue.Contains(q.SituacaoAnalise)
			, pagedFilter);

			foreach (var item in listaDUE.Items)
			{
				string codigoPais = item.CodigoPais.ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
				item.DataAverbacaoFormatada = item.DataAverbacao != DateTime.MinValue ? item.DataAverbacao.ToShortDateString() : "-";
			}

			if (!string.IsNullOrWhiteSpace(sort))
			{
				switch (sort)
				{
					case "DescricaoPais":
						if (pagedFilter.Reverse)
						{
							listaDUE.Items = listaDUE.Items.OrderBy(q => q.DescricaoPais).ThenBy(q => q.DescricaoPais).ToList();
						}
						else
						{
							listaDUE.Items = listaDUE.Items.OrderByDescending(q => q.DescricaoPais).ThenByDescending(q => q.DescricaoPais).ToList();
						}
						break;
				}
			}

			return listaDUE;
		}
		public PlanoExportacaoVM Selecionar(int idPlanoExportacao)
		{
			var pe = _uowSciex.QueryStackSciex.PlanoExportacao.SelecionarGrafo(o => new PlanoExportacaoVM()
			{
				IdPlanoExportacao = o.IdPlanoExportacao,
				NumeroPlano = o.NumeroPlano,
				AnoPlano = o.AnoPlano,
				NumeroInscricaoCadastral = o.NumeroInscricaoCadastral,
				Cnpj = o.Cnpj,
				RazaoSocial = o.RazaoSocial,
				TipoModalidade = o.TipoModalidade,
				TipoExportacao = o.TipoExportacao,
				Situacao = o.Situacao,
				DataEnvio = o.DataEnvio,
				DataCadastro = o.DataCadastro,
				DataStatus = o.DataStatus,
				CpfResponsavel = o.CpfResponsavel,
				NomeResponsavel = o.NomeResponsavel,
				NumeroProcesso = o.NumeroProcesso,
				NumeroAnoProcesso= o.NumeroAnoProcesso,
				
				ListaPEProdutos = o.ListaPEProdutos.Select(q => new PEProdutoVM()
				{
					IdPEProduto = q.IdPEProduto,
					IdPlanoExportacao = q.IdPlanoExportacao,
					CodigoProdutoExportacao = q.CodigoProdutoExportacao,
					CodigoProdutoSuframa = q.CodigoProdutoSuframa,
					CodigoNCM = q.CodigoNCM,
					CodigoTipoProduto = q.CodigoTipoProduto,
					DescricaoModelo = q.DescricaoModelo,
					Qtd = q.Qtd,
					ValorDolar = q.ValorDolar,
					ValorFluxoCaixa = q.ValorFluxoCaixa,
					CodigoUnidade = q.CodigoUnidade,
					ValorNacional =q.ValorNacional
				}
				).ToList(),
				ListaAnexos = o.ListaAnexos.Select(q => new PEArquivoVM()
				{
					Anexo = q.Anexo,
					IdPlanoExportacao = q.IdPlanoExportacao,
					IdPlanoExportacaoArquivo = q.IdPlanoExportacaoArquivo,
					NomeArquivo= q.NomeArquivo
					
				}).ToList()
			},
			o => o.IdPlanoExportacao == idPlanoExportacao);


			pe.DataCadastroFormatada = pe.DataCadastro == DateTime.MinValue || pe.DataCadastro == null ? DateTime.MinValue.ToShortDateString() : ((DateTime)pe.DataCadastro).ToShortDateString();
			pe.NumeroAnoPlanoFormatado = pe.NumeroPlano.ToString("D5") + "/" + pe.AnoPlano;
			pe.NumeroAnoProcessoFormatado = pe.NumeroProcesso != null
															? ((int)(pe.NumeroProcesso)).ToString("D4") + "/" + pe.NumeroAnoProcesso
															: "-";
			pe.SituacaoString = pe.Situacao != null ? Enum.GetName(typeof(EnumSituacaoPlanoExportacao), pe.Situacao).Replace("_", " ") : "-";
			//pe.SituacaoString = pe.Situacao == 1 ? "EM ELABORAÇÃO"
			//								: pe.Situacao == 2 ? "ENTREGUE"
			//								: pe.Situacao == 3 ? "EM ANÁLISE"
			//								: "-";

			pe.TipoExportacaoString = pe.TipoExportacao == "AP" ? "APROVAÇÃO"
												: pe.TipoExportacao == "CO" ? "COMPROVAÇÃO"
												: "-";

			pe.TipoModalidadeString = pe.TipoModalidade == "S" ? "SUSPENSÃO"
												: "-";

			foreach (var item in pe.ListaPEProdutos)
			{
				var codProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == item.CodigoProdutoSuframa).FirstOrDefault();
				item.DescCodigoProdutoSuframa = codProdSuf.CodigoProduto.ToString("D4") + " | " + codProdSuf.DescricaoProduto;
				var codTipoProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == item.CodigoProdutoSuframa && o.CodigoTipoProduto == item.CodigoTipoProduto).FirstOrDefault();
				item.DescCodigoTipoProduto = codTipoProdSuf.CodigoTipoProduto.ToString("D3") + " | " + codTipoProdSuf.DescricaoTipoProduto;
				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == item.CodigoUnidade);
				item.DescCodigoUnidade = undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao;

				item.QtdFormatado = item.Qtd != 0 ? item.Qtd.ToString("N5") : "0";
				item.ValorDolarFormatado = item.ValorDolar != 0 ? item.ValorDolar.ToString("N") : "0";
			}

			return pe;
		}

		public NovoPlanoExportacaoVM SalvarNovoPlano(NovoPlanoExportacaoVM vm)
		{
			if (vm == null) { return new NovoPlanoExportacaoVM() { Resultado = false, Mensagem = "sem dados informado na vm: " + vm }; }
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

				if (vm.TipoExportacao == "CO")
				{
					int i;
					string NumeroProcesso = " ";
					string AnoProcesso = "";

					vm.NumeroProcesso = vm.NumeroProcesso.Replace("/", "");
					for (i = 0; i < vm.NumeroProcesso.Length; i++)
					{
						if (i < 4)
						{
							NumeroProcesso += vm.NumeroProcesso[i];
						}
						else
						{

							AnoProcesso += vm.NumeroProcesso[i];
						}
					}
					int AnoProcessoInt = Int32.Parse(AnoProcesso);
					int NumeroProcessoInt = Int32.Parse(NumeroProcesso);
					var possuiProcessoEmAprovacao = _uowSciex.QueryStackSciex.Processo.Selecionar(x => x.AnoProcesso == AnoProcessoInt 
																							&& x.NumeroProcesso == NumeroProcessoInt 
																							&& x.Cnpj == cnpj
																							&& x.TipoStatus == "AP"
																							);

					if (possuiProcessoEmAprovacao == null)
					{
						vm.Resultado = false;
						vm.CodigoErro = 1;
						vm.Mensagem = "Ano e Número de Processo é inválido.";
						return vm;
					}

					var jaPossuiProcessoDoTipoComprovacao = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(x => x.NumeroAnoProcesso == AnoProcessoInt
																							&& x.NumeroProcesso == NumeroProcessoInt
																							&& x.Cnpj == cnpj
																							&& x.TipoExportacao == "CO"
																							);

					if (jaPossuiProcessoDoTipoComprovacao != null)
					{
						vm.Resultado = false;
						vm.CodigoErro = 1;
						vm.Mensagem = "Número de Processo já possui Plano de Exportacao do tipo comprovação.";
						return vm;
					}



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
						DataCadastro = GetDateTimeNowUtc(),
						NumeroProcesso = NumeroProcessoInt,
						NumeroAnoProcesso = AnoProcessoInt
					};

					_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(regNovoPlano);
					_uowSciex.CommandStackSciex.Save();

					var regNovoArquivo = new PEArquivoEntity()
					{
						IdPlanoExportacao = regNovoPlano.IdPlanoExportacao,
						Anexo = vm.Anexo,
						NomeArquivo = vm.NomeAnexo
					};
					_uowSciex.CommandStackSciex.PEArquivo.Salvar(regNovoArquivo);
					_uowSciex.CommandStackSciex.Save();

					var IdPlanoExportacaoAprovacao = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.NumeroProcesso == NumeroProcessoInt
																											&&
																											q.NumeroAnoProcesso == AnoProcessoInt
																											&&
																											q.Cnpj == cnpj
																											&&
																											q.TipoExportacao == "AP").IdPlanoExportacao;

					_uowSciex.CommandStackSciex.DetachEntries();

					var listaProdutosParaComprovacao = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q =>
																								q.IdPlanoExportacao == IdPlanoExportacaoAprovacao);

					foreach (var produto in listaProdutosParaComprovacao)
					{
						var regProdutoComprovacao = new PEProdutoEntity();
						regProdutoComprovacao = produto;
						regProdutoComprovacao.IdPEProduto = 0;
						regProdutoComprovacao.ValorDolar = 0;
						regProdutoComprovacao.ValorFluxoCaixa = 0;
						regProdutoComprovacao.Qtd = 0;
						regProdutoComprovacao.IdPlanoExportacao = regNovoPlano.IdPlanoExportacao;

						_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProdutoComprovacao);
						
					}
					_uowSciex.CommandStackSciex.Save();

					retorno.IdPlanoExportacao = regNovoPlano.IdPlanoExportacao;

				}
				else
				{
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
						Anexo = vm.Anexo,
						NomeArquivo = vm.NomeAnexo
					};
					_uowSciex.CommandStackSciex.PEArquivo.Salvar(regNovoArquivo);
					_uowSciex.CommandStackSciex.Save();

					retorno.IdPlanoExportacao = regNovoPlano.IdPlanoExportacao;
				}
			
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

			var regAnexo = _uowSciex.QueryStackSciex.PEArquivo.Listar(q => q.IdPlanoExportacao == idPlanoExportacaoOrigem).LastOrDefault();

			dadosCopiaPlano.Anexo = regAnexo?.Anexo ?? null;
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

			if (arq != null)
			{
				arq.Anexo = vm.ListaAnexos[0].Anexo;

				arq.NomeArquivo = vm.ListaAnexos[0].NomeArquivo;
			}
			else
			{
				arq = new PEArquivoEntity()
				{
					NomeArquivo = vm.ListaAnexos[0].NomeArquivo,
					Anexo = vm.ListaAnexos[0].Anexo,
					IdPlanoExportacao = vm.IdPlanoExportacao
				};
			}

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
				return new ResultadoProcessamentoVM { Resultado = false, Mensagem = "Faltando dados" };

			var retorno = new ResultadoProcessamentoVM()
			{
				Resultado = true
			};

			try
			{
				var PlanoVM = _uowSciex.QueryStackSciex.PlanoExportacao.SelecionarGrafo(r => new PlanoExportacaoVM()
				{
					IdPlanoExportacao = r.IdPlanoExportacao,
					NumeroPlano = r.NumeroPlano,
					AnoPlano = r.AnoPlano,
					NumeroInscricaoCadastral = r.NumeroInscricaoCadastral,
					Cnpj = r.Cnpj,
					RazaoSocial = r.RazaoSocial,
					TipoModalidade = r.TipoModalidade,
					TipoExportacao = r.TipoExportacao,
					Situacao = r.Situacao,
					DataEnvio = r.DataEnvio,
					DataCadastro = r.DataCadastro,
					DataStatus = r.DataStatus,
					CpfResponsavel = r.CpfResponsavel,
					NomeResponsavel = r.NomeResponsavel,
					DescricaoJustificativaErro = r.DescricaoJustificativaErro,
					NumeroProcesso = r.NumeroProcesso,
					NumeroAnoProcesso = r.NumeroAnoProcesso,
					ListaPEProdutos = r.ListaPEProdutos.Select(w => new PEProdutoVM()
					{
						IdPEProduto = w.IdPEProduto,
						IdPlanoExportacao = w.IdPlanoExportacao,
						CodigoProdutoExportacao = w.CodigoProdutoExportacao,
						CodigoProdutoSuframa = w.CodigoProdutoSuframa,
						CodigoNCM = w.CodigoNCM,
						CodigoTipoProduto = w.CodigoTipoProduto,
						DescricaoModelo = w.DescricaoModelo,
						Qtd = w.Qtd,
						ValorDolar = w.ValorDolar,
						ValorFluxoCaixa = w.ValorFluxoCaixa,
						CodigoUnidade = w.CodigoUnidade,
						ListaPEInsumo = w.ListaPEInsumo.Select(q => new PEInsumoVM()
						{
							IdPEInsumo = q.IdPEInsumo,
							IdPEProduto = q.IdPEProduto,
							CodigoInsumo = q.CodigoInsumo,
							CodigoUnidade = q.CodigoUnidade,
							TipoInsumo = q.TipoInsumo,
							CodigoNcm = q.CodigoNcm,
							ValorPercentualPerda = q.ValorPercentualPerda,
							CodigoDetalhe = q.CodigoDetalhe,
							DescricaoInsumo = q.DescricaoInsumo,
							DescricaoPartNumber = q.DescricaoPartNumber,
							DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
							ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
							ValorDolar = q.ValorDolar,
							SituacaoAnalise = q.SituacaoAnalise,
							DescricaoJustificativaErro = q.DescricaoJustificativaErro,
							ListaPEDetalheInsumo = q.ListaPEDetalheInsumo.Select(e => new PEDetalheInsumoVM()
							{
								IdPEDetalheInsumo = e.IdPEDetalheInsumo,
								IdPEInsumo = e.IdPEInsumo,
								IdMoeda = e.IdMoeda,
								NumeroSequencial = e.NumeroSequencial,
								CodigoPais = e.CodigoPais,
								Quantidade = e.Quantidade,
								ValorUnitario = e.ValorUnitario,
								ValorFrete = e.ValorFrete,
								ValorDolar = e.ValorDolar,
								ValorDolarFOB = e.ValorDolarFOB,
								ValorDolarCRF = e.ValorDolarCRF,
								SituacaoAnalise = e.SituacaoAnalise,
								DescricaoJustificativaErro = e.DescricaoJustificativaErro

							}).ToList()

						}).ToList()
					}).ToList()
				}
				,
				q=> 
				q.IdPlanoExportacao == vm.IdPlanoExportacao);

				if (PlanoVM.ListaPEProdutos.Count > 0)
				{
					foreach (var produto in PlanoVM.ListaPEProdutos)
					{

						if (produto.ListaPEInsumo.Count > 0)
						{
							foreach (var insumo in produto.ListaPEInsumo)
							{

								if (insumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
								{
									var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == insumo.IdPEInsumo);
									regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;

									_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
									_uowSciex.CommandStackSciex.Save();
									_uowSciex.CommandStackSciex.DetachEntries();
								}

								if (insumo.ListaPEDetalheInsumo.Count > 0)
								{

									foreach (var detalhe in insumo.ListaPEDetalheInsumo)
									{
										if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
										{
											var regDetalheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(q => q.IdPEDetalheInsumo == detalhe.IdPEDetalheInsumo);
											regDetalheInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;

											_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalheInsumo);
											_uowSciex.CommandStackSciex.Save();
											_uowSciex.CommandStackSciex.DetachEntries();
										}
									}
								}
							} 
						}
					} 
				}

				var regPlano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == PlanoVM.IdPlanoExportacao);

				if (regPlano.Situacao == (int)EnumSituacaoPlanoExportacao.EM_CORREÇÃO)
				{
					regPlano.Situacao = (int)EnumSituacaoPlanoExportacao.AGUARDANDO_ANÁLISE;
				}
				else
				{
					regPlano.Situacao = (int)EnumSituacaoPlanoExportacao.ENTREGUE;
				}
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
		public ResultadoProcessamentoVM ValidarPlanoExportacaoComprovacao(int idPlanoExportacao, ResultadoProcessamentoVM retorno)
		{
			var PlanoProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(x => x.IdPlanoExportacao == idPlanoExportacao).ToList();

			if (PlanoProduto.Count == 0)
			{
				if (retorno.CamposNaoValidos == null)
					retorno.CamposNaoValidos = new CamposNaoValidadosVM();
				retorno.CamposNaoValidos.NaoExisteProduto = true;
				retorno.Resultado = false;
			}
			else
			{
				retorno.CamposNaoValidos = new CamposNaoValidadosVM();
				foreach (var produto in PlanoProduto)
				{
					var ProdutoPaisLista = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(w => w.IdPEProduto == produto.IdPEProduto).ToList();
					if (ProdutoPaisLista.Count > 0)
					{
						retorno.Resultado = true;
					}
					else
					{
						retorno.Resultado = false;
						retorno.CamposNaoValidos.NaoExisteDue = true;
						break;
					}
				}
			}
			return retorno;

			
		}

		
		public ResultadoProcessamentoVM EntregarPlanoComprovacao(int idPlanoExportacao, ResultadoProcessamentoVM retorno)
		{

			var validacao = ValidarPlanoExportacaoComprovacao(idPlanoExportacao, retorno);
			if (validacao.Resultado==true)
			{
				var PesquisaEntrega = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(x => x.IdPlanoExportacao==idPlanoExportacao);

				PesquisaEntrega.Situacao = 2;
				PesquisaEntrega.DataEnvio = DateTime.Now;
				retorno.Resultado = true;
				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(PesquisaEntrega);
				_uowSciex.CommandStackSciex.Save();

			}
			else
			{
				retorno.Resultado = false;
			}
			

			return retorno;
		}

		public bool DeletarPlano(int idPlanoExportacao)
		{
			if (idPlanoExportacao == 0)
				return false;

			var listaIdAnexo = _uowSciex.QueryStackSciex.PEArquivo.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).Select(q => q.IdPlanoExportacaoArquivo).ToList();

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

					if (listaPais.Count > 0)
					{
						foreach (var idPais in listaPais)
						{

							var listaDue = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(q => q.IdPEProdutoPais == idPais);

							foreach (var regDue in listaDue)
							{
								_uowSciex.CommandStackSciex.PlanoExportacaoDue.Apagar(regDue.IdDue);
							}
							_uowSciex.CommandStackSciex.Save();

							_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Apagar(idPais);
						}
						_uowSciex.CommandStackSciex.Save();
					}


					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Apagar(idProduto);
					_uowSciex.CommandStackSciex.Save();
				}
			}

			var listaHist = _uowSciex.QueryStackSciex.PEHistorico.Listar(q => q.IdPlanoExportacao == idPlanoExportacao);

			if (listaHist.Count > 0)
			{
				foreach (var regHist in listaHist)
				{
					_uowSciex.CommandStackSciex.PEHistorico.Apagar(regHist.IdPEHistorico);
				}
			}

			_uowSciex.CommandStackSciex.PlanoExportacao.Apagar(idPlanoExportacao);
			_uowSciex.CommandStackSciex.Save();

			return true;
		}
		public bool DeletarDUE(int idDue)
		{
			var regDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(q => q.IdDue == idDue);

			var listaPaisParaDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(q => q.CodigoPais == regDUE.CodigoPais
																							&&
																							q.PEProdutoPais.IdPEProduto == regDUE.PEProdutoPais.IdPEProduto);

			var idPEProduto = regDUE.PEProdutoPais.IdPEProduto;
			var idPEProdutoPais = regDUE.PEProdutoPais.IdPEProdutoPais;

			if (listaPaisParaDUE.Count > 1)
			{
				var somatorioPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == idPEProduto
																												&&
																												o.CodigoPais == regDUE.CodigoPais).Sum(o => o.Quantidade);

				var somatorioPEProdutoPaisDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == idPEProduto
																												&&
																												o.CodigoPais == regDUE.CodigoPais).Sum(o => o.ValorDolar);

				_uowSciex.CommandStackSciex.DetachEntries();

				var regProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(q => q.IdPEProdutoPais == idPEProdutoPais);

				regProdutoPais.Quantidade = somatorioPEProdutoPaisQtd - regDUE.Quantidade;
				regProdutoPais.ValorDolar = somatorioPEProdutoPaisDolar - regDUE.ValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regProdutoPais);
				_uowSciex.CommandStackSciex.Save();

				var regProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == idPEProduto);

				regProduto.Qtd = regProdutoPais.Quantidade;
				regProduto.ValorDolar = regProdutoPais.ValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProduto);
				_uowSciex.CommandStackSciex.Save();

				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Apagar(regDUE.IdDue);
				_uowSciex.CommandStackSciex.Save();

			}
			else
			{
				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Apagar(regDUE.IdDue);
				_uowSciex.CommandStackSciex.Save();

				_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Apagar(idPEProdutoPais);
				_uowSciex.CommandStackSciex.Save();

				var somatorioPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == idPEProduto).Sum(o => o.Quantidade);
				var somatorioPEProdutoPaisDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == idPEProduto).Sum(o => o.ValorDolar);

				var regProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == idPEProduto);

				regProduto.Qtd = somatorioPEProdutoPaisQtd;
				regProduto.ValorDolar = somatorioPEProdutoPaisDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProduto);
				_uowSciex.CommandStackSciex.Save();

				
			}

			return true;
		}

		public ResultadoMensagemProcessamentoVM SolicitarCorrecaoPlanoExportacao(PlanoExportacaoVM vm)
		{
			if (vm == null || vm.IdPlanoExportacao == 0)
			{
				return new ResultadoMensagemProcessamentoVM()
				{
					Resultado = false,
					Mensagem = "Identificador de Plano Exportacao inválido."
				};
			}

			var retorno = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = true
			};


			PlanoExportacaoEntity regPlano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == vm.IdPlanoExportacao);

			if (regPlano == null)
			{
				return new ResultadoMensagemProcessamentoVM()
				{
					Resultado = false,
					Mensagem = "Registro de Plano não encontrado."
				};
			}
			else
			{

				try
				{
					var dataHoje = DateTime.Now;
					regPlano.Situacao = (int)EnumSituacaoPlanoExportacao.EM_CORREÇÃO;
					regPlano.DataStatus = dataHoje;

					var regHistPlano = new PEHistoricoEntity()
					{
						Data = dataHoje,
						SituacaoPlano = regPlano.Situacao,
						IdPlanoExportacao = regPlano.IdPlanoExportacao
					};

					_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(regPlano);
					_uowSciex.CommandStackSciex.PEHistorico.Salvar(regHistPlano);
					_uowSciex.CommandStackSciex.Save();
				}
				catch (Exception e)
				{
					retorno.Resultado = false;
					retorno.Mensagem = $"Falha ao processar solicitação. (MENSAGEM: {e.Message} / INNER.EXCEP: {e.InnerException} /STACKTRACE: {e.StackTrace})";

					return retorno;
				}
			}


			return retorno;
		}

		public ResultadoMensagemProcessamentoVM DeletarCorrecaoPlanoExportacao(int id)
		{
			if (id == 0)
			{
				return new ResultadoMensagemProcessamentoVM() { Resultado = false, Mensagem = "Id não pode ser 0." };
			}

			var retorno = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = false
			};

			var regPlano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == id);

			regPlano.Situacao = (int)EnumSituacaoPlanoExportacao.INDEFERIDO;

			_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(regPlano);
			_uowSciex.CommandStackSciex.Save();
			_uowSciex.CommandStackSciex.DetachEntries();

			var listaProdutos = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.ListarGrafo(q => new PEProdutoVM()
			{
				IdPEProduto = q.IdPEProduto,
				IdPlanoExportacao = q.IdPlanoExportacao,
				ListaPEInsumo = q.ListaPEInsumo.Select(w=> new PEInsumoVM()
				{
					IdPEProduto = w.IdPEProduto,
					IdPEInsumo = w.IdPEInsumo,
					SituacaoAnalise = w.SituacaoAnalise,
					ListaPEDetalheInsumo = w.ListaPEDetalheInsumo.Select(e => new PEDetalheInsumoVM()
					{
						IdPEInsumo = e.IdPEInsumo,
						IdPEDetalheInsumo = e.IdPEDetalheInsumo,
						SituacaoAnalise = e.SituacaoAnalise
					}).ToList()

				}).ToList()
			}, 
			q=> q.IdPlanoExportacao == id);
			if (regPlano.TipoExportacao == "AP")
			{
				foreach (var produto in listaProdutos)
				{
					var existeInsumoComStatusCorrigido = false;
					var existeDetalheInsumoComStatusCorrigido = false;
					var existeInsumoComStatusAlterado = false;
					var existeDetalheInsumoComStatusAlterado = false;
					var existeInsumoComStatusInativoPelaEmpresa = false;
					var existeDetalheInsumoComStatusInativoPelaEmpresa = false;

					if (produto.ListaPEInsumo.Count > 0)
					{

						foreach (var insumo in produto.ListaPEInsumo)
						{
							if (insumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
								existeInsumoComStatusCorrigido = true;

							if (insumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO)
								existeInsumoComStatusAlterado = true;

							if (insumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
								existeInsumoComStatusInativoPelaEmpresa = true;

							if (insumo.ListaPEDetalheInsumo.Count > 0)
							{
								foreach (var detalhe in insumo.ListaPEDetalheInsumo)
								{

									if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
										existeDetalheInsumoComStatusCorrigido = true;

									if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO)
										existeDetalheInsumoComStatusAlterado = true;

									if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
										existeDetalheInsumoComStatusInativoPelaEmpresa = true;

								}
							}
						}
					}

					if (existeInsumoComStatusCorrigido || existeInsumoComStatusAlterado)
					{
						foreach (var insumo in produto.ListaPEInsumo)
						{
							var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == insumo.IdPEInsumo);

							if (regInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
							{

								foreach (var detalhe in regInsumo.ListaPEDetalheInsumo)
								{
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Apagar(detalhe.IdPEDetalheInsumo);
									_uowSciex.CommandStackSciex.Save();
								}

								_uowSciex.CommandStackSciex.PEInsumo.Apagar(regInsumo.IdPEInsumo);
								_uowSciex.CommandStackSciex.Save();
								//_uowSciex.CommandStackSciex.DetachEntries();
							}
							else if (regInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO)
							{
								if (existeDetalheInsumoComStatusAlterado)
								{
									foreach (var detalhe in regInsumo.ListaPEDetalheInsumo)
									{
										if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO)
										{
											detalhe.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
											_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhe);
										}
									}
								}
								regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;

								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
								_uowSciex.CommandStackSciex.Save();
								//_uowSciex.CommandStackSciex.DetachEntries();
							}

						}
					}



					if (existeInsumoComStatusInativoPelaEmpresa || existeDetalheInsumoComStatusInativoPelaEmpresa)
					{
						_uowSciex.CommandStackSciex.DetachEntries();

						var listaProdutos_ = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.ListarGrafo(q => new PEProdutoVM()
						{
							IdPEProduto = q.IdPEProduto,
							IdPlanoExportacao = q.IdPlanoExportacao,
							ListaPEInsumo = q.ListaPEInsumo.Select(w => new PEInsumoVM()
							{
								IdPEProduto = w.IdPEProduto,
								IdPEInsumo = w.IdPEInsumo,
								SituacaoAnalise = w.SituacaoAnalise,
								ListaPEDetalheInsumo = w.ListaPEDetalheInsumo.Select(e => new PEDetalheInsumoVM()
								{
									IdPEInsumo = e.IdPEInsumo,
									IdPEDetalheInsumo = e.IdPEDetalheInsumo,
									SituacaoAnalise = e.SituacaoAnalise
								}).ToList()

							}).ToList()
						},
						q => q.IdPlanoExportacao == id);

						foreach (var produtoVM in listaProdutos_)
						{
							foreach (var insumo in produtoVM.ListaPEInsumo)
							{
								var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == insumo.IdPEInsumo);

								if (regInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
								{


									foreach (var detalhe in regInsumo.ListaPEDetalheInsumo)
									{
										if (detalhe.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
										{
											detalhe.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
											_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhe);
											_uowSciex.CommandStackSciex.Save();
										}
									}

									regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
									_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
									_uowSciex.CommandStackSciex.Save();
									_uowSciex.CommandStackSciex.DetachEntries();
								}

							}
						}
					}
				}
				return retorno;
			}
			else
			{
				var listaPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.ListarGrafo(x => new PEProdutoComplementoVM()
				{
					IdPEProduto = x.IdPEProduto,
					IdPlanoExportacao = x.IdPlanoExportacao,
					ListaPEProdutoPais = x.ListaPEProdutoPais.Select(y => new PEProdutoPaisComplementoVM()
					{
						IdPEProduto = y.IdPEProduto,
						IdPEProdutoPais = y.IdPEProdutoPais,
						SituacaoAnalise = y.SituacaoAnalise,
						ListaPEDue = y.ListaPEDue.Select(z => new PlanoExportacaoDUEComplementoVM()
						{
							IdDue = z.IdDue,
							IdPEProdutoPais = z.IdPEProdutoPais
						}).ToList()
					}).ToList()
				},
				q => q.IdPlanoExportacao == id);


				foreach (var PEProduto in listaPEProduto)

				{
					foreach (var PEProdutoPais in PEProduto.ListaPEProdutoPais)
					{
						if (PEProdutoPais.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO || PEProdutoPais.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.INATIVO_EMPRESA)
						{
							foreach (var PEDue in PEProdutoPais.ListaPEDue)
							{
								_uowSciex.CommandStackSciex.PlanoExportacaoDue.Apagar(PEProdutoPais.IdPEProdutoPais);
								_uowSciex.CommandStackSciex.Save();
							}
						}
						if (PEProdutoPais.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO)
						{
							var regPEProdutoPaisEntity = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(x => x.IdPEProduto == PEProdutoPais.IdPEProdutoPais);

							regPEProdutoPaisEntity.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
							_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPEProdutoPaisEntity);
							_uowSciex.CommandStackSciex.Save();
							_uowSciex.CommandStackSciex.DetachEntries();
						}
					}	
				}
			}
			return retorno;
		}

		private DuePorProdutoVM Validacao(DuePorProdutoVM vm)
		{
			var regProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);

			var regPlanoExportacao = regProdutoPais.PlanoExportacao;

			var verificaProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(o => o.NumeroProcesso == regPlanoExportacao.NumeroProcesso
																					&& o.AnoProcesso == regPlanoExportacao.NumeroAnoProcesso
																					&& o.Cnpj == regPlanoExportacao.Cnpj);
			if (verificaProcesso == null)
			{
				vm.RetornoString = $"Não há registro de processo. idPlanaExportacao {regPlanoExportacao.IdPlanoExportacao}";
				vm.Sucesso = false;
				return vm;
			}
			else
			{
				var validacaoProcessoComDataEmVigor = _uowSciex.QueryStackSciex.Processo.Listar(q => q.NumeroProcesso == regPlanoExportacao.NumeroProcesso
																			&&
																			q.AnoProcesso == regPlanoExportacao.NumeroAnoProcesso
																			&&

																			q.DataValidade > vm.DataAverbacao
																			&&
																			q.ListaStatus.Where(w => w.Data < vm.DataAverbacao && w.Tipo == "AP").Any()
																			);
				if (validacaoProcessoComDataEmVigor.Count() == 0)
				{
					vm.RetornoString = "Não há Processo com data em Vigor.";
					vm.Sucesso = false;
					return vm;

				}
			}
			var listaregistroDue = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(q => q.Numero == vm.Numero).ToList();
			if (listaregistroDue.Count > 0)
			{
				var codigoPais = listaregistroDue.FirstOrDefault().CodigoPais;

				if (codigoPais == vm.CodigoPais)
				{
					var listaIdProduto = listaregistroDue.Select(q => q.PEProdutoPais.PlanoExportacaoProduto.IdPEProduto).Distinct().ToList();


					if (listaIdProduto.Contains(vm.IdPEProduto))
					{
						vm.RetornoString = "Due já cadastrada para esse Produto.";
						vm.Sucesso = false;
						return vm;
					}
				}
				else
				{
					vm.RetornoString = "Due já cadastrada para outro País.";
					vm.Sucesso = false;
					return vm;
				}
			}
			return vm;
		}
		private DuePorProdutoVM ValidacaoAlterar(DuePorProdutoVM vm)
		{
			var regProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);

			var regPlanoExportacao = regProdutoPais.PlanoExportacao;

			var verificaProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(o => o.NumeroProcesso == regPlanoExportacao.NumeroProcesso
																					&& o.AnoProcesso == regPlanoExportacao.NumeroAnoProcesso
																					&& o.Cnpj == regPlanoExportacao.Cnpj);
			if (verificaProcesso == null)
			{
				vm.RetornoString = $"Não há registro de processo. idPlanaExportacao {regPlanoExportacao.IdPlanoExportacao}";
				vm.Sucesso = false;
				return vm;
			}
			else
			{
				var validacaoProcessoComDataEmVigor = _uowSciex.QueryStackSciex.Processo.Listar(q => q.NumeroProcesso == regPlanoExportacao.NumeroProcesso
																			&&
																			q.AnoProcesso == regPlanoExportacao.NumeroAnoProcesso
																			&&

																			q.DataValidade > vm.DataAverbacao
																			&&
																			q.ListaStatus.Where(w => w.Data < vm.DataAverbacao && w.Tipo == "AP").Any()
																			);
				if (validacaoProcessoComDataEmVigor.Count() == 0)
				{
					vm.RetornoString = "Não há Processo com data em Vigor.";
					vm.Sucesso = false;
					return vm;

				}
			}
			var listaregistroDue = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(q => q.Numero == vm.Numero).ToList();
			if (listaregistroDue.Count > 0)
			{
				var codigoPais = listaregistroDue.FirstOrDefault().CodigoPais;

				if (codigoPais == vm.CodigoPais)
				{
					return vm;
				}
				else
				{
					vm.RetornoString = "Due já cadastrada para outro País.";
					vm.Sucesso = false;
					return vm;
				}
			}
			return vm;
		}

		public DuePorProdutoVM SalvarDocumentosComprobatorios(DuePorProdutoVM vm)

		{
			vm.Sucesso = true;
			vm = Validacao(vm);
			if(!vm.Sucesso) {
				return vm;

			}
			var regPEProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(o => o.IdPEProduto == vm.IdPEProduto && o.CodigoPais == vm.CodigoPais);

			var statusPlanoExportacao = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q=> q.IdPEProduto == vm.IdPEProduto).PlanoExportacao.Situacao;
			try
			{
				if (regPEProdutoPais == null)
				{

					var PEProdutoPaisEntity = new PEProdutoPaisEntity()
					{
						Quantidade = vm.Quantidade,
						ValorDolar = vm.ValorDolar,
						CodigoPais = vm.CodigoPais,
						IdPEProduto = vm.IdPEProduto

					};
					_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(PEProdutoPaisEntity);
					_uowSciex.CommandStackSciex.Save();

					var PEDueEntity = new PlanoExportacaoDUEEntity()
					{
						Numero = vm.Numero,
						DataAverbacao = vm.DataAverbacao,
						Quantidade = vm.Quantidade,
						ValorDolar = vm.ValorDolar,
						CodigoPais = vm.CodigoPais,
						IdPEProdutoPais = PEProdutoPaisEntity.IdPEProdutoPais

					};

					if (statusPlanoExportacao == (int)EnumSituacaoPlanoExportacao.EM_CORREÇÃO)
					{
						PEDueEntity.SituacaoAnalise = (int)EnumSituacaoAnaliseDUE.NOVO;
					}

					_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(PEDueEntity);
					_uowSciex.CommandStackSciex.Save();

					var somatorioPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.Quantidade);
					var somatorioPEProdutoPaisDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.ValorDolar);

					_uowSciex.CommandStackSciex.DetachEntries();
					var regPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);

					regPEProduto.Qtd = somatorioPEProdutoPaisQtd;
					regPEProduto.ValorDolar = somatorioPEProdutoPaisDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regPEProduto);
					_uowSciex.CommandStackSciex.Save();
					vm.Sucesso = true;
					return vm;
				}
				else
				{

					regPEProdutoPais.Quantidade += vm.Quantidade;
					regPEProdutoPais.ValorDolar += vm.ValorDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPEProdutoPais);
					_uowSciex.CommandStackSciex.Save();

					var PEDueEntity = new PlanoExportacaoDUEEntity()
					{
						Numero = vm.Numero,
						DataAverbacao = vm.DataAverbacao,
						Quantidade = vm.Quantidade,
						ValorDolar = vm.ValorDolar,
						CodigoPais = vm.CodigoPais,
						IdPEProdutoPais = regPEProdutoPais.IdPEProdutoPais
					};

					if (statusPlanoExportacao == (int)EnumSituacaoPlanoExportacao.EM_CORREÇÃO)
					{
						PEDueEntity.SituacaoAnalise = (int)EnumSituacaoAnaliseDUE.NOVO;
					}

					_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(PEDueEntity);
					_uowSciex.CommandStackSciex.Save();

					var somatorioPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.Quantidade);
					var somatorioPEProdutoPaisDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.ValorDolar);

					_uowSciex.CommandStackSciex.DetachEntries();
					var regPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);

					regPEProduto.Qtd = somatorioPEProdutoPaisQtd;
					regPEProduto.ValorDolar = somatorioPEProdutoPaisDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regPEProduto);
					_uowSciex.CommandStackSciex.Save();
					vm.Sucesso = true;
					return vm;
				}
			}
			catch (Exception e)
			{
				vm.Sucesso = false;
				vm.RetornoString = "Erro ao Incluir DUE.";
				return vm;
				

			}
			return vm;
		}			

		public DuePorProdutoVM EditarDocumentosCombprobatorios(DuePorProdutoVM vm)
		{
			vm.Sucesso = true;
			vm = ValidacaoAlterar(vm);
			if (!vm.Sucesso)
			{
				return vm;

			}
			try
			{
				var regPEDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(o => o.IdDue == vm.IdDue);

				regPEDUE.Numero = vm.Numero;
				regPEDUE.DataAverbacao = vm.DataAverbacao;
				regPEDUE.Quantidade = vm.Quantidade;
				regPEDUE.ValorDolar = vm.ValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regPEDUE);
				_uowSciex.CommandStackSciex.Save();

				var regPEProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais);
				var somatorioPEDUEqtd= _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(o => o.IdDue == vm.IdDue).Sum(o => o.Quantidade);
				var somatorioPEDUOValorDolar =	_uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(o => o.IdDue == vm.IdDue).Sum(o => o.ValorDolar);

				regPEProdutoPais.Quantidade = somatorioPEDUEqtd;
				regPEProdutoPais.ValorDolar = somatorioPEDUOValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPEProdutoPais);
				_uowSciex.CommandStackSciex.Save();



				var regPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == regPEProdutoPais.IdPEProduto);
				var regPEProdutoPaisQtd =  _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.Quantidade);
				var regPEProdutoPaisValorDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.ValorDolar);

				regPEProduto.Qtd = regPEProdutoPaisQtd;
				regPEProduto.ValorDolar = regPEProdutoPaisValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regPEProduto);
				_uowSciex.CommandStackSciex.Save();

				vm.Sucesso = true;
				return vm;
			}
			catch (Exception e)
			{
				vm.Sucesso = false;
				vm.RetornoString = "Erro ao Editar DUE.";
				return vm;
			}
		}

		public PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUEPaginadoParaAnalise(PEProdutoVM pagedFilter)
		{
			if (pagedFilter == null || pagedFilter.IdPEProduto == 0) { return new PagedItems<PlanoExportacaoDUEComplementoVM>(); }

			var listaProdutoPaisEntity = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.ListarGrafo(o => new PEProdutoPaisVM()
			{
				IdPEProduto = o.IdPEProduto,
				IdPEProdutoPais = o.IdPEProdutoPais,
				CodigoPais = o.CodigoPais
			},
			o =>
			(
				o.IdPEProduto == pagedFilter.IdPEProduto
			)
			)
			.ToList();
			var idPEProduto = listaProdutoPaisEntity.Select(q => q.IdPEProduto).FirstOrDefault();
			var listaIdProdutoPais = listaProdutoPaisEntity.Select(q => (int?)q.IdPEProdutoPais).ToList();

			string sort = null;
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DescricaoPais"))
			{
				sort = "DescricaoPais";
				pagedFilter.Sort = null;
			}

			var listaDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.ListarPaginadoGrafo(q => new PlanoExportacaoDUEComplementoVM()
			{
				IdPEProduto = idPEProduto,
				IdPEProdutoPais = q.IdPEProdutoPais,
				IdDue = q.IdDue,
				CodigoPais = q.CodigoPais,
				Numero = q.Numero,
				DataAverbacao = q.DataAverbacao,
				Quantidade = q.Quantidade,
				ValorDolar = q.ValorDolar,
				SituacaoAnalise = q.SituacaoAnalise,
				DescricaoJustificativa = q.DescricaoJustificativa
			},
			q => listaIdProdutoPais.Contains(q.IdPEProdutoPais)
			, pagedFilter);

			foreach (var item in listaDUE.Items)
			{
				string codigoPais = item.CodigoPais.ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
				item.DataAverbacaoFormatada = item.DataAverbacao != DateTime.MinValue ? item.DataAverbacao.ToShortDateString() : "-";
				item.DescricaoSituacaoAnalise = ObterDescricaoSituacaoAnalise(item.SituacaoAnalise);
			}

			if (!string.IsNullOrWhiteSpace(sort))
			{
				switch (sort)
				{
					case "DescricaoPais":
						if (pagedFilter.Reverse)
						{
							listaDUE.Items = listaDUE.Items.OrderBy(q => q.DescricaoPais).ThenBy(q => q.DescricaoPais).ToList();
						}
						else
						{
							listaDUE.Items = listaDUE.Items.OrderByDescending(q => q.DescricaoPais).ThenByDescending(q => q.DescricaoPais).ToList();
						}
						break;
				}
			}

			return listaDUE;
		}

		private string ObterDescricaoSituacaoAnalise(int? situacaoAnalise)
		{
			switch (situacaoAnalise)
			{
				case null: return "-";
				case 1: return "Aprovado";
				case 2: return "Reprovado";
				case 3: return "Alterado";
				case 4: return "Corrigido";
				case 5: return "Inativo";
				case 6: return "Novo";
				default: return "-";

			}
		}

		public PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUECorrecaoPaginado(PEProdutoVM pagedFilter)
		{
			if (pagedFilter == null || pagedFilter.IdPEProduto == 0) { return new PagedItems<PlanoExportacaoDUEComplementoVM>(); }

			var listaProdutoPaisEntity = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.ListarGrafo(o => new PEProdutoPaisVM()
			{
				IdPEProduto = o.IdPEProduto,
				IdPEProdutoPais = o.IdPEProdutoPais,
				CodigoPais = o.CodigoPais
			},
			o =>
			(
				o.IdPEProduto == pagedFilter.IdPEProduto
			)
			)
			.ToList();
			var idPEProduto = listaProdutoPaisEntity.Select(q => q.IdPEProduto).FirstOrDefault();
			var listaIdProdutoPais = listaProdutoPaisEntity.Select(q => (int?)q.IdPEProdutoPais).ToList();

			string sort = null;
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DescricaoPais"))
			{
				sort = "DescricaoPais";
				pagedFilter.Sort = null;
			}

			var listaDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.ListarPaginadoGrafo(q => new PlanoExportacaoDUEComplementoVM()
			{
				IdPEProduto = idPEProduto,
				IdPEProdutoPais = q.IdPEProdutoPais,
				IdDue = q.IdDue,
				CodigoPais = q.CodigoPais,
				Numero = q.Numero,
				DataAverbacao = q.DataAverbacao,
				Quantidade = q.Quantidade,
				ValorDolar = q.ValorDolar,
				SituacaoAnalise = q.SituacaoAnalise,
				DescricaoJustificativa = q.DescricaoJustificativa,
				PEProdutoPais = new PEProdutoPaisComplementoVM()
				{
					IdPEProduto = q.PEProdutoPais.IdPEProduto,
					IdPEProdutoPais = q.PEProdutoPais.IdPEProdutoPais,
				}
			},
			q => (listaIdProdutoPais.Contains(q.IdPEProdutoPais) &&
				(q.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.INATIVO &&
				q.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.ALTERADO)), pagedFilter);


			foreach (var item in listaDUE.Items)
			{
				string codigoPais = item.CodigoPais.ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
				item.DataAverbacaoFormatada = item.DataAverbacao != DateTime.MinValue ? item.DataAverbacao.ToShortDateString() : "-";
				if (item.SituacaoAnalise == 1)
				{
					item.SituacaoAnaliseString = "Aprovado";
				}
				else if (item.SituacaoAnalise == 2)
				{
					item.SituacaoAnaliseString = "Reprovado";
				}
				else if (item.SituacaoAnalise == 3)
				{
					item.SituacaoAnaliseString = "Alterado";
				}
				else if (item.SituacaoAnalise == 4)
				{
					item.SituacaoAnaliseString = "Corrigido";
				}
				else if (item.SituacaoAnalise == 5)
				{
					item.SituacaoAnaliseString = "Inativo";
				}
				else if (item.SituacaoAnalise == 6)
				{
					item.SituacaoAnaliseString = "Novo";
				}
				else
				{
					item.SituacaoAnaliseString = "Não Analisado	";
				}
			}

			if (!string.IsNullOrWhiteSpace(sort))
			{
				switch (sort)
				{
					case "DescricaoPais":
						if (pagedFilter.Reverse)
						{
							listaDUE.Items = listaDUE.Items.OrderBy(q => q.DescricaoPais).ThenBy(q => q.DescricaoPais).ToList();
						}
						else
						{
							listaDUE.Items = listaDUE.Items.OrderByDescending(q => q.DescricaoPais).ThenByDescending(q => q.DescricaoPais).ToList();
						}
						break;
				}
			}
			return listaDUE;
		}

		public DuePorProdutoVM CorrigirDocumentosComprobatorios(DuePorProdutoVM vm)
		{
			vm.Sucesso = true;
			vm = ValidacaoAlterar(vm);
			if (!vm.Sucesso)
			{
				return vm;
			}
			try
			{
				var regPEDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(o => o.IdDue == vm.IdDue);

				if (regPEDUE.SituacaoAnalise == (int)EnumSituacaoAnalisePEDue.REPROVADO)
				{
					regPEDUE.SituacaoAnalise = (int)EnumSituacaoAnalisePEDue.ALTERADO;
					_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regPEDUE);
					_uowSciex.CommandStackSciex.Save();

					var PEDueEntity = new PlanoExportacaoDUEEntity()
					{
						SituacaoAnalise = (int)EnumSituacaoAnalisePEDue.CORRIGIDO,
						Numero = vm.Numero,
						DataAverbacao = vm.DataAverbacao,
						Quantidade = vm.Quantidade,
						ValorDolar = vm.ValorDolar,
						CodigoPais = vm.CodigoPais,
						IdPEProdutoPais = vm.IdPEProdutoPais
					};
					_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(PEDueEntity);
					_uowSciex.CommandStackSciex.Save();
					_uowSciex.CommandStackSciex.DetachEntries();

					var regPEProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais);
					var somatorioPEDUEqtd = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais &&
																								(o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.INATIVO &&
																								o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.ALTERADO)).Sum(o => o.Quantidade);
					var somatorioPEDUOValorDolar = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais &&
																								(o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.INATIVO &&
																								o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.ALTERADO)).Sum(o => o.ValorDolar);

					regPEProdutoPais.Quantidade = somatorioPEDUEqtd;
					regPEProdutoPais.ValorDolar = somatorioPEDUOValorDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPEProdutoPais);
					_uowSciex.CommandStackSciex.Save();
					_uowSciex.CommandStackSciex.DetachEntries();

					var regPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == regPEProdutoPais.IdPEProduto);
					var regPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.Quantidade);
					var regPEProdutoPaisValorDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.ValorDolar);

					regPEProduto.Qtd = regPEProdutoPaisQtd;
					regPEProduto.ValorDolar = regPEProdutoPaisValorDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regPEProduto);
					_uowSciex.CommandStackSciex.Save();


					vm.Sucesso = true;
				}
				else if (regPEDUE.SituacaoAnalise == (int)EnumSituacaoAnalisePEDue.CORRIGIDO)
				{
					regPEDUE.Numero = vm.Numero;
					regPEDUE.DataAverbacao = vm.DataAverbacao;
					regPEDUE.Quantidade = vm.Quantidade;
					regPEDUE.ValorDolar = vm.ValorDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regPEDUE);
					_uowSciex.CommandStackSciex.Save();
					_uowSciex.CommandStackSciex.DetachEntries();

					var regPEProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais);
					var somatorioPEDUEqtd = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais &&
																								(o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.INATIVO &&
																								o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.ALTERADO)).Sum(o => o.Quantidade);
					var somatorioPEDUOValorDolar = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(o => o.IdPEProdutoPais == vm.IdPEProdutoPais &&
																								(o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.INATIVO &&
																								o.SituacaoAnalise != (int)EnumSituacaoAnalisePEDue.ALTERADO)).Sum(o => o.ValorDolar);

					regPEProdutoPais.Quantidade = somatorioPEDUEqtd;
					regPEProdutoPais.ValorDolar = somatorioPEDUOValorDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPEProdutoPais);
					_uowSciex.CommandStackSciex.Save();
					_uowSciex.CommandStackSciex.DetachEntries();

					var regPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == regPEProdutoPais.IdPEProduto);
					var regPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.Quantidade);
					var regPEProdutoPaisValorDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == vm.IdPEProduto).Sum(o => o.ValorDolar);

					regPEProduto.Qtd = regPEProdutoPaisQtd;
					regPEProduto.ValorDolar = regPEProdutoPaisValorDolar;

					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regPEProduto);
					_uowSciex.CommandStackSciex.Save();

					vm.Sucesso = true;
				}
				return vm;
			}
			catch (Exception e)
			{
				vm.Sucesso = false;
				vm.RetornoString = "Erro ao Editar DUE.";
				return vm;
			}
		}

		public int InativarDocumentosComprobatorios(DuePorProdutoVM vm)
		{
			try
			{
				var regPEDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(o => o.IdDue == vm.IdDue);

				regPEDUE.SituacaoAnalise = (int)EnumSituacaoAnalisePEDue.INATIVO;

				var somatorioPEProdutoPaisQtd = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == regPEDUE.PEProdutoPais.IdPEProduto)
																									.Sum(o => o.Quantidade);
				var somatorioPEProdutoPaisDolar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == regPEDUE.PEProdutoPais.IdPEProduto)
																									.Sum(o => o.ValorDolar);
				_uowSciex.CommandStackSciex.DetachEntries();

				var regPEProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);

				regPEProduto.Qtd = somatorioPEProdutoPaisQtd - regPEDUE.Quantidade;
				regPEProduto.ValorDolar = somatorioPEProdutoPaisDolar - regPEDUE.ValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regPEProduto);
				_uowSciex.CommandStackSciex.Save();

				regPEDUE.PEProdutoPais.Quantidade -= regPEDUE.Quantidade;
				regPEDUE.PEProdutoPais.ValorDolar -= regPEDUE.ValorDolar;

				_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regPEDUE.PEProdutoPais);
				_uowSciex.CommandStackSciex.Save();

				regPEDUE.ValorDolar = 0;
				regPEDUE.Quantidade = 0;

				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regPEDUE);
				_uowSciex.CommandStackSciex.Save();
				return 0;
			}
			catch (Exception e)
			{
				return 1;
			}
		}
	}

}