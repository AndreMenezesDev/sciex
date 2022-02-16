import { naturezaQualificacaoVM } from './NaturezaQualificacaoVM';

export class quadroAdministradoresVM {
	constructor() {
		this.naturezaQualificacao = new Array<naturezaQualificacaoVM>();
	}

	idAdministrador?: number;
	idPessoaJuridica?: number;
	cpf?: string;
	nome: string;
	nomeSocial: string;
	idQualificacao?: number;
	descricaoQualificacao: string;
	naturezaQualificacao: naturezaQualificacaoVM[];
}
