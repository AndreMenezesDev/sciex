import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ConsultarProcessoExportacaoSuframaComponent } from './consultar-processo-exportacao-suframa.component';
import { ConsultarProcessoExportacaoSuframaGridComponent } from './grid/grid.component';
import { FormularioAcompanharProcessoSuframaComponent } from './formulario/formulario-acompanhar-processo-suframa.component';
import { RelatorioCertificadoRegistroSuframaComponent } from './relatorio/relatorio-certificado-registro-suframa.component';
import { ConsultarFormularioPropriedadeProdutoSuframaComponent } from './formulario/formulario-propriedade-produto-suframa.component';
import { ConsultarFormularioQuadrosInsumosSuframaComponent } from './formulario/formulario-quadros-insumos-suframa.component';
import { ConsultarInsumosNacionalSuframaGridComponent } from './grid/grid-insumos-nacional-suframa.component';
import { ConsultarInsumosImportadosSuframaGridComponent } from './grid/grid-insumos-importados-suframa.component';
import { ModalDetalhesInsumoSuframaComponent } from './modal/modal-detalhes-insumo-suframa.component';
import { ModalDescricaoObservacaoSuframaComponent } from './modal/modal-descricao-observacao/modal-descricao-observacao-suframa.component';
import { ModalParecerSuframaComponent } from './modal/modal-parecer-suframa.component';
import { RelatorioParecerTecnicoSuframaComponent } from './modal/grid-modal-parecer/relatorio/relatorio-parecer-tecnico-suframa.component';
import { ModalGridParecerTecSuframaComponent } from './modal/grid-modal-parecer/grid-modal-parecer-tec-suframa.component';
import { DetalheInsumosModalSuframaGridComponent } from './modal/grid-modal-detalhe-processo-insumos/grid-modal-detalhe-processo-insumos-suframa.component';
import { ModalAnalisarPedidoProrrogacaoComponent } from './modal/modal-analisar-pedido-prorrogacao/modal-analisar-pedido-prorrogacao.component';
import { ModalCertificadoSuframaComponent } from './modal/modal-certificado/modal-certificado-suframa.component';
import { ModalGridCertificadoSuframaComponent } from './modal/modal-certificado/grid-modal-certificado/grid-modal-certificado-suframa.component';
import { ModalAnaliseSolicitacaoComponent } from './modal/modal-analise-solicitacao-alteracao/analise-solicitacao-alteracao.component';
import { AnaliseDetalheInsumoGridComponent } from './modal/modal-analise-solicitacao-alteracao/grid/grid-analise-detalhe-insumo.component';

import { ConsultarHistoricoSuframaGridComponent } from './grid/grid-historico-suframa.component';
import { ModalHistoricoInsumosImportadosComponent } from './modal/modal-visualizar-historico-insumos-importados/modal-historico-insumos-importados-suframa.component';
import { RelatorioHistoricoInsumosSuframaComponent } from './modal/modal-visualizar-historico-insumos-importados/relatorio/relatorio-historico-insumos-importados-suframa.component';
import { DetalheInsumosFormularioSuframaGridComponent } from './formulario/formulario-detalhe-processo-insumo-suframa/grid-form-detalhe-suframa/grid-detalhe-proc-insumos-suframa.component';
import { FormularioDetalhesInsumoSuframaComponent } from './formulario/formulario-detalhe-processo-insumo-suframa/formulario-detalhes-insumo-suframa.component';
import { ModalJustificativaReprovacaoComponent } from './modal/modal-justificativa/modal-justificativa.component';



@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ConsultarProcessoExportacaoSuframaComponent,
		ConsultarProcessoExportacaoSuframaGridComponent,
		FormularioAcompanharProcessoSuframaComponent,
		ConsultarFormularioPropriedadeProdutoSuframaComponent,
		ConsultarFormularioQuadrosInsumosSuframaComponent,
		ConsultarInsumosNacionalSuframaGridComponent,
		ConsultarInsumosImportadosSuframaGridComponent,
		DetalheInsumosModalSuframaGridComponent,
		ModalDetalhesInsumoSuframaComponent,
		RelatorioCertificadoRegistroSuframaComponent,
		ModalParecerSuframaComponent,
		ModalGridParecerTecSuframaComponent,
		RelatorioParecerTecnicoSuframaComponent,
		ModalAnalisarPedidoProrrogacaoComponent,
		ModalCertificadoSuframaComponent,
		ModalGridCertificadoSuframaComponent,
		ConsultarHistoricoSuframaGridComponent,
		ModalDescricaoObservacaoSuframaComponent,
		ModalAnaliseSolicitacaoComponent,
		AnaliseDetalheInsumoGridComponent,
		ModalHistoricoInsumosImportadosComponent,
		RelatorioHistoricoInsumosSuframaComponent,
		DetalheInsumosFormularioSuframaGridComponent,
		FormularioDetalhesInsumoSuframaComponent,
		ModalJustificativaReprovacaoComponent

	],
})
export class ConsultarProcessoExportacaoSuframaModule { }
