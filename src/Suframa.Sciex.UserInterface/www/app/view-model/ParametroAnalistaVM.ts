import { statusPLIVM } from './StatusPLIVM';
import { analistaVM } from './AnalistaVM';


export class parametroAnalistaVM {
	idParametroAnalista?: number;
	idAnalista: number;
	cpf: string;
	nome: string;
	statusAnaliseVisual: number;
	dataAnaliseVisualInicio: Date;
	horaAnaliseVisualInicio?: Date;
	horaAnaliseVisualFim?: Date;
	statusAnaliseVisualBloqueio: number;
	dataAnaliseVisualBloqueioInicio?: Date;
	horaAnaliseVisualBloqueioInicio?: Date;
	horaAnaliseVisualBloqueioFim?: Date;
	descricaoAnaliseVisualBloqueioFim: string;
	listaStatusAnaliseVisual: Array<statusPLIVM>;
	statusAnaliseLoteListagem: number;
	dataAnaliseLoteListagemInicio?: Date;
	horaAnaliseLoteListagemInicio?: Date;
	horaAnaliseLoteListagemFim?: Date;
	statusAnaliseLoteListagemBloqueio: number;
	dataAnaliseListagemLoteBloqueioInicio?: Date;
	horaAnaliseLoteListagemBloqueioInicio?: Date;
	horaAnaliseLoteListagemBloqueioFim?: Date;
	descricaoAnaliseLoteListagemBloqueioFim: string;
	listaStatusAnaliseListagem: Array<statusPLIVM>;
    tipoSistema: number;
    analista: analistaVM;
}
