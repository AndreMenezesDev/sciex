import { ErroProcessamentoVM } from './ErroProcessamentoVM';

export class SolicitacaoPliVM {

	idSolicitacaoPli: number;
	cnpjEmpresa: string;
	idTipoAplicacaoPli: number;
	numeroLIReferencia: string;
	numeroPEXPAM?: number;
	numeroCPFRepresentanteLegal: string;
	codigoCNAE: string;
	inscricaoCadastral?: number;
	codigoSetor?: number;
	descricaoSetor: string;
	tipoDocumento: number;
	tipoOrigem?: number; 
	razaoSocialEmpresa: string;
	codigoPliAplicacao: number;
	statusIndicacaoPliExigencia: string;
	numeroPliImportador: string;
	idEstruturaPropriaPli?: number;
	statusSolicitacao: number;

	// Complemento de Classe
	dataValidacao: string;
	qtdErrosPli: number;
	qtdSucessoPli: number;
	nPliImportador: string;
	listaErros: Array<ErroProcessamentoVM>;
	statusSolicitacaoNome: string;
	dataInicioProcessamento: Date;
	numeroPliSuframa: string;

}
