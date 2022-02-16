import { manterPliMercadoriaVM } from './ManterPliMercadoriaVM';

export class manterPliProdutoVM {
	idPliProduto?: number;
	idPLI: number;
	codigoProduto: number;
	codigoTipoProduto: number;
	codigoModeloProduto: number;
	descricao: string;

	// complemento de classe
	idProdutoEmpresa: number;
	manterPliMercadoriaVM: manterPliMercadoriaVM[];
	mensagemErro: string;
}
