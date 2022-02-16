import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterConsultarPliComponent } from './manter-consultar-pli.component';
import { ManterConsultarPliFormularioComponent } from './formulario/formulario.component';
import { ManterConsultarPliGridComponent } from './grid/grid.component';
import { ModalAliIndeferidaComponent } from './modal/modal-ali-indeferida.component';
import { ModalJustificativaReprocessarComponent } from './modal/modal-justificativa-reprocessar.component';
import { ManterConsultarPliMercadoriasFormularioComponent } from './formulario/formulario-mercadorias.component';
import { ManterConsultarPliMercadoriaGridComponent } from './grid/grid-mercadoria.component';
import { ManterConsultarPliMercadoriasDescricaoFormularioComponent } from './formulario/formulario-mercadorias-descricao.component';
import { ManterConsultarPliMercadoriasDetalheFormularioComponent } from './formulario/formulario-mercadorias-detalhe.component';
import { ManterConsultarPliMercadoriaDetalheGridComponent } from './grid/grid-mercadoria-detalhe.component';
import { ManterConsultarPliMercadoriasFornecedorFormularioComponent } from './formulario/formulario-mercadorias-fornecedor.component';
import { ManterConsultarRelatorioStatusPliFormularioComponent } from './formulario/formulario-relatorio-status.component';
import { ManterConsultarRelatorioStatusPliGridComponent } from './grid/grid-status-pli.component';
import { ManterPliProcessoAnuenteGridComponent } from './grid/grid-processo-anuente.component'
import { ManterConsultarPliMercadoriasDetalheALIFormularioComponent } from './formulario/formulario-detalhamento-ali.component';
import { ManterConsultarPliMercadoriasDetalheALISubstitutivoFormularioComponent } from './formulario/formulario-detalhamento-ali-substitutivo.component';
import { ManterConsultarPliMercadoriasDetalheLIFormularioComponent } from './formulario/formulario-detalhamento-li.component';
import { ManterConsultarPliMercadoriasDetalheLISubstitutivoFormularioComponent } from './formulario/formulario-detalhamento-li-substitutivo.component';
import { ManterConsultarPliMercadoriasNegociacaoFormularioComponent } from './formulario/formulario-mercadorias-negociacao.component';
import { ManterConsultarDetalheItemMercadoriaFormularioComponent } from './formulario/formulario-detalhe-item-mercadoria.component';
import { ManterConsultarListagemErroPliComponent } from './formulario/formulario-listagem-erro.component';
import { ManterListagemErroPliGridComponent } from './grid/grid-listagem-erro-pli.component';
import { ManterConsultarListagemErroAliComponent } from './formulario/formulario-listagem-erro-ali.component';
import { ManterListagemErroAliGridComponent } from './grid/grid-listagem-erro-ali.component';
import { ModalListagemPadraoComponent } from './modal/modal-listagem-padrao.component';
import { ManterConsultarDiFormularioComponent } from './formulario/formulario-detalhamento-di.component';
import { ManterDetalhamentoDiAdicoesFormularioComponent } from './formulario/formulario-detalhamento-di-adicoes.component';
import { ManterDetalhamentoAdicoesDetalheFormularioComponent } from './formulario/formulario-detalhamento-adicoes-detalhe.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
    ],
    declarations: [
		ManterConsultarPliComponent,      
		ManterConsultarPliGridComponent,
		ManterConsultarPliFormularioComponent,
		ModalAliIndeferidaComponent,
		ModalJustificativaReprocessarComponent,
		ManterConsultarPliMercadoriasFormularioComponent,
		ManterConsultarPliMercadoriaGridComponent,
		ManterConsultarPliMercadoriasDescricaoFormularioComponent,
		ManterConsultarPliMercadoriasDetalheFormularioComponent,
		ManterConsultarPliMercadoriaDetalheGridComponent,
		ManterConsultarPliMercadoriasFornecedorFormularioComponent,
		ManterConsultarRelatorioStatusPliFormularioComponent,
		ManterConsultarRelatorioStatusPliGridComponent,
		ManterPliProcessoAnuenteGridComponent,
		ManterConsultarPliMercadoriasNegociacaoFormularioComponent,
		ManterConsultarDetalheItemMercadoriaFormularioComponent,
		ManterConsultarPliMercadoriasDetalheALIFormularioComponent,
		ManterConsultarPliMercadoriasDetalheALISubstitutivoFormularioComponent,
		ManterConsultarPliMercadoriasDetalheLIFormularioComponent,
		ManterConsultarPliMercadoriasDetalheLISubstitutivoFormularioComponent,
		ManterConsultarListagemErroPliComponent,
		ManterListagemErroPliGridComponent,
		ManterConsultarListagemErroAliComponent,
		ManterListagemErroAliGridComponent,
		ModalListagemPadraoComponent,
		ManterConsultarDiFormularioComponent,
		ManterDetalhamentoDiAdicoesFormularioComponent,
		ManterDetalhamentoAdicoesDetalheFormularioComponent

	],
})
export class ManterConsultarPliModule { }
