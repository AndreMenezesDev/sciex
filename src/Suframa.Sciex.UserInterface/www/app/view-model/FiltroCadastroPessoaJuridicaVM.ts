import { naturezaQualificacaoVM } from './NaturezaQualificacaoVM';

export class FiltroCadastroPessoaJuridicaVM {
	constructor() {
		this.naturezaQualificacao = new Array<naturezaQualificacaoVM>();
	}

	cnpj: string;
	idNaturezaGrupo?: number;
	idNaturezaJuridica?: number;
	idPessoaJuridica?: number;
	idRequerimento?: number;
	isQuadroSocietario: boolean;
	naturezaQualificacao: naturezaQualificacaoVM[];
	tipoEntidadeRegistro: number;
	tipoEstabelecimento: number;
}
