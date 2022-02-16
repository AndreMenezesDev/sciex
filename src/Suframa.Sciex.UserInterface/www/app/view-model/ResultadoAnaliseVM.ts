import { resumoVM } from './ResumoVM';
import { consultaPublicaVM } from './ConsultaPublicaVM';
import { workflowProtocoloVM } from './WorkflowProtocoloVM';

export class resultadoAnaliseVM {
	conferenciasAdministrativas: workflowProtocoloVM[];
	consultasPublica: consultaPublicaVM[];
	diligencias: workflowProtocoloVM[];
	idProtocolo?: number;
	isConsultaPublicaPendente: boolean;
	isDiligenciaAberta: boolean;
	isStatusEmAberto: boolean;
	pedidosCorrigir: resumoVM[];
	pedidosAtualizar: resumoVM[];
}
