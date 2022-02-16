import { PagedOption } from './PagedOption';

export class PRCSolicDetalheVM extends PagedOption {

    id : number;
    status : number;
	descricaoDe : string;
	descricaoPara : string;
	idInsumo : number;
	idDetalheInsumo : number;
	idSolicitacaoAlteracao : number;
	idTipoSolicitacao : number;
		
	//complementos
	codigoDetalheInsumo : number;
	detalheInsumo : number;
    codigoInsumo : number;
	descricaoStatus : string;
	descricaoInsumo : string;
	descricaoTipoAlteracao : string;
}