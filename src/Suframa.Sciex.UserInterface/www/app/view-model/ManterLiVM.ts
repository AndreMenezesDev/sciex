export class manterLiVM {
	idPliMercadoria :number;
	idAliArquivoEnvio? :number;
	idDI? :number;
	idLiArquivoRetorno? :number;
	numeroLi? :number;
	status :number;
	tipoAli: number;
	dataCadastro: Date;
	dataCancelamento: Date;
	mensagemErroLI: string;

	//Complemento de Classe
	descricaoStatus: string;
	numeroPli: number;
	anoPli: number;
	numeroNCM :string;
	descricaoNCMMercadoria: string;
	listaSelecionados: Array<number>;
	checkbox: boolean;
	mensagemErro: string;
	dataCadastroFormatado: string;
	numeroPLIFormatado: string;
	CNPJEmpresa: string;
	razaoSocialEmpresa: string;
	numeroALI: number;
	numeroLiMascarado: string;
}
