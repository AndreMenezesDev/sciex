import { calendarioDiaVM } from './CalendarioDiaVM';
import { calendarioHoraVM } from './CalendarioHoraVM';
import { usuarioInternoVM } from './UsuarioInternoVM';

export class calendarioAgendamentoVM {
	constructor() {
		this.analistas = new Array<usuarioInternoVM>();
		this.dias = new Array<calendarioDiaVM>();
		this.horarios = new Array<calendarioHoraVM>();
	}

	analistas: usuarioInternoVM[];
	ano?: number;
	dias: calendarioDiaVM[];
	calendario: any[];
	horarios: calendarioHoraVM[];
	idCalendarioAgendamento?: number;
	idUnidadeCadastradora?: number;
	idUsuarioInterno?: number;
	isMesInteiro: boolean;
	mes?: number;
}
