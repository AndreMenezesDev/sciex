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
using System.Linq.Expressions;

namespace Suframa.Sciex.BusinessLogic
{
	public class ProrrogarSolicitacaoBll : IProrrogarSolicitacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public ProrrogarSolicitacaoBll(
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


		public ResultadoMensagemProcessamentoVM CriarSolicitacao(PRCSolicitacaoAlteracaoVM vm)
		{
			if (vm.IdProcesso == 0) { return new ResultadoMensagemProcessamentoVM() { Resultado = false, Mensagem = "Id Processo não pode ser nulo." }; }

			var empresaLogada = _usuarioPssBll.ObterUsuarioLogado();
			var anoCorrente = DateTime.Now.Year;

			var cnpj = _uowSciex.QueryStackSciex.Processo.Selecionar(q => q.IdProcesso == vm.IdProcesso).Cnpj;

			var regSolicAlteracao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Listar(q => q.Cnpj.Equals(cnpj)
																									&&
																									q.AnoSolicitacao == anoCorrente
																							)?.LastOrDefault() ?? null;


			if (regSolicAlteracao == null)
			{
				var novaSolic = new PRCSolicitacaoAlteracaoEntity()
				{
					NumeroSolicitacao = 1,
					AnoSolicitacao = anoCorrente,
					RazaoSocial = empresaLogada.empresaRepresentadaRazaoSocial,
					Cnpj = cnpj,
					Status = (int)EnumStatusSolicitacaoAlteracao.EmElaboracao,
					DataInclusao = DateTime.Now.Date,
					IdProcesso = vm.IdProcesso
				};

				_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(novaSolic);
				_uowSciex.CommandStackSciex.Save();

			}
			else
			{
				var novaSolic = new PRCSolicitacaoAlteracaoEntity()
				{
					NumeroSolicitacao = regSolicAlteracao.NumeroSolicitacao + 1,
					AnoSolicitacao = anoCorrente,
					RazaoSocial = empresaLogada.usuNomeEmpresaOuLogado,
					Cnpj = cnpj,
					Status = (int)EnumStatusSolicitacaoAlteracao.EmElaboracao,
					DataInclusao = GetDateTimeNowUtc(),
					IdProcesso = vm.IdProcesso
				};

				_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(novaSolic);
				_uowSciex.CommandStackSciex.Save();
			}

			return new ResultadoMensagemProcessamentoVM() { Resultado = true };
		}

		public PRCSolicitacaoAlteracaoVM SelecionarSolicitacao(int idProcesso)
		{
			if (idProcesso == 0) { return null; }

			var dadosSolicitacao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.SelecionarGrafo(q => new PRCSolicitacaoAlteracaoVM()
			{
				Id = q.Id,
				NumeroSolicitacao = q.NumeroSolicitacao,
				AnoSolicitacao = q.AnoSolicitacao,
				Status = q.Status,
				DataInclusao = q.DataInclusao,
				CpfResponsavel = q.CpfResponsavel,
				NomeResponsavel = q.NomeResponsavel,
				IdProcesso = q.IdProcesso,
				DataAlteracao = q.DataAlteracao,
				Cnpj = q.Cnpj,
				RazaoSocial = q.RazaoSocial
			}
								,
								q =>
								q.IdProcesso == idProcesso
								&&
								q.Status == (int)EnumStatusSolicitacaoAlteracao.EmElaboracao
								);

			if (dadosSolicitacao != null)
			{
				return dadosSolicitacao;
			}
			else
			{
				return null;
			}

		}

		public bool ExcluirSolicitacao(int id)
		{
			if (id == 0) return false;

			var regSolicitacao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(q => q.Id == id);

			var solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdSolicitacaoAlteracao == id);

			if (solicDetalhe.Count > 0)
			{
				foreach (var item in solicDetalhe)
				{
					_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(item.Id);
				}
			}

			if (regSolicitacao != null)
			{
				_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Apagar(regSolicitacao.Id);
				_uowSciex.CommandStackSciex.Save();
			}

			return true;
		}

		public ResultadoMensagemProcessamentoVM EntregarSolicitacao(PRCSolicitacaoAlteracaoVM vm)
		{
			if (vm == null || vm.IdProcesso == 0)
			{
				return new ResultadoMensagemProcessamentoVM() { Resultado = false, Mensagem = "Id Processo não pode ser nulo." };
			}

			try
			{
				var regSolicAlteracao = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(q => q.Id == vm.Id);

				regSolicAlteracao.Status = (int)EnumStatusSolicitacaoAlteracao.EmAnalise;
				regSolicAlteracao.DataAlteracao = DateTime.Now.Date;

				_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(regSolicAlteracao);

				var listaRegSoliciDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(q => q.IdSolicitacaoAlteracao == vm.Id);

				if (listaRegSoliciDetalhe.Count > 0)
				{
					regSolicAlteracao.Status = (int)EnumStatusSolicitacaoAlteracao.Entregue;
					regSolicAlteracao.DataAlteracao = DateTime.Now.Date;

					_uowSciex.CommandStackSciex.PRCSolicitacaoAlteracao.Salvar(regSolicAlteracao);

					foreach (var regSolicDetalhe in listaRegSoliciDetalhe)
					{
						regSolicDetalhe.Status = (int)EnumStatusSolicitacaoDetalheAlteracao.EM_ANALISE;

						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(regSolicDetalhe);
					}

					_uowSciex.CommandStackSciex.Save();
				}
				else
				{
					return new ResultadoMensagemProcessamentoVM()
					{
						Resultado = false,
						Mensagem = "Esta solicitação não possui nenhuma alteração cadastrada. Para entregar a solicitação deve ser cadastrado ao menos 01(uma) alteração."
					};
				}
			}
			catch (Exception e)
			{
				return new ResultadoMensagemProcessamentoVM()
				{
					Resultado = false,
					Mensagem = $@"Mensagem: {e.Message} / InnerException: {e.InnerException} / StackTrace: {e.StackTrace}"
				};
			}

			return new ResultadoMensagemProcessamentoVM() { Resultado = true };
		}

		public ProrrogarSolicitacaoVM SalvarProrrogacao(ProrrogarSolicitacaoVM view)
		{

			var regProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(q => q.IdProcesso == view.IdProcesso);

			try
			{
				if (!view.JaPossuiProrrogacao)
				{
					regProcesso.DataValidade = ((DateTime)(regProcesso.DataValidade)).AddDays(180);

					var novoStatus = new PRCStatusEntity()
					{
						Data = GetDateTimeNowUtc(),
						Tipo = "PR",
						IdProcesso = regProcesso.IdProcesso,
						DataValidade = regProcesso.DataValidade
					};


					_uowSciex.CommandStackSciex.Processo.Salvar(regProcesso);
					_uowSciex.CommandStackSciex.PRCStatus.Salvar(novoStatus);

					_uowSciex.CommandStackSciex.Save();

					NotificarEmailBll email = new NotificarEmailBll();
					email.EnviarEmail(novoStatus.Tipo, regProcesso.RazaoSocial, ((int)(regProcesso.NumeroProcesso)).ToString("D4") + "/" + regProcesso.AnoProcesso.ToString(), regProcesso.DataValidade.ToString(), _uowSciex);

					return new ProrrogarSolicitacaoVM() { Sucesso = true };
				}
				else
				{
					var regNovaSolicProrrogacao = new PRCSolicProrrogacaoEntity()
					{
						Data = GetDateTimeNowUtc(),
						Status = (int)EnumSolicitacaoProrrogacao.EM_ANALISE,
						Justificativa = view.Justificativa,
						IdProcesso = (int)view.IdProcesso
					};

					_uowSciex.CommandStackSciex.ProcessoSolicitacaoProrrogacao.Salvar(regNovaSolicProrrogacao);
					_uowSciex.CommandStackSciex.Save();

					return new ProrrogarSolicitacaoVM() { Sucesso = true };
				}
			}
			catch (Exception e)
			{
				return new ProrrogarSolicitacaoVM() { Sucesso = true , MensagemErro = e.Message};
			}

		}
	}
}