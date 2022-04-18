import { RelatorioDuesVM } from './RelatorioDuesVM';
import { RequestResponseVM } from './RequestResponseVM';

export class RelatorioErroDuesVM extends RequestResponseVM
{
	nomeEmpresa : string;
	anoPlano : number;
	numeroPlano : any;
	numeroPlanoFormated : string;
	anoNumPlano : string;
	dataImpressao : string;
	inscricaoCadastral : number;
	modalidade : string;
	tipo : string;
	dataStatus : string;
	dataRecebimento : string;
	anoNumProcesso : number;
	relatorioHistoricoAnalise : Array<RelatorioDuesVM>;
	relatorioDePara : Array<RelatorioDuesVM>;
}
