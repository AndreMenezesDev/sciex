using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.DataAccess
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
	/// </summary>
	public class QueryStack : IQueryStack
	{
		private readonly IDatabaseContext context;

		public IQueryRepository<AgendaAtendimentoEntity> AgendaAtendimento { get; }
		public IQueryRepository<ArquivoEntity> Arquivo { get; }
		public IQueryRepository<ArquivoInformacaoEntity> ArquivoInformacao { get; }
		public IQueryRepository<CalendarioEntity> Calendario { get; }
		public IQueryRepository<CalendarioAgendamentoEntity> CalendarioAgendamento { get; }
		public IQueryRepository<CalendarioDiaEntity> CalendarioDia { get; }
		public IQueryRepository<CalendarioHoraEntity> CalendarioHora { get; }
		public IQueryRepository<CampoSistemaEntity> CampoSistema { get; }
		public IQueryRepository<CepEntity> Cep { get; }
		public IQueryRepository<ClasseAtividadeEntity> ClasseAtividade { get; }
		public IQueryRepository<ConferenciaDocumentoEntity> ConferenciaDocumento { get; }
		public IQueryRepository<ConsultaPublicaEntity> ConsultaPublica { get; }
		public IQueryRepository<ControleExecucaoServicoEntity> ControleExecucaoServico { get; }
		public IQueryRepository<CredenciamentoEntity> Credenciamento { get; }
		public IQueryRepository<DadosSolicitanteEntity> DadosSolicitante { get; }
		public IQueryRepository<DicionarioDropDownEntity> DicionarioDropDown { get; }
		public IQueryRepository<DiligenciaEntity> Diligencia { get; }
		public IQueryRepository<DiligenciaAnexosEntity> DiligenciaAnexos { get; }
		public IQueryRepository<DiligenciaAtividadesEntity> DiligenciaAtividades { get; }
		public IQueryRepository<DiligenciaAtividadesSetorEntity> DiligenciaAtividadesSetor { get; }
		public IQueryRepository<DivisaoAtividadeEntity> DivisaoAtividade { get; }
		public IQueryRepository<FeriadoEntity> Feriado { get; }
		public IQueryRepository<FilaAnalistaEntity> FilaAnalista { get; }
		public IQueryRepository<GrupoAtividadeEntity> GrupoAtividade { get; }
		public IQueryRepository<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; }
		public IQueryRepository<InscricaoCadastralEntity> InscricaoCadastral { get; }
		public IQueryRepository<InscricaoCadastralCredenciamentoEntity> InscricaoCadastralCredenciamento { get; }
		public IQueryRepository<InscricaoSuframaLegadoEntity> InscricaoSuframaLegado { get; }
		public IQueryRepository<JustificativaProtocoloEntity> JustificativaProtocolo { get; }
		public IQueryRepository<ListaDocumentoEntity> ListaDocumento { get; }
		public IQueryRepository<ListaServicoEntity> ListaServico { get; }
		public IQueryRepository<MensagemPadraoEntity> MensagemPadrao { get; }
		public IQueryRepository<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; }
		public IQueryRepository<MunicipioEntity> Municipio { get; }
		public IQueryRepository<NaturezaGrupoEntity> NaturezaGrupo { get; }
		public IQueryRepository<NaturezaJuridicaEntity> NaturezaJuridica { get; }
		public IQueryRepository<NaturezaQualificacaoEntity> NaturezaQualificacao { get; }
		public IQueryRepository<PaisEntity> Pais { get; }
		public IQueryRepository<PapelEntity> Papel { get; }
		public IQueryRepository<ParametroAnalistaEntity> ParametroAnalista { get; }
		public IQueryRepository<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; }
		public IQueryRepository<ParametroDistribuicaoAutomaticaEntity> ParametroDistribuicaoAutomatica { get; }
		public IQueryRepository<PedidoCorrecaoEntity> PedidoCorrecao { get; }
		public IQueryRepository<PessoaFisicaEntity> PessoaFisica { get; }
		public IQueryRepository<PessoaFisicaAprovadoEntity> PessoaFisicaAprovado { get; }
		public IQueryRepository<PessoaJuridicaEntity> PessoaJuridica { get; }
		public IQueryRepository<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrativa { get; }
		public IQueryRepository<PessoaJuridicaAprovadoEntity> PessoaJuridicaAprovado { get; }
		public IQueryRepository<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; }
		public IQueryRepository<PessoaJuridicaInscricaoEstadualEntity> PessoaJuridicaInscricaoEstadual { get; }
		public IQueryRepository<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; }
		public IQueryRepository<PorteEmpresaEntity> PorteEmpresa { get; }
		public IQueryRepository<ProtocoloEntity> Protocolo { get; }
		public IQueryRepository<QuadroPessoalEntity> QuadroPessoal { get; }
		public IQueryRepository<QualificacaoEntity> Qualificacao { get; }
		public IQueryRepository<RecursoEntity> Recurso { get; }
		public IQueryRepository<RequerimentoEntity> Requerimento { get; }
		public IQueryRepository<RequerimentoDocumentoEntity> RequerimentoDocumento { get; }
		public IQueryRepository<SecaoAtividadeEntity> SecaoAtividade { get; }
		public IQueryRepository<SeqInscricaoCadastralEntity> SeqInscricaoCadastral { get; }
		public IQueryRepository<ServicoEntity> Servico { get; }
		public IQueryRepository<SetorEntity> Setor { get; }
		public IQueryRepository<SetorAtividadeEntity> SetorAtividade { get; }
		public IQueryRepository<SituacaoInscricaoEntity> SituacaoInscricao { get; }
		public IQueryRepository<StatusProtocoloEntity> StatusProtocolo { get; }
		public IQueryRepository<SubclasseAtividadeEntity> SubclasseAtividade { get; }
		public IQueryRepository<TaxaServicoEntity> TaxaServico { get; }
		public IQueryRepository<TipoConsultaPublicaEntity> TipoConsultaPublica { get; }
		public IQueryRepository<TipoDocumentoEntity> TipoDocumento { get; }
		public IQueryRepository<TipoIncentivoEntity> TipoIncentivo { get; }
		public IQueryRepository<TipoRequerimentoEntity> TipoRequerimento { get; }
		public IQueryRepository<TipoUsuarioEntity> TipoUsuario { get; }
		public IQueryRepository<UFEntity> UF { get; }
		public IQueryRepository<UnidadeCadastradoraEntity> UnidadeCadastradora { get; }
		public IQueryRepository<UnidadeSecundariaEntity> UnidadeSecundaria { get; }
		public IQueryRepository<UsuarioInternoEntity> UsuarioInterno { get; }
		public IQueryRepository<UsuarioPapelEntity> UsuarioPapel { get; }
		public IQueryRepository<VWDocumentoEntity> VWDocumento { get; }
		public IQueryRepository<VWPessoaFisicaAprovadoEntity> VWPessoaFisicaAprovado { get; }
		public IQueryRepository<VWPessoaJuridicaAdministradorAprovadoEntity> VWPessoaJuridicaAdministradorAprovado { get; }
		public IQueryRepository<VWPessoaJuridicaAprovadoEntity> VWPessoaJuridicaAprovado { get; }
		public IQueryRepository<VWPessoaJuridicaAtividadeAprovadoEntity> VWPessoaJuridicaAtividadeAprovado { get; }
		public IQueryRepository<VWPessoaJuridicaInscricaoEstadualAprovadoEntity> VWPessoaJuridicaInscricaoEstadualAprovado { get; }
		public IQueryRepository<VWPessoaJuridicaSocioAprovadoEntity> VWPessoaJuridicaSocioAprovado { get; }
		public IQueryRepository<VWQuadroPessoalAprovadoEntity> VWQuadroPessoalAprovado { get; }
		public IQueryRepository<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; }
		public IQueryRepository<WorkflowProtocoloEntity> WorkflowProtocolo { get; }
		public IQueryRepository<WorkflowSituacaoInscricaoEntity> WorkflowSituacaoInscricao { get; }
		public IQueryRepository<AladiEntity> Aladi { get; }
		public IQueryRepository<NaladiEntity> Naladi { get; }
		public IQueryRepository<RegimeTributarioEntity> RegimeTributario { get; }
		public IQueryRepository<RegimeTributarioTesteEntity> RegimeTributarioTeste { get; }
		public IQueryRepository<UnidadeReceitaFederalEntity> UnidadeReceitaFederal { get; }
		public IQueryRepository<FundamentoLegalEntity> FundamentoLegal { get; }
		public IQueryRepository<AnalistaEntity> Analista { get; }
		public IQueryRepository<ParametroAnalista1Entity> ParametroAnalista1 { get; }
		public IQueryRepository<PaizEntity> Paiz { get; }
		public IQueryRepository<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; }
		public IQueryRepository<ViewSetorEmpresaEntity> ViewSetorEmpresa { get; }
		public IQueryRepository<ViewSetorEntity> ViewSetor { get; }
		
		

		public IQueryRepository<ViewUnidadeMedidaEntity> ViewUnidadeMedida { get; }
		public IQueryRepository<ViewDetalheMercadoriaEntity> ViewDetalheMercadoria { get; }
		public IQueryRepository<ViewMunicipioEntity> ViewMunicipio { get; }

		public QueryStack(IDatabaseContext databaseContext)
		{
			context = databaseContext;

			AgendaAtendimento = new QueryRepository<AgendaAtendimentoEntity>(context);
			Arquivo = new QueryRepository<ArquivoEntity>(context);
			ArquivoInformacao = new QueryRepository<ArquivoInformacaoEntity>(context);
			Calendario = new QueryRepository<CalendarioEntity>(context);
			CalendarioAgendamento = new QueryRepository<CalendarioAgendamentoEntity>(context);
			CalendarioDia = new QueryRepository<CalendarioDiaEntity>(context);
			CalendarioHora = new QueryRepository<CalendarioHoraEntity>(context);
			CampoSistema = new QueryRepository<CampoSistemaEntity>(context);
			Cep = new QueryRepository<CepEntity>(context);
			ClasseAtividade = new QueryRepository<ClasseAtividadeEntity>(context);
			ConferenciaDocumento = new QueryRepository<ConferenciaDocumentoEntity>(context);
			ConsultaPublica = new QueryRepository<ConsultaPublicaEntity>(context);
			ControleExecucaoServico = new QueryRepository<ControleExecucaoServicoEntity>(context);
			Credenciamento = new QueryRepository<CredenciamentoEntity>(context);
			DadosSolicitante = new QueryRepository<DadosSolicitanteEntity>(context);
			DicionarioDropDown = new QueryRepository<DicionarioDropDownEntity>(context);
			Diligencia = new QueryRepository<DiligenciaEntity>(context);
			DiligenciaAnexos = new QueryRepository<DiligenciaAnexosEntity>(context);
			DiligenciaAtividades = new QueryRepository<DiligenciaAtividadesEntity>(context);
			DiligenciaAtividadesSetor = new QueryRepository<DiligenciaAtividadesSetorEntity>(context);
			DivisaoAtividade = new QueryRepository<DivisaoAtividadeEntity>(context);
			Feriado = new QueryRepository<FeriadoEntity>(context);
			FilaAnalista = new QueryRepository<FilaAnalistaEntity>(context);
			GrupoAtividade = new QueryRepository<GrupoAtividadeEntity>(context);
			HistoricoSituacaoInscricao = new QueryRepository<HistoricoSituacaoInscricaoEntity>(context);
			InscricaoCadastral = new QueryRepository<InscricaoCadastralEntity>(context);
			InscricaoCadastralCredenciamento = new QueryRepository<InscricaoCadastralCredenciamentoEntity>(context);
			InscricaoSuframaLegado = new QueryRepository<InscricaoSuframaLegadoEntity>(context);
			JustificativaProtocolo = new QueryRepository<JustificativaProtocoloEntity>(context);
			ListaDocumento = new QueryRepository<ListaDocumentoEntity>(context);
			ListaServico = new QueryRepository<ListaServicoEntity>(context);			
			MensagemPadrao = new QueryRepository<MensagemPadraoEntity>(context);
			MotivoSituacaoInscricao = new QueryRepository<MotivoSituacaoInscricaoEntity>(context);
			Municipio = new QueryRepository<MunicipioEntity>(context);
			NaturezaGrupo = new QueryRepository<NaturezaGrupoEntity>(context);
			NaturezaJuridica = new QueryRepository<NaturezaJuridicaEntity>(context);
			NaturezaQualificacao = new QueryRepository<NaturezaQualificacaoEntity>(context);
			Pais = new QueryRepository<PaisEntity>(context);
			Papel = new QueryRepository<PapelEntity>(context);
			ParametroAnalista = new QueryRepository<ParametroAnalistaEntity>(context);
			ParametroDistribuicaoAutomatica = new QueryRepository<ParametroDistribuicaoAutomaticaEntity>(context);
			ParametroAnalistaServico = new QueryRepository<ParametroAnalistaServicoEntity>(context);
			PedidoCorrecao = new QueryRepository<PedidoCorrecaoEntity>(context);
			PessoaFisica = new QueryRepository<PessoaFisicaEntity>(context);
			PessoaFisicaAprovado = new QueryRepository<PessoaFisicaAprovadoEntity>(context);
			PessoaJuridica = new QueryRepository<PessoaJuridicaEntity>(context);
			PessoaJuridicaAdministrativa = new QueryRepository<PessoaJuridicaAdministradorEntity>(context);
			PessoaJuridicaAprovado = new QueryRepository<PessoaJuridicaAprovadoEntity>(context);
			PessoaJuridicaAtividade = new QueryRepository<PessoaJuridicaAtividadeEntity>(context);
			PessoaJuridicaInscricaoEstadual = new QueryRepository<PessoaJuridicaInscricaoEstadualEntity>(context);
			PessoaJuridicaSocio = new QueryRepository<PessoaJuridicaSocioEntity>(context);
			PorteEmpresa = new QueryRepository<PorteEmpresaEntity>(context);
			Protocolo = new QueryRepository<ProtocoloEntity>(context);
			QuadroPessoal = new QueryRepository<QuadroPessoalEntity>(context);
			Qualificacao = new QueryRepository<QualificacaoEntity>(context);
			Recurso = new QueryRepository<RecursoEntity>(context);
			Requerimento = new QueryRepository<RequerimentoEntity>(context);
			RequerimentoDocumento = new QueryRepository<RequerimentoDocumentoEntity>(context);
			SecaoAtividade = new QueryRepository<SecaoAtividadeEntity>(context);
			SeqInscricaoCadastral = new QueryRepository<SeqInscricaoCadastralEntity>(context);
			Servico = new QueryRepository<ServicoEntity>(context);
			Setor = new QueryRepository<SetorEntity>(context);
			SetorAtividade = new QueryRepository<SetorAtividadeEntity>(context);
			SituacaoInscricao = new QueryRepository<SituacaoInscricaoEntity>(context);
			StatusProtocolo = new QueryRepository<StatusProtocoloEntity>(context);
			SubclasseAtividade = new QueryRepository<SubclasseAtividadeEntity>(context);
			TaxaServico = new QueryRepository<TaxaServicoEntity>(context);
			TipoConsultaPublica = new QueryRepository<TipoConsultaPublicaEntity>(context);
			TipoDocumento = new QueryRepository<TipoDocumentoEntity>(context);
			TipoIncentivo = new QueryRepository<TipoIncentivoEntity>(context);
			TipoRequerimento = new QueryRepository<TipoRequerimentoEntity>(context);
			TipoUsuario = new QueryRepository<TipoUsuarioEntity>(context);
			UF = new QueryRepository<UFEntity>(context);
			UnidadeCadastradora = new QueryRepository<UnidadeCadastradoraEntity>(context);
			UnidadeSecundaria = new QueryRepository<UnidadeSecundariaEntity>(context);
			UsuarioInterno = new QueryRepository<UsuarioInternoEntity>(context);
			UsuarioPapel = new QueryRepository<UsuarioPapelEntity>(context);
			VWDocumento = new QueryRepository<VWDocumentoEntity>(context);
			VWPessoaFisicaAprovado = new QueryRepository<VWPessoaFisicaAprovadoEntity>(context);
			VWPessoaJuridicaAdministradorAprovado = new QueryRepository<VWPessoaJuridicaAdministradorAprovadoEntity>(context);
			VWPessoaJuridicaAprovado = new QueryRepository<VWPessoaJuridicaAprovadoEntity>(context);
			VWPessoaJuridicaAtividadeAprovado = new QueryRepository<VWPessoaJuridicaAtividadeAprovadoEntity>(context);
			VWPessoaJuridicaInscricaoEstadualAprovado = new QueryRepository<VWPessoaJuridicaInscricaoEstadualAprovadoEntity>(context);
			VWPessoaJuridicaSocioAprovado = new QueryRepository<VWPessoaJuridicaSocioAprovadoEntity>(context);
			VWQuadroPessoalAprovado = new QueryRepository<VWQuadroPessoalAprovadoEntity>(context);
			WorkflowMensagemPadrao = new QueryRepository<WorkflowMensagemPadraoEntity>(context);
			WorkflowProtocolo = new QueryRepository<WorkflowProtocoloEntity>(context);
			WorkflowSituacaoInscricao = new QueryRepository<WorkflowSituacaoInscricaoEntity>(context);
			Paiz = new QueryRepository<PaizEntity>(context);
			ViewMunicipio = new QueryRepository<ViewMunicipioEntity>(context);
			ViewAtividadeEconomicaPrincipal = new QueryRepository<ViewAtividadeEconomicaPrincipalEntity>(context);
			ViewSetorEmpresa = new QueryRepository<ViewSetorEmpresaEntity>(context);
			ViewSetor = new QueryRepository<ViewSetorEntity>(context);
			
			
		}
	}
}