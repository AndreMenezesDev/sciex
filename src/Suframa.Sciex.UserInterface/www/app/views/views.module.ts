import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaskModule } from 'soft-angular-mask';
import { NgModule } from '@angular/core';

import { IndexComponent } from './index/index.component';

import { ManterFornecedorModule } from './manter-fornecedor/manter-fornecedor.module';

import { ParametrizacaoModule } from './parametrizacao/parametrizacao.module';

import { UsuarioPapelModule } from './usuario-papel/usuario-papel.module';
import { ViewsComponentsModule } from '../views-components/views-components.module';

import { ManterAladiModule } from './manter-aladi/manter-aladi.module';
import { ManterNaladiModule } from './manter-naladi/manter-naladi.module';

import { ManterFundamentoLegalModule } from './manter-fundamento-legal/manter-fundamento-legal.module';
import { ManterParidadeCambialModule } from './manter-paridade-cambial/manter-paridade-cambial.module';
import { ParametrizarAnalistaModule } from './parametrizar-analista/parametrizar-analista.module';
import { ManterFabricanteModule } from './manter-fabricante/manter-fabricante.module';
import { ManterParametrosModule } from './manter-parametros/manter-parametros.module';
import { ManterCodigoContaModule } from './manter-codigo-conta/manter-codigo-conta.module';
import { ManterCodigoUtilizacaoModule } from './manter-codigo-utilizacao/manter-codigo-utilizacao.module';
import { ManterRegimeTributarioModule } from './manter-regime-tributario/manter-regime-tributario.module';
import { ManterRegimeTributarioMercadoriaModule } from './manter-regime-tributario-mercadoria/manter-regime-tributario-mercadoria.module';
import { ManterControleImportacaoModule } from './manter-controle-importacao/manter-controle-importacao.module';
import { ManterNcmModule } from './manter-ncm/manter-ncm.module';
import { ManterNCMExcecaoModule } from './manter-ncm-excecao/manter-ncm-excecao.module';
import { ManterPliModule } from './manter-pli/manter-pli.module';
import { ManterConsultarPliModule } from './consultar-pli/manter-consultar-pli.module';
import { ManterCancelamentoLiModule } from './manter-cancelamento-li/manter-cancelamento-li.module';
import { EstruturaPropriaPLIModule } from '../views/estrutura-propria-pli/estrutura-propria-pli.module';
import { ManterCancelarLiModule } from '../views/cancelar-li/cancelar-li.module';
import { ManterConsultarProtocoloEnvioModule } from '../views/consultar-protocolo-envio/consultar-protocolo-envio.module';
import { ManterMonitoramentoSiscomexModule } from '../views/manter-monitoramento-siscomex/manter-monitoramento-siscomex.module';
import { ManterUnidadeReceitaFederalModule } from './manter-unidade-receita-federal/manter-unidade-receita-federal.module';
import { ManterGrupoBeneficioModule } from '../views/manter-grupo-beneficio/manter-grupo-beneficio.module';
import { ManterAnaliseVisualModule } from './manter-analise-visual/manter-analise-visual.module';
import { ManterParametrizarAnalistaModule } from './manter-parametrizar-analista/manter-parametrizar-analista.module';
import { DesignarPliModule } from './designar-pli/designar-pli.module';
import { ManterTipoDeclaracaoModule } from './manter-tipo-declaracao/manter-tipo-declaracao.module';
import { ManterViaTransporteModule } from './manter-via-transporte/manter-via-transporte.module';
import { ConsultarEntradaDiModule } from './consultar-entrada-di/consultar-entrada-di.module';
import { ManterTipoEmbalagemModule } from './manter-tipo-embalagem/manter-tipo-embalagem.module';
import { ManterRecintoAlfandegaModule } from './manter-recinto-alfandega/manter-recinto-alfandega.module';
import { ManterSetorArmazenamentoModule } from './manter-setor-armazenamento/manter-setor-armazenamento.component.module';
import { ManterListagemExportacaoModule } from './manter-listagem-exportacao/manter-listagem-exportacao.module';
import { EstruturaPropriaLEModule } from './estrutura-propria-le/estrutura-propria-le.module';
import { AnalisarListagemExportacaoModule } from './analisar-listagem-exportacao/analisar-listagem-exportacao.module';
import { ConsultarListagemExportacaoModule } from './consultar-listagem-exportacao/consultar-listagem-exportacao.module';
import { ManterPlanoExportacaoModule } from './manter-plano-exportacao/manter-plano-exportacao.module';
import { PlanoDeExportacaoModule } from './plano-de-exportacao/plano-de-exportacao.module';
import { ConsultarProcessoExportacaoModule } from './consultar-processo-exportacao/consultar-processo-exportacao.module';
import { EstruturaPropriaPEModule } from './estrututra-propria-pe/estrutura-propria-pe.module';
import { ConsultarProcessoExportacaoSuframaModule } from './consultar-processo-exportacao-suframa/consultar-processo-exportacao-suframa.module';

@NgModule({
	declarations: [
		IndexComponent,
	],
	exports: [
		ManterConsultarProtocoloEnvioModule,
		ManterFornecedorModule,
		MaskModule,
		ParametrizacaoModule,
		UsuarioPapelModule,
		ManterAladiModule,
		ManterNaladiModule,
		ManterFundamentoLegalModule,
		ManterParidadeCambialModule,
		ParametrizarAnalistaModule,
		ManterFabricanteModule,
		ManterParametrosModule,
		ManterCodigoContaModule,
		ManterCodigoUtilizacaoModule,
		ManterRegimeTributarioModule,
		ManterRegimeTributarioMercadoriaModule,
		ManterControleImportacaoModule,
		ManterNcmModule,
		ManterPliModule,
		ManterAnaliseVisualModule,
		ManterNCMExcecaoModule,
		ManterConsultarPliModule,
		ManterCancelamentoLiModule,
		EstruturaPropriaPLIModule,
		ManterCancelarLiModule,
		ManterMonitoramentoSiscomexModule,
		ManterUnidadeReceitaFederalModule,
		ManterGrupoBeneficioModule,
		ManterParametrizarAnalistaModule,
		DesignarPliModule,
		ManterTipoDeclaracaoModule,
		ManterViaTransporteModule,
		ConsultarEntradaDiModule,
		ManterTipoEmbalagemModule,
		ManterRecintoAlfandegaModule,
		ManterSetorArmazenamentoModule,
		AnalisarListagemExportacaoModule,
		ManterPlanoExportacaoModule,
		EstruturaPropriaPEModule,
		ManterPlanoExportacaoModule,
		ConsultarProcessoExportacaoModule,
		ConsultarProcessoExportacaoSuframaModule
	],
	imports: [
		CommonModule,
		ManterConsultarProtocoloEnvioModule,
		FormsModule,
		ManterFornecedorModule,
		MaskModule,
		ParametrizacaoModule,
		UsuarioPapelModule,
		ViewsComponentsModule,
		ManterAladiModule,
		ManterNaladiModule,
		ManterFundamentoLegalModule,
		ManterParidadeCambialModule,
		ParametrizarAnalistaModule,
		ManterFabricanteModule,
		ManterParametrosModule,
		ManterCodigoContaModule,
		ManterCodigoUtilizacaoModule,
		ManterControleImportacaoModule,
		ManterNcmModule,
		ManterPliModule,
		ManterAnaliseVisualModule,
		ManterNCMExcecaoModule,
		ManterConsultarPliModule,
		ManterCancelamentoLiModule,
		ManterRegimeTributarioModule,
		EstruturaPropriaPLIModule,
		ManterCancelarLiModule,
		ManterMonitoramentoSiscomexModule,
		ManterUnidadeReceitaFederalModule,
		ManterGrupoBeneficioModule,
		ManterParametrizarAnalistaModule,
		DesignarPliModule,
		ManterTipoDeclaracaoModule,
		ManterViaTransporteModule,
		ConsultarEntradaDiModule,
		ManterTipoEmbalagemModule,
		ManterRecintoAlfandegaModule,
		ManterSetorArmazenamentoModule,
		ManterListagemExportacaoModule,
		EstruturaPropriaLEModule,
		ManterListagemExportacaoModule,
		AnalisarListagemExportacaoModule,
		ConsultarListagemExportacaoModule,
		ManterPlanoExportacaoModule,
		ConsultarProcessoExportacaoModule,
		PlanoDeExportacaoModule,
		ManterPlanoExportacaoModule,
		EstruturaPropriaPEModule,
		ConsultarProcessoExportacaoSuframaModule,
		ConsultarProcessoExportacaoModule
	]
})
export class ViewsModule { }
