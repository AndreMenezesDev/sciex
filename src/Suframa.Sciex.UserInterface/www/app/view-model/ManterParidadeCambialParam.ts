import { manterParidadeCambialVM } from './ManterParidadeCambialVM';

export class manterParidadeCambialParam {
    dataParidade: Date;
    dataParidadeInicio: Date;
    dataOrigem: Date; // Se TipoGeracao=1, Data do arquivo - Se TipoGeracao=2, Data da cópia
    dataParidadeProxima?: Date;
    adicionaParidade: boolean = true;
    tipoGeracao: number; // 1-Baixar Arquivo, 2-Copiar Arquivo
    dias: number;
	indSobrepor: number; // 0-Não, 1-Sim
    indRetorno: number;
    listaParidadeCambialRemover: Array<manterParidadeCambialVM>;
    listaParidadeCambialAdd: Array<manterParidadeCambialVM>;
    paridadeCambialVM: manterParidadeCambialVM;
	mensagem: string;
	diaAtual: number;
}
