import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ApplicationService } from '../../shared/services/application.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
	usuario = '22222222222';
	senha: string;
	mensagem: string;

	@Output() onLogon = new EventEmitter;

	constructor(private applicationService: ApplicationService) { }

	ngOnInit() {
		this.senha = 'testeteste';
		localStorage.removeItem("Aladi");
	}

	click() {
		this.applicationService
			.get('Login', { usuario: this.usuario, senha: this.senha })
			.subscribe(
			(data: any) => {
				this.mensagem = 'Login realizado com sucesso';
				this.applicationService.setToken(data.token);
				this.onLogon.emit();
			},
			(err: any) => {
				this.mensagem = 'Ocorreu um erro ao autenticar usuário';
			});
	}
}
