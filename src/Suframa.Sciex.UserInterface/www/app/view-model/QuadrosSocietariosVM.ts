import { quadroSocietarioVM } from './QuadroSocietarioVM';

export class quadrosSocietariosVM {
	constructor() {
		this.quadrosSocietarios = new Array<quadroSocietarioVM>();
	}

	idPessoaJuridica?: number;
	quadrosSocietarios: quadroSocietarioVM[];
}
