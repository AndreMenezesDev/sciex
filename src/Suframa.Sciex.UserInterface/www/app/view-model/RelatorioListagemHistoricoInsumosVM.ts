import { InsumosRelatorioListagemHistoricoVM } from './InsumosRelatorioListagemHistoricoVM';

export class RelatorioListagemHistoricoInsumosVM
{
	inscricaoCadastral : number;
	nomeEmpresa : string;
	codigoProduto : number;
	descricaoModelo : string;
	dataImpressao : string;
	insumos : Array<InsumosRelatorioListagemHistoricoVM> = new Array<InsumosRelatorioListagemHistoricoVM>();
}
