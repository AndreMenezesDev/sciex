export class manterMonitoramentoSiscomexVM {

		idAliArquivoEnvio: number;
		nomeArquivo: string;
		dataGeracao: Date;
		tipoArquivo: number;
		codigoStatusEnvioSiscomex: number;
		quantidadePLIs: number;
		quantidadeALIs: number;
		valorTotalReal: number;
		valorTotalDolar: number;
		tentativasEnvio: number;

		//variáveis de apoio
		dataEnvioInicial?: Date;
		dataEnvioFinal?: Date;
		tipoDeConsulta: number;
		numeroAli: number;
		descricaoStatusEnvioSiscomex: string;


}
