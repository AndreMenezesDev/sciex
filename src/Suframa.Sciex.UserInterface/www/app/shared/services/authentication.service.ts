import { Injectable } from '@angular/core';
import { ApplicationService } from './application.service';

@Injectable()
export class AuthenticationService {
	authenticated = false;
	isUsuarioInterno = false;
	CNPJ: string;

	constructor(private applicationService: ApplicationService) {
		/*this.applicationService.get('UsuarioInternoLogado')
			.subscribe(result => {
				if (result) {
					this.isUsuarioInterno = true;
					this.authenticated = true;
				}
			});
			*/
		
	}


	public obterCNPJEmpresaSelecionada() {
		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {	
			if (result != null)
				this.CNPJ = result;
		});

		return this.CNPJ;
	}

	public IsUsuarioInterno(): boolean {
		return this.isUsuarioInterno;
	}

	public IsAuthenticated(): boolean {
		return this.authenticated;
	}
}
