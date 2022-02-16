using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.DataAccess
{
	public interface IQueryStack
	{
		IQueryRepository<AgendaAtendimentoEntity> AgendaAtendimento { get; }
		IQueryRepository<ArquivoEntity> Arquivo { get; }
		IQueryRepository<ArquivoInformacaoEntity> ArquivoInformacao { get; }
		IQueryRepository<CalendarioEntity> Calendario { get; }
		IQueryRepository<CalendarioAgendamentoEntity> CalendarioAgendamento { get; }
		IQueryRepository<CalendarioDiaEntity> CalendarioDia { get; }
		IQueryRepository<CalendarioHoraEntity> CalendarioHora { get; }
		IQueryRepository<CampoSistemaEntity> CampoSistema { get; }
		IQueryRepository<CepEntity> Cep { get; }
		IQueryRepository<ClasseAtividadeEntity> ClasseAtividade { get; }
		IQueryRepository<ConferenciaDocumentoEntity> ConferenciaDocumento { get; }
		IQueryRepository<ConsultaPublicaEntity> ConsultaPublica { get; }
		IQueryRepository<CredenciamentoEntity> Credenciamento { get; }
		IQueryRepository<DadosSolicitanteEntity> DadosSolicitante { get; }
		IQueryRepository<DicionarioDropDownEntity> DicionarioDropDown { get; }
		IQueryRepository<DiligenciaEntity> Diligencia { get; }
		IQueryRepository<DiligenciaAnexosEntity> DiligenciaAnexos { get; }
		IQueryRepository<DiligenciaAtividadesEntity> DiligenciaAtividades { get; }
		IQueryRepository<DiligenciaAtividadesSetorEntity> DiligenciaAtividadesSetor { get; }
		IQueryRepository<DivisaoAtividadeEntity> DivisaoAtividade { get; }
		IQueryRepository<FeriadoEntity> Feriado { get; }
		IQueryRepository<FilaAnalistaEntity> FilaAnalista { get; }
		IQueryRepository<GrupoAtividadeEntity> GrupoAtividade { get; }
		IQueryRepository<HistoricoSituacaoInscricaoEntity> HistoricoSituacaoInscricao { get; }
		IQueryRepository<InscricaoCadastralEntity> InscricaoCadastral { get; }
		IQueryRepository<InscricaoCadastralCredenciamentoEntity> InscricaoCadastralCredenciamento { get; }
		IQueryRepository<InscricaoSuframaLegadoEntity> InscricaoSuframaLegado { get; }
		IQueryRepository<ListaDocumentoEntity> ListaDocumento { get; }
		IQueryRepository<MensagemPadraoEntity> MensagemPadrao { get; }
		IQueryRepository<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; }
		IQueryRepository<MunicipioEntity> Municipio { get; }
		IQueryRepository<NaturezaGrupoEntity> NaturezaGrupo { get; }
		IQueryRepository<NaturezaJuridicaEntity> NaturezaJuridica { get; }
		IQueryRepository<NaturezaQualificacaoEntity> NaturezaQualificacao { get; }
		IQueryRepository<PaisEntity> Pais { get; }
		IQueryRepository<PapelEntity> Papel { get; }
		IQueryRepository<ParametroAnalistaEntity> ParametroAnalista { get; }
		IQueryRepository<ParametroAnalistaServicoEntity> ParametroAnalistaServico { get; }
		IQueryRepository<ParametroDistribuicaoAutomaticaEntity> ParametroDistribuicaoAutomatica { get; }
		IQueryRepository<PedidoCorrecaoEntity> PedidoCorrecao { get; }
		IQueryRepository<PessoaFisicaEntity> PessoaFisica { get; }
		IQueryRepository<PessoaFisicaAprovadoEntity> PessoaFisicaAprovado { get; }
		IQueryRepository<PessoaJuridicaEntity> PessoaJuridica { get; }
		IQueryRepository<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrativa { get; }
		IQueryRepository<PessoaJuridicaAprovadoEntity> PessoaJuridicaAprovado { get; }
		IQueryRepository<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; }
		IQueryRepository<PessoaJuridicaInscricaoEstadualEntity> PessoaJuridicaInscricaoEstadual { get; }
		IQueryRepository<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; }
		IQueryRepository<PorteEmpresaEntity> PorteEmpresa { get; }
		IQueryRepository<ProtocoloEntity> Protocolo { get; }
		IQueryRepository<QuadroPessoalEntity> QuadroPessoal { get; }
		IQueryRepository<QualificacaoEntity> Qualificacao { get; }
		IQueryRepository<RecursoEntity> Recurso { get; }
		IQueryRepository<RegimeTributarioEntity> RegimeTributario { get; }
		IQueryRepository<RegimeTributarioTesteEntity> RegimeTributarioTeste { get; }
		IQueryRepository<RequerimentoEntity> Requerimento { get; }
		IQueryRepository<RequerimentoDocumentoEntity> RequerimentoDocumento { get; }
		IQueryRepository<SecaoAtividadeEntity> SecaoAtividade { get; }
		IQueryRepository<SeqInscricaoCadastralEntity> SeqInscricaoCadastral { get; }
		IQueryRepository<ServicoEntity> Servico { get; }
		IQueryRepository<SetorEntity> Setor { get; }
		IQueryRepository<SetorAtividadeEntity> SetorAtividade { get; }
		IQueryRepository<SituacaoInscricaoEntity> SituacaoInscricao { get; }
		IQueryRepository<StatusProtocoloEntity> StatusProtocolo { get; }
		IQueryRepository<SubclasseAtividadeEntity> SubclasseAtividade { get; }
		IQueryRepository<TaxaServicoEntity> TaxaServico { get; }
		IQueryRepository<TipoConsultaPublicaEntity> TipoConsultaPublica { get; }
		IQueryRepository<TipoDocumentoEntity> TipoDocumento { get; }
		IQueryRepository<TipoIncentivoEntity> TipoIncentivo { get; }
		IQueryRepository<TipoRequerimentoEntity> TipoRequerimento { get; }
		IQueryRepository<TipoUsuarioEntity> TipoUsuario { get; }
		IQueryRepository<UFEntity> UF { get; }
		IQueryRepository<UnidadeCadastradoraEntity> UnidadeCadastradora { get; }
		IQueryRepository<UnidadeSecundariaEntity> UnidadeSecundaria { get; }
		IQueryRepository<UsuarioInternoEntity> UsuarioInterno { get; }
		IQueryRepository<UsuarioPapelEntity> UsuarioPapel { get; }
		IQueryRepository<VWDocumentoEntity> VWDocumento { get; }
		IQueryRepository<VWPessoaFisicaAprovadoEntity> VWPessoaFisicaAprovado { get; }
		IQueryRepository<VWPessoaJuridicaAdministradorAprovadoEntity> VWPessoaJuridicaAdministradorAprovado { get; }
		IQueryRepository<VWPessoaJuridicaAprovadoEntity> VWPessoaJuridicaAprovado { get; }
		IQueryRepository<VWPessoaJuridicaAtividadeAprovadoEntity> VWPessoaJuridicaAtividadeAprovado { get; }
		IQueryRepository<VWPessoaJuridicaInscricaoEstadualAprovadoEntity> VWPessoaJuridicaInscricaoEstadualAprovado { get; }
		IQueryRepository<VWPessoaJuridicaSocioAprovadoEntity> VWPessoaJuridicaSocioAprovado { get; }
		IQueryRepository<VWQuadroPessoalAprovadoEntity> VWQuadroPessoalAprovado { get; }
		IQueryRepository<WorkflowMensagemPadraoEntity> WorkflowMensagemPadrao { get; }
		IQueryRepository<WorkflowProtocoloEntity> WorkflowProtocolo { get; }
		IQueryRepository<WorkflowSituacaoInscricaoEntity> WorkflowSituacaoInscricao { get; }
		IQueryRepository<PaizEntity> Paiz { get; }


		IQueryRepository<ViewAtividadeEconomicaPrincipalEntity> ViewAtividadeEconomicaPrincipal { get; }
		IQueryRepository<ViewSetorEmpresaEntity> ViewSetorEmpresa { get; }
		IQueryRepository<ViewSetorEntity> ViewSetor { get; }		
		IQueryRepository<ViewMunicipioEntity> ViewMunicipio { get; }
		IQueryRepository<ViewUnidadeMedidaEntity> ViewUnidadeMedida { get; }
		IQueryRepository<ViewDetalheMercadoriaEntity> ViewDetalheMercadoria { get; }
	}
}