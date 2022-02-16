import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { PlanoDeExportacaoComponent } from './plano-de-exportacao.component';
import { PlanoDeExportacaoGridComponent } from './grid/grid.component';
import { ModalJustificaGlosaQuadroExportacaooComponent } from './analise-plano-exportacao/modal/justificativa/modal-justificativa.component';
import { AnalisePlanoFormularioPlanoComponent } from './analise-plano-exportacao/formulario-analise-plano.component';
import { AnalisarFormularioPropriedadeProdutoComponent } from './analise-plano-exportacao/propriedade-produto/formulario-analisar-propriedade-produto.component';
import { AnalisarFormularioQuadrosInsumosComponent } from './analise-plano-exportacao/quadros/formulario-analisar-quadro-Insumos.component';
import { AnalisarInsumosImportadosGridComponent } from './analise-plano-exportacao/quadros/grid/grid-analisar-insumos-importados.component';
import { AnalisarInsumosNacionalGridComponent } from './analise-plano-exportacao/quadros/grid/grid-analisar-insumos-nacional.component';
import { ModalAnaliseDetalhesInsumoComponent } from './analise-plano-exportacao/modal/detalhe-do-insumo/modal-analise-detalhes-insumo.component';
import { AnaliseDetalheInsumosModalGridComponent } from './analise-plano-exportacao/modal/detalhe-do-insumo/grid-modal-detalhe-processo-insumos/grid-analise-detalhe-insumos.component';
import { ModalJustificativaIndeferirComponent } from './justificativa/modal-justificativa.component';
import { AnaliseDetalheInsumosAnterioresModalGridComponent } from './analise-plano-exportacao/modal/detalhe-do-insumo/grid-modal-detalhe-processo-insumos/grid-analise-detalhe-insumos-anteriores.component';


@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
        PlanoDeExportacaoComponent,
        PlanoDeExportacaoGridComponent,
		ModalJustificaGlosaQuadroExportacaooComponent,
		AnalisePlanoFormularioPlanoComponent,
		AnalisarFormularioPropriedadeProdutoComponent,
		AnalisarFormularioQuadrosInsumosComponent,
		AnalisarInsumosImportadosGridComponent,
		AnalisarInsumosNacionalGridComponent,
		ModalAnaliseDetalhesInsumoComponent,
		AnaliseDetalheInsumosModalGridComponent,
		ModalJustificativaIndeferirComponent,
		AnaliseDetalheInsumosAnterioresModalGridComponent
	],
})
export class PlanoDeExportacaoModule { }
