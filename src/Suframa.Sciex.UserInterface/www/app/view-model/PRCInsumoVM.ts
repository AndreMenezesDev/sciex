import { PagedItems } from './PagedItems';

export class PRCInsumoVM extends PagedItems {
    produto : any[];
    listaDetalheInsumos : any[];
    idInsumo : number;
    idPrcProduto : number;
    codigoInsumo : number;
    codigoUnidade : number;
    descCodigoUnidade : string;
    tipoInsumo : string;
    descricaoTipoInsumo : string;
    codigoNCM : string;
    valorPercentualPerda : string;
    codigoDetalhe : number;
    descricaoPartNumber : string;
    descricaoInsumo : string;
    descricaoEspecificacaoTecnica : string;
    valorCoeficienteTecnico : number;
    valorDolarAprovado : number;
    quantidadeAprovado : number;
    valorNacionalAprovado : number;
    valorDolarFOBAprovado : number;
    valorDolarCFRAprovado : number;
    valorFreteAprovado : number;
    valorDolarComp : number;
    quantidadeComp : number;
    valorDolarSaldo : number;
    quantidadeSaldo : number;
    qtdMaxInsumo : number;
    descricaoNCM : string;

    quantidadeAdicional : number;
    valorDolarAdicional : number;

}