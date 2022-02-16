import { Injectable } from '@angular/core';
import { ApplicationService } from './application.service';
import { ExtractNumberService } from './extract-number.service';
import { identificacaoPessoaFisicaVM } from '../../view-model/IdentificacaoPessoaFisicaVM';
import { identificacaoPessoaJuridicaVM } from '../../view-model/IdentificacaoPessoaJuridicaVM';

@Injectable()
export class PessoaService {
	constructor(private applicationService: ApplicationService, private extractNumberService: ExtractNumberService) { }

	public selecionar(cpfCnpj: string) {
		if (!cpfCnpj) { return; }

		if (this.extractNumberService.extractNumbers(cpfCnpj).toString().length <= 11) {
			return this.pessoaFisica(cpfCnpj);
		} else {
			return this.pessoaJuridica(cpfCnpj);
		}
	}

	private pessoaFisica(cpfCnpj: string) {
		const parametros = new identificacaoPessoaFisicaVM();
		parametros.cpf = cpfCnpj;
		return this.applicationService.get<any>('IdentificacaoPessoaFisica', parametros);
	}

	private pessoaJuridica(cpfCnpj: string) {
		const parametros = new identificacaoPessoaJuridicaVM();
		parametros.cnpj = cpfCnpj;
		return this.applicationService.get<any>('IdentificacaoPessoaJuridica', parametros);
	}
}
