import { Component, OnInit, Input, ViewChild, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApplicationService } from '../../shared/services/application.service';
import { empresaRepresentadaVM } from '../../view-model/EmpresaRepresentadaVM';
import { CnpjPipe } from '../../shared/pipes/cnpj.pipe';
import { CpfPipe } from '../../shared/pipes/cpf.pipe';

@Component({
	selector: 'app-menu',
	templateUrl: './menu.component.html'
})


export class MenuComponent implements OnInit {


	public Pss = [];
	Externo = [];
	MeusDados = [];
	MinhasSolicitacoes = [];
	MeusProtocolos = [];
	Protocolo = [];
	Diligencias = [];
	Inscricoes = [];
	DadosCadastrais = [];
	Perfil = [];
	UsuarioPapel = [];
	Parametros = [];
	Agendamento = [];

	usuario: string;
	cpfcnpj: string;
	@Input()
	Cadastros = [];
	Parametrizacao = [];
	Importador = [];
	Pli = [];
	isEmpresaSelecionada: boolean;
	empresa: string;
	cnpj: string;
	@ViewChild('menu') menu;

	constructor(
		private applicationService: ApplicationService,
	) { }

	ngOnInit() {

		if (environment.developmentMode) {
			this.montarMenuMock();

			this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
				this.usuario = result.usuNomeUsuario;
				this.cpfcnpj = this.formatarLogin(result.usuarioLogadoCpfCnpj);
			});

		} else {
			this.applicationService.get('Menu').subscribe((result: any) => {
				if (result && result.length > 0)
					this.Pss = JSON.parse(result.replace(/\\"/g, '"'));
				for (var i = 0; i < this.Pss.length; i++) {
					this.Pss[i].nome = this.Pss[i].nome.split("/")[1];
				}
			});
		}
		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
			this.usuario = result.usuNomeUsuario;
			this.cpfcnpj = this.formatarLogin(result.usuarioLogadoCpfCnpj);
		});
	}

	formatarLogin(cpfcnpj) {
		if (cpfcnpj) {
			if (cpfcnpj.length == 14)
				return new CnpjPipe().transform(cpfcnpj);
			else
				return new CpfPipe().transform(cpfcnpj);
		}
	}

	montarMenuMock() {
		this.Pss.push({
			nome: 'Importador',
			id: 1,
			funcoesSistema: [{
				nome: 'Fabricante',
				url: '/fabricante',
				descricao: 'Fabricante'
			},
			{
				nome: 'Fornecedor',
				url: '/fornecedor',
				descricao: 'Fornecedor'
			},
			{
				nome: 'Parâmetros',
				url: '/parametros',
				descricao: 'Parâmetros'
			},
			{
				nome: 'Cadastrar PLI',
				url: '/manter-pli',
				descricao: 'Cadastrar PLI'
			},
			{
				nome: 'Cancelar PLI',
				url: '/manter-cancelar-li',
				descricao: 'Cancelar PLI'
			},
			{
				nome: 'Consultar Protocolo de Envio',
				url: '/consultar-protocolo-envio',
				descricao: 'Consultar Protocolo de Envio'
			},
			{
				nome: 'Enviar PLI',
				url: '/estrutura-propria-pli',
				descricao: 'Enviar PLI'
			},
			{
				nome: 'Enviar PE',
				url: '/estrutura-propria-pe',
				descricao: 'Enviar PE'
			},
			{
				nome: 'Enviar LE',
				url: '/estrutura-propria-le',
				descricao: 'Enviar LE'
			}]
		});
		this.Pss.push({
			nome: 'PLI',
			id: 2,
			funcoesSistema: [
				{
				nome: 'Consultar PLI',
				url: '/consultar-pli',
				descricao: 'Consultar PLI'
				},
				{
					nome: 'Análise Visual',
					url: '/consultar-analisevisual',
					descricao: 'Análise Visual'
				},
				{
					nome: 'Designar PLI',
					url: '/designar-pli',
					descricao: 'Designar PLI'
				}
			]
		});
		this.Pss.push({
			nome: 'Parametrização',
			id: 3,
			funcoesSistema: [
				{
					nome: 'Código da Conta',
					url: '/manter-codigo-conta',
					descricao: 'Código da Conta'
				},
				{
					nome: 'Código de Utilização',
					url: '/manter-codigo-utilizacao',
					descricao: 'Código de Utilização'
				},
				{
					nome: 'Grupo de Benefício',
					url: '/grupo-beneficio',
					descricao: 'Grupo de Benefício'
				},
				{
					nome: 'Regime Tributário da Mercadoria',
					url: '/manter-regime-tributario-mercadoria',
					descricao: 'Regime Tributário da Mercadoria'
				},
				{
					nome: 'Controle Importação',
					url: '/manter-controle-importacao',
					descricao: 'Controle Importação'
				},
				{
					nome: 'Manter NCM',
					url: '/manter-ncm',
					descricao: 'Manter NCM'
				}
				,
				{
					nome: 'NCM Exceção',
					url: '/manter-ncm-excecao',
					descricao: 'NCM Exceção'
				}
			]
		});
		this.Pss.push({
			nome: 'Cadastros',
			id: 4,
			funcoesSistema: [{
				nome: 'Cadastro do ALADI',
				url: '/aladi',
				descricao: 'Cadastro do ALADI'
			},
			{
				nome: 'Cadastro do NALADI',
				url: '/naladi',
				descricao: 'Cadastro do NALADI'
			},
			{
				nome: 'Regime Tributário',
				url: '/regime-tributario',
				descricao: 'Regime Tributário'
			},
			{
				nome: 'Cadastro do Unidade da RFB',
				url: '/unidadeReceitaFederal',
				descricao: 'Cadastro do Unidade da RFB'
			},
			{
				nome: 'Fundamento Legal',
				url: '/fundamento-legal',
				descricao: 'Fundamento Legal'
			},
			{
				nome: 'Parametrizar Analista',
				url: '/parametrizarAnalista',
				descricao: 'Parametrizar Analista'
			},
			{
				nome: 'Paridade Cambial',
				url: '/paridade-cambial',
				descricao: 'Paridade Cambial'
			},
			{
				nome: 'Recinto da Alfândega',
				url: '/manter-recinto-alfandega',
				descricao: 'Manter Recinto da Alfândega'
			},
			{
				nome: 'Setor Armazenamento',
				url: '/manter-setor-armazenamento',
				descricao: 'Manter Setor Armazenamento'
			},
			{
				nome: 'Tipo Declaração',
				url: '/manter-tipo-declaracao',
				descricao: 'Cadastro Tipo de Declaração'
			},
			{
				nome: 'Tipo de Embalagem',
				url: '/manter-tipo-embalagem',
				descricao: 'Manter Tipo de Embalagens'
			},
			{
				nome: 'Via de Transporte',
				url: '/via-transporte',
				descricao: 'Via de Transporte'
			}]
		});
		this.Pss.push({
			nome: 'Consulta',
			id: 5,
			funcoesSistema: [{
				nome: 'Consultar Entrada de DI',
				url: '/consultar-entrada-di',
				descricao: 'Consultar Entrada de DI'
			}]
		});
		this.Pss.push({
			nome: 'Exportador',
			id: 6,
			funcoesSistema: [
			{
				nome: 'Cadastrar LE',
				url: '/manter-listagem-exportacao',
				descricao: 'Cadastrar Listagem de Exportação'
			}
			,
			{
				nome: 'Cadastrar Plano de Exportação',
				url: '/manter-plano-exportacao',
				descricao: 'Cadastrar Plano de Exportação'
			}
			,
			{
				nome: 'Minhas Solicitações de Alteração',
				url: '/minhas-solicitacoes-alteracao',
				descricao: 'Consultar Minhas Solicitações de Alteração'
			}]
		});
		this.Pss.push({
			nome: 'LE',
			id: 7,
			funcoesSistema: [{
				nome: 'Analisar LE',
				url: '/analisar-listagem-exportacao',
				descricao: 'Analisar Listagem de Exportação'
			},
			{
				nome: 'Consultar LE',
				url: '/consultar-listagem-exportacao',
				descricao: 'Consultar Listagem de Exportação'
			}
		]
		},
		{
			nome: 'Exportação',
			id: 8,
			funcoesSistema: [
				{
					nome: 'Analisar Plano',
					url: '/plano-de-exportacao',
					descricao: 'Analisar Plano de Exportacao'
				},
				{
				nome: 'Acompanhar Processo Empresa',
				url: '/consultar-processo-exportacao',
				descricao: 'Acompanhar Processo Exportação'
				},
				{
					nome: 'Acompanhar Processo Suframa',
					url: '/consultar-processo-exportacao-suframa',
					descricao: 'Acompanhar Processo Exportação'
				},
				{
				nome: 'Relatorio Teste',
				url: '/relatorio-parecer-tecnico',
				descricao: 'Relatorio Teste'
				}
			]
		}
		);
		this.Pss.push({
			nome: 'Relatorios',
			id: 9,
			funcoesSistema: [
			{
				nome: 'Listagem do Histórico de Processo de Exportação',
				url: '/relatorio-historico',
				descricao: 'Relatório Listagem do Histórico de Processo de Exportação'
			}
			,
			{
				nome: 'Analisador de DU-Es Contidas no Plano',
				url: '/relatorio-analisador-due',
				descricao: 'Analisador de DU-Es Contidas no PlanoCadastrar Plano de Exportação'
			}
			,
			{
				nome: 'Erros nas DU-Es - Comprovação',
				url: '/relatorio-erro-due',
				descricao: 'Erros nas DU-Es - Comprovação'
			},
			{
				nome: 'Histórico de Insumos da Listagem de Exportação',
				url: '/relatorio-listagem-historico-insumos',
				descricao: 'Relatório Listagem do Histórico de Insumos da Listagem de Exportação'
			}
			,
			{
				nome: 'Histórico de Insumos do Processo de Exportação',
				url: '/relatorio-historico-insumos',
				descricao: 'Relatório Histórico de Insumos do Processo de Exportação'
			}
			,
			{
				nome: 'Listagem de Exportação Aprovadas',
				url: '/relatorio-listagem-exportacao',
				descricao: 'Relatório de listagem de exportação aprovadas por exportador'
			}]
		});
	}


	recuperarMenu() {
		var self = this;
		this.applicationService.get('Menu').subscribe((result: any) => {
			this.Pss = [];
			if (result && result.length > 0)
				var novoMenu = [];
			novoMenu = JSON.parse(result.replace(/\\"/g, '"'));
			for (var i = 0; i < novoMenu.length; i++) {
				novoMenu[i].nome = novoMenu[i].nome.split("/")[1];
			}
			this.Pss = novoMenu;

		});
	}

	carregarEmpresaSelecionada(razaoSocial, cnpj) {
		localStorage.clear();
		if (razaoSocial != "Encerrar Representação") {
			this.empresa = razaoSocial;
			this.cnpj = cnpj;
		} else {
			this.empresa = "";
			this.cnpj = "";
		}
	}

	trackElement(index: number, element: any) {
		return element ? element.nome : null;
	}
}
