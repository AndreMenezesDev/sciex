import { RelatoriosDuesVM } from "./RelatoriosDuesVM";

export class RelatorioErroDuesVM
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
	relatorios : RelatoriosDuesVM = new RelatoriosDuesVM();
}
