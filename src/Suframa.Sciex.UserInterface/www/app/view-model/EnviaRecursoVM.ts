import { arquivoVM } from './ArquivoVM';

export class enviaRecursoVM {
	ano?: number;
	arquivos: arquivoVM[];
	descricaoServico: string;
	descricaoUnidadeCadastradora: string;
	documentos: string[];
	idProtocolo?: number;
	idPapel?: number;
	isConferenciaAdministrativa: boolean;
	isIndeferidoAguardandoRecurso: boolean;
	justificativa: string;
	justificativaIndeferimento: string;
	motivoIndeferimento: string[];
	numeroProtocolo: string;
	numeroSequencial?: number;
}
