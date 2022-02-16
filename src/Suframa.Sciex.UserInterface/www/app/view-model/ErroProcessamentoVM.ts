export class ErroProcessamentoVM {

	idErroProcessamento: number;
	idPli: number;
	idErroMensagem: number;
	codigoNivelErro: number;
	idPliMercadoriaOuPliDetalheMercadoria: number;
	descricao: string;
	dataProcessamento?: Date;
	idSolicitacaoPLI: number;
	cnpjImportador: string;
	numeroPLIImportador: string;

	// Complemento de classe
	localErro: string;
	mensagemErro: string; 
	origemErro: string;
	idPliMercadoria: number;
}
