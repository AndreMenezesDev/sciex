import { ActivatedRoute } from '@angular/router';
import { ApplicationService } from '../../../shared/services/application.service';
import { Component, ViewChild } from '@angular/core';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ToastrService } from 'toastr-ng2';

@Component({
	selector: 'app-parametrizacao-distribuicao-automatica',
	templateUrl: './distribuicao-automatica.component.html'
})
export class ParametrizacaoDistribuicaoAutomaticaComponent {
	idUnidadeCadastradora?: number;
	idUnidadeCadastradoraParametro?: number;
	isUnidadeCadastradoraManaus?: boolean;
	lista: any;
	radioUnidadeCadastradora: any;

	@ViewChild('unidadeCadastradoraDropList') unidadeCadastradoraDropList;

	constructor(
		private activatedRoute: ActivatedRoute,
		private applicationService: ApplicationService,
		private messagesService: MessagesService,
		private toastrService: ToastrService,
	) {
		this.selecionarUsuario();
	}

	selecionarUsuario() {
		this.applicationService.get<any>('UsuarioInternoLogado').subscribe(result => {
			this.isUnidadeCadastradoraManaus = result.isUnidadeCadastradoraManaus;

			if (!this.isUnidadeCadastradoraManaus) {
				this.idUnidadeCadastradoraParametro = result.idUnidadeCadastradora;
				setTimeout(() => { this.unidadeCadastradoraDropList.listItems(); }, 0);
				this.listar(this.idUnidadeCadastradoraParametro);
			} else {
				this.unidadeCadastradoraDropList.listItems();
			}
		});
	}

	changeRadioUnidadeCadastradora(value) {
		this.radioUnidadeCadastradora = value;

		if (value == 1) {
			this.idUnidadeCadastradora = null;
			this.listar();
		}
	}

	selecionarUnidadeCadastradora() {
		if (!this.idUnidadeCadastradora) {
			this.lista = null;
			return;
		}

		this.listar(this.idUnidadeCadastradora);
	}

	listar(idUnidadeCadastradora?: number) {
		const parametros = {
			size: 2147483647,
			IdUnidadeCadastradora: idUnidadeCadastradora
		};

		this.applicationService.get<any>('ParametroDistribuicaoAutomaticaGrid', parametros).subscribe(result => {
			this.lista = result.items;
		});
	}

	atualizarStatus(item, isAtivo) {
		const parametros = {
			IdParametroDistribuicaoAutomatica: item.idParametroDistribuicaoAutomatica,
			IsAtivo: isAtivo
		};

		this.applicationService.post('ParametroDistribuicaoAutomatica', parametros).subscribe(result => {
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}
}
