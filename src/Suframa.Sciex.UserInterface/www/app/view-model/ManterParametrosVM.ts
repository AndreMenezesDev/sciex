export class manterParametrosVM {
	idParametro?: number;
	idMoeda?: number;
	idIncoterms?: number;
	idUnidadeReceitaFederalEntrada?: number;
	idUnidadeReceitaFederalDespacho?: number;
	idFornecedor?: number;
	idFabricante?: number;
	idAladi?: number;
	idNaladi?: number;
	idRegimeTributario?: number;
	idFundamentoLegal?: number;
	idModalidadePagamento?: number;
	idMotivo?: number;
	idInstituicaoFinanceira?: number;
	descricao: string;
	tipoCorbeturaCambial?: number;
	quantidadeDiaLimite?: number;
	tipoAcordoTarifario: string;
	tipoFornecedor?: number = -1;
	codigoPaiMercadoria: string;
	descricaoPaiMercadoria: string;
	codigoPaisOrigemFabricante: string;
	descricaoPaisOrigemFabricante: string;
	CPNJImportador: string;
	codigo: number;
}
