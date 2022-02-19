import { ModalDescricaoObservacaoComponent } from './modal/modal-descricao-observacao/modal-descricao-observacao.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ConsultarInsumosNacionalGridComponent } from './grid/grid-insumos-nacional.component';
import { ConsultarInsumosImportadosGridComponent } from './grid/grid-insumos-importados.component';
import { ConsultarProcessoExportacaoComponent } from './consultar-processo-exportacao.component';
import { ConsultarProcessoExportacaoGridComponent } from './grid/grid.component';
import { FormularioAcompanharProcessoComponent } from './formulario/formulario-acompanhar-processo.component';
import { ConsultarFormularioPropriedadeProdutoComponent } from './formulario/formulario-propriedade-produto.component';
import { ConsultarFormularioQuadrosInsumosComponent } from './formulario/formulario-quadros-insumos.component';
import { DetalheInsumosModalGridComponent } from './modal/grid-modal-detalhe-processo-insumos/grid-modal-detalhe-processo-insumos.component';
import { ModalDetalhesInsumoComponent } from './modal/modal-detalhes-insumo.component';
import { RelatorioCertificadoRegistroComponent } from './relatorio/relatorio-certificado-registro.component';
import { ModalParecerComponent } from './modal/modal-parecer.component';
import { ModalGridParecerTecComponent } from './modal/grid-modal-parecer/grid-modal-parecer-tec.component';
import { RelatorioParecerTecnicoComponent } from './modal/grid-modal-parecer/relatorio/relatorio-parecer-tecnico.component';
import { ModalSolicitarInclusaoInsumoComponent } from './modal/modal-solicitar-inclusao-insumo.component';
import { SolicitarInclusaInsumoGridComponent } from './modal/grid-solicitar-inclusao-insumo/grid.component';
import { ConsultarPlanoFormularioDetalhesInsumosComponent } from './formulario/formulario-detalhes-insumos.component';
import { DetalheInsumosGridComponent } from './grid/grid-detalhe-insumos/grid-detalhe-processo-insumos.component';
import { ConsultarHistoricoGridComponent } from './grid/grid-historico.component';
import { ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent } from './modal/modais-solicitacao-alteracao/quantidade/modal-quantidade-coeficiente-tecnico.component';
import { ModalTransferenciaInsumoComponent } from './modal/modal-transferencia-insumo.component';
import { DocumentosComprobatoriosGridComponent } from './grid/grid-documentos-comprobatorios.component';


import { ModalPaisComponent } from './modal/modais-solicitacao-alteracao/pais/modal-pais.component';
import { ModalMoedaComponent } from './modal/modais-solicitacao-alteracao/moeda/modal-moeda.component';
import { ModalSolicitacaoComponent } from './modal/modais-solicitacao-alteracao/solicitacoes-alteracao/solicitacao-alteracao.component';
import { ModalSolicitacaoDetalhadaComponent } from './modal/modais-solicitacao-alteracao/modal-visualizar-solicitacao-detalhada/solicitacao-detalhada-alteracao.component';
import { ModalSolicitarAlteracaoValorUnitarioComponent } from './modal/modais-solicitacao-alteracao/valor-unitario/modal-valor-unitario.component';
import { ModalSolicitarAlteracaoValorFreteComponent } from './modal/modais-solicitacao-alteracao/valor-frete/modal-valor-frete.component';
import { ModalCancelarComponent } from './modal/modal-cancelamento/modal-cancelamento.component';
import { ModalAdiamentoComponent } from './modal/modal-adiamento/modal-adiamento.component';
import { ModalCertificadoComponent } from './modal/modal-certificado/modal-certificado.component';
import { ModalGridCertificadoComponent } from './modal/grid-modal-certificado/grid-modal-certificado.component';
@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ConsultarProcessoExportacaoComponent,
		ConsultarProcessoExportacaoGridComponent,
		FormularioAcompanharProcessoComponent,
		ConsultarFormularioPropriedadeProdutoComponent,
		ConsultarFormularioQuadrosInsumosComponent,
		ConsultarInsumosNacionalGridComponent,
		ConsultarInsumosImportadosGridComponent,
		DetalheInsumosModalGridComponent,
		ModalDetalhesInsumoComponent,
		RelatorioCertificadoRegistroComponent,
		ModalParecerComponent,
		ModalGridParecerTecComponent,
		RelatorioParecerTecnicoComponent,
		ConsultarPlanoFormularioDetalhesInsumosComponent,
		ModalSolicitarInclusaoInsumoComponent,
		SolicitarInclusaInsumoGridComponent,
		ConsultarHistoricoGridComponent,
		DetalheInsumosGridComponent,
		ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent,
		ModalTransferenciaInsumoComponent,
		ModalPaisComponent,
		ModalMoedaComponent,
		ModalSolicitarAlteracaoValorUnitarioComponent,
		ModalSolicitacaoComponent,
		ModalSolicitacaoDetalhadaComponent,
		ModalSolicitarAlteracaoValorFreteComponent,
		ModalCancelarComponent,
		ModalAdiamentoComponent,
		ModalCertificadoComponent,
		ModalGridCertificadoComponent,
		ModalDescricaoObservacaoComponent,
		DocumentosComprobatoriosGridComponent
	],
})
export class ConsultarProcessoExportacaoModule { }
