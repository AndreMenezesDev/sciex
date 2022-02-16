import { quadroAdministradoresVM } from './QuadroAdministradoresVM';
import { naturezaQualificacaoVM } from './NaturezaQualificacaoVM';

export class quadrosAdministradoresVM {
	constructor() {
		this.naturezaQualificacao = Array<naturezaQualificacaoVM>();
		this.quadrosAdministradores = Array<quadroAdministradoresVM>();
	}

	idPessoaJuridica?: number;
	idNaturezaJuridica?: number;
	hasQuadroSocietario?: boolean;
	naturezaQualificacao: naturezaQualificacaoVM[];
	quadrosAdministradores: quadroAdministradoresVM[];
}
