import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'enum' })
export class EnumPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		value = Number(value);

		if (args == 'EnumTipoOrigemDocumento') {
			if (value == 1) {
				return 'Anexo';
			} else if (value == 2) { return 'REDESim'; }
		}

		if (args == 'EnumTipoSocio') {
			if (value == 1) {
				return 'Nacional';
			} else if (value == 2) { return 'Estrangeiro'; }
		}

		if (args == 'EnumTipoPessoa') {
			if (value == 1) {
				return 'Pessoa Fisica';
			} else if (value == 2) { return 'Pessoa Juridica'; }
		}

		if (args == 'EnumStatusDiligencia') {
			if (value == 1) {
				return 'Aberta';
			} else if (value == 0) { return 'Finalizada'; }
		}

		if (args == 'EnumStatusPedidoCorrecao') {
			if (value == 1) {
				return 'Aberto';
			} else if (value == 2) {
				return 'Enviado';
			} else if (value == 3) { return 'Finalizado'; }
		}

		if (args == 'EnumAcao') {
			if (value == 1) {
				return 'Inclusão';
			} else if (value == 2) {
				return 'Alteração';
			} else if (value == 3) { return 'Exclusão'; }
		}

		if (args == 'EnumOrigemConsultaPublica') {
			if (value == 1) {
				return 'Manual';
			} else if (value == 2) { return 'Automática'; }
		}

		return null;
	}
}
