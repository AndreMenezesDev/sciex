import { diligenciaAtividadeSetorVM } from './diligenciaAtividadeSetorVM';

export class diligenciaAtividadeVM {
	codigoSubclasse: string;
	codigoSubclasseAtividade: string;
	descricaoSubclasse: string;
	idDiligencia?: number;
	idDiligenciaAtividade?: number;
	idPessoaJuridica?: number;
	isAtividadeExercida: boolean;
	setor: diligenciaAtividadeSetorVM;
}
