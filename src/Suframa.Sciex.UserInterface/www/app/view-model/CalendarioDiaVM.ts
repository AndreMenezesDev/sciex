import { calendarioHoraVM } from './CalendarioHoraVM';

export class calendarioDiaVM {
	constructor() {
		this.horas = new Array<calendarioHoraVM>();
	}

	idCalendarioDia?: number;
	idUnidadeCadastradora?: number;
	idUsuarioInterno?: number;
	dataAtendimento?: Date;
	dia?: number;
	diaAtendimento?: number;
	horas: calendarioHoraVM[];
}
