using HandlebarsDotNet;
using NLog;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Resources;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.BusinessLogic
{
	public class NotificacaoBll : INotificacaoBll
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();
		private readonly IControleExecucaoServicoBll _controleExecucaoServicoBll;
		private readonly IUnitOfWork _uow;

		public NotificacaoBll(IUnitOfWork uow,
			IControleExecucaoServicoBll controleExecucaoServicoBll)
		{
			_uow = uow;
			_controleExecucaoServicoBll = controleExecucaoServicoBll;
		}

		private void AtualizarDataNotificacaoWorkflowProtocolo(NotificacaoVM notificacao)
		{
			var workflowProtocolo = _uow.QueryStack.WorkflowProtocolo.Selecionar(x => x.IdWorkflowProtocolo == notificacao.IdWorkflowProtocolo);

			workflowProtocolo.DataNotificacao = DateTime.Now;

			_uow.CommandStack.DetachEntries();
			_uow.CommandStack.WorkflowProtocolo.Salvar(workflowProtocolo);
			_uow.CommandStack.Save();
		}

		private string GerarNotificacao(NotificacaoVM notificacaoVM)
		{
			Handlebars.RegisterTemplate("style", Resources.style);
			Handlebars.RegisterTemplate("notificacaoCabecalho", Resources.notificacaoCabecalho);

			var templateResource = SelecionarTemplate(notificacaoVM);

			var template = Handlebars.Compile(templateResource);

			return template(notificacaoVM);
		}

		private void SalvarExecucaoServico(NotificacaoVM notificacaoVM, EnumStatusControleExecucaoServico status)
		{
			
		}

		private string SelecionarTemplate(NotificacaoVM notificacaoVM)
		{
			if (notificacaoVM == null) { return null; }

			if (notificacaoVM.IsAtualizacaoDadosCadastraisCredenciamento)
			{
				return Resources.atualizacaoDadosCadastrais;
			}

			if (notificacaoVM.IsCredenciamento)
			{
				return Resources.credenciamento;
			}

			if (notificacaoVM.IsCancelamentoInscricao)
			{
				return Resources.cancelamento;
			}

			switch (notificacaoVM.StatusProtocolo)
			{
				case EnumStatusProtocolo.Deferido:
					return SelecionarTemplateDeferido(notificacaoVM);

				case EnumStatusProtocolo.Indeferido:
					// Tipo Indeferimento = > Notifica com [Figura 3]
					return Resources.indeferimento;

				case EnumStatusProtocolo.ComPendencia:
					// Tipo Correção Informação do Requerimento = > Notifica com [Figura 1]
					return Resources.correcaoInformacaoRequerimento;

				case EnumStatusProtocolo.AguardandoConferenciaDocumental:
					// Tipo Conferência Administrativa = > Notifica com [Figura 2]
					return Resources.conferenciaAdministrativa;

				case EnumStatusProtocolo.IndeferidoAguardandoRecurso:
					return Resources.indeferimento;

				case EnumStatusProtocolo.Cancelado:
					return Resources.cancelamento;
			}

			return null;
		}

		private string SelecionarTemplateDeferido(NotificacaoVM notificacaoVM)
		{
			switch (notificacaoVM.TipoRequerimento)
			{
				// Tipo Atualização de Dados = > Notifica com[Figura 6]
				case EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaConsultor:
				case EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaFisicaPreposto:
				case EnumTipoRequerimento.AtualizacaoCredenciamentoPessoaJuridicaAuditor:
				case EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaFisica:
				case EnumTipoRequerimento.AtualizacaoInscricaoCadastralPessoaJuridica:
					return Resources.deferimentoAtualizacaoDados;

				// Tipo Atualização de Documentos = > Notifica com [Figura 7]
				case EnumTipoRequerimento.AtualizacaoDocumentosPessoaFisica:
				case EnumTipoRequerimento.AtualizacaoDocumentosPessoaJuridica:
					return Resources.deferimentoAtualizacaoDocumentos;

				// Tipo Reativação = > Notifica com [Figura 8]
				case EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaFisica:
				case EnumTipoRequerimento.ReativacaoInscricaoCadastralPessoaJuridica:
					return Resources.deferimentoReativacao;

				// Tipo Inscrição Cadastral = > Notifica com [Figura 4]
				case EnumTipoRequerimento.InscricaoCadastralPessoaFisica:
				case EnumTipoRequerimento.InscricaoCadastralPessoaJuridica:
					return Resources.deferimentoInscricaoCadastral;

				// Tipo Credenciamento = > Notifica com [Figura 5]
				case EnumTipoRequerimento.CredenciamentoPessoaFisicaConsultor:
				case EnumTipoRequerimento.CredenciamentoPessoaFisicaPreposto:
				case EnumTipoRequerimento.CredenciamentoPessoaJuridicaAuditor:
					return Resources.deferimentoCredenciamento;
			}

			return null;
		}

		public void Enviar(NotificacaoVM notificacao)
		{
			var notificacaoVM = notificacao ?? new NotificacaoVM();

			if (notificacao.IdWorkflowProtocolo.HasValue)
			{
				notificacaoVM = _uow.QueryStack.WorkflowProtocolo.Selecionar<NotificacaoVM>(x => x.IdWorkflowProtocolo == notificacao.IdWorkflowProtocolo);
			}

			if (notificacao.IdRequerimento.HasValue)
			{
				notificacaoVM = _uow.QueryStack.Requerimento.Selecionar<NotificacaoVM>(x => x.IdRequerimento == notificacao.IdRequerimento);
			}

			notificacaoVM.MensagemPadrao = notificacao.MensagemPadrao ?? null;

			if ((notificacaoVM.StatusProtocolo == EnumStatusProtocolo.IndeferidoAguardandoRecurso && notificacaoVM.Servico.ToUpper() == "CANCELAMENTO") ||
				notificacaoVM.IsCancelamentoInscricaoCadastral)
			{
				notificacaoVM.StatusProtocolo = EnumStatusProtocolo.Cancelado;
			}

			try
			{
				//TODO verificar porque foi colocado um enviado sem sucesso ou erro?
				//SalvarExecucaoServico(notificacaoVM, EnumStatusControleExecucaoServico.ServicoEnviado);

				var body = GerarNotificacao(notificacaoVM);

				Email.Enviar(body, "Notificação – Sistema de Cadastro SUFRAMA", notificacaoVM.Email);

				if (notificacao.IdWorkflowProtocolo.HasValue)
				{
					AtualizarDataNotificacaoWorkflowProtocolo(notificacaoVM);
				}

				SalvarExecucaoServico(notificacaoVM, EnumStatusControleExecucaoServico.RetornadoComSucesso);
			}
			catch (Exception e)
			{
				logger.Debug("Erro ao notificar: " + e.Message);
				SalvarExecucaoServico(notificacaoVM, EnumStatusControleExecucaoServico.RetornadoComErro);
			}
		}
	}
}