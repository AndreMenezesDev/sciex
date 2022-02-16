using FluentValidation;
using NLog;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Linq;
using System.Linq.Expressions;

namespace Suframa.Sciex.BusinessLogic
{
	public class AnalisePlanoDeExportacaoBll : IAnalisePlanoDeExportacaoBll
	{
		#region Status Plano de Exportacao
		private readonly int StatusDeferido = 4, StatusIndeferido = 5, StatusAguardandoAnalise = 3;
		#endregion

		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _IUsuarioLogado;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public AnalisePlanoDeExportacaoBll(IUnitOfWorkSciex uowSciex,
											IUsuarioPssBll usuarioPssBll,
										   IUsuarioPssBll IUsuarioLogado)
		{
			_uowSciex = uowSciex;
			_IUsuarioLogado = IUsuarioLogado;
			_usuarioPssBll = usuarioPssBll;
		}

		public AnalisarPlanoExportacaoVM IndeferirPlano(AnalisarPlanoExportacaoVM pagedFilter)
		{
			if (pagedFilter.IdPlanoExportacao == 0) return new AnalisarPlanoExportacaoVM() { Resultado = false, Mensagem = "Registro sem identificação de plano." };

			var retorno = new AnalisarPlanoExportacaoVM()
			{
				Resultado = true
			};

			try
			{

				var regPlano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(q => q.IdPlanoExportacao == pagedFilter.IdPlanoExportacao);

				if (regPlano.Situacao != (int)EnumSituacaoPlanoExportacao.AGUARDANDO_ANÁLISE)
				{
					return new AnalisarPlanoExportacaoVM()
					{
						Resultado = false,
						Mensagem = "Operação não realizada. Este registro foi modificado por outro usuário. Realize a consulta novamente."
					};
				}

				regPlano.Situacao = (int)EnumSituacaoPlanoExportacao.INDEFERIDO;
				regPlano.DataStatus = DateTime.Now;
				regPlano.DescricaoJustificativaErro = pagedFilter.DescricaoJustificativaErro;

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(regPlano);

				var usuario = _usuarioPssBll.ObterUsuarioLogado();

				var histPlanoExp = new PEHistoricoEntity()
				{
					Data = regPlano.DataStatus,
					SituacaoPlano = (int)EnumSituacaoPlanoExportacao.INDEFERIDO,
					CpfResponsavel = usuario.usuarioLogadoCpfCnpj.CnpjCpfUnformat(),
					NomeResponsavel = usuario.usuarioLogadoNome,
					DescricaoObservacao = null,
					IdPlanoExportacao = pagedFilter.IdPlanoExportacao
				};

				_uowSciex.CommandStackSciex.PEHistorico.Salvar(histPlanoExp);

				_uowSciex.CommandStackSciex.Save();
			}
			catch (Exception e)
			{
				return new AnalisarPlanoExportacaoVM()
				{
					Resultado = false,
					Mensagem = $"Falha ao salvar: {e.Message}"
				};
			}


			return retorno;

		}

		public PagedItems<PlanoExportacaoVM> ListarPaginado(PlanoExportacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);

			var usuarioLogado = _IUsuarioLogado.ObterUsuarioLogado();

			#region Analista Designado
			if (pagedFilter.IdAnalistaDesignado > 0)
			{
				pagedFilter.CpfResponsavel = _uowSciex.QueryStackSciex.Analista.Selecionar(o => o.IdAnalista == pagedFilter.IdAnalistaDesignado).CPF;
			}
			#endregion

			var listaStatus = new List<int?>();

			long digitoPlano = 0;
			int anoPlano = 0;

			if (!string.IsNullOrEmpty(pagedFilter.NumeroAnoPlanoConcat))
			{
				digitoPlano = Convert.ToInt64(pagedFilter.NumeroAnoPlanoConcat.Substring(0, 5));
				anoPlano = Convert.ToInt32(pagedFilter.NumeroAnoPlanoConcat.Substring(6, 4));
			}

			PagedItems<PlanoExportacaoVM> ListaServico;

			string campoFiltroAplicacaoPosterior = "";

			if (pagedFilter.Sort != null)
			{
				if (pagedFilter.Sort.Equals("Fluxo") || pagedFilter.Sort.Equals("Perda"))
				{
					campoFiltroAplicacaoPosterior = pagedFilter.Sort;
					pagedFilter.Sort = null;
				}
			}

			if (pagedFilter.Situacao == 3)
			{
				var cpflogado = usuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat();

				if (string.IsNullOrEmpty(pagedFilter.CpfResponsavel) || pagedFilter.CpfResponsavel == cpflogado)
				{

					listaStatus.Add(StatusAguardandoAnalise);
				}
				else
				{
					return new PagedItems<PlanoExportacaoVM>();
				}

				ListaServico = _uowSciex.QueryStackSciex.PlanoExportacao.ListarPaginadoGrafo(q => new PlanoExportacaoVM()
				{
					IdPlanoExportacao = q.IdPlanoExportacao,
					NumeroPlano = q.NumeroPlano,
					AnoPlano = q.AnoPlano,
					NumeroInscricaoCadastral = q.NumeroInscricaoCadastral,
					Cnpj = q.Cnpj,
					RazaoSocial = q.RazaoSocial,
					TipoModalidade = q.TipoModalidade,
					TipoExportacao = q.TipoExportacao,
					Situacao = q.Situacao,
					DataEnvio = q.DataEnvio,
					DataCadastro = q.DataCadastro,
					DataStatus = q.DataStatus,
					CpfResponsavel = q.CpfResponsavel,
					NomeResponsavel = q.NomeResponsavel,
					NumeroProcesso = q.NumeroProcesso,
					NumeroAnoProcesso = q.NumeroAnoProcesso,
					ListaPEProdutos = q.ListaPEProdutos.Select(w => new PEProdutoVM()
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
						CodigoUnidade = w.CodigoUnidade
					}).ToList()
				},
				o =>
						(
							(
								digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano
							) &&
							(
								listaStatus.Contains(o.Situacao)
							) &&
							(
								pagedFilter.DataInicio == null || o.DataStatus >= dataInicio && o.DataStatus <= dataFim
							) &&
							(
								pagedFilter.NumeroInscricaoCadastral == null ||
								o.NumeroInscricaoCadastral == pagedFilter.NumeroInscricaoCadastral
							) &&
							(
								pagedFilter.RazaoSocial == null || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
							)
							&&
							(
								o.CpfResponsavel == cpflogado
							)
						),
						pagedFilter);
			}
			else
			{
				if (pagedFilter.Situacao == 0)
				{
					listaStatus = new List<int?>()
					{
						StatusDeferido,
						StatusIndeferido
					};

					var cpfLogado = usuarioLogado.usuarioLogadoCpfCnpj.CnpjCpfUnformat();
					Expression<Func<PlanoExportacaoVM, bool>> pred = null;

					if (!string.IsNullOrEmpty(pagedFilter.CpfResponsavel))
					{
						if (pagedFilter.CpfResponsavel == cpfLogado)
						{
							listaStatus.Add(StatusAguardandoAnalise);
							pred = o => (
											(
												digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano
											) &&
											(
												listaStatus.Contains(o.Situacao) && o.CpfResponsavel == pagedFilter.CpfResponsavel
											) &&
											(
												pagedFilter.DataInicio == null || o.DataStatus >= dataInicio && o.DataStatus <= dataFim
											) &&
											(
												pagedFilter.NumeroInscricaoCadastral == null ||
												o.NumeroInscricaoCadastral == pagedFilter.NumeroInscricaoCadastral
											) &&
											(
												pagedFilter.RazaoSocial == null || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
											)

										);
						}
						else
						{
							pred = o => (
											(
												digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano
											) &&
											(
												listaStatus.Contains(o.Situacao) && o.CpfResponsavel == pagedFilter.CpfResponsavel
											) &&
											(
												pagedFilter.DataInicio == null || o.DataStatus >= dataInicio && o.DataStatus <= dataFim
											) &&
											(
												pagedFilter.NumeroInscricaoCadastral == null ||
												o.NumeroInscricaoCadastral == pagedFilter.NumeroInscricaoCadastral
											) &&
											(
												pagedFilter.RazaoSocial == null || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
											)

										);
						}

					}
					else
					{
						pred = o => (
											(
												digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano
											) &&
											(
												listaStatus.Contains(o.Situacao) || o.Situacao == 3 && o.CpfResponsavel == cpfLogado
											) &&
											(
												pagedFilter.DataInicio == null || o.DataStatus >= dataInicio && o.DataStatus <= dataFim
											) &&
											(
												pagedFilter.NumeroInscricaoCadastral == null ||
												o.NumeroInscricaoCadastral == pagedFilter.NumeroInscricaoCadastral
											) &&
											(
												pagedFilter.RazaoSocial == null || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
											)

										);
					}

					ListaServico = _uowSciex.QueryStackSciex.PlanoExportacao.ListarPaginadoGrafo(q => new PlanoExportacaoVM()
					{
						IdPlanoExportacao = q.IdPlanoExportacao,
						NumeroPlano = q.NumeroPlano,
						AnoPlano = q.AnoPlano,
						NumeroInscricaoCadastral = q.NumeroInscricaoCadastral,
						Cnpj = q.Cnpj,
						RazaoSocial = q.RazaoSocial,
						TipoModalidade = q.TipoModalidade,
						TipoExportacao = q.TipoExportacao,
						Situacao = q.Situacao,
						DataEnvio = q.DataEnvio,
						DataCadastro = q.DataCadastro,
						DataStatus = q.DataStatus,
						CpfResponsavel = q.CpfResponsavel,
						NomeResponsavel = q.NomeResponsavel,
						NumeroProcesso = q.NumeroProcesso,
						NumeroAnoProcesso = q.NumeroAnoProcesso,
						ListaPEProdutos = q.ListaPEProdutos.Select(w => new PEProdutoVM()
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
							ListaPEInsumo = w.ListaPEInsumo.Select(e => new PEInsumoVM()
							{
								IdPEInsumo = e.IdPEInsumo,
								IdPEProduto = e.IdPEProduto,
								CodigoInsumo = e.CodigoInsumo,
								CodigoUnidade = e.CodigoUnidade,
								TipoInsumo = e.TipoInsumo,
								CodigoNcm = e.CodigoNcm,
								ValorPercentualPerda = e.ValorPercentualPerda,
								CodigoDetalhe = e.CodigoDetalhe,
								DescricaoInsumo = e.DescricaoInsumo,
								DescricaoPartNumber = e.DescricaoPartNumber,
								DescricaoEspecificacaoTecnica = e.DescricaoEspecificacaoTecnica,
								ValorCoeficienteTecnico = e.ValorCoeficienteTecnico,
								ValorDolar = e.ValorDolar,
							}).ToList()
						}).ToList()
					}
					,
					pred
					,
							pagedFilter);
				}
				else
				{


					ListaServico = _uowSciex.QueryStackSciex.PlanoExportacao.ListarPaginadoGrafo(q => new PlanoExportacaoVM()
					{
						IdPlanoExportacao = q.IdPlanoExportacao,
						NumeroPlano = q.NumeroPlano,
						AnoPlano = q.AnoPlano,
						NumeroInscricaoCadastral = q.NumeroInscricaoCadastral,
						Cnpj = q.Cnpj,
						RazaoSocial = q.RazaoSocial,
						TipoModalidade = q.TipoModalidade,
						TipoExportacao = q.TipoExportacao,
						Situacao = q.Situacao,
						DataEnvio = q.DataEnvio,
						DataCadastro = q.DataCadastro,
						DataStatus = q.DataStatus,
						CpfResponsavel = q.CpfResponsavel,
						NomeResponsavel = q.NomeResponsavel,
						NumeroProcesso = q.NumeroProcesso,
						NumeroAnoProcesso = q.NumeroAnoProcesso,
						ListaPEProdutos = q.ListaPEProdutos.Select(w => new PEProdutoVM()
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
							ListaPEInsumo = w.ListaPEInsumo.Select(e => new PEInsumoVM()
							{
								IdPEInsumo = e.IdPEInsumo,
								IdPEProduto = e.IdPEProduto,
								CodigoInsumo = e.CodigoInsumo,
								CodigoUnidade = e.CodigoUnidade,
								TipoInsumo = e.TipoInsumo,
								CodigoNcm = e.CodigoNcm,
								ValorPercentualPerda = e.ValorPercentualPerda,
								CodigoDetalhe = e.CodigoDetalhe,
								DescricaoInsumo = e.DescricaoInsumo,
								DescricaoPartNumber = e.DescricaoPartNumber,
								DescricaoEspecificacaoTecnica = e.DescricaoEspecificacaoTecnica,
								ValorCoeficienteTecnico = e.ValorCoeficienteTecnico,
								ValorDolar = e.ValorDolar,
							}).ToList()
						}).ToList()
					}
				,
				o =>
						(
							(
								digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano
							) 
							&&
							pagedFilter.Situacao == o.Situacao
							&&
							(
								pagedFilter.DataInicio == null || o.DataStatus >= dataInicio && o.DataStatus <= dataFim
							)
							&&
							(
								pagedFilter.NumeroInscricaoCadastral == null ||
								o.NumeroInscricaoCadastral == pagedFilter.NumeroInscricaoCadastral
							)
							&&
							(
								pagedFilter.RazaoSocial == null || o.RazaoSocial.ToUpper().Contains(pagedFilter.RazaoSocial.ToUpper())
							)
							&&
							(pagedFilter.IdAnalistaDesignado == null || pagedFilter.CpfResponsavel == o.CpfResponsavel)
						)
							,
						pagedFilter);
				}


			}

			#region Trativas Itens
			if (ListaServico.Total > 0)
			{
				foreach (var registro in ListaServico.Items)
				{
					registro.NomeResponsavel = !string.IsNullOrEmpty(registro.NomeResponsavel) ? registro.NomeResponsavel : "-";
					registro.DataCadastroFormatada = registro.DataCadastro == DateTime.MinValue || registro.DataCadastro == null ? "-" : ((DateTime)registro.DataCadastro).ToShortDateString();
					registro.DataEnvioFormatada = registro.DataEnvio == DateTime.MinValue || registro.DataEnvio == null ? "-" : ((DateTime)registro.DataEnvio).ToShortDateString();
					registro.DataStatusFormatada = registro.DataStatus == DateTime.MinValue || registro.DataStatus == null ? "-" : ((DateTime)registro.DataStatus).ToShortDateString();
					registro.NumeroAnoPlanoFormatado = registro.NumeroPlano.ToString("D5") + "/" + registro.AnoPlano;
					registro.NumeroAnoProcessoFormatado = registro.NumeroProcesso > 0 ? Convert.ToInt32(registro.NumeroProcesso).ToString("D4") + "/" + registro.NumeroAnoProcesso : "-";
					registro.SituacaoString = registro.Situacao == 1 ? "EM ELABORAÇÃO"
													: registro.Situacao == 2 ? "ENTREGUE"
													: registro.Situacao == 3 ? "EM ANÁLISE"
													: registro.Situacao == 4 ? "DEFERIDO"
													: registro.Situacao == 5 ? "INDEFERIDO"
													: "-";

					registro.TipoExportacaoString = registro.TipoExportacao == "AP" ? "APROVAÇÃO"
														: registro.TipoExportacao == "CO" ? "COMPROVAÇÃO"
														: "-";

					registro.TipoModalidadeString = registro.TipoModalidade == "S" ? "SUSPENSÃO"
														: "-";

					registro.QtdFluxoMenor70porcento = registro.ListaPEProdutos.FindAll(o => o.ValorFluxoCaixa < 70)?.Count ?? 0;
					registro.QtdFluxoMenor70porcentoString = registro.QtdFluxoMenor70porcento > 0 ? "Sim" : "Não";

					#region Filtrar IdPEProduto PlanoExportacao

					if (registro.ListaPEProdutos.Count > 0)
					{
						foreach (var regProduto in registro.ListaPEProdutos)
						{

							registro.QtdPerdaMaior2porcento = regProduto.ListaPEInsumo?.Where(w => w.ValorPercentualPerda > 2)?.ToList()?.Count ?? 0;
							registro.QtdPerdaMaior2porcentoString = registro.QtdPerdaMaior2porcento > 0 ? "Sim" : "Não";
						}

					}
					#endregion

				}
			}
			#endregion

			if (!string.IsNullOrEmpty(campoFiltroAplicacaoPosterior))
			{
				if (campoFiltroAplicacaoPosterior.Equals("Fluxo"))
				{
					if (pagedFilter.Reverse)
					{
						ListaServico.Items = ListaServico.Items.OrderByDescending(q => q.QtdFluxoMenor70porcentoString).ToList();
					}
					else
					{
						ListaServico.Items = ListaServico.Items.OrderBy(q => q.QtdFluxoMenor70porcentoString).ToList();
					}

				}

				if (campoFiltroAplicacaoPosterior.Equals("Perda"))
				{
					if (pagedFilter.Reverse)
					{
						ListaServico.Items = ListaServico.Items.OrderBy(q => q.QtdPerdaMaior2porcentoString).ToList();
					}
					else
					{
						ListaServico.Items = ListaServico.Items.OrderByDescending(q => q.QtdPerdaMaior2porcentoString).ToList();
					}

				}
			}

			return ListaServico;
		}

		public AnalisarPlanoExportacaoVM SalvarAnalise(AnalisarPlanoExportacaoVM pagedFilter)
		{
			var processo = new AnalisarPlanoExportacaoVM()
			{
				Resultado = true
			};

			try
			{

				switch (pagedFilter.TelaSolicitada)
				{
					//case "PRODUTO-PAIS":
					//	var regProdutoPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(q => q.IdPEProdutoPais == pagedFilter.IdPEProdutoPais);

					//	if (pagedFilter.AcaoIsAprovar)
					//	{
					//		regProdutoPais.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
					//		regProdutoPais.DescricaoJustificativaErro = null;
					//	}
					//	else
					//	{
					//		regProdutoPais.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
					//		regProdutoPais.DescricaoJustificativaErro = pagedFilter.DescricaoJustificativaErro;
					//	}

					//	_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(regProdutoPais);

					//	break;

					case "QUADRO-INSUMO":
						var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == pagedFilter.IdPEInsumo);

						if (pagedFilter.AcaoIsAprovar)
						{							

							if (regInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
							{
								regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
								regInsumo.DescricaoJustificativaErro = null;

								int situacaoAlterado = 3;
								var regInsumoAnterior = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.CodigoInsumo == regInsumo.CodigoInsumo
																									&& q.IdPEProduto == regInsumo.IdPEProduto
																									&& q.SituacaoAnalise == situacaoAlterado).LastOrDefault();

								

								foreach (var detalhesAnteriores in regInsumoAnterior.ListaPEDetalheInsumo)
								{
									detalhesAnteriores.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhesAnteriores);
								}

								foreach (var detalhesAtual in regInsumo.ListaPEDetalheInsumo)
								{
									detalhesAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhesAtual);
								}

								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();

								regInsumoAnterior.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAnterior);
							}
							else
							{
								regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
								regInsumo.DescricaoJustificativaErro = null;

								foreach (var detalhesAtual in regInsumo.ListaPEDetalheInsumo)
								{
									detalhesAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhesAtual);
								}

								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();


							}
						}
						else
						{						
							

							if (regInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
							{
								regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
								regInsumo.DescricaoJustificativaErro = pagedFilter.DescricaoJustificativaErro;

								int situacaoAlterado = 3;
								var regInsumoAnterior = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.CodigoInsumo == regInsumo.CodigoInsumo
																									&& q.IdPEProduto == regInsumo.IdPEProduto
																									&& q.SituacaoAnalise == situacaoAlterado).LastOrDefault();

								
								foreach (var detalhesAnteriores in regInsumoAnterior.ListaPEDetalheInsumo)
								{
									detalhesAnteriores.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhesAnteriores);
								}
								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();

								regInsumoAnterior.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAnterior);
							}
							else
							{
								regInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
								regInsumo.DescricaoJustificativaErro = pagedFilter.DescricaoJustificativaErro;

								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);

								foreach (var detalheAtual in regInsumo.ListaPEDetalheInsumo)
								{
									detalheAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalheAtual);
								}
								_uowSciex.CommandStackSciex.Save();
							}


						}

						_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);

						break;

					case "DETALHE-INSUMO":
						var regDetInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(q => q.IdPEDetalheInsumo == pagedFilter.IdPEDetalheInsumo);						

						if (pagedFilter.AcaoIsAprovar)
						{
							
							if (regDetInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
							{
								regDetInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
								regDetInsumo.DescricaoJustificativaErro = null;

								int situacaoAlterado = 3;
								var idProduto = regDetInsumo.PEInsumo.IdPEProduto;
								var codInsumo = regDetInsumo.PEInsumo.CodigoInsumo;

								var regInsumoAnterior = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == idProduto
																										&& q.CodigoInsumo == codInsumo
																										&& q.SituacaoAnalise == situacaoAlterado).LastOrDefault();
								

								foreach (var detalhesAnteriores in regInsumoAnterior.ListaPEDetalheInsumo)
								{
									detalhesAnteriores.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhesAnteriores);
								}
								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();

								regInsumoAnterior.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAnterior);
								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();

								var regInsumoAtual = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == regDetInsumo.IdPEInsumo);

								regInsumoAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAtual);
								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();
							}
							else
							{
								regDetInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
								regDetInsumo.DescricaoJustificativaErro = null;
							}
						}
						else
						{
							_uowSciex.CommandStackSciex.DetachEntries();

							var registroInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == regDetInsumo.IdPEInsumo);

							if (registroInsumo.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO)
							{
								registroInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
								registroInsumo.DescricaoJustificativaErro = "Existe(m) detalhe(s) com erro(s).";
								_uowSciex.CommandStackSciex.PEInsumo.Salvar(registroInsumo);

								foreach (var detalheAtual in registroInsumo.ListaPEDetalheInsumo)
								{
									detalheAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalheAtual);
								}
								_uowSciex.CommandStackSciex.Save();

								
								int situacaoAlterado = 3;
								var idProduto = regDetInsumo.PEInsumo.IdPEProduto;
								var codInsumo = regDetInsumo.PEInsumo.CodigoInsumo;

								var regInsumoAnterior = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == idProduto
																										&& q.CodigoInsumo == codInsumo
																										&& q.SituacaoAnalise == situacaoAlterado).LastOrDefault();

								regInsumoAnterior.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
								_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAnterior);

								foreach (var detalhesAnteriores in regInsumoAnterior.ListaPEDetalheInsumo)
								{
									detalhesAnteriores.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.INATIVO;
									_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhesAnteriores);
								}
								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();

							}
							else
							{
								regDetInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
								regDetInsumo.DescricaoJustificativaErro = pagedFilter.DescricaoJustificativaErro;

								_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetInsumo);

								registroInsumo.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO;
								registroInsumo.DescricaoJustificativaErro = "Existe(m) detalhe(s) com erro(s).";

								_uowSciex.CommandStackSciex.PEInsumo.Salvar(registroInsumo);

								_uowSciex.CommandStackSciex.Save();
								_uowSciex.CommandStackSciex.DetachEntries();
							}
							
						}

						_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetInsumo);

						break;
				}

				_uowSciex.CommandStackSciex.Save();

			}
			catch (Exception e)
			{
				processo.Resultado = false;

				processo.Mensagem = $"Mensagem: {e.Message}";
			}



			return processo;
		}
	}
}
