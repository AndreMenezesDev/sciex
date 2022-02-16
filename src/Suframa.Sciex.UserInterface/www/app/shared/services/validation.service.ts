import { Injectable } from '@angular/core';
import { ModalService } from './modal.service';
import { ExtractNumberService } from './extract-number.service';
import { MessagesService } from './messages.service';

import { debug } from 'util';

@Injectable()
export class ValidationService {
	constructor(
		private modal: ModalService,
		private extractNumber: ExtractNumberService,
		private msg: MessagesService) { }

	form(id) {		
		if (!id || !document.forms || !document.forms[id] || !document.forms[id].reportValidity) { return true; }
		const form = document.forms[id];
		return form.reportValidity();
	}

	cpf(cpf) {
		cpf = this.extractNumber.extractNumbers(cpf);
		let numeros, digitos, soma, i, resultado, digitos_iguais, retorno = true;
		digitos_iguais = 1;
		if (cpf.length < 11) {
			retorno = false;
		}
		for (i = 0; i < cpf.length - 1; i++) {
			if (cpf.charAt(i) != cpf.charAt(i + 1)) {
				digitos_iguais = 0;
				break;
			}
		}
		if (!digitos_iguais) {
			numeros = cpf.substring(0, 9);
			digitos = cpf.substring(9);
			soma = 0;
			for (i = 10; i > 1; i--) {
				soma += numeros.charAt(10 - i) * i;
			}
			resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
			if (resultado != digitos.charAt(0)) {
				retorno = false;
			}
			numeros = cpf.substring(0, 10);
			soma = 0;
			for (i = 11; i > 1; i--) {
				soma += numeros.charAt(11 - i) * i;
			}
			resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
			if (resultado != digitos.charAt(1)) {
				retorno = false;
			}
		} else {
			retorno = false;
		}

		if (!retorno) {
			this.modal.alerta(this.msg.CPF_INVALIDO);
			return false;
		}

		return true;
	}

	cnpj(cnpj) {
		cnpj = this.extractNumber.extractNumbers(cnpj);

		let retorno = true;

		if (cnpj == '') { retorno = false; }

		if (cnpj.length != 14) {
			retorno = false;
		}

		// Elimina CNPJs invalidos conhecidos
		if (cnpj == '00000000000000' ||
			cnpj == '11111111111111' ||
			cnpj == '22222222222222' ||
			cnpj == '33333333333333' ||
			cnpj == '44444444444444' ||
			cnpj == '55555555555555' ||
			cnpj == '66666666666666' ||
			cnpj == '77777777777777' ||
			cnpj == '88888888888888' ||
			cnpj == '99999999999999') {
			retorno = false;
		}

		// Valida DVs
		let tamanho = cnpj.length - 2;
		let numeros = cnpj.substring(0, tamanho);
		const digitos = cnpj.substring(tamanho);
		let soma = 0;
		let pos = tamanho - 7;
		for (let i = tamanho; i >= 1; i--) {
			soma += numeros.charAt(tamanho - i) * pos--;
			if (pos < 2) {
				pos = 9;
			}
		}
		let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
		if (resultado != digitos.charAt(0)) {
			retorno = false;
		}

		tamanho = tamanho + 1;
		numeros = cnpj.substring(0, tamanho);
		soma = 0;
		pos = tamanho - 7;
		for (let i = tamanho; i >= 1; i--) {
			soma += numeros.charAt(tamanho - i) * pos--;
			if (pos < 2) {
				pos = 9;
			}
		}
		resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
		if (resultado != digitos.charAt(1)) {
			retorno = false;
		}

		if (!retorno) {
			this.modal.alerta(this.msg.CNPJ_INVALIDO);
			return false;
		}

		return true;
	}
}
