import { recursoVM } from './RecursoVM';

export class julgamentoRecursoVM {
	cpfCnpj: string;
	descricaoPapel?: string;
	idPapel?: number;
	idProtocolo?: number;
	idRecurso?: number;
	idStatusProtocolo: number;
	isCoordenadorDescentralizado: boolean;
	isCoordenadorGeral: boolean;
	isSuperintendenteAdjunto: boolean;
	justificativa: string;
	justificativaIndeferimento: string;
	motivoIndeferimento: string[];
	nomeRazaoSocial: string;
	nomeResponsavel: string;
	nomeUsuarioInterno: string;
	numeroProtocolo: string;
	parecerCoordenador: string;
	parecerSuperintendente: string;
	recursos: recursoVM[];
	tipoProtocolo: string;
}
