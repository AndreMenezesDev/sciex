export class aliVM {
	idPliMercadoria : number;
	iIdAliArquivoEnvio : number;
	numeroAli : number;
	status : number;
	tipoAli: number;
	dataCadastro: Date;
	dataCancelamento?: Date;
	dataProcessamentoSuframa?: Date;
	dataRespostaSISCOMEX?: Date;
		
		//Complemento de Classe
	idPli: number;
	descricaoStatus: string;
	numeroPliConcatenado: string;
	numeroLi: string;
	nomenclaturaComumMercosul: string;
	codigoProduto: string;
	tipoProduto: string;
	codigoModeloProduto: string;
	statusAli: number;

	quantidadeErroAli: string;
}
