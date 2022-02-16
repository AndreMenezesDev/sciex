import { Component, OnInit, ViewEncapsulation, Input, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationService } from '../shared/services/application.service';
import { AuthenticationService } from '../shared/services/authentication.service';
import { empresaRepresentadaVM } from '../view-model/EmpresaRepresentadaVM';
import { AppComponent } from '../app.component';
import { MenuComponent } from "./menu/menu.component";
import { environment } from '../../environments/environment';
import { RepresentacaoVM } from "../view-model/RepresentacaoVM";
import { CnpjPipe } from '../shared/pipes/cnpj.pipe';

@Component({
	selector: 'app-layout',
	templateUrl: './layout.component.html'
})

export class LayoutComponent implements OnInit {
	versao: string;
	contraste: boolean;
	aumentar: string;
	CNPJ: string;
	descricaoEmpresa: string;
	@ViewChild('menu') menu;

	@ViewChild(MenuComponent) menutt: MenuComponent

	listaEmpresas: Array<empresaRepresentadaVM>;

	isEmpresaSelecionada: boolean;
	isExibirRepresentacao: boolean = false;
	empresa: any;

	@Input() authenticated: boolean;

	@Input() intranet: boolean;

	username: string;
	razaoSocial: string;
	logoutUrl: string;

	constructor(
		private appSciex: AppComponent,
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private authenticationService: AuthenticationService,
		private router: Router,
	) {
	}

	ngOnInit() {
		this.logoutUrl = environment.logout

		this.applicationService.get("Informacao").subscribe((result: any) => {
			this.versao = result.versao;
		});

		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
			this.username = result.empresaRepresentadaCnpj;
			this.razaoSocial = result.empresaRepresentadaRazaoSocial;
			if (this.username != null && this.username != "") {
				this.isExibirRepresentacao = true;
			}
		});

		const hash = window.location.hash.toLowerCase();
		const menuExterno = hash.indexOf("#/menu-externo") == 0;
		const inscricao = hash.indexOf("#/inscricao") == 0;

		this.intranet = menuExterno || inscricao;
		document.body.style.backgroundImage = ""; // remover o fundo verde do login
	}

	montarMenuEmpresa() {
		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {

			this.listaEmpresas = result.usuario.listaEmpresaRepresentadas;

			if (result.usuario.listaEmpresaRepresentadas == null || result.usuario.listaEmpresaRepresentadas.length == 0) {
				//this.isOcultarRepresentacao = true;
				this.menu.carregarMenu();
			}
		});

		return this.listaEmpresas;//Object.keys(this.listaEmpresas).map(function (key) { return this.listaEmpresas[key]; });
	}

	empresaSelecionada(item) {
		this.isEmpresaSelecionada = false;
		if (item.cnpj != "") {
			localStorage.clear();
			this.isEmpresaSelecionada = true;

			this.empresa = item;
			this.descricaoEmpresa = item.cnpj + " | " + item.razaoSocial;

			if (window.screen.width === 800) {
				this.empresa.razaoSocialAbreviada = item.razaoSocial.length > 0 ? item.razaoSocial.substring(0, 0) + "..." : item.razaoSocial;
			} else if (window.screen.width === 1024) {
				this.empresa.razaoSocialAbreviada = item.razaoSocial.length > 8 ? item.razaoSocial.substring(0, 11) + "..." : item.razaoSocial;
			} else if (window.screen.width === 1280) {
				this.empresa.razaoSocialAbreviada = item.razaoSocial.length > 45 ? item.razaoSocial.substring(0, 42) + "..." : item.razaoSocial;
			} else if (window.screen.width === 1360) {
				this.empresa.razaoSocialAbreviada = item.razaoSocial.length > 48 ? item.razaoSocial.substring(0, 51) + "..." : item.razaoSocial;
			}
			else if (window.screen.width === 1366) {
				this.empresa.razaoSocialAbreviada = item.razaoSocial.length > 49 ? item.razaoSocial.substring(0, 40) + "..." : item.razaoSocial;
			}
			else {
				this.empresa.razaoSocialAbreviada = item.razaoSocial;
			}
			this.CNPJ = item.cnpj.replace(".", "").replace("/", "").replace("-", "");

			this.applicationService.get("TrocaToken", { razaoSocial: item.razaoSocial, cnpj: this.CNPJ.replace(".", "") }).subscribe((result: any) => {
				sessionStorage.removeItem('token');
				this.applicationService.setToken(result);
			});

			this.menu.carregarEmpresaSelecionada(item.razaoSocial, item.cnpj);

			this.menu.carregarMenu();
		}
	}

	recarregarLayout(representacao: RepresentacaoVM) {

		var cnpjFormatado = new CnpjPipe().transform(this.username);
		if (cnpjFormatado != representacao.cnpj) {
		}
		this.menu.carregarEmpresaSelecionada("", "");

		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {

			this.username = result.empresaRepresentadaCnpj;
			this.razaoSocial = representacao.nome;

			this.menu.carregarEmpresaSelecionada(result.empresaRepresentadaRazaoSocial, result.empresaRepresentadaCnpj);

			this.CNPJ = representacao.cnpj;
			if (this.username != null && this.username != "" && this.razaoSocial != "Encerrar Representação") {
				this.isExibirRepresentacao = true;
			} else {
				this.isExibirRepresentacao = false;
			}

			if (window.screen.width === 800) {
				this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 0 ? result.empresaRepresentadaRazaoSocial.substring(0, 0) + "..." : result.empresaRepresentadaRazaoSocial;
			} else if (window.screen.width === 1024) {
				this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 8 ? result.empresaRepresentadaRazaoSocial.substring(0, 11) + "..." : result.empresaRepresentadaRazaoSocial;
			} else if (window.screen.width === 1280) {
				this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 45 ? result.empresaRepresentadaRazaoSocial.substring(0, 42) + "..." : result.empresaRepresentadaRazaoSocial;
			} else if (window.screen.width === 1360) {
				this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 48 ? result.empresaRepresentadaRazaoSocial.substring(0, 51) + "..." : result.empresaRepresentadaRazaoSocial;
			}
			else if (window.screen.width === 1366) {
				this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 49 ? result.empresaRepresentadaRazaoSocial.substring(0, 40) + "..." : result.empresaRepresentadaRazaoSocial;
			}
			else {
				this.razaoSocial = result.empresaRepresentadaRazaoSocial;

				if (window.screen.width === 800) {
					this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 0 ? result.empresaRepresentadaRazaoSocial.substring(0, 0) + "..." : result.empresaRepresentadaRazaoSocial;
				} else if (window.screen.width === 1024) {
					this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 8 ? result.empresaRepresentadaRazaoSocial.substring(0, 11) + "..." : result.empresaRepresentadaRazaoSocial;
				} else if (window.screen.width === 1280) {
					this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 45 ? result.empresaRepresentadaRazaoSocial.substring(0, 42) + "..." : result.empresaRepresentadaRazaoSocial;
				} else if (window.screen.width === 1360) {
					this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 48 ? result.empresaRepresentadaRazaoSocial.substring(0, 51) + "..." : result.empresaRepresentadaRazaoSocial;
				}
				else if (window.screen.width === 1366) {
					this.razaoSocial = result.empresaRepresentadaRazaoSocial.length > 49 ? result.empresaRepresentadaRazaoSocial.substring(0, 40) + "..." : result.empresaRepresentadaRazaoSocial;
				}
				else {
					this.razaoSocial = result.empresaRepresentadaRazaoSocial;
				}

				if (representacao && representacao.isUsuarioLogado) {
					this.username = "";
					this.isExibirRepresentacao = false;
				} else if (this.username != null && this.username != "") {
					this.isExibirRepresentacao = true;
				}
			}


			if (!environment.developmentMode) {
				this.menutt.recuperarMenu();
			}
		});

	}


	onLogon() {
		this.authenticated = true;
	}

	logout() {

		this.applicationService.logout().subscribe((result: any) => {
			window.location.replace(this.logoutUrl);
		});
		this.applicationService.removeToken();
		window.location.replace(this.logoutUrl);
	}

	contrasteFuncao() {
		if (this.contraste == false) {
			this.contraste = true;
		} else {
			this.contraste = false;
		}
	}
}
