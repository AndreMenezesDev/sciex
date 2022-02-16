import { workflowProtocoloVM } from './WorkflowProtocoloVM';

export class consultaProtocoloResultadoVM {
	idProtocolo?: number;
	numeroSequencial?: number;
	ano?: number;
	idServico?: number;
	descricaoServico: string;
	idStatusProtocolo?: number;
	descricaoStatusProtocolo: string;
	descricaoOrientacaoStatusProtocolo: string;
	isEnviarRecurso: boolean;
	isEditar: boolean;
	workflows: workflowProtocoloVM[];
}
