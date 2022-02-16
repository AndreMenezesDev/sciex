import { qualificacaoVM } from './QualificacaoVM';

export class manterNaturezaJuridicaVM {
	idNaturezaJuridica?: number;
	idNaturezaGrupo?: number;
	codigo?: number;
	descricao: string;
	status: boolean;
	statusQuadroSocial: boolean;
	qualificacoes: qualificacaoVM[];
}
