import { manterPliDetalheMercadoriaVM } from "./ManterPliDetalheMercadoriaVM";
import { manterPliProcessoAnuenteVM } from "./ManterPliProcessoAnuenteVM";
import { manterPliFornecedorFabricanteVM } from "./ManterPliFornecedorFabricanteVM";

import { validarDadosMercadoriaVM } from './ValidarDadosMercadoriaVM';
import { validarFornecedorFabricanteMercadoriaVM } from './ValidarFornecedorFabricanteMercadoriaVM';
import { validarNegociacaoMercadoriaVM } from './ValidarNegociacaoMercadoriaVM';
import { validarDetalheMercadoriaVM } from "./ValidarDetalheMercadoriaVM";

export class manterPliMercadoriaVM {

	constructor() {
	}

	idPliMercadoria? : number;		
	idPLI: number;
	idPLIAplicacao: number;
	idMoeda? : number;
	inscricaoCadastral: string;
	idIncoterms? : number;
	idRegimeTributario? : number;
	idFabricante? : number;
	idFornecedor? : number;
	idFundamentoLegal? : number;
	idInstituicaoFinanceira? : number;
	idMotivo?: number;
	idPliProduto?: number;
	idModalidadePagamento? : number;
	idAladi? : number;
	idNaladi? : number;
	idURFEntrada? : number;
	idURFDespacho? : number;
	codigoPais?: number;
	descricaoPais: string;
	codigoPaisOrigemFabricante?: number;
	descricaoPaisOrigemFabricante: string;
	pesoLiquido? : number;
	quantidadeUnidadeMedidaEstatistica? : number;
	numeroComunicadoCompra : string;
	numeroAtoDrawback : string;
	numeroAgenciaSecex : string;
	valorCRA? : number;
	tipoCOBCambial? : number;
	numeroCOBCambialLimiteDiasPagamento? : number;
	tipoAcordoTarifario? : number;
	descricaoInformacaoComplementar : string;
	tipoBemEncomenda? : number;
	tipoMaterialUsado? : number;
	numeroNCMDestaque : string;
	codigoNCMMercadoria : string;
	descricaoNCMMercadoria : string;
	tipoFornecedor? : number;
	codigoProduto? : number;
	codigoTipoProduto? : number;
	codigoModeloProduto? : number;
	valorTotalCondicaoVenda? : number;
	valorTotalCondicaoVendaReal? : number;
	valorTotalCondicaoVendaDolar? : number;	
	descricaoProduto: string;
	rowVersion: number[];
	idCodigoConta?: number;
    idCodigoUtilizacao?: number;
	numeroLIReferencia?: string;
	numeroLIRetificador: string;
	//complemento de classe
	idMercadoria: number;	
	codigoProdutoConcatenado: string;	
	mensagemErro: string;
	idProdutoEmpresa: number;
	listaPliDetalheMercadoriaVM: Array<manterPliDetalheMercadoriaVM>;
	listaPliProcessoAnuenteVM: Array<manterPliProcessoAnuenteVM>;
	aplicarParametros: boolean;
	idParametro: number;
	isValidarItemPli: boolean;
	parametroAplicado: boolean;
	confirmacaoClienteParametro: boolean;

	validarDadosMercadoria: validarDadosMercadoriaVM;
	validarDetalhesMercadoria: validarDetalheMercadoriaVM;
	validarFornecedorFabricanteMercadoria: validarFornecedorFabricanteMercadoriaVM;
	validarNegociacaoMercadoria: validarNegociacaoMercadoriaVM;

	valorTotalCondicaoVendaFormatado: string;
	valorTotalCondicaoVendaRealFormatado: string;
	valorTotalCondicaoVendaDolarFormatado: string;
	codigoMoeda: number;
	descricaoMoeda: string;
	codigoDescricaoMoeda: string;

	codigoProdutoFormatado: string;
	tipoProdutoFormatado: string;
	modeloProdutoFormatado: string;

	codigoIncoterms: string;
	urfNaladi: string;
	urfDespacho: string;
	entradaMercadoria: string;

	descricaoTipoFornecedor: string;
	descricaoFornecedor: string;
	numeroALI?: number;
	codigoDetalheMercadoria: string;
	statusALI: number;

	utilizadaDI: string;
	numeroDI: string;

	codigoAladi: string;
	descricaoAladi: string;
	codigoRegimeTributario: string;
	descricaoRegimeTributario: string;
	codigoFundamentalLegal: string;
	descricaoFundamentalLegal: string;
	tipoAcordoTarifarioDescricao?: string
	infCodigo: string;
	infDescricao: string;
	mopCodigo: string;
	mopDescricao: string;
	motCodigo: string;
	motDescricao: string;

	numeroLiSERPRO: string;
	tipoALI: string;
	numeroTransmissaoSERPRO: string;
	dataALIFormatada: string;
	dataALICancelada?: Date;
	numeroALISubstituida: string;
	numeroLISubstituida: string;
	numeroPLISubstituido: string;
	cnpj: string;
	razaoSocial: string;
	numeroPliConcatenado: string;
	dataProcessamento: string;
	numeroLI?: string;
	dataGeracaoLI: string;
	codigoDescricaoPaisFornecedorConcatenado: string;
	quantidadeErroAli: string;
	dataDIFormatada: string;

	tipoOrigem: number;
	dadosFabricanteFornecedor: manterPliFornecedorFabricanteVM;

	pesoLiquidoString: string;
	quantidadeEstatisticaString: string;

	dataALICanceladaFormatada: string;
	tipoCOBCambialDescricao: string;

}
