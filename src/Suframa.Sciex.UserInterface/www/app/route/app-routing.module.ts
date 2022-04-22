import { NgModule } from '@angular/core';
import { RouterModule, Routes, CanActivate } from '@angular/router';

import { IndexComponent } from '../views/index/index.component';
import { AuthGuard } from '../shared/guards/auth-guard.service';
import { ManterAladiComponent } from '../views/manter-aladi/manter-aladi.component';
import { ManterAladiFormularioComponent } from '../views/manter-aladi/formulario/formulario.component';
import { ManterNaladiComponent } from '../views/manter-naladi/manter-naladi.component';
import { ManterNaladiFormularioComponent } from '../views/manter-naladi/formulario/formulario.component';
import { ManterRegimeTributarioComponent } from '../views/manter-regime-tributario/manter-regime-tributario.component';
import { ManterRegimeTributarioFormularioComponent } from '../views/manter-regime-tributario/formulario/formulario.component';
import { ManterFundamentoLegalComponent } from '../views/manter-fundamento-legal/manter-fundamento-legal.component';
import { ManterFundamentoLegalFormularioComponent } from '../views/manter-fundamento-legal/formulario/formulario.component';
import { ManterParidadeCambialComponent } from '../views/manter-paridade-cambial/manter-paridade-cambial.component';
import { ManterParidadeCambialFormularioComponent } from '../views/manter-paridade-cambial/formulario/formulario.component';
import { ParametrizarAnalistaComponent } from '../views/parametrizar-analista/parametrizar-analista.component';
import { ManterFornecedorComponent } from '../views/manter-fornecedor/manter-fornecedor.component';
import { ManterFornecedorFormularioComponent } from '../views/manter-fornecedor/formulario/formulario.component';
import { ManterFabricanteComponent } from '../views/manter-fabricante/manter-fabricante.component';
import { ManterFabricanteFormularioComponent } from '../views/manter-fabricante/formulario/formulario.component';
import { ManterParametrosComponent } from '../views/manter-parametros/manter-parametros.component';
import { ManterParametrosFormularioComponent } from '../views/manter-parametros/formulario/formulario.component';
import { ManterCodigoContaComponent } from '../views/manter-codigo-conta/manter-codigo-conta.component';
import { ManterCodigoContaFormularioComponent } from '../views/manter-codigo-conta/formulario/formulario.component';
import { ManterCodigoUtilizacaoComponent } from '../views/manter-codigo-utilizacao/manter-codigo-utilizacao.component';
import { ManterCodigoUtilizacaoFormularioComponent } from '../views/manter-codigo-utilizacao/formulario/formulario.component';
import { ManterRegimeTributarioMercadoriaComponent } from '../views/manter-regime-tributario-mercadoria/manter-regime-tributario-mercadoria.component';
import { ManterRegimeTributarioMercadoriaFormularioComponent } from '../views/manter-regime-tributario-mercadoria/formulario/formulario.component';
import { ManterControleImportacaoComponent } from '../views/manter-controle-importacao/manter-controle-importacao.component';
import { ManterControleImportacaoFormularioComponent } from '../views/manter-controle-importacao/formulario/formulario.component';
import { ManterNcmComponent } from '../views/manter-ncm/manter-ncm.component';
import { ManterNcmFormularioComponent } from '../views/manter-ncm/formulario/formulario.component';
import { ManterUnidadeReceitaFederalComponent } from '../views/manter-unidade-receita-federal/manter-unidade-receita-federal.component';
import { ManterUnidadeReceitaFederalFormularioComponent } from '../views/manter-unidade-receita-federal/formulario/formulario.component';

import { ManterConsultarPliComponent } from '../views/consultar-pli/manter-consultar-pli.component';
import { ManterConsultarPliFormularioComponent } from '../views/consultar-pli/formulario/formulario.component';
import { ManterConsultarPliGridComponent } from '../views/consultar-pli/grid/grid.component';
import { ManterConsultarPliMercadoriasFormularioComponent } from '../views/consultar-pli/formulario/formulario-mercadorias.component';
import { ManterConsultarRelatorioStatusPliFormularioComponent } from '../views/consultar-pli/formulario/formulario-relatorio-status.component';
import { ManterConsultarPliMercadoriasDescricaoFormularioComponent } from '../views/consultar-pli/formulario/formulario-mercadorias-descricao.component';
import { ManterConsultarPliMercadoriasDetalheFormularioComponent } from '../views/consultar-pli/formulario/formulario-mercadorias-detalhe.component';
import { ManterConsultarPliMercadoriasFornecedorFormularioComponent } from '../views/consultar-pli/formulario/formulario-mercadorias-fornecedor.component';
import { ManterConsultarPliMercadoriasNegociacaoFormularioComponent } from '../views/consultar-pli/formulario/formulario-mercadorias-negociacao.component';
import { ManterConsultarPliMercadoriasDetalheALIFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-ali.component';
import { ManterConsultarPliMercadoriasDetalheLIFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-li.component';
import { ManterConsultarDetalheItemMercadoriaFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhe-item-mercadoria.component';
import { ManterConsultarListagemErroPliComponent } from '../views/consultar-pli/formulario/formulario-listagem-erro.component';
import { ManterMonitoramentoSiscomex } from '../views/manter-monitoramento-siscomex/manter-monitoramento-siscomex.component';
import { ManterCancelarLiGridComponent } from '../views/cancelar-li/grid/grid.component';
import { ManterCancelarLiComponent } from '../views/cancelar-li/cancelar-li.component';
import { CancelaLIFormularioPLIFormularioComponent } from '../views/cancelar-li/formulario/formulario-cancelar-consultar-pli.component';
import { CancelaLIFormularioALIFormularioComponent } from '../views/cancelar-li/formulario/formulario-cancelar-detalhamento-ali.component';
import { CancelaLIFormularioLIFormularioComponent } from '../views/cancelar-li/formulario/formulario-cancelar-detalhamento-li.component';
import { CancelaLIFormularioLISubstitutivoFormularioComponent } from '../views/cancelar-li/formulario/formulario-cancelar-detalhamento-li-substitutivo.component';
import { CancelaLIFormularioALISubstitutivoFormularioComponent } from '../views/cancelar-li/formulario/formulario-cancelar-detalhamento-ali-substitutivo.component';


import { UsuarioPapelComponent } from '../views/usuario-papel/usuario-papel.component';

import { ParametrizacaoAnalistasComponent } from '../views/parametrizacao/analistas/analistas.component';
import { ParametrizacaoComponent } from '../views/parametrizacao/parametrizacao.component';
import { ParametrizacaoDistribuicaoAutomaticaComponent } from '../views/parametrizacao/distribuicao-automatica/distribuicao-automatica.component';
import { ParametrizacaoServicosComponent } from '../views/parametrizacao/servicos/servicos.component';
import { SincronizarParidadeCambialComponent } from '../views/manter-paridade-cambial/sincronizar-paridade/sincronizar-paridade-cambial.component';
import { ManterPliComponent } from '../views/manter-pli/manter-pli.component';
import { ManterPliFormularioComponent } from '../views/manter-pli/formulario/formulario.component';
import { ManterNCMExcecaoComponent } from '../views/manter-ncm-excecao/manter-ncm-excecao.component';
import { ManterNCMExcecaoFormularioComponent } from '../views/manter-ncm-excecao/formulario/formulario.component';
import { EstruturaPropriaPLIComponent } from '../views/estrutura-propria-pli/estrutura-propria-pli.component'
import { ManterCancelamentoLiComponent } from '../views/manter-cancelamento-li/manter-cancelamento-li.component';
import { ManterConsultarProtocoloEnvioComponent } from '../views/consultar-protocolo-envio/consultar-protocolo-envio.component';
import { ManterConsultarProtocoloEnvioPliFormularioComponent } from '../views/consultar-protocolo-envio/formulario/consultar-protocolo-envio-pli.component';
import { ManterConsultarProtocoloEnvioPliErrosFormularioComponent } from '../views/consultar-protocolo-envio/formulario/consultar-protocolo-envio-pli-erros.component';
import { ManterConsultarListagemErroAliComponent } from '../views/consultar-pli/formulario/formulario-listagem-erro-ali.component';
import { ManterPliFormularioSubstutivoComponent } from "../views/manter-pli/formulario/formularioSubstitutivo.component";
import { ManterPliFormularioComercializacaoSubstitutivoComponent } from "../views/manter-pli/formulario/formularioComercializacaoSubstitutivo.component";

import { ManterGrupoBeneficioComponent } from "../views/manter-grupo-beneficio/manter-grupo-beneficio.component";
import { CadastrarBeneficioComponent } from "../views/manter-grupo-beneficio/formulario/formulario-cadastrar-beneficio.component";
import { AlterarBeneficioComponent } from "../views/manter-grupo-beneficio/formulario/formulario-alterar-beneficio.component";
import { CadastrarNCMBeneficioComponent } from "../views/manter-grupo-beneficio/formulario/formulario-cadastrar-ncm-beneficio.component";
import { ManterPliFormularioComercializacaoComponent } from '../views/manter-pli/formulario/formularioComercializacao.component';
import { ManterConsultarPliMercadoriasDetalheALISubstitutivoFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-ali-substitutivo.component';
import { ManterConsultarPliMercadoriasDetalheLISubstitutivoFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-li-substitutivo.component';
import { ManterAnaliseVisualComponent } from '../views/manter-analise-visual/manter-analise-visual.component';
import { ManterAnaliseVisualFormularioComponent } from '../views/manter-analise-visual/formulario/formulario.component';
import { ManterParametrizarAnalistaComponent } from '../views/manter-parametrizar-analista/manter-parametrizar-analista.component';
import { DesignarPliComponent } from '../views/designar-pli/designar-pli.component';
import { ManterConsultarDiFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-di.component';
import { ManterDetalhamentoDiAdicoesFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-di-adicoes.component';
import { ManterDetalhamentoAdicoesDetalheFormularioComponent } from '../views/consultar-pli/formulario/formulario-detalhamento-adicoes-detalhe.component';
import { ManterTipoDeclaracaoComponent } from '../views/manter-tipo-declaracao/manter-tipo-declaracao.component';
import { ManterTipoDeclaracaoFormularioComponent } from '../views/manter-tipo-declaracao/formulario/formulario.component';
import { ManterViaTransporteComponent } from '../views/manter-via-transporte/manter-via-transporte.component';
import { ManterViaTransporteFormularioComponent } from '../views/manter-via-transporte/formulario/formulario.component';
import { ConsultarEntradaDiComponent } from '../views/consultar-entrada-di/consultar-entrada-di.component';
import { ConsultarEntradaDiProcessadoFormularioComponent } from '../views/consultar-entrada-di/formulario/consultar-entrada-di-processado.component';
import { ConsultarEntradaDiErrosFormularioComponent } from '../views/consultar-entrada-di/formulario/consultar-entrada-di-erros.component';
import { ManterPliFormularioRetificadorComponent } from '../views/manter-pli/formulario/formularioRetificador.component';
import { ManterPliFormularioRetificadorComercializacaoComponent } from '../views/manter-pli/formulario/formularioRetificadorComercializacao.component';

import { ManterTipoEmabalagemComponent } from '../views/manter-tipo-embalagem/manter-tipo-embalagem.component';
import { ManteTipoEmbalagemFormularioComponent } from '../views/manter-tipo-embalagem/formulario/formulario-tipo-embalagem.component';
import { ManterRecintoAlfandegasComponent } from '../views/manter-recinto-alfandega/manter-recinto-alfandega.component';
import { FormularioManteRecintoAlfandegaComponent } from '../views/manter-recinto-alfandega/formulario/formulario-manter-recinto-alfandega.component';
import { ManterSetorArmazenamentoComponent } from '../views/manter-setor-armazenamento/manter-setor-armazenamento.component';
import { FormularioSetorArmazenamentoComponent } from '../views/manter-setor-armazenamento/formulario/formulario.component';
import { ManterListagemExportacaoComponent } from '../views/manter-listagem-exportacao/manter-listagem-exportacao.component';
import { ManterLEProdutoFormularioComponent } from '../views/manter-listagem-exportacao/formulario/formulario.component';
import { ManterLEInsumoFormularioComponent } from '../views/manter-listagem-exportacao/formulario/formularioInsumo.component';
import { EstruturaPropriaLEComponent } from '../views/estrutura-propria-le/estrutura-propria-le.component';
import { AnalisarListagemExportacaoComponent } from '../views/analisar-listagem-exportacao/analisar-listagem-exportacao.component';
import { AnalisarLEInsumoFormularioComponent } from '../views/analisar-listagem-exportacao/formulario/formularioInsumo.component';
import { ConsultarListagemExportacaoComponent } from '../views/consultar-listagem-exportacao/consultar-listagem-exportacao.component';
import { ConsultarLEInsumoFormularioComponent } from '../views/consultar-listagem-exportacao/formulario/formularioInsumo.component';
import { ManterConsultarProtocoloEnvioLeFormularioComponent } from '../views/consultar-protocolo-envio/formulario/consultar-protocolo-envio-le.component';
import { ManterConsultarProtocoloEnvioLeErrosFormularioComponent } from '../views/consultar-protocolo-envio/formulario/consultar-protocolo-envio-le-erros.component';
import { ManterPlanoExportacaoComponent } from '../views/manter-plano-exportacao/manter-plano-exportacao.component';
import { ManterPlanoFormularioPlanoComponent } from '../views/manter-plano-exportacao/formulario/formularioPlano.component';
import { ManterPlanoFormularioPropriedadeProdutoComponent } from '../views/manter-plano-exportacao/formulario/formularioPropriedadeProduto.component';
import { ManterPlanoFormularioPropriedadeProdutoComprovacaoComponent } from '../views/manter-plano-exportacao/formulario/formularioPropriedadeProdutoComprovacao.component';
import { ManterPlanoFormularioQuadrosInsumosComponent } from '../views/manter-plano-exportacao/formulario/formularioQuadrosInsumos.component';
import { ManterPlanoFormularioDetalhesInsumosComponent } from '../views/manter-plano-exportacao/formulario/formularioDetalhesInsumos.component';
import { PlanoDeExportacaoComponent } from '../views/plano-de-exportacao/plano-de-exportacao.component';
import { ConsultarProcessoExportacaoComponent } from '../views/consultar-processo-exportacao/consultar-processo-exportacao.component';
import { FormularioAcompanharProcessoComponent } from '../views/consultar-processo-exportacao/formulario/formulario-acompanhar-processo.component';
import { ConsultarFormularioPropriedadeProdutoComponent } from '../views/consultar-processo-exportacao/formulario/formulario-propriedade-produto.component';
import { ConsultarFormularioPropriedadeProdutoComprovacaoComponent } from '../views/consultar-processo-exportacao/formulario/formulario-propriedade-produto-comprovacao.component';
import { ConsultarFormularioPropriedadeProdutoComprovacaoSuframaComponent } from '../views/consultar-processo-exportacao-suframa/formulario/formulario-propriedade-produto-comprovacao-suframa.component';
import { ConsultarFormularioQuadrosInsumosComponent } from '../views/consultar-processo-exportacao/formulario/formulario-quadros-insumos.component';
import { EstruturaPropriaPEComponent } from '../views/estrututra-propria-pe/estrutura-propria-pe.component';
import { AnalisePlanoFormularioPlanoComponent } from '../views/plano-de-exportacao/analise-plano-exportacao/formulario-analise-plano.component';
import { AnalisarFormularioPropriedadeProdutoComponent } from '../views/plano-de-exportacao/analise-plano-exportacao/propriedade-produto/formulario-analisar-propriedade-produto.component';
import { AnalisarFormularioQuadrosInsumosComponent } from '../views/plano-de-exportacao/analise-plano-exportacao/quadros/formulario-analisar-quadro-Insumos.component';
import { RelatorioCertificadoRegistroComponent } from '../views/consultar-processo-exportacao/relatorio/relatorio-certificado-registro.component';
import { ManterConsultarProtocoloEnvioPlanoFormularioComponent } from '../views/consultar-protocolo-envio/formulario/consultar-protocolo-envio-plano.component';
import { ManterConsultarProtocoloEnvioPlanoErrosFormularioComponent } from '../views/consultar-protocolo-envio/formulario/consultar-protocolo-envio-plano-erros.component';
import { RelatorioParecerTecnicoComponent } from '../views/consultar-processo-exportacao/modal/grid-modal-parecer/relatorio/relatorio-parecer-tecnico.component';
import { SolicitacoesAlteracaoComponent } from '../views-components/minhas-solicitacoes-alteracao/solicitacoes-alteracao.component';
import { DetalheSolicitacaoComponent } from '../views-components/minhas-solicitacoes-alteracao/detalhes-solicitacao/detalhe-solicitacao.component';
import { ConsultarPlanoFormularioDetalhesInsumosComponent } from '../views/consultar-processo-exportacao/formulario/formulario-detalhes-insumos.component';
import { ConsultarProcessoExportacaoSuframaComponent } from '../views/consultar-processo-exportacao-suframa/consultar-processo-exportacao-suframa.component';
import { FormularioAcompanharProcessoSuframaComponent } from '../views/consultar-processo-exportacao-suframa/formulario/formulario-acompanhar-processo-suframa.component';
import { RelatorioCertificadoRegistroSuframaComponent } from '../views/consultar-processo-exportacao-suframa/relatorio/relatorio-certificado-registro-suframa.component';
import { ConsultarFormularioPropriedadeProdutoSuframaComponent } from '../views/consultar-processo-exportacao-suframa/formulario/formulario-propriedade-produto-suframa.component';
import { ConsultarFormularioQuadrosInsumosSuframaComponent } from '../views/consultar-processo-exportacao-suframa/formulario/formulario-quadros-insumos-suframa.component';
import { RelatorioParecerTecnicoSuframaComponent } from '../views/consultar-processo-exportacao-suframa/modal/grid-modal-parecer/relatorio/relatorio-parecer-tecnico-suframa.component';
import { FormularioDetalhesInsumoSuframaComponent } from '../views/consultar-processo-exportacao-suframa/formulario/formulario-detalhe-processo-insumo-suframa/formulario-detalhes-insumo-suframa.component';
import { ManterPlanoFormularioPlanoComprovacaoComponent } from '../views/manter-plano-exportacao/formulario/formularioPlanoComprovacao.component';
import { AnalisarPlanoFormularioPropriedadeProdutoComprovacaoComponent } from '../views/plano-de-exportacao/analise-plano-exportacao/propriedade-produto/formularioAnalisePropriedadeProdutoComprovacao.component';
import { ManterPlanoFormularioPropriedadeProdutoComprovacaoCorrecaoComponent } from '../views/manter-plano-exportacao/formulario/formularioPropriedadeProdutoComprovacaoCorrecao.component';
import { RelatoriErrosoDueComponent } from '../views/relatorios-due/relatorio-erro-due.component';
import { RelatorioHistoricoComponent } from '../views/relatorios-historico/relatorio-historico.component';
import { RelatorioErrosDueModule } from '../views/relatorios-due/relatorio-erros-due.module';
import { RelatorioAnalisadorDue } from '../views/relatorios-analisador-due/relatorio-analisador-due.component';
import { RelatoriListagensHistoricoInsumoComponent } from '../views/relatorio-listagem-historico-insumos/relatorio-listagem-historico-insumos.component';

const routes: Routes = [
	{ path: 'manter-plano-exportacao', component: ManterPlanoExportacaoComponent },
	{ path: 'manter-plano-exportacao/:id',
		children:[
			{path: 'cadastrar', component: ManterPlanoFormularioPlanoComponent },
			{path: 'cadastrarcomprovacao', component: ManterPlanoFormularioPlanoComprovacaoComponent },
		]
	},
	{ path: 'manter-plano-exportacao/:id/visualizarcomprovacao', component: ManterPlanoFormularioPlanoComprovacaoComponent },
	{ path: 'manter-plano-exportacao/:id/visualizar', component: ManterPlanoFormularioPlanoComponent },
	{ path: 'manter-plano-exportacao/:id/correcao', component: ManterPlanoFormularioPlanoComponent },
	{ path: 'manter-plano-exportacao/:id/correcaoComprovacao', component: ManterPlanoFormularioPlanoComprovacaoComponent },
	{ path: 'manter-plano-exportacao/:id/propriedadeproduto', component: ManterPlanoFormularioPropriedadeProdutoComponent },
	{ path: 'manter-plano-exportacao/:id/propriedadeprodutocomprovacao', component: ManterPlanoFormularioPropriedadeProdutoComprovacaoComponent },
	{ path: 'manter-plano-exportacao/:id/visualizarpropriedadeprodutocomprovacao', component: ManterPlanoFormularioPropriedadeProdutoComprovacaoComponent },
	{ path: 'manter-plano-exportacao/:id/propriedadeprodutocomprovacaocorrecao', component: ManterPlanoFormularioPropriedadeProdutoComprovacaoCorrecaoComponent },
	{ path: 'manter-plano-exportacao/:id/visualizarpropriedadeproduto', component: ManterPlanoFormularioPropriedadeProdutoComponent },
	{ path: 'manter-plano-exportacao/:id/validar-produto', component: ManterPlanoFormularioPlanoComponent },
	{ path: 'manter-plano-exportacao/:id/validar-insumo', component: ManterPlanoFormularioPlanoComponent },
	{ path: 'manter-plano-exportacao/:id/validar-propriedadeproduto', component: ManterPlanoFormularioPropriedadeProdutoComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/quadro-nacional', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/quadro-nacional-correcao', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/quadro-nacional-visualizar', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/quadro-importado', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/quadro-importado-correcao', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/quadro-importado-visualizar', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/validar-quadro-nacional', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-quadros-insumos/:id/validar-quadro-importado', component: ManterPlanoFormularioQuadrosInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/detalhe-nacional', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/detalhe-nacional-correcao', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/detalhe-nacional-visualizar', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/detalhe-importado', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/detalhe-importado-correcao', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/detalhe-importado-visualizar', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/validar-detalhe-nacional', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos/:id/validar-detalhe-importado', component: ManterPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes-insumos-acompanhar/:id/detalhe-importado-visualizar', component: ConsultarPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes/:id/analisar-detalhe-insumos', component: ConsultarPlanoFormularioDetalhesInsumosComponent },
	{ path: 'manter-plano-exportacao-detalhes/:id/editar-detalhe-insumos-novo', component: ConsultarPlanoFormularioDetalhesInsumosComponent },
	{ path: 'relatorio-historico', component: RelatorioHistoricoComponent },
	{ path: 'relatorio-erro-due', component: RelatoriErrosoDueComponent },
	{ path: 'relatorio-analisador-due', component: RelatorioAnalisadorDue },

	{ path: 'consultar-protocolo-envio', component: ManterConsultarProtocoloEnvioComponent },
	{
		path: 'consultar-protocolo-envio/:id/:status',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterConsultarProtocoloEnvioPliFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterConsultarProtocoloEnvioPliFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio-le/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterConsultarProtocoloEnvioLeFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio-le/:id/:status',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterConsultarProtocoloEnvioLeFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio-le/:id/:idSolicitacaoLeInsumo',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar-erros', component: ManterConsultarProtocoloEnvioLeErrosFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio-plano/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterConsultarProtocoloEnvioPlanoFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio-plano/:id/:status',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterConsultarProtocoloEnvioPlanoFormularioComponent },
		]
	},
	{
		path: 'consultar-protocolo-envio-plano/:id/:loteId',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar-erros', component: ManterConsultarProtocoloEnvioPlanoErrosFormularioComponent },
		]
	},

	{
		path: 'consultar-pli/:id',
		children: [
			{ path: 'relatorio-status-ali', component: ManterConsultarListagemErroAliComponent },
		]
	},
	{ path: 'manter-monitoramento-siscomex', component: ManterMonitoramentoSiscomex },
	{ path: 'manter-monitoramento-siscomex/pesquisar', component: ManterMonitoramentoSiscomex },
	{
		path: 'ManterMonitoramentoSiscomex/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterMonitoramentoSiscomex },
			{ path: 'editar', component: ManterMonitoramentoSiscomex }
		]
	},
	{
		path: 'consultar-protocolo-envio/:id/:idsolicitacaopli',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar-erros', component: ManterConsultarProtocoloEnvioPliErrosFormularioComponent },
		]
	},
	{ path: 'manter-cancelar-li', component: ManterCancelarLiComponent },

	/*-----------------------------------------------------------------------------------------------*/
	{ path: 'manter-cancelar-li/:id/visualizar-detalhamento-pli', component: CancelaLIFormularioPLIFormularioComponent },
	{ path: 'manter-cancelar-li/:id/visualizar-mercadoria-pli', component: ManterConsultarPliMercadoriasFormularioComponent },
	{ path: 'manter-cancelar-li/:id/visualizar-detalhamento-li', component: CancelaLIFormularioLIFormularioComponent },
	{ path: 'manter-cancelar-li/:id/visualizar-detalhamento-ali', component: CancelaLIFormularioALIFormularioComponent },
	{ path: 'manter-cancelar-li/:id/visualizar-detalhamento-li-substitutivo', component: CancelaLIFormularioLISubstitutivoFormularioComponent },
	{ path: 'manter-cancelar-li/:id/visualizar-detalhamento-ali-substitutivo', component: CancelaLIFormularioALISubstitutivoFormularioComponent },
	/*-----------------------------------------------------------------------------------------------*/
	{
		path: 'manter-cancelar-li/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' }
		]
	},
	{ path: 'manter-cancelamento-li', component: ManterCancelamentoLiComponent },
	{ path: 'manter-ncm-excecao', component: ManterNCMExcecaoComponent },
	{ path: 'manter-ncm-excecao/pesquisar', component: ManterNCMExcecaoComponent },
	{ path: 'manter-ncm-excecao/cadastrar', component: ManterNCMExcecaoFormularioComponent },
	{
		path: 'manter-ncm-excecao/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' }
		]
	},
	{ path: 'manter-ncm', component: ManterNcmComponent },
	{ path: 'manter-ncm/pesquisar', component: ManterNcmComponent },
	{ path: 'manter-ncm/cadastrar', component: ManterNcmFormularioComponent },
	{
		path: 'manter-ncm/:id',
		children: [
			{ path: 'editar', component: ManterNcmFormularioComponent}
		]
	},
	{ path: 'unidadeReceitaFederal', component: ManterUnidadeReceitaFederalComponent },
	{ path: 'unidadeReceitaFederal/pesquisar', component: ManterUnidadeReceitaFederalComponent },
	{ path: 'unidadeReceitaFederal/cadastrar', component: ManterUnidadeReceitaFederalFormularioComponent },
	{
		path: 'unidadeReceitaFederal/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterUnidadeReceitaFederalFormularioComponent },
			{ path: 'editar', component: ManterUnidadeReceitaFederalFormularioComponent }
		]
	},
	{ path: 'manter-controle-importacao', component: ManterControleImportacaoComponent },
	{ path: 'manter-controle-importacao/pesquisar', component: ManterControleImportacaoComponent },
	{ path: 'manter-controle-importacao/cadastrar', component: ManterControleImportacaoFormularioComponent },
	{
		path: 'manter-controle-importacao/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' }
		]
	},
	{ path: 'manter-regime-tributario-mercadoria', component: ManterRegimeTributarioMercadoriaComponent },
	{ path: 'manter-regime-tributario-mercadoria/pesquisar', component: ManterRegimeTributarioMercadoriaComponent },
	{ path: 'manter-regime-tributario-mercadoria/cadastrar', component: ManterRegimeTributarioMercadoriaFormularioComponent },
	{
		path: 'manter-regime-tributario-mercadoria/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' }
		]
	},
	{ path: 'manter-pli', component: ManterPliComponent },
	{ path: 'manter-pli/pesquisar', component: ManterPliComponent },
    { path: 'manter-pli/cadastrar', canActivate: [AuthGuard], component: ManterPliFormularioComponent },
	{
		path: 'manter-pli/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterPliFormularioComponent },
            { path: 'editar', component: ManterPliFormularioComponent },
			{ path: 'visualizarsubstitutivo', component: ManterPliFormularioSubstutivoComponent },
			{ path: 'cadastrarsubstitutivo', component: ManterPliFormularioSubstutivoComponent },
			{ path: 'editarsubstitutivo', component: ManterPliFormularioSubstutivoComponent },
			{ path: 'cadastrarcomercializacao', component: ManterPliFormularioComercializacaoComponent },
			{ path: 'visualizarcomercializacao', component: ManterPliFormularioComercializacaoComponent },
			{ path: 'editarcomercializacao', component: ManterPliFormularioComercializacaoComponent },
			{ path: 'cadastrarcomercializacaosubstitutivo', component: ManterPliFormularioComercializacaoSubstitutivoComponent },
			{ path: 'visualizarcomercializacaosubstitutivo', component: ManterPliFormularioComercializacaoSubstitutivoComponent },
			{ path: 'editarcomercializacaosubstitutivo', component: ManterPliFormularioComercializacaoSubstitutivoComponent },
			{ path: 'visualizarretificadora', component: ManterPliFormularioRetificadorComponent },
			{ path: 'cadastrarretificadora', component: ManterPliFormularioRetificadorComponent },
			{ path: 'editarretificadora', component: ManterPliFormularioRetificadorComponent },
			//
			{ path: 'visualizarretificadoracomercializacao', component: ManterPliFormularioRetificadorComercializacaoComponent },
			{ path: 'cadastrarretificadoracomercializacao', component: ManterPliFormularioRetificadorComercializacaoComponent },
			{ path: 'editarretificadoracomercializacao', component: ManterPliFormularioRetificadorComercializacaoComponent },
		]
	},
	{ path: 'manter-codigo-utilizacao', component: ManterCodigoUtilizacaoComponent },
	{ path: 'manter-codigo-utilizacao/pesquisar', component: ManterCodigoUtilizacaoComponent },
	{ path: 'manter-codigo-utilizacao/cadastrar', component: ManterCodigoUtilizacaoFormularioComponent },
	{
		path: 'manter-codigo-utilizacao/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterCodigoUtilizacaoFormularioComponent },
			{ path: 'editar', component: ManterCodigoUtilizacaoFormularioComponent }
		]
	},
	{ path: 'manter-codigo-conta', component: ManterCodigoContaComponent },
	{ path: 'manter-codigo-conta/pesquisar', component: ManterCodigoContaComponent },
	{ path: 'manter-codigo-conta/cadastrar', component: ManterCodigoContaFormularioComponent },
	{
		path: 'manter-codigo-conta/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterCodigoContaFormularioComponent },
			{ path: 'editar', component: ManterCodigoContaFormularioComponent }
		]
	},
	{ path: 'manter-tipo-declaracao', component: ManterTipoDeclaracaoComponent },
	{ path: 'manter-tipo-declaracao/pesquisar', component: ManterTipoDeclaracaoComponent },
	{ path: 'manter-tipo-declaracao/cadastrar', component: ManterTipoDeclaracaoFormularioComponent },
	{
		path: 'manter-tipo-declaracao/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterTipoDeclaracaoFormularioComponent },
			{ path: 'editar', component: ManterTipoDeclaracaoFormularioComponent }
		]
	},
	{ path: 'parametros', component: ManterParametrosComponent },
	{ path: 'parametros/pesquisar', component: ManterParametrosComponent },
	{ path: 'parametros/cadastrar', component: ManterParametrosFormularioComponent },
	{
		path: 'parametros/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterParametrosFormularioComponent },
			{ path: 'editar', component: ManterParametrosFormularioComponent }
		]
	},
	{ path: 'aladi', component: ManterAladiComponent },
	{ path: 'aladi/pesquisar', component: ManterAladiComponent },
	{ path: 'aladi/cadastrar', component: ManterAladiFormularioComponent },
	{
		path: 'aladi/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterAladiFormularioComponent },
			{ path: 'editar', component: ManterAladiFormularioComponent }
		]
	},

	{ path: 'naladi', component: ManterNaladiComponent },
	{ path: 'naladi/pesquisar', component: ManterNaladiComponent },
	{ path: 'naladi/cadastrar', component: ManterNaladiFormularioComponent },
	{
		path: 'naladi/:id',
		children: [
			//{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterNaladiFormularioComponent },
			{ path: 'editar', component: ManterNaladiFormularioComponent }
		]
	},
	{ path: 'consultar-analisevisual', component: ManterAnaliseVisualComponent },
	{ path: 'consultar-analisevisual/:id',
		children: [
			{ path: 'analisar', component: ManterAnaliseVisualFormularioComponent }
		]
	},
	{ path: 'designar-pli', component: DesignarPliComponent },
	{
		path: 'designar-pli/:id',
		children: [
			{ path: 'consultarPli', component: DesignarPliComponent },
			{ path: 'consultarLe', component: DesignarPliComponent },
			{ path: 'consultarPe', component: DesignarPliComponent },
			{ path: 'consultarSolic', component: DesignarPliComponent },
		]
	},
	{ path: 'consultar-pli', component: ManterConsultarPliComponent },
	{ path: 'consultar-pli/pesquisar', component: ManterConsultarPliGridComponent },
	{
		path: 'consultar-pli/:id',
		children: [
			{ path: 'visualizar', component: ManterConsultarPliFormularioComponent },
			{ path: 'visualizar-mercadoria-pli', component: ManterConsultarPliMercadoriasFormularioComponent },
			{ path: 'relatorio-status', component: ManterConsultarRelatorioStatusPliFormularioComponent },
			{ path: 'visualizar-detalhamento-ali-substitutivo', component: ManterConsultarPliMercadoriasDetalheALISubstitutivoFormularioComponent },
			{ path: 'visualizar-detalhamento-li', component: ManterConsultarPliMercadoriasDetalheLIFormularioComponent },
			{ path: 'visualizar-detalhamento-li-substitutivo', component: ManterConsultarPliMercadoriasDetalheLISubstitutivoFormularioComponent },
			{ path: 'visualizar-detalhamento-pli', component: ManterConsultarPliFormularioComponent },
			{ path: 'visualizar-relatorio-listagem-erro-pli', component: ManterConsultarListagemErroPliComponent },
			{ path: 'visualizar-detalhamento-di', component: ManterConsultarDiFormularioComponent },
			{ path: 'visualizar-detalhamento-di-adicoes', component: ManterDetalhamentoDiAdicoesFormularioComponent },
		]
	},
	{ path: 'consultar-pli/:id/visualizar-detalhamento-ali/:tela', component: ManterConsultarPliMercadoriasDetalheALIFormularioComponent },
	{
		path: 'consultar-pli/:id/:idmercadoria',
		children: [
			{ path: 'visualizar-mercadoria-descricao', component: ManterConsultarPliMercadoriasDescricaoFormularioComponent },
			{ path: 'visualizar-mercadoria-detalhe', component: ManterConsultarPliMercadoriasDetalheFormularioComponent },
			{ path: 'visualizar-mercadoria-fornecedor', component: ManterConsultarPliMercadoriasFornecedorFormularioComponent },
			{ path: 'visualizar-mercadoria-negociacao', component: ManterConsultarPliMercadoriasNegociacaoFormularioComponent }
		]
	},
	{
		path: 'consultar-pli/:id/:iddi',
		children: [
			{ path: 'visualizar-detalhamento-adicoes-detalhe', component: ManterDetalhamentoAdicoesDetalheFormularioComponent },
		]
	},
	{
		path: 'consultar-pli/:id/:idmercadoria/:iddetalhe',
		children: [
			{ path: 'visualizar-detalhe-item-mercadoria', component: ManterConsultarDetalheItemMercadoriaFormularioComponent }
		]
	},

	{ path: 'paridade-cambial', component: ManterParidadeCambialComponent },
	{ path: 'paridade-cambial/pesquisar', component: ManterParidadeCambialComponent },
	{ path: 'paridade-cambial/cadastrar', component: ManterParidadeCambialFormularioComponent },
	{
		path: 'paridade-cambial/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterParidadeCambialFormularioComponent },
			{ path: 'editar', component: ManterParidadeCambialFormularioComponent }
		]
	},
	{ path: 'via-transporte', component: ManterViaTransporteComponent },
	{ path: 'via-transporte/pesquisar', component: ManterViaTransporteComponent },
	{ path: 'via-transporte/cadastrar', component: ManterViaTransporteFormularioComponent },
	{
		path: 'via-transporte/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterViaTransporteFormularioComponent },
			{ path: 'editar', component: ManterViaTransporteFormularioComponent }
		]
	},
	{ path: 'parametrizarAnalista', component: ManterParametrizarAnalistaComponent },
	{ path: 'fabricante', component: ManterFabricanteComponent },
	{ path: 'fabricante/pesquisar', component: ManterFabricanteComponent },
	{ path: 'fabricante/cadastrar', component: ManterFabricanteFormularioComponent },
	{
		path: 'fabricante/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterFabricanteFormularioComponent },
			{ path: 'editar', component: ManterFabricanteFormularioComponent }
		]
	},
	{ path: 'regime-tributario', component: ManterRegimeTributarioComponent },
	{ path: 'regime-tributario/pesquisar', component: ManterRegimeTributarioComponent },
	{ path: 'regime-tributario/cadastrar', component: ManterRegimeTributarioFormularioComponent },
	{
		path: 'regime-tributario/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterRegimeTributarioFormularioComponent },
			{ path: 'editar', component: ManterRegimeTributarioFormularioComponent }
		]
	},

	{ path: 'fornecedor', component: ManterFornecedorComponent },
	{ path: 'fornecedor/pesquisar', component: ManterFornecedorComponent },
	{ path: 'fornecedor/cadastrar', component: ManterFornecedorFormularioComponent },
	{
		path: 'fornecedor/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterFornecedorFormularioComponent },
			{ path: 'editar', component: ManterFornecedorFormularioComponent }
		]
	},
	{ path: 'fundamento-legal', component: ManterFundamentoLegalComponent },
	{ path: 'fundamento-legal/pesquisar', component: ManterFundamentoLegalComponent },
	{ path: 'fundamento-legal/cadastrar', component: ManterFundamentoLegalFormularioComponent },
	{
		path: 'fundamento-legal/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ManterFundamentoLegalFormularioComponent },
			{ path: 'editar', component: ManterFundamentoLegalFormularioComponent }
		]
	},
	//Sprint02
	{ path: 'grupo-beneficio', component: ManterGrupoBeneficioComponent},
	{ path: 'grupo-beneficio/cadastrar', component: CadastrarBeneficioComponent},
	{ path: 'grupo-beneficio/:id/alterar', component: AlterarBeneficioComponent},
	{ path: 'grupo-beneficio/:id/cadastrar-ncm-beneficio', component: CadastrarNCMBeneficioComponent},
	//-----------
	{ path: 'sincronizar-paridade-cambial', component: SincronizarParidadeCambialComponent },

	{ path: 'usuario-papel', component: UsuarioPapelComponent },
	{ path: '', component: IndexComponent, pathMatch: 'full' },
	{ path: 'estrutura-propria-pli', component: EstruturaPropriaPLIComponent },
	{ path: 'estrutura-propria-pe', component: EstruturaPropriaPEComponent },
	{ path: 'estrutura-propria-le', component: EstruturaPropriaLEComponent },


	{ path: 'consultar-entrada-di', component: ConsultarEntradaDiComponent },
	{
		path: 'consultar-entrada-di-processado/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ConsultarEntradaDiProcessadoFormularioComponent },
		]
	},
	{
		path: 'consultar-entrada-di-processado/:id/:situacaoLeitura',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar', component: ConsultarEntradaDiProcessadoFormularioComponent },
		]
	},
	{
		path: 'consultar-entrada-di-erros/:id',
		children: [
			{ path: '', redirectTo: 'visualizar', pathMatch: 'full' },
			{ path: 'visualizar-erros', component: ConsultarEntradaDiErrosFormularioComponent },
		]
	},

	{ path: 'manter-tipo-embalagem', component: ManterTipoEmabalagemComponent },
	{ path: 'formulario-tipo-embalagem/novo', component: ManteTipoEmbalagemFormularioComponent },
	{ path: 'formulario-tipo-embalagem/:id/:tipoOperacao', component: ManteTipoEmbalagemFormularioComponent },

	{ path: 'manter-recinto-alfandega', component: ManterRecintoAlfandegasComponent },
	{ path: 'formulario-recinto-alfandega/novo', component: FormularioManteRecintoAlfandegaComponent },
	{ path: 'formulario-recinto-alfandega/:id/:tipoOperacao', component: FormularioManteRecintoAlfandegaComponent },

	{ path: 'manter-setor-armazenamento', component: ManterSetorArmazenamentoComponent },
	{ path: 'formulario-setor-armazenamento/novo', component: FormularioSetorArmazenamentoComponent },
	{ path: 'formulario-setor-armazenamento/:id/:tipoOperacao', component: FormularioSetorArmazenamentoComponent },

	{ path: 'manter-listagem-exportacao', component: ManterListagemExportacaoComponent },
	{ path: 'manter-listagem-exportacao/cadastrar', component: ManterLEProdutoFormularioComponent },
	{
		path: 'manter-listagem-exportacao/:id',
		children: [
			{ path: 'visualizar', component: ManterLEInsumoFormularioComponent },
			{ path: 'editar', component: ManterLEInsumoFormularioComponent },
			{ path: 'corrigir', component: ManterLEInsumoFormularioComponent },
		]
	},
	{ path: 'analisar-listagem-exportacao', component: AnalisarListagemExportacaoComponent },
	{
		path: 'analisar-listagem-exportacao/:id',
		children:[
			{path: 'analisar', component: AnalisarLEInsumoFormularioComponent}
	]
	},
	{ path: 'consultar-listagem-exportacao', component: ConsultarListagemExportacaoComponent },
	{
		path: 'consultar-listagem-exportacao/:id',
		children:[
			{path: 'consultar', component: ConsultarLEInsumoFormularioComponent}
		]
	},
	{
		path: 'plano-de-exportacao' , component: PlanoDeExportacaoComponent
	},
	{
		path: 'analisar-plano-exportacao/:id',
		children:[
			{path: 'analisar-info-plano', component: AnalisePlanoFormularioPlanoComponent},
			{path: 'visualizar-info-plano', component: AnalisePlanoFormularioPlanoComponent},
			{path: 'analisar-info-plano-comp', component: AnalisePlanoFormularioPlanoComponent},
			{path: 'visualizar-info-plano-comp', component: AnalisePlanoFormularioPlanoComponent},
			{path: 'analisar-propriedade-produto', component: AnalisarFormularioPropriedadeProdutoComponent},
			{path: 'visualizar-propriedade-produto', component: AnalisarFormularioPropriedadeProdutoComponent},
			{path: 'analisar-propriedade-produto-comp', component: AnalisarPlanoFormularioPropriedadeProdutoComprovacaoComponent},
			{path: 'visualizar-propriedade-produto-comp', component: AnalisarPlanoFormularioPropriedadeProdutoComprovacaoComponent},
			{path: 'analisar-quadro-nacional', component: AnalisarFormularioQuadrosInsumosComponent},
			{path: 'visualizar-quadro-nacional', component: AnalisarFormularioQuadrosInsumosComponent},
			{path: 'analisar-quadro-importado', component: AnalisarFormularioQuadrosInsumosComponent},
			{path: 'visualizar-quadro-importado', component: AnalisarFormularioQuadrosInsumosComponent},
		]
	},
	{ path: 'consultar-processo-exportacao', component: ConsultarProcessoExportacaoComponent },
	{ path: 'consultar-processo-exportacao/:idProcesso',
		children:[
			{path: 'visualizar', component: FormularioAcompanharProcessoComponent},
			{path: 'visualizar-propriedade-produto', component: ConsultarFormularioPropriedadeProdutoComponent},
			{path: 'visualizar-propriedade-produto-comprovacao', component: ConsultarFormularioPropriedadeProdutoComprovacaoComponent},
			{path: 'visualizar-quadro-nacional', component: ConsultarFormularioQuadrosInsumosComponent},
			{path: 'visualizar-quadro-importado', component: ConsultarFormularioQuadrosInsumosComponent},
		]
	},
	{ path: 'consultar-detalhe-processo-insumo/:idInsumo',
		children: [
			{ path: 'visualizar-quadro-importado', component: FormularioDetalhesInsumoSuframaComponent}
		]
	},
	{ path: 'consultar-processo-exportacao-suframa', component: ConsultarProcessoExportacaoSuframaComponent },
	{ path: 'consultar-processo-exportacao-suframa/:idProcesso',
		children:[
			{path: 'visualizar', component: FormularioAcompanharProcessoSuframaComponent}
		]
	},
	{ path: 'consultar-processo-exportacao-suframa/:idProduto',
		children:[
			{path: 'visualizar-propriedade-produto', component: ConsultarFormularioPropriedadeProdutoSuframaComponent},
			{path: 'visualizar-propriedade-produto-comprovacao', component: ConsultarFormularioPropriedadeProdutoComprovacaoSuframaComponent},
			{path: 'visualizar-quadro-nacional', component: ConsultarFormularioQuadrosInsumosSuframaComponent},
			{path: 'visualizar-quadro-importado', component: ConsultarFormularioQuadrosInsumosSuframaComponent},
		]
	},
	{ path: 'relatorio-parecer-tecnico/:id',
		children:[
			{path: 'visualizar', component: RelatorioParecerTecnicoComponent},
		]
	},
	{ path: 'relatorio-parecer-tecnico-suframa/:id',
		children:[
			{path: 'visualizar', component: RelatorioParecerTecnicoSuframaComponent},
		]
	},
	{ path: 'relatorio-certificado-registro', component: RelatorioCertificadoRegistroComponent },
	{ path: 'relatorio-certificado-registro/:idProcesso',
		children:[
			{path: 'visualizar', component: RelatorioCertificadoRegistroComponent},
		]
	},
	{ path: 'relatorio-certificado-registro-suframa', component: RelatorioCertificadoRegistroSuframaComponent },
	{ path: 'relatorio-certificado-registro-suframa/:idProcesso',
		children:[
			{path: 'visualizar', component: RelatorioCertificadoRegistroSuframaComponent},
		]
	},
	{
		path : 'minhas-solicitacoes-alteracao/:idProcesso',
		children:[
			{path: 'visualizar-quadro-importado', component: SolicitacoesAlteracaoComponent}
		]
	},
	{
		path : 'minhas-solicitacoes-alteracao', component: SolicitacoesAlteracaoComponent
	},
	{
		path : 'detalhe-minha-solicitacao/:idSolicitacao' , component: DetalheSolicitacaoComponent
	},
	{
		path : 'relatorio-listagem-historico-insumos' , component: RelatoriListagensHistoricoInsumoComponent
	},
	{ path: '', component: IndexComponent, pathMatch: 'full' },

	{ path: '**', redirectTo: '' }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }
