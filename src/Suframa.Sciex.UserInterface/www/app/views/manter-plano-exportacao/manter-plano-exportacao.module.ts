import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterPlanoExportacaoComponent } from './manter-plano-exportacao.component';
import { ManterPlanoExportacaoGridComponent } from './grid/grid.component';
import { ManterPlanoFormularioPlanoComponent } from './formulario/formularioPlano.component';
import { ModalNovoPlanoComponent } from './modal/modal-novo-plano.component';
import { ManterPlanoFormularioPropriedadeProdutoComponent } from './formulario/formularioPropriedadeProduto.component';
import { ManterPlanoFormularioPropriedadeProdutoComprovacaoComponent } from './formulario/formularioPropriedadeProdutoComprovacao.component';
import { ModalAlterarPaisComponent } from './modal/modal-alterar-pais.component';
import { ModalEditarDocumentoDueComponent } from './modal/modal-editar-documento-due.component';
import { ManterPEInsumosNacionalGridComponent } from './grid/grid-insumos-nacional.component';
import { ManterPlanoFormularioQuadrosInsumosComponent } from './formulario/formularioQuadrosInsumos.component';
import { ManterPEInsumosImportadosGridComponent } from './grid/grid-insumos-importados.component';
import { ModalIncluirInsumoComponent } from './modal/modal-incluir-insumo.component';
import { InsumosModalGridComponent } from './modal/grid-modal-incluir-insumos/grid-modal-insumos.component';
import { ManterPlanoFormularioDetalhesInsumosComponent } from './formulario/formularioDetalhesInsumos.component';
import { ModalEditarDetalheInsumoComponent } from './modal/modal-editar-detalhe-insumo.component';
import { ModalJustificativaErroComponent } from './modal/justificativa-de-erro/modal-justificativa-erro.component';
import { ManterPlanoFormularioPlanoComprovacaoComponent } from './formulario/formularioPlanoComprovacao.component';
import { DocumentosComprobatorioslGridComponent } from './grid/grid-dados-comprobatorios/grid-dados-comprobatorios.component';
@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ManterPlanoExportacaoComponent,
		ManterPlanoExportacaoGridComponent,
		ModalNovoPlanoComponent,
		ManterPlanoFormularioPropriedadeProdutoComprovacaoComponent,
		ManterPlanoFormularioPlanoComponent,
		ManterPlanoFormularioPropriedadeProdutoComponent,
		ModalAlterarPaisComponent,
		ManterPlanoFormularioQuadrosInsumosComponent,
		ManterPEInsumosNacionalGridComponent,
		ManterPEInsumosImportadosGridComponent,
		ModalIncluirInsumoComponent,
		ModalEditarDocumentoDueComponent,
		InsumosModalGridComponent,
		ManterPlanoFormularioDetalhesInsumosComponent,
		ModalEditarDetalheInsumoComponent,
		ModalJustificativaErroComponent,
		ManterPlanoFormularioPlanoComprovacaoComponent,
		DocumentosComprobatorioslGridComponent
	],
})
export class ManterPlanoExportacaoModule { }
