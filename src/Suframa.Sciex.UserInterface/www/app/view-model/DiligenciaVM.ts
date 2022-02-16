import { diligenciaAnexoVM } from './DiligenciaAnexoVM';
import { diligenciaAtividadeVM } from './DiligenciaAtividadeVM';

export class diligenciaVM {
	idUnidadeCadastradora?: number;
	idDiligencia?: number;
	idUsuarioInterno?: number;
	idPessoaFisica?: number;
	idPessoaJuridica?: number;
	idProtocolo?: number;
	analistaResponsavel: string;
	cpfCnpj: string;
	nomeRazaoSocial: string;
	dataDiligencia?: Date;
	dataDiligenciaAte?: Date;
	status: number;
	hora: string;
	horaDiligencia: string;
	statusRealizada: boolean;
	motivo: string;
	pessoaResponsavel: string;
	atividades: diligenciaAtividadeVM[];
	parecer: string;
	arquivos: diligenciaAnexoVM[];
}
