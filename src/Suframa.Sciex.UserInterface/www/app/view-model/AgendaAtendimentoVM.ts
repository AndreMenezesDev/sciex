import { dadosSolicitanteVM } from './DadosSolicitanteVM';

export class AgendaAtendimentoVM {
	idAgendaAtendimento?: number;
	tipo?: number;
	isLiConcordo?: boolean;
	dadosSolicitante?: dadosSolicitanteVM = new dadosSolicitanteVM();
	token: string;
	idServico?: number;
	descricaoServico: string;
	cpfCnpj: string;
	nomeRazaoSocial: string;
	idUnidadeCadastradora?: number;
	descricaoUnidadeCadastradora: string;
	numeroProtocolo: string;
	descricaoDataAtendimento: string;
	descricaoHorarioAtendimento: string;
	dataAtendimento?: Date;
	horarioAtendimento?: Date;
	idCalendarioHora?: number;
}
