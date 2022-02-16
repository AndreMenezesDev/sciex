import { PagedOption } from './PagedOption';

export class PRCSolicitacaoAlteracaoVM extends PagedOption {

    //campos tabela
	id : number;
	numeroSolicitacao : number;
    anoSolicitacao : number;
    status : number;
    dataInclusao : Date;  
    cpfResponsavel : string;
	nomeResponsavel : string;
    processoVM : any = {};
    idProcesso : number;
    dataAlteracao : Date;
	cnpj : string;
	razaoSocial : string;

    //complementos
    quantidaDeItens : number;
    dataInicio : Date;
    dataFim : Date;
    numeroProcesso : number;
	anoProcesso : number;
    descricaoStatus : string;
    numeroAnoSolicitacaoFormatado : string;
    numeroAnoProcessoFormatado : string;
    dataInicioString : string;
    dataFimString : string;
}
