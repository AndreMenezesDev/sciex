import { manterPliMercadoriaVM } from "./ManterPliMercadoriaVM";
import { DecimalPipe } from "@angular/common";

export class manterPliVM {

	idPLI?: number;
	idPLIStatus: number;
	idPLIAplicacao: number;
	tipoDocumento: number;
	numeroPli: number;
	ano: number;
	cnpj: string;
	inscricaoCadastral?: number;
	statusAnaliseVisual?: number;
	statusDistribuicao?: number;
	valorTCIF?: number;
	valorTECIFItens?: number;
	debito?: number;
	debitoAno?: number;
	dataDebitoPagamento?: Date;
	dataDebitoGeracao?: Date;
	numeroLIReferencia: any;
	numeroDIReferencia: any;
	idLiReferencia: number;
	numeroDiReferencia: string;
	numeroPEXPAM?: number;
	anoPEXPAM?: number;
	lotePEXPAM: string;
	MEALIArquivo: string;
	tipoOrigem?: number;
	numeroResponsavelRegistro: string;
	nomeResponsavelRegistro: string;
	dataCadastro: Date;
	dataEnvioPli?: Date;
	numCPFRepLegalSISCO: string;
	codigoCNAE: string;
	descricaoSetor: string;
	codigoSetor: number;
	razaoSocial: string;
	statusPli: number;
	statusPliProcessamento: number;
	statusALI: number;
	descricaoHistoricoPli: string;
	numeroPliImportador: string;	
	statusPliTecnologiaAssistiva: number;
	statusIndicacaoPliExigencia: string;
	idEstruturaPropria?: number;
	dataDiFormatada: string;
	utilizadaDI: string;
	nomeAnexo : string;
	anexo : string;

	//PliAnalise Visual

	analiseVisualStatus?: number;
	descricaoMotivo: string;
	idCodigoUtilizacao? : number;
	idCodigoConta? : number;
	dataAnalise?: Date;
	dataPendencia?: Date;
	motivoPendencia: string;
	descricaoResposta: string;
	analiseVisualNomeAnexo : string;
	analiseVisualAnexo : string;
	//

	// complemento da classe
    ativarLIOriginal: boolean;
	numeroPLI: string;
	descricaoDebito: string;
	situacao: string;
	descricaoValorGeralTcif: string;
	descricaoAplicacao: string;
	descricaoStatus: string;
	descricaoTipoPli: string;
	opcaoSelecionada: boolean;
	codigoPliAplicao: number;
	dataInicio?: Date;
	dataFim?: Date;
	mensagem: string;
	copiaPli: boolean;
	dataPliFormatada: string;
	status: number;
	checkbox: boolean;
	consultarPli: number;
	statusTaxa: number;
	quantidadeErroProcessamento: number;
	dataProcessamento: string;
	listaPli: Array<number>;
	descricaoTipoDocumento: string;
	numeroALIReferencia: string;
	statusPliSelecionado: number;

	listaMercadorias: Array<manterPliMercadoriaVM>;
	validarPli: boolean;
	isPliValidado: boolean;
	isMercadorias: boolean;
	idPliProduto: number;
	numeroPliFormatado: string;
	numeroPliConcatenado: string;
	mensagemErro: string;
	tipoErro: number;
	entregarPli: boolean;
	codigoPLIStatus: number;
	temProjetoAprovado: string;

	di: any;

	endereco: string;
	numero: string;
	complemento: string;
	bairro: string;
	codigoMunicipio: string;
	municipio: string;
	uf: string;
	cep: string;
	descricaoCNAE: string;
	paisCodigo: string;
	paisDescricao: string;
	telefone: string;
	codigoUtilizacao: string;
	descricaoUtilizacao: string;
	codigoConta: string;
	descricaoConta: string;
	codigoProduto: string;

	quantidadeMercadorias: number;
	valorTotalDolarMercadorias: string; 
	valorTotalRealMercadorias: string;

	listaSelecionados: Array<number>;
	observacao: string;
	enviadoAoSiscomex: number;
	respondidoPeloSiscomex: number;
	quantidadeErro: number;
	idPliMercadoria: number;

	idUtilizadaDI: string;
	numeroDI: string;

	dataEnvioPliFormatada: string;
	temALIIndeferida: boolean;

	numeroALISubstitutiva: number;
	numeroPLISubstitutivo: number;
	anoPliSubstitutivo: number;
	numeroPliSubstitutivoConcatenado: string;
	idPLISubstitutivo : number;
	idPliMercadoriaSubstitutivo : number;

}
