import { SolicitacaoPliVM } from './SolicitacaoPLIVM';

export class EstruturaPropriaPLIVM {

	idEstruturaPropria?: number;
	dataEnvio: Date;
	dataInicioProcessamento?: Date;
	dataFimProcessamento?: Date;
	statusProcessamentoArquivo?: number;
	quantidadePLIArquivo?: number;
	quantidadePLIProcessadoSucesso?: number;
	quantidadePLIProcessadoFalha?: number;
	versaoEstrutura: string;
	loginUsuarioEnvio: string;
	nomeUsuarioEnvio: string;
	CNPJImportador: string;
	razaoSocialImportador: string;
	inscricaoCadastral?: number
	nomeArquivoEnvio: number;
	listaDePLI: string;
	statusPLITecnologiaAssistiva?: number;
	descricaoPendenciaImportador: string;
	statusUltimoArquivoProcessamento: number;

	// Complemento de Classe
	dataInicio?: Date;
	dataFim?: Date;
	listaSolicitacao: Array<SolicitacaoPliVM>;
	quantidadePliConcatenado: string
	statusValidacaoArquivoConcatenado: string;	

}
