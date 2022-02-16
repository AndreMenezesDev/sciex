export class manterRegimeTributarioMercadoriaVM {
		idRegimeTributarioMercadoria: number;
		status: number;
		idRegimeTributario: number;
		idFundamentoLegal: number;
		codigoMunicipio: number;
		descricaoMunicipio: string;
		UF: string;
		dataInicioVigencia: Date;
		dataInicio?: Date;
		dataFim?: Date;

		//variável de apoio
		codigoRegimeTributario: number;
		codigoFundamentoLegal: number;
		descricaoRegimeTributario: string;
		descricaoFundamentoLegal: string;
		codigoDescricaoRegimeTributario: string;
		codigoDescricaoFundamentoLegal: string;
		codigoDescricaoMunicipio: string;
		codigoDoMunicipio: string;
		mensagemErro: string;
		isEditStatus: number;
		dataVigenciaFormatado: string;

}
