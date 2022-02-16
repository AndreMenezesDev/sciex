import { atividadeEconomicaVM } from './AtividadeEconomicaVM';
import { setorAtividadeVM } from './SetorAtividadeVM';

export class manterSetorEmpresarialVM {
	idSetor: number;
	codigo: number;
	descricao: string;
	tipo: number;
	observacao: string;
	status: boolean;
	codigoDivisaoAtividade: number;
	codigoGrupoAtividade: number;
	codigoClasseAtividade: number;
	atividadesEconomicasGrid: atividadeEconomicaVM[];
	setorAtividade: any[];
}
