using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class DiBll : IDiBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IPliBll _pliBll;
		private readonly IUsuarioPssBll _usuarioPssBll;

		private int qtdLinhasArquivo = 0;

		public DiBll(
			IUnitOfWorkSciex uowSciex,
			IPliBll pliBll,
			IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_pliBll = pliBll;
			_usuarioPssBll = usuarioPssBll;
		}

		public PliVM Selecionar(long? idPliMercadoria)
		{
			var pliVM = new PliVM();
			if (!idPliMercadoria.HasValue)
			{
				return pliVM;
			}

			var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(x => x.IdPliMercadoria == idPliMercadoria);

			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == pliMercadoria.IdPLI);
			if (pli == null)
			{
				return null;
			}

			pliVM = AutoMapper.Mapper.Map<PliVM>(pli);
			pliVM.IdPliMercadoria = pliMercadoria.IdPliMercadoria;
			//DI
			long? idDi = null;
			if (pliMercadoria.Li != null)
				idDi = pliMercadoria.Li.Di.IdDi;
			else if (pliMercadoria.NumeroLiRetificador != null && pliMercadoria.NumeroLiRetificador > 0)
			{
				long numeroLi = Convert.ToInt64(pliMercadoria.NumeroLiRetificador);
				idDi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLi).IdDI;
			}
				
			if (idDi != null)
			{
				DiVM Di = new DiVM();
				var di = _uowSciex.QueryStackSciex.Di.Selecionar(p => p.IdDi == idDi);
				Di.IdDi = di.IdDi;
				Di.IdUrfDespacho = di.IdUrfDespacho;
				Di.IdUrfEntrada = di.IdUrfEntrada;
				var urfDespacho = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(o => o.IdUnidadeReceitaFederal == di.IdUrfDespacho);
				Di.UrfDespachoFormatado = urfDespacho.Codigo + " | " + urfDespacho.Descricao;
				var urfEntrada = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(o => o.IdUnidadeReceitaFederal == di.IdUrfEntrada);
				Di.UrfEntradaFormatado = urfEntrada.Codigo + " | " + urfEntrada.Descricao;
				Di.NumeroDi = di.NumeroDi;
				Di.QtdAdicao = di.QtdAdicao;
				Di.LocalAlfandega = di.LocalAlfandega;

				#region Alfandega
				if (di.LocalAlfandega != null)
				{
					var Alfandega = _uowSciex.QueryStackSciex.RecintoAlfandega.Listar(x => x.Codigo == di.LocalAlfandega);
					if (Alfandega.Count == 0)
					{
						Di.DescricaoLocalAlfandega = "--";
					}
					else
					{
						Di.DescricaoLocalAlfandega = Alfandega.FirstOrDefault().Codigo + " | " + Alfandega.FirstOrDefault().Descricao;
					}
				}
				else
				{
					Di.DescricaoLocalAlfandega = "--";
				}				
				#endregion

				Di.SetorArmazenamento = di.SetorArmazenamento;

				#region Setor Armazenamento

				if (di.SetorArmazenamento > 0)
				{
					PagedItems<SetorArmazenamentoVM> ListaSetorArmazenamento = null;
					if (di.LocalAlfandega != null)
						ListaSetorArmazenamento = _uowSciex.QueryStackSciex.ListarPaginadoSql<SetorArmazenamentoVM>(this.gerarQueryConsultarSetorArmazenamento((int)di.LocalAlfandega), new SetorArmazenamentoVM());

					if(ListaSetorArmazenamento != null && ListaSetorArmazenamento.Total > 0)
					{
						Di.SetorArmazenamentoDescricao = ListaSetorArmazenamento.Items.FirstOrDefault().Codigo + " | " + ListaSetorArmazenamento.Items.FirstOrDefault().Descricao;
					}
					else
					{
						Di.SetorArmazenamentoDescricao = "--";
					}
				} 
				else
				{
					Di.SetorArmazenamentoDescricao = "--";
				}	
				
				#endregion

				Di.SetorArmazenamento = di.SetorArmazenamento;
				var tipoDeclaracaoEntity = _uowSciex.QueryStackSciex.TipoDeclaracao.Selecionar(o => o.Codigo == di.TipoDeclaracaoCodigo);
				Di.TipoDeclaracaoDescricao = tipoDeclaracaoEntity.Codigo + " | " + tipoDeclaracaoEntity.Descricao;
				Di.TipoMultimodal = di.TipoMultimodal;
				Di.TipoMultimodalFormatado = di.TipoMultimodal == 0 ? "Não" : "Sim";
				var viaTransporteEntity = _uowSciex.QueryStackSciex.ViaTransporte.Selecionar(o => o.Codigo == di.ViaTransporteCodigo);
				Di.ViaTransporteDescricao = viaTransporteEntity.Codigo + " | " + viaTransporteEntity.Descricao;
				Di.ValorTotalMn = String.Format("{0:n2}", di.ValorTotalMn);
				Di.ValorTotalDolar = String.Format("{0:n2}", di.ValorTotalDolar);
				Di.DataDesembaracoFormatada = di.DataDesembaraco.Value.ToShortDateString();
				Di.DataProcessamentoFormatada = di.DataProcessamento.Value.ToShortDateString();
				Di.DataRegistroFormatada = di.DataRegistro.Value.ToShortDateString();

				List<DiArmazemVM> listArmazems = new List<DiArmazemVM>();
				var armazem = _uowSciex.QueryStackSciex.DiArmazem.Listar(w => w.IdDi == idDi);
				foreach (var item in armazem)
				{
					DiArmazemVM diArmazem = new DiArmazemVM();
					diArmazem.Descricao = item.Descricao;
					diArmazem.Id = item.Id;
					diArmazem.IdDi = item.IdDi;
					listArmazems.Add(diArmazem);
				}
				Di.ListaArmazems = listArmazems;

				#region Embalagens
				List<DiEmbalagemVM> listEmbalagems = new List<DiEmbalagemVM>();
				var embalagems = _uowSciex.QueryStackSciex.ListarPaginadoSql<DiEmbalagemVM>(this.gerarQueryConsultarEmbalagemGrid(idDi), new DiEmbalagemVM());
				if(embalagems.Total > 0)
				{
					foreach (var item in embalagems.Items)
					{
						DiEmbalagemVM diEmbalagem = new DiEmbalagemVM();
						diEmbalagem.QuantidadeVolumeCarga = item.QuantidadeVolumeCarga;
						diEmbalagem.Descricao = item.Descricao;
						diEmbalagem.CodigoTipoEmbalagem = item.CodigoTipoEmbalagem;
						listEmbalagems.Add(diEmbalagem);
					}
				}				
				Di.ListaEmbalagems = listEmbalagems;
				#endregion

				pliVM.Di = Di;
				pliVM.UtilizadaDI = "Sim";
				pliVM.IdDI = idDi.ToString();

				pliVM.NumeroDI = di.NumeroDi.ToString();
				pliVM.DataDiFormatada = di.DataRegistro.Value.ToShortDateString();

				List<DiLiVM> listaDiLi = new List<DiLiVM>();
				var lis = _uowSciex.QueryStackSciex.DiLi.Listar(w => w.IdDi == idDi);
				foreach (var item in lis)
				{
					DiLiVM diLi = new DiLiVM();
					diLi.CodigoViaTransporte = item.CodigoViaTransporte;
					diLi.Id = item.Id;
					diLi.IdDi = item.IdDi;
					diLi.IdFundamentoLegal = item.IdFundamentoLegal;
					diLi.IdMoedaFrete = item.IdMoedaFrete;
					diLi.IdMoedaSeguro = item.IdMoedaSeguro;
					diLi.NumeroLi = item.NumeroLi;
					diLi.TipoMultimodal = item.TipoMultimodal;
					diLi.ValorFrete = item.ValorFrete;
					diLi.ValorFreteDolar = item.ValorFreteDolar;
					diLi.ValorFreteMoedaNegociada = item.ValorFreteMoedaNegociada;
					diLi.ValorMercadoriaDolar = item.ValorMercadoriaDolar;
					diLi.ValorMercadoriaDolarFormatado = String.Format("{0:n2}", item.ValorMercadoriaDolar);
					diLi.ValorMercadoriaMoedaNegociadaFormatado = String.Format("{0:n2}", item.ValorMercadoriaMoedaNegociada);
					diLi.ValorPesoLiquido = Convert.ToDecimal(item.ValorPesoLiquido);
					diLi.ValorSeguro = item.ValorSeguro;
					diLi.ValorSeguroDolar = item.ValorSeguroDolar;
					diLi.ValorSeguroMoedaNegociada = item.ValorSeguroMoedaNegociada;
					listaDiLi.Add(diLi);
				}
				Di.ListaDiLis = listaDiLi;

			}
			else
			{
				pliVM.UtilizadaDI = "Não";
			}


			return pliVM;
		}

		public PagedItems<DiLiVM> ListarPaginado(DiLiVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<DiLiVM>(); }


			var ret = _uowSciex.QueryStackSciex.DiLi.ListarPaginado<DiLiVM>(o =>
				(
					(
						pagedFilter.IdDi == 0 || o.IdDi == pagedFilter.IdDi
					)
				),
				pagedFilter);

			foreach (var item in ret.Items)
			{
				item.ValorMercadoriaDolarFormatado = String.Format("{0:n2}", item.ValorMercadoriaDolar);
				item.ValorMercadoriaMoedaNegociadaFormatado = String.Format("{0:n2}", item.ValorMercadoriaMoedaNegociada);
			}

			return ret;
		}

		public DiLiVM SelecionarDiLi(long? idDi)
		{
			var diLiVM = new DiLiVM();
			if (!idDi.HasValue)
			{
				return diLiVM;
			}

			var dili = _uowSciex.QueryStackSciex.DiLi.Selecionar(x => x.Id == idDi);

			if (dili == null)
			{
				return null;
			}

			diLiVM = AutoMapper.Mapper.Map<DiLiVM>(dili);
			var viaTransporteEntity = _uowSciex.QueryStackSciex.ViaTransporte.Selecionar(o => o.Codigo == diLiVM.CodigoViaTransporte);
			if(viaTransporteEntity != null)
			{
				diLiVM.ViaTransporte = AutoMapper.Mapper.Map<ViaTransporteVM>(viaTransporteEntity);
				diLiVM.ViaTransporteDescricao = viaTransporteEntity.Codigo + " | " + viaTransporteEntity.Descricao;
			}
			else
			{
				diLiVM.ViaTransporte = new ViaTransporteVM();
				diLiVM.ViaTransporteDescricao = "--";
			}
			

			return diLiVM;
		}

		#region - Processamento de DI

		public string ProcessarDI()
		{
			#region - RN01 Registro de inicio de Execucao

			ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
			_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.ProcessarDi;
			_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.StatusExecucao = (int)EnumStatusControleExecucaoServico.ServicoEnviado;
			_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA"; // Verificar
			_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143"; // Verificar

			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.Save();

			#endregion

			#region - RN02 preencher tabela de DI

			var listaDiEntrada = _uowSciex.QueryStackSciex.DiEntrada.Listar(o => o.Situacao == (int)EnumStatusProcessamentoDi.AGUARDANDO_PROCESSAMENTO).OrderBy(o => o.IdDiArquivoEntrada);

			#endregion

			if (listaDiEntrada.Any()) // RN07 ~ RN17
			{
				if (!ParidadeCambialValidacao(listaDiEntrada.FirstOrDefault().Cnpj))
				{
					return "TAXA CAMBIAL NÃO CADASTRADA PARA O DIA CORRENTE";
				}
				long idDiArquivoEntradaAnterior = 0;
				Int16 sucessoCount = 0;
				Int16 falhaCount = 0;
				var ultimoItem = listaDiEntrada.LastOrDefault();
				foreach (var diEntrada in listaDiEntrada)
				{
					if (idDiArquivoEntradaAnterior != diEntrada.IdDiArquivoEntrada)
					{
						if (idDiArquivoEntradaAnterior > 0)
						{
							var arqEntity = _uowSciex.QueryStackSciex.DiArquivoEntrada.Selecionar(o => o.Id == idDiArquivoEntradaAnterior);
							arqEntity.DataHoraFimProcesso = GetDateTimeNowUtc();
							arqEntity.SituacaoLeitura = 2;
							arqEntity.QuantidadeDiProcessada = sucessoCount;
							arqEntity.QuantidadeDiErro = falhaCount;

							sucessoCount = 0;
							falhaCount = 0;

							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.DiArquivoEntrada.Salvar(arqEntity);
							_uowSciex.CommandStackSciex.Save();

						}

						idDiArquivoEntradaAnterior = diEntrada.IdDiArquivoEntrada;
						var diArquivoEntradaEntity = _uowSciex.QueryStackSciex.DiArquivoEntrada.Selecionar(o => o.Id == diEntrada.IdDiArquivoEntrada);
						diArquivoEntradaEntity.DataHoraInicioProcesso = GetDateTimeNowUtc();
						_uowSciex.CommandStackSciex.DetachEntries();
						_uowSciex.CommandStackSciex.DiArquivoEntrada.Salvar(diArquivoEntradaEntity);
						_uowSciex.CommandStackSciex.Save();
					}

					if (!ValidarDi(diEntrada))
					{
						falhaCount++;

						if (ultimoItem.Equals(diEntrada))
						{
							var diArquivoEntradaEntity = _uowSciex.QueryStackSciex.DiArquivoEntrada.Selecionar(o => o.Id == diEntrada.IdDiArquivoEntrada);
							diArquivoEntradaEntity.DataHoraFimProcesso = GetDateTimeNowUtc();
							diArquivoEntradaEntity.SituacaoLeitura = 2;
							diArquivoEntradaEntity.QuantidadeDiProcessada = sucessoCount;
							diArquivoEntradaEntity.QuantidadeDiErro = falhaCount;
							_uowSciex.CommandStackSciex.DetachEntries();
							_uowSciex.CommandStackSciex.DiArquivoEntrada.Salvar(diArquivoEntradaEntity);
							_uowSciex.CommandStackSciex.Save();
						}
						continue;
					}
					else
					{
						sucessoCount++;
					}


					// Processamento de DI
					var idDi = RegraSalvarDi(diEntrada);

					// Processamento de Embalagem
					var embalagemEntrada = _uowSciex.QueryStackSciex.DiEmbalagemEntrada.Selecionar(o => o.IdDiEntrada == diEntrada.Id);

					RegraSalvarEmbalagem(embalagemEntrada, idDi);

					// Processamento de Armazem
					var armazemEntrada = _uowSciex.QueryStackSciex.DiArmazemEntrada.Selecionar(o => o.IdDiEntrada == diEntrada.Id);

					RegraSalvarArmazem(armazemEntrada, idDi);

					// Processamento de Adicao
					var listaAdicaoEntrada = _uowSciex.QueryStackSciex.DiAdicaoEntrada.Listar(o => o.IdDiEntrada == diEntrada.Id);

					RegraSalvarAdicao(listaAdicaoEntrada, idDi);

					if (ultimoItem.Equals(diEntrada))
					{
						var diArquivoEntradaEntity = _uowSciex.QueryStackSciex.DiArquivoEntrada.Selecionar(o => o.Id == diEntrada.IdDiArquivoEntrada);
						diArquivoEntradaEntity.DataHoraFimProcesso = GetDateTimeNowUtc();
						diArquivoEntradaEntity.SituacaoLeitura = 2;
						diArquivoEntradaEntity.QuantidadeDiProcessada = sucessoCount;
						diArquivoEntradaEntity.QuantidadeDiErro = falhaCount;
						_uowSciex.CommandStackSciex.DiArquivoEntrada.Salvar(diArquivoEntradaEntity);
						_uowSciex.CommandStackSciex.Save();
					}
				}
			}

			return "Sucesso";
		}

		private bool ParidadeCambialValidacao(string cnpj)
		{
			#region - RN07 Validar paridade cambial

			var dataAtual = DateTime.Now.AddDays(-1);

			var paridadeCambial = _uowSciex.QueryStackSciex.ParidadeCambial.Listar(o => o.DataParidade > dataAtual);

			int countParidade = paridadeCambial.Count();

			foreach (var item in paridadeCambial)
			{
				var dataParidade = item.DataParidade.ToString("dd/MM/yyyy");

				var dataComparacao = DateTime.Now.ToString("dd/MM/yyyy");

				if (dataParidade != dataComparacao)
				{
					--countParidade;
				}
			}

			if (countParidade == 0)
			{
				//var entityErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.);

				//RegistrarErroProcessamento(null, "NÃO EXISTE PARIDADE CAMBIAL PARA A DATA CORRENTE", cnpj);

				return false;
			}

			#endregion

			return true;
		}

		private bool ValidarDi(DiEntradaEntity diEntrada)
		{
			#region - RN08 Validar situacao da empresa

			var empresaValida = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == diEntrada.Cnpj);

			if (empresaValida.IdSituacaoInscricao != (int)EnumSituacaoInscricao.Ativa
				&& empresaValida.IdSituacaoInscricao != (int)EnumSituacaoInscricao.Bloqueada)
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.INSCRICAO_CADASTRAL_EMPRESA);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao.Replace("[SITUACAO]", "[" + empresaValida.DescricaoSituacaoInscricao + "]"), diEntrada.Cnpj,
					(int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN09 Validar DI Duplicada

			long numeroDiComparacao = Int64.Parse(diEntrada.Numero);

			var di = _uowSciex.QueryStackSciex.Di.Selecionar(o => o.NumeroDi == numeroDiComparacao);

			if (di != null)
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.DI_DUPLICADA);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN20 Validar Tipo de Declaração

			if (!string.IsNullOrEmpty(diEntrada.CodigoDeclaracao))
			{
				var codigoDeclaracao = Convert.ToInt16(diEntrada.CodigoDeclaracao);
				var tipoDeclaracaoEntity = _uowSciex.QueryStackSciex.TipoDeclaracao.Selecionar(o => o.Codigo == codigoDeclaracao);
				if (tipoDeclaracaoEntity == null)
				{
					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 510);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, 510);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}
			}
			#endregion

			#region - RN22 Validar Via Transporte de Carga

			if (!string.IsNullOrEmpty(diEntrada.CodigoViaTransCarga))
			{
				var viaTransporteCodigo = Convert.ToInt16(diEntrada.CodigoViaTransCarga);
				var viaTransporteEntity = _uowSciex.QueryStackSciex.ViaTransporte.Selecionar(o => o.Codigo == viaTransporteCodigo);
				if (viaTransporteEntity == null)
				{
					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 512);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, 512);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}
			}

			#endregion

			#region - RN24 Validar Tipo Embalagen

			var diEmbalagenEntity = _uowSciex.QueryStackSciex.DiEmbalagemEntrada.Selecionar(o => o.IdDiEntrada == diEntrada.Id);

			if (diEmbalagenEntity != null)
			{
				var diEmbalagemCodigo = Convert.ToInt16(diEmbalagenEntity.CodigoTipoEmbalagem);

				var tipoEmbalagemEntity = _uowSciex.QueryStackSciex.TipoEmbalagem.Listar(o => o.Codigo == diEmbalagemCodigo);
				if (tipoEmbalagemEntity.Count == 0)
				{
					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 513);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, 513);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}
			}

			#endregion

			#region - RN25 Validar Recinto Alfandega

			if (!string.IsNullOrEmpty(diEntrada.CodigoRecintoAlfandega))
			{
				var CodigoRecintoAlfandega = Convert.ToInt32(diEntrada.CodigoRecintoAlfandega);

				var recintoAlfandegaEntity = _uowSciex.QueryStackSciex.RecintoAlfandega.Listar(o => o.Codigo == CodigoRecintoAlfandega);
				if (recintoAlfandegaEntity.Count == 0)
				{
					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 514);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, 514);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}
			}
			#endregion

			#region - RN26 Validar Codigo Setor Armazenamento

			if (!string.IsNullOrEmpty(diEntrada.CodigoSetorArmazena))
			{
				var SetorArmazenamentoCodigo = Convert.ToInt32(diEntrada.CodigoSetorArmazena);
				var setorArmazenamentoEntity = _uowSciex.QueryStackSciex.SetorArmazenamento.Listar(o => o.Codigo == SetorArmazenamentoCodigo);
				if (setorArmazenamentoEntity.Count == 0)
				{
					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 515);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, 515);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}
			}
			#endregion
				
			#region - RN10 Validar quantidade de adicoes DI

			var quantidadeAdicao = _uowSciex.QueryStackSciex.DiAdicaoEntrada.Listar(o => o.IdDiEntrada == diEntrada.Id)?.Count;

			if (Int32.Parse(diEntrada.QuantidadeAdicao) != quantidadeAdicao)
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.QUANTIDADE_ADICOES_DIVERGENTE);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN11 Validar data de registro

			var dataStringRegistro = diEntrada.DataRegistro.Substring(0, 4) + "/" + diEntrada.DataRegistro.Substring(4, 2) + "/" + diEntrada.DataRegistro.Substring(6, 2);

			if (!RegexValidacaoData(dataStringRegistro))
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.DATA_REGISTRO_INVALIDA);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN12 Validar data de desembaraco

			var dataStringDesembaraco = diEntrada.DataDesembaraco.Substring(0, 4) + "/" + diEntrada.DataDesembaraco.Substring(4, 2) + "/" + diEntrada.DataDesembaraco.Substring(6, 2);

			if (!RegexValidacaoData(dataStringDesembaraco))
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.DATA_DESEMBARAÇO_INVALIDA);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN13 Validar URF de entrada

			var codigoCargaComparacao = Int32.Parse(diEntrada.CodigoUrfEntradaCarga);

			var entradaCarga = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Listar(o => o.Codigo == codigoCargaComparacao);

			if (!entradaCarga.Any())
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.CODIGO_URF_ENTRADA_NAO_CADASTRADO);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN14 Validar URF de despacho

			var codigoDespachoComparacao = Int32.Parse(diEntrada.CodigoUrfDespacho);

			var despacho = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Listar(o => o.Codigo == codigoDespachoComparacao);

			if (!despacho.Any())
			{
				short idMensagem = Convert.ToInt16(EnumMensagemErro.CODIGO_URF_DESPACHO_NAO_CADASTRADO);

				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

				RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.PLI, idMensagem);

				// RN05 - Atualizar status da DI de Entrada
				AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

				return false;
			}

			#endregion

			#region - RN15, RN16 e RN17

			var listaAdicaoEntradaValidacao = _uowSciex.QueryStackSciex.DiAdicaoEntrada.Listar(o => o.IdDiEntrada == diEntrada.Id);

			foreach (var item in listaAdicaoEntradaValidacao)
			{
				#region - RN15 Validar Li

				var numeroLiComparacao = Int32.Parse(item.NumeroLi);

				var li = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLiComparacao);

				if (li == null)
				{
					short idMensagem = Convert.ToInt16(EnumMensagemErro.LI_NAO_ENCONTRADA);

					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao.Replace("[DAD_NU_LI]", "[" + numeroLiComparacao + "]"), diEntrada.Cnpj,
						(int)EnumErroProcessamentoNivelErro.MERCADORIA, idMensagem);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					// RN06 - Atualizar status da LI de Entrada
					AtualizarSituacaoDiLiEntrada(item.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}

				#endregion

				#region - RN16 Validar status Li

				AliEntity aliValid = null;
				if (li != null)
					aliValid = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == li.IdPliMercadoria && o.Status == 3);

				if (li.Status != (int)EnumLiStatus.LI_DEFERIDA || aliValid == null)
				{
					short idMensagem = Convert.ToInt16(EnumMensagemErro.LI_STATUS_INVALIDO);

					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao.Replace("[LI_ST]", "[" + SelecionarNomeStatusLI(li.Status) + "]"), diEntrada.Cnpj,
						(int)EnumErroProcessamentoNivelErro.MERCADORIA, idMensagem);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					// RN06 - Atualizar status da LI de Entrada
					AtualizarSituacaoDiLiEntrada(item.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}

				#endregion

				#region - RN17 Validar Fundamentacao Legal

				var codigoFundamentoComparacao = Int32.Parse(item.CodigoFundamentoLegal);

				var fundamentoLegal = _uowSciex.QueryStackSciex.FundamentoLegal.Selecionar(o => o.Codigo == codigoFundamentoComparacao);

				if (fundamentoLegal == null)
				{
					short idMensagem = Convert.ToInt16(EnumMensagemErro.CODIGO_FUNDAMENTACAO_LEGAL_NAO_CADASTRADO);

					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);

					RegistrarErroProcessamento(diEntrada.Id, entityMensagemErro.Descricao, diEntrada.Cnpj, (int)EnumErroProcessamentoNivelErro.MERCADORIA,
						idMensagem);

					// RN05 - Atualizar status da DI de Entrada
					AtualizarSituacaoDiEntrada(diEntrada.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					// RN06 - Atualizar status da LI de Entrada
					AtualizarSituacaoDiLiEntrada(item.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_ERRO);

					return false;
				}

				#endregion
			}

			#endregion

			return true;
		}

		private string SelecionarNomeStatusLI(byte status)
		{
			switch (status)
			{
				case (byte)EnumLiStatus.LI_INDEFERIDA:
					return "INDEFERIDA";
					break;
				case (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO:
					return "ENVIADA PARA CANCELAMENTO";
					break;
				case (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR:
					return "CANCELADA PELO IMPORTADOR";
					break;
				case (byte)EnumLiStatus.LI_CANCELADA_PELO_SISCOMEX:
					return "CANCELADA PELO SISCOMEX";
					break;
				case (byte)EnumLiStatus.LI_SUBSTITUIDA:
					return "SUBSTITUIDA";
					break;

				default:
					return "UTILIZADA";
					break;
			}
		}

		private void RegistrarErroProcessamento(long? idDiEntrada, string descricao, string cnpj, byte nivel, short idMensagemErro)
		{
			var entity = new ErroProcessamentoEntity
			{
				CNPJImportador = cnpj,
				CodigoNivelErro = nivel,
				IdErroMensagem = idMensagemErro,
				DataProcessamento = DateTime.Now,
				Descricao = descricao,
				IdDiEntrada = idDiEntrada
			};

			_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();
		}

		private long RegraSalvarDi(DiEntradaEntity diEntrada)
		{
			// Desembaraco
			int anoDesembaraco = Int32.Parse(diEntrada.DataDesembaraco.Substring(0, 4));
			int mesDesembaraco = Int32.Parse(diEntrada.DataDesembaraco.Substring(4, 2));
			int diaDesembaraco = Int32.Parse(diEntrada.DataDesembaraco.Substring(6, 2));

			// Registro
			int anoRegistro = Int32.Parse(diEntrada.DataRegistro.Substring(0, 4));
			int mesRegistro = Int32.Parse(diEntrada.DataRegistro.Substring(4, 2));
			int diaRegistro = Int32.Parse(diEntrada.DataRegistro.Substring(6, 2));

			var entity = new DiEntity();

			entity.Cnpj = diEntrada.Cnpj;
			entity.DataDesembaraco = new DateTime(anoDesembaraco, mesDesembaraco, diaDesembaraco);
			entity.DataProcessamento = GetDateTimeNowUtc();
			entity.DataRegistro = new DateTime(anoRegistro, mesRegistro, diaRegistro);
			entity.NumeroDi = Int64.Parse(diEntrada.Numero);
			entity.QtdAdicao = Int32.Parse(diEntrada.QuantidadeAdicao);
			entity.TipoDeclaracaoCodigo = Int32.Parse(diEntrada.CodigoDeclaracao);
			entity.IdUrfEntrada = SelecioarIdUrfEntradaDespacho(diEntrada.CodigoUrfEntradaCarga);
			entity.IdUrfDespacho = SelecioarIdUrfEntradaDespacho(diEntrada.CodigoUrfDespacho);

			if ((diEntrada.CodigoRecintoAlfandega != ""))
			{
				entity.LocalAlfandega = Int32.Parse(diEntrada.CodigoRecintoAlfandega);
			}
			entity.ViaTransporteCodigo = Int32.Parse(diEntrada.CodigoViaTransCarga);

			if (diEntrada.TipoMultimodal.Equals("N"))
			{
				entity.TipoMultimodal = 0;
			}
			else
			{
				entity.TipoMultimodal = 1;
			}

			entity.ValorTotalDolar = Convert.ToDecimal(diEntrada.ValorTotalMleDolar) / 100;
			entity.ValorTotalMn = Convert.ToDecimal(diEntrada.ValorTotalMleMn) / 100;
			entity.SetorArmazenamento = Int32.Parse(diEntrada.CodigoSetorArmazena);

			_uowSciex.CommandStackSciex.Di.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();

			// Atualiza a situacao da DI de Entrada
			diEntrada.Situacao = (int)EnumStatusProcessamentoDi.PROCESSADO_SUCESSO;
			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.DiEntrada.Salvar(diEntrada);
			_uowSciex.CommandStackSciex.Save();

			return entity.IdDi;
		}

		private void RegraSalvarAdicao(List<DiAdicaoEntradaEntity> listaAdicaoEntrada, long idDi)
		{
			foreach (var item in listaAdicaoEntrada)
			{
				var entity = new DiLiEntity();
				entity.NumeroLi = Int32.Parse(item.NumeroLi);
				entity.IdDi = idDi;
				entity.CodigoViaTransporte = Int32.Parse(item.CodigoViaTransporte);

				if (item.TipoMultimodal.Equals("N"))
				{
					entity.TipoMultimodal = 0;
				}
				else
				{
					entity.TipoMultimodal = 1;
				}

				entity.ValorPesoLiquido = Convert.ToDecimal(item.ValorPesoLiquido) / 100000; // Verificar casas decimais
				entity.ValorFreteMoedaNegociada = Convert.ToDecimal(item.ValorFreteMoedaNegociada) / 100; // Verificar casas decimais
				entity.ValorFreteDolar = Convert.ToDecimal(item.ValorFreteDolar) / 100; // Verificar casas decimais
				entity.ValorFrete = Convert.ToDecimal(item.ValorFrete) / 100; // Verificar casas decimais
				entity.ValorSeguroMoedaNegociada = Convert.ToDecimal(item.ValorSeguroMoedaNegociada) / 100; // Verificar casas decimais
				entity.ValorSeguroDolar = Convert.ToDecimal(item.ValorSeguroDolar) / 100; // Verificar casas decimais
				entity.ValorSeguro = Convert.ToDecimal(item.ValorSeguro) / 100; // Verificar casas decimais
				entity.IdMoedaFrete = SelecionarIdMoeda(item.CodigoMoedaFrete);
				entity.IdMoedaSeguro = SelecionarIdMoeda(item.MoedaSeguro);
				entity.IdFundamentoLegal = SelecionarIdFundamentoLegal(item.CodigoFundamentoLegal);
				entity.ValorMercadoriaDolar = Convert.ToDecimal(item.ValorMercadoriaDolar) / 100; // Verificar casas decimais
				entity.ValorMercadoriaMoedaNegociada = Convert.ToDecimal(item.ValorMercadoriaMoedaNegociada) / 100; // Verificar casas decimais					

				_uowSciex.CommandStackSciex.DiLi.Salvar(entity);
				_uowSciex.CommandStackSciex.Save();

				// Atualizar Status da Adição
				AtualizarSituacaoDiLiEntrada(item.Id, (int)EnumStatusProcessamentoDi.PROCESSADO_SUCESSO);

				// Atualizar Status da Li
				AtualizarSituacaoLi(item.NumeroLi, (byte)EnumLiStatus.LI_UTILIZADA, idDi);
			}
		}

		private void AtualizarSituacaoLi(string numeroLi, byte situacao, long idDi)
		{
			var numero = Int32.Parse(numeroLi);

			var entity = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numero);

			entity.Status = situacao;
			entity.IdDI = idDi;

			_uowSciex.CommandStackSciex.Li.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();
		}

		private bool RegexValidacaoData(string data)
		{
			Match match = Regex.Match(data.ToString(), @"^\d{4}\/(0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01])$");
			return match.Success;
		}

		private void AtualizarSituacaoDiEntrada(long idDiEntrada, int situacao)
		{
			var entity = _uowSciex.QueryStackSciex.DiEntrada.Selecionar(o => o.Id == idDiEntrada);

			entity.Situacao = situacao;

			_uowSciex.CommandStackSciex.DiEntrada.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();
		}

		private void AtualizarSituacaoDiLiEntrada(long idAdicaoEntrada, int situacao)
		{
			var entity = _uowSciex.QueryStackSciex.DiAdicaoEntrada.Selecionar(o => o.Id == idAdicaoEntrada);

			entity.Situacao = situacao;

			_uowSciex.CommandStackSciex.DiAdicaoEntrada.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();
		}

		private int SelecionarIdFundamentoLegal(string codigoFundamentoLegal)
		{
			var codigo = Int32.Parse(codigoFundamentoLegal);

			var entity = _uowSciex.QueryStackSciex.FundamentoLegal.Selecionar(o => o.Codigo == codigo);

			return entity.IdFundamentoLegal;
		}

		private int SelecionarIdMoeda(string codigoMoedaFrete)
		{
			var codigo = Int32.Parse(codigoMoedaFrete);

			var entity = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.CodigoMoeda == codigo);

			return entity.IdMoeda;
		}

		private void RegraSalvarArmazem(DiArmazemEntradaEntity armazemEntrada, long idDi)
		{
			var entity = new DiArmazemEntity
			{
				Descricao = armazemEntrada.Descricao,
				IdDi = idDi
			};

			_uowSciex.CommandStackSciex.DiArmazem.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();
		}

		private void RegraSalvarEmbalagem(DiEmbalagemEntradaEntity embalagemEntrada, long idDi)
		{
			var entity = new DiEmbalagemEntity
			{
				CodigoTipoEmbalagem = Int32.Parse(embalagemEntrada.CodigoTipoEmbalagem),
				QuantidadeVolumeCarga = Int32.Parse(embalagemEntrada.QuantidadeVolumeCarga),
				IdDi = idDi
			};

			_uowSciex.CommandStackSciex.DiEmbalagem.Salvar(entity);
			_uowSciex.CommandStackSciex.Save();
		}

		private int SelecioarIdUrfEntradaDespacho(string codigoUrfEntradaCarga)
		{
			var codigo = Int32.Parse(codigoUrfEntradaCarga);

			var entity = _uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(o => o.Codigo == codigo);

			return entity.IdUnidadeReceitaFederal;
		}

		#endregion

		public void LerAquivoDI()
		{
			string linha;
			StreamReader file = null;

			#region RN01
			var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == (int)EnumListaServico.SalvarArquivoDiNormal);

			var localArquivoFTP = configuracoesFTP.FirstOrDefault().Valor;
			var usuario = configuracoesFTP.ElementAt(1).Valor;
			var senha = configuracoesFTP.ElementAt(2).Valor;

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
				_controleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.LerArquivoDi;

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
				var arquivoEntrada = new DiArquivoEntradaEntity();

				arquivoEntrada.SituacaoLeitura = 0;
				arquivoEntrada.DataHoraRecepcao = GetDateTimeNowUtc();
				arquivoEntrada.QuantidadeDiErro = 0;
				arquivoEntrada.QuantidadeDi = 0;
				arquivoEntrada.QuantidadeDiProcessada = 0;

				//Salva também na tabela li arquivo
				arquivoEntrada.DiArquivo = new DiArquivoEntity();
				arquivoEntrada.DiArquivo.Arquivo = arquivo;

				_uowSciex.CommandStackSciex.DiArquivoEntrada.Salvar(arquivoEntrada);
				_uowSciex.CommandStackSciex.Save();

				//Finaliza o serviço de salvar o arquivo DI
				//Inicia o serviço de salvar o arquivo DI
				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.LerArquivoDi;
				_controleExecucaoServicoVM.MemoObjetoEnvio = localArquivoFTP;
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.StatusExecucao = 1;
				_controleExecucaoServicoVM.MemoObjetoRetorno = "Tabela: SCIEX_DI_ARQUIVO_ENTRADA, Campo DAR_ID:" + arquivoEntrada.Id;

				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA"; // Verificar
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143"; // Verificar

				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();

				//exclui o arquivo do FTP
				Ftp.DeleteFile(localArquivoFTP, usuario, senha);

				#region RN04
				var li_arquivo_retorno = _uowSciex.QueryStackSciex.DiArquivoEntrada.Listar(o => o.SituacaoLeitura == 0);
				#endregion

				if (li_arquivo_retorno.Count > 0)
				{
					foreach (var item in li_arquivo_retorno)
					{
						int qtdAdicao = 0;
						Int16 qtdDi = 0;
						long IdDiEntrada = 0;

						file = new StreamReader(new MemoryStream(item.DiArquivo.Arquivo));

						while ((linha = file.ReadLine()) != null)
						{
							if (linha.Length > 1)
							{
								string tipoRegistro = linha.Substring(0, 2);

								var entrada = new DiEntradaEntity();

								#region REGRA DE NEGOCIO 04
								switch (tipoRegistro.Trim())
								{
									case "1": //SX_INFO_DI
										{
											entrada.Numero = linha.Substring(2, 10).Trim();
											entrada.QuantidadeAdicao = linha.Substring(12, 3).Trim();
											qtdAdicao = Int32.Parse(entrada.QuantidadeAdicao);
											entrada.DataDesembaraco = linha.Substring(15, 8).Trim();
											entrada.DataRegistro = linha.Substring(23, 8).Trim();
											entrada.CodigoDeclaracao = linha.Substring(31, 2).Trim();
											entrada.Cnpj = linha.Substring(33, 14).Trim();
											entrada.CodigoUrfEntradaCarga = linha.Substring(47, 7).Trim();
											entrada.CodigoUrfDespacho = linha.Substring(54, 7).Trim();
											entrada.CodigoRecintoAlfandega = linha.Substring(61, 7).Trim();
											entrada.CodigoViaTransCarga = linha.Substring(68, 2).Trim();
											entrada.TipoMultimodal = linha.Substring(70, 1).Trim();
											entrada.ValorTotalMleDolar = linha.Substring(71, 15).Trim();
											entrada.ValorTotalMleMn = linha.Substring(86, 15).Trim();
											entrada.CodigoSetorArmazena = linha.Substring(101, 3).Trim();
											entrada.Situacao = (int)EnumStatusProcessamentoDi.AGUARDANDO_PROCESSAMENTO;
											entrada.DataEntrada = GetDateTimeNowUtc();
											entrada.IdDiArquivoEntrada = arquivoEntrada.Id;

											_uowSciex.CommandStackSciex.DiEntrada.Salvar(entrada);
											_uowSciex.CommandStackSciex.Save();

											IdDiEntrada = entrada.Id;

											qtdDi++;

											break;
										}
									case "4": //SX_EMBALAGEM											
										{
											var embalagem = new DiEmbalagemEntradaEntity();
											embalagem.NumeroDi = linha.Substring(2, 10).Trim();
											embalagem.CodigoTipoEmbalagem = linha.Substring(12, 2).Trim();
											embalagem.QuantidadeVolumeCarga = linha.Substring(14, 5).Trim();
											embalagem.IdDiEntrada = IdDiEntrada;

											_uowSciex.CommandStackSciex.DiEmbalagemEntrada.Salvar(embalagem);
											_uowSciex.CommandStackSciex.Save();

											break;
										}
									case "5": //SX_ARMAZEM
										{
											var armazem = new DiArmazemEntradaEntity();
											armazem.Numero = linha.Substring(2, 10).Trim();
											armazem.Descricao = linha.Substring(12, 10).Trim();
											armazem.IdDiEntrada = IdDiEntrada;

											_uowSciex.CommandStackSciex.DiArmazemEntrada.Salvar(armazem);
											_uowSciex.CommandStackSciex.Save();

											break;
										}
									case "10": //SX_ADICAO_DI											
										{
											do
											{
												var adicao = new DiAdicaoEntradaEntity();
												adicao.NumeroDi = linha.Substring(2, 10).Trim();
												adicao.QuantidadeAdicao = linha.Substring(12, 3).Trim();
												adicao.NumeroLi = linha.Substring(15, 10).Trim();
												adicao.CodigoViaTransporte = linha.Substring(25, 2).Trim();
												adicao.TipoMultimodal = linha.Substring(27, 1).Trim();
												adicao.ValorPesoLiquido = linha.Substring(28, 15).Trim();
												adicao.ValorFreteMoedaNegociada = linha.Substring(43, 15).Trim();
												adicao.CodigoMoedaFrete = linha.Substring(58, 3).Trim();
												adicao.ValorFreteDolar = linha.Substring(61, 11).Trim();
												adicao.ValorFrete = linha.Substring(72, 15).Trim();
												adicao.ValorSeguroMoedaNegociada = linha.Substring(87, 15).Trim();
												adicao.MoedaSeguro = linha.Substring(102, 3).Trim();
												adicao.ValorSeguroDolar = linha.Substring(105, 11).Trim();
												adicao.ValorSeguro = linha.Substring(116, 15).Trim();
												adicao.CodigoFundamentoLegal = linha.Substring(131, 2).Trim();
												adicao.ValorMercadoriaDolar = linha.Substring(133, 11).Trim();
												adicao.ValorMercadoriaMoedaNegociada = linha.Substring(144, 15).Trim();
												adicao.Situacao = 1; // 1 - Não Processado 
												adicao.IdDiEntrada = IdDiEntrada;

												_uowSciex.CommandStackSciex.DiAdicaoEntrada.Salvar(adicao);
												_uowSciex.CommandStackSciex.Save();

												qtdAdicao -= 1;

												if (qtdAdicao > 0)
												{
													linha = file.ReadLine();
												}
											} while (qtdAdicao > 0);

											break;
										}
									default:
										break;
								}
								#endregion
							}
						}

						//Salva os arquivo
						file.Close();

						#region Atualiza registro de arquivo de entrada

						arquivoEntrada.SituacaoLeitura = 1;
						arquivoEntrada.QuantidadeDi = qtdDi;

						_uowSciex.CommandStackSciex.DiArquivoEntrada.Salvar(arquivoEntrada);
						_uowSciex.CommandStackSciex.Save();

						#endregion
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
				_controleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.LerArquivoDi;

				//Registra Fim de Execução
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.StatusExecucao = 1;
				_controleExecucaoServicoVM.MemoObjetoRetorno = "NENHUM ARQUIVO DISPONIBILIZADO";
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();
				#endregion
			}
		}

		public string GerarArquivoSimulacaoDI()
		{
			try
			{
				var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == (int)EnumListaServico.SalvarArquivoDiNormal);
			var configuracoesUsuario = configuracoesFTP.ElementAt(1).Valor;
				var configuracoesSenha = configuracoesFTP.ElementAt(2).Valor;

				var listaCnpjs = _uowSciex.CommandStackSciex.ListarCnpjsLisDeferidas();

				if (listaCnpjs == null || listaCnpjs?.Count() == 0)
				{
					return "Sem LI para geração do arquivo";
				}

				StringBuilder arquivo = new StringBuilder();

				var sequencialEntity = _uowSciex.QueryStackSciex.Sequencial.Listar().FirstOrDefault();

				sequencialEntity = (sequencialEntity == null) ? new SequencialEntity() : sequencialEntity;

				var quantidadeInteracao = (listaCnpjs.Count() <= 5) ? listaCnpjs.Count() : 5;

				for (int i = 0; i < quantidadeInteracao; i++)
				{
					sequencialEntity.Sequencial += 1;

					string cnpj = listaCnpjs.ElementAt(i);

					var listaLIEnumerable = _uowSciex.QueryStackSciex.Li.Listar(
						o => o.Status == (int)EnumLiStatus.LI_DEFERIDA &&
						o.PliMercadoria.Pli.Cnpj == cnpj)?.Take(5);

					if (listaLIEnumerable == null) continue;

					List<LiEntity> listaLI = listaLIEnumerable.ToList();

					#region - SX_INFO_DI

					var data = GetDateTimeNowUtc();

					var objetoAlfandegaSetorArmazenamento = GerarCodigoAlfandegaESetorArmazenamento();

					arquivo.Append("1".PadRight(2, ' ')); // CD_TIPO_REGISTRO  
					arquivo.Append("20").Append(sequencialEntity.Sequencial.ToString().PadLeft(8, '0')); // NR_DECLARACAO_IMP ----- 20 + SEQUENCIAL
					arquivo.Append(listaLI.Count().ToString().PadLeft(3, '0')); // QT_ADICAO_DI      
					arquivo.Append(data.ToString("yyyyMMdd")); // DT_REGISTRO_DI    
					arquivo.Append(data.AddDays(-2).ToString("yyyyMMdd")); // DT_DESEMBARACO_DI 
					arquivo.Append(GerarTipoDeclaracaoAleatorio().PadLeft(2, '0')); // CD_TIPO_DECLARACAO ==>> posição 32/33
					arquivo.Append(cnpj); // NR_IMPORTADOR     					
					arquivo.Append(listaLI.FirstOrDefault()?.PliMercadoria?.UnidadeReceitaFederalEntrada?.Codigo.ToString().PadLeft(7, '0')); //"0227700" CD_URF_ENTR_CARGA 
					arquivo.Append(listaLI.FirstOrDefault()?.PliMercadoria?.UnidadeReceitaFederalDespacho?.Codigo.ToString().PadLeft(7, '0')); //"0227700" CD_URF_DESPACHO   					

					#region Recinto Alfandegario 
					if(objetoAlfandegaSetorArmazenamento == null)// CD_RECINTO_ALFANDEGARIO ==>> posição 62/68		
					{
						arquivo.Append("0000000".PadRight(7, ' ')); 
					}
					else
					{
						arquivo.Append(objetoAlfandegaSetorArmazenamento.CodigoRecintoAlfandega.ToString().PadRight(7, ' '));	
					}						
					#endregion

					arquivo.Append(GerarViaTransporteAleatorio().PadLeft(2, '0')); // CD_VIA_TRANSP_CARG ==>> posição 69/70
					arquivo.Append("N"); // IN_MULTIMODAL     					
					arquivo.Append(CalcularTotaValorDolar(listaLI).PadLeft(15, '0')); // VL_TOTAL_MLE_DOLAR
					arquivo.Append(CalcularTotaValorNaMoedaNegociada(listaLI).PadLeft(15, '0')); // VL_TOTAL_MLE_MN  

					#region Setor Armazenamento					
					if (objetoAlfandegaSetorArmazenamento == null)// CD_SETOR_ARMAZENAMENTO ==>> posição 102/104
					{
						arquivo.Append("000".PadRight(3, ' '));
					}
					else
					{
						arquivo.Append(objetoAlfandegaSetorArmazenamento.CodigoSetorArmazenamento.ToString().PadRight(3, ' '));
					}
					#endregion

					arquivo.Append("00000000"); // DT_RETIFICACAO
					arquivo.Append("0000"); // CÓDIGO DESCONHECIDO

					#endregion

					arquivo.AppendLine();

					#region - SX_INFO_DI

					arquivo.Append("4 "); // CD_TIPO_REGISTRO 

					arquivo.Append("20").Append(sequencialEntity.Sequencial.ToString().PadLeft(8, '0')); // NR_DECLARACAO_IMP - Mesmo numero

					arquivo.Append(GeraCodigoTipoEmbalagem().PadRight(2, ' ')); // CD_TIPO_EMBALAGEM

					arquivo.Append(GerarVolumeAleatorio().PadLeft(5, '0')); // QTD_VOLUME_CARGA 

					#endregion

					arquivo.AppendLine();

					#region - SX_ARMAZEM

					arquivo.Append("5 "); // CD_TIPO_REGISTRO 
					arquivo.Append("20").Append(sequencialEntity.Sequencial.ToString().PadLeft(8, '0')); // NR_DECLARACAO_IMP - Mesmo numero
					arquivo.Append("TECA II".PadRight(10, ' ')); // NM_ARMAZEM_CARGA

					#endregion

					arquivo.AppendLine();

					#region - SX_ADICAO_DI

					foreach (LiEntity item in listaLI)
					{
						arquivo.Append("10"); // CD_TIPO_REGISTRO  
						arquivo.Append("20").Append(sequencialEntity.Sequencial.ToString().PadLeft(8, '0')); // NR_DECLARACAO_IMP 
						arquivo.Append(listaLI.Count().ToString().PadLeft(3, '0')); // NR_ADICAO_USUARIO 
						arquivo.Append(item.NumeroLi.ToString().PadLeft(10, '0')); // NR_OPER_TRAT_PREV 
						arquivo.Append(GerarViaTransporteAleatorio().PadLeft(2, '0')); // CD_VIA_TRANSPORTE ==>> posição 26/27
						arquivo.Append("N"); // IN_MULTIMODAL     						
						arquivo.Append(item.PliMercadoria.PesoLiquido?.ToString().Replace(",", "").PadLeft(15, '0')); // PL_MERCADORIA     
						arquivo.Append(GerarValorRandom().PadLeft(15, '0')); // VL_FRETE_MERC_MNEG
						arquivo.Append(GerarCodigoMoeda().PadLeft(3, '0')); // CD_MD_FRETE_MERC  

						arquivo.Append(GerarValorRandom().PadLeft(11, '0')); // VL_FRT_MERC_DOLAR 
						arquivo.Append(GerarValorRandom().PadLeft(15, '0')); // VL_FRETE_MERC_MN  
						arquivo.Append(GerarValorRandom().PadLeft(15, '0')); // VL_SEG_MERC_MNEG  
						arquivo.Append(GerarCodigoMoeda().PadLeft(3, '0')); // CD_MOEDA_SEG_MERC 
						arquivo.Append(GerarValorRandom().PadLeft(11, '0')); // VL_SEG_MERC_DOLAR 
						arquivo.Append(GerarValorRandom().PadLeft(15, '0')); // VL_SEG_MERC_MN    
						arquivo.Append(item.PliMercadoria.FundamentoLegal.Codigo.ToString().PadLeft(2, '0')); // CD_FUND_LEG_REGIME
						arquivo.Append(item.PliMercadoria.ValorTotalCondicaoVendaDolar.ToString().Replace(",", "").PadLeft(11, '0')); // VL_MERC_EMB_DOLAR 
						arquivo.Append(item.PliMercadoria.ValorTotalCondicaoVenda.ToString().Replace(",", "").PadLeft(15, '0')); // VL_MERC_EMB_MN    


						arquivo.AppendLine();
					}

					#endregion
				}

				if (!Ftp.VerificarSeExisteArquivo(configuracoesFTP.FirstOrDefault().Valor, configuracoesUsuario, configuracoesSenha))
				{
					Ftp.EnviarArquivo(configuracoesFTP.FirstOrDefault().Valor, configuracoesUsuario, configuracoesSenha, arquivo.ToString());

					_uowSciex.CommandStackSciex.Sequencial.Salvar(sequencialEntity);
					_uowSciex.CommandStackSciex.Save();

					return "Arquivo enviado com sucesso.";
				}
				else
				{
					return "Arquivo existente na pasta no momento do envio";
				}
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}

		}

		private string GerarVolumeAleatorio()
		{
			Random numAleatorio = new Random();

			var valorInteiro = numAleatorio.Next(1, 99999);

			return valorInteiro.ToString();
		}

		private string GerarCodigoMoeda()
		{
			var lista = _uowSciex.QueryStackSciex.Moeda.Listar();

			var codigos = new List<short>();

			foreach (var moeda in lista)
			{
				codigos.Add(moeda.CodigoMoeda);
			}

			#region - Randon codigos
			var count = codigos.Count();
			var rand = new System.Random();

			var randomLista = codigos.Skip(rand.Next(count));
			#endregion

			return randomLista.FirstOrDefault().ToString();
		}

		private string GerarValorRandom()
		{
			Random numAleatorio = new Random();

			var valorInteiro = numAleatorio.Next(1000, 10000);

			return valorInteiro.ToString();
		}

		private string CalcularTotaValorDolar(List<LiEntity> listaLI)
		{
			decimal? total = 0;

			foreach (var li in listaLI)
			{
				total += (li.PliMercadoria?.ValorTotalCondicaoVendaDolar != null) ? li.PliMercadoria?.ValorTotalCondicaoVendaDolar : 0;
			}

			return total.ToString()?.Replace(",", "");
		}

		private string CalcularTotaValorNaMoedaNegociada(List<LiEntity> listaLI)
		{
			decimal? total = 0;

			foreach (var li in listaLI)
			{
				total += (li.PliMercadoria?.ValorTotalCondicaoVenda != null) ? li.PliMercadoria?.ValorTotalCondicaoVenda : 0;
			}

			return total.ToString()?.Replace(",", "");
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public IEnumerable<object> ListarLiAdicoes(DiVM diVM)
		{
			var numeroDi = Convert.ToInt64(diVM.NumeroDi);
			var DI = _uowSciex.QueryStackSciex.Di.Selecionar(o => o.NumeroDi == numeroDi);

			var LisDi = _uowSciex.QueryStackSciex.DiLi.Listar(o => o.IdDi == DI.IdDi);

			List<LiEntity> lisAplicacao = new List<LiEntity>();
			foreach (var item in LisDi)
			{
				var li = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == item.NumeroLi);
				var pliMerc = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == li.IdPliMercadoria);
				if (pliMerc.Pli.IdPLIAplicacao == diVM.IdPliAplicacao)
					lisAplicacao.Add(li);
			}

			return lisAplicacao.Select(
					s => new
					{
						id = s.IdPliMercadoria,
						text = s.NumeroLi
					}); ;
		}

		private string GerarViaTransporteAleatorio()
		{
			Random numAleatorio = new Random();

			var qtdRegistros = _uowSciex.QueryStackSciex.ViaTransporte.Listar().Count;

			var valorAleatorio = numAleatorio.Next(1, qtdRegistros);

			var codigoViaTransporte = _uowSciex.QueryStackSciex.ViaTransporte.Selecionar(s => s.IdViaTransporte == valorAleatorio).Codigo;

			if (codigoViaTransporte.ToString().Length > 2)
			{
				GerarViaTransporteAleatorio();
			}

			return codigoViaTransporte.ToString();
		}
		
		private string GerarTipoDeclaracaoAleatorio()
		{
			// Regra 1 = 80% para o codigo = 6  - ADMISSAO NA ZFM - ZONA FRANCA DE MANAUS
			// Regra 2 = 10% para o codigo = 15 - SAIDA DE EIZOF - ENTREPOSTO INTERNACIONAL DA ZFM
			// Regra 3 = 5%  codigo = 8  - ADMISSAO EM ALC - AREA DE LIVRE COMERCIO
			// Regra 4 = 5%  outros códigos que existam na tabela ==>> SCIEX_TIPO_DECLARACAO

			short codigoTipoDeclaracao = 0;

			Random numAleatorio = new Random();

			var valorAleatorio = numAleatorio.Next(1, 100);

			if (valorAleatorio <= 80)
			{
				codigoTipoDeclaracao = 6;
			}
			else if (valorAleatorio > 80 && valorAleatorio <= 90)
			{
				codigoTipoDeclaracao = 15;
			}
			else if (valorAleatorio > 90 && valorAleatorio <= 95)
			{
				codigoTipoDeclaracao = 8;
			}
			else
			{
				if (valorAleatorio != 6 || valorAleatorio != 8 || valorAleatorio != 15)
				{
					codigoTipoDeclaracao = _uowSciex.QueryStackSciex.TipoDeclaracao.Selecionar(s => s.IdTipoDeclaracao == valorAleatorio).Codigo;
				}
			}

			return codigoTipoDeclaracao.ToString();
		}

		private string GeraRecintoAlfandegario()
		{
			Random rnd = new Random();
			var lista = _uowSciex.QueryStackSciex.RecintoAlfandega.Listar(o => o.Id > 0);

			if(lista.Count == 0) { return "0000000"; }

			var valorAleatorio = lista[rnd.Next(lista.Count)];
			var retorno = valorAleatorio.Codigo.ToString("D7");
			return retorno;
		}

		private string GeraCodigoSetorArmazenamento()
		{
			Random rnd = new Random();
			var lista = _uowSciex.QueryStackSciex.SetorArmazenamento.Listar(o => o.Id > 0);

			if (lista.Count == 0) { return "000"; }

			var valorAleatorio = lista[rnd.Next(lista.Count)];
			var retorno = valorAleatorio.Codigo.ToString("D3");
			return retorno;
		}

		private string GeraCodigoTipoEmbalagem()
		{
			Random rnd = new Random();
			var lista = _uowSciex.QueryStackSciex.TipoEmbalagem.Listar();

			if (lista.Count == 0) { return "00"; }

			var valorAleatorio = lista[rnd.Next(lista.Count)];
			var retorno = valorAleatorio.Codigo.ToString("D2");
			return retorno;
		}

		private string gerarQueryConsultarSetorArmazenamento(int Codigo)
		{
			StringBuilder query = new StringBuilder();
			query.AppendLine(" SELECT ");
			query.AppendLine(" SAR_ID as Id, ");
			query.AppendLine(" SAR_DS as Descricao, ");
			query.AppendLine(" SAR_CO as Codigo ");	
			query.AppendLine(" FROM SCIEX_DI ");
			query.AppendLine(" inner join SCIEX_SETOR_ARMAZENAMENTO on di_co_setor_armazenamento = SCIEX_SETOR_ARMAZENAMENTO.sar_co ");
			query.AppendLine(" inner join SCIEX_RECINTO_ALFANDEGA on di_co_recinto_alfandega = SCIEX_RECINTO_ALFANDEGA.ral_co ");
			query.AppendLine(" and SCIEX_RECINTO_ALFANDEGA.ral_id = SCIEX_SETOR_ARMAZENAMENTO.ral_id ");
			query.AppendLine(" and ral_co  = " + Codigo);
			return query.ToString();
		}

		private string gerarQueryConsultarEmbalagemGrid(long? id)
		{
			StringBuilder query = new StringBuilder();
			query.AppendLine(" select tem_ds as Descricao, ");
			query.AppendLine(" dem_co_tipo_embalagem as CodigoTipoEmbalagem, ");
			query.AppendLine(" dem_qt_volume_carga QuantidadeVolumeCarga ");
			query.AppendLine(" from SCIEX_DI_EMBALAGEM ");
			query.AppendLine(" inner join SCIEX_TIPO_EMBALAGEM on dem_co_tipo_embalagem = tem_co ");
			query.AppendLine(" where di_id = " + id);
			return query.ToString();			
		}

		private CodigoDescricaoAlfandegaSetorArmazenamentoVM GerarCodigoAlfandegaESetorArmazenamento()
		{
			StringBuilder query = new StringBuilder();
			query.AppendLine(" select ");
			query.AppendLine(" sar_co as CodigoSetorArmazenamento, ");
			query.AppendLine(" ral_co as CodigoRecintoAlfandega ");
			query.AppendLine(" from ");
			query.AppendLine(" SCIEX_SETOR_ARMAZENAMENTO ");
			query.AppendLine(" inner join ");
			query.AppendLine(" SCIEX_RECINTO_ALFANDEGA on SCIEX_SETOR_ARMAZENAMENTO.ral_id = SCIEX_RECINTO_ALFANDEGA.ral_id ");

			var ListaSetorArmazenamento = _uowSciex.QueryStackSciex.ListarPaginadoSql<CodigoDescricaoAlfandegaSetorArmazenamentoVM>(query.ToString(), new CodigoDescricaoAlfandegaSetorArmazenamentoVM { Page = 1, Size = 1000000});

			if(ListaSetorArmazenamento.Total == 0) { return null; }

			Random rnd = new Random();
			var valorAleatorio = ListaSetorArmazenamento.Items[rnd.Next((int)ListaSetorArmazenamento.Total)];

			return new CodigoDescricaoAlfandegaSetorArmazenamentoVM {
				CodigoRecintoAlfandega = valorAleatorio.CodigoRecintoAlfandega,
				CodigoSetorArmazenamento = valorAleatorio.CodigoSetorArmazenamento
			};
		}

	}
}