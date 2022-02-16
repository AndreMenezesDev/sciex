export class manterPliDetalheMercadoriaVM {
	idPliDetalheMercadoria? : number;	
	idPliMercadoria?: number;
	idPLIAplicacao?: number;
	idUnidadeMedida: number;
	descricaoUnidadeMedida: string;
	siglaUnidadeMedida: string;
	codigoDetalheMercadoria? : number;
	descricaoDetalhe : string;
	descricaoComplemento : string;
	descricaoMateriaPrimaBasica : string;
	descricaoPartNumber : string;
	descricaoREFFabricante : string;
	quantidadeComercializada? : number;
	valorUnitarioCondicaoVenda? : number;
	valorUnitarioCondicaoVendaDolar?: number;
	valorCondicaoVenda? : number;
	valorTotalCondicaoVendaReal? : number;
	valorTotalCondicaoVendaDolar?: number;
	
	
	//Complemento de classe
	idDetalheMercadoria: number;
	index: number;
	valorTotalCondicaoVenda: number;

	mensagemErro: string;
	excluir: boolean
	idPli: number;

	quantidadeComercializadaFormatada: string;	
	valorUnitarioCondicaoVendaFormatada: string;
	valorTotalCondicaoVendaDolarFormatada: string;
	valorUnitarioCondicaoVendaDolarFormatada: string;

	codigoDetalheMercadoriaFormatado: string;
}
