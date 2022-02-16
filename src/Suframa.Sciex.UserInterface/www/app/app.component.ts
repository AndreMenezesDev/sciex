import { Component, OnInit, ViewEncapsulation, Input, Injectable } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser'; // https://angular.io/guide/set-document-title // https://angular.io/api/platform-browser/Meta
import { ApplicationService } from "./shared/services/application.service";
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})

@Injectable()
export class AppComponent {
    public constructor(
        private titleService: Title,
        private Meta: Meta,
        private applicationService: ApplicationService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    private sub: any;

    usuario: string;
    senha: string;
    mensagem: string;
    isExibirLogin: boolean = false;

    public setTitle(newTitle: string) {
        this.titleService.setTitle(newTitle);
    }

    @Input()
    authenticated: boolean;

	ngOnInit() {
        this.Meta.addTag({ name: 'description', content: 'Sistema de Importação e Exportaçao de Mercadoria Estrangeira' });
        this.Meta.addTag({ name: 'keywords', content: 'ingresso, suframa, sciex, mercadoria, nacional' });
		this.titleService.setTitle("SCIEX");

		if(!window.location.hash.includes("visualizar-mercadoria-pli") && !window.location.hash.includes("visualizar-detalhamento-li") && !window.location.hash.includes("visualizar-detalhamento-di") && !window.location.hash.includes("consultar-analisevisual") ){
          //localStorage.clear();
          this.applicationService.removeToken();

        }

        this.isExibirLogin = environment.developmentMode;

        let hash = window.location.hash.toLowerCase();
        var menuExterno = hash.indexOf("#/menu-externo") == 0;
        var inscricao = hash.indexOf("#/inscricao") == 0;

        this.authenticated = (menuExterno || inscricao);
        //
        document.body.style.backgroundImage = "url('assets/images/bg.png')";

        if (window.location.search.startsWith("?ticket=")) {
        
            window.location.replace(environment.simnacUrl);
        }else if (window.location.search.startsWith("?token=")){
            var token = window.location.search.substr(7);
            this.applicationService.setToken(token);
            this.authenticated = true;
        }else if(window.location.hash.includes("visualizar-mercadoria-pli") || window.location.hash.includes("visualizar-detalhamento-li") || window.location.hash.includes("visualizar-detalhamento-di")){
            var index = window.location.hash.indexOf('%3Ftoken%3D');
            var token = window.location.hash.substring(index+11);
            this.applicationService.setToken(token);
            this.authenticated = true;
            var rota = window.location.hash.substring(1,index);
            this.router.navigate([rota]);
        }
        else  {
            if(!environment.developmentMode)
            window.location.replace(environment.pssUrl);
        }
    }

    onLogout() {
    }

	

    click() {

	
        this.applicationService
            .get("Login", { usuario: this.usuario, senha: this.senha })
            .subscribe(
            (data: any) => {
                this.mensagem = "Login realizado com sucesso";
                this.applicationService.setToken(data.token);
                localStorage.setItem('teste','teste');
                //this.onLogon.emit();
                this.authenticated = true;
                //location.reload();
            },
            (err: any) => {
                this.mensagem = "Ocorreu um erro ao autenticar usuário";
            });
    }
}
