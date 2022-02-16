import { ApplicationService } from '../../../shared/services/application.service';
import { Component, ViewChild } from '@angular/core';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ToastrService } from 'toastr-ng2';

@Component({
	selector: 'app-parametrizacao-analistas',
	templateUrl: './analistas.component.html'
})
export class ParametrizacaoAnalistasComponent {
	idParametroAnalista?: number;
	idUnidadeCadastradora?: number;
	idUnidadeCadastradoraParametro?: number;
	idUsuarioInterno?: number;
	isUnidadeCadastradoraManaus?: boolean;
	lista: any;

	@ViewChild('appModalServicos') appModalServicos;
	@ViewChild('unidadeCadastradoraDropList') unidadeCadastradoraDropList;
	@ViewChild('usuarioInternoDropList') usuarioInternoDropList;

	constructor(
		private applicationService: ApplicationService,
		private messagesService: MessagesService,
		private toastrService: ToastrService,
	) {
		this.selecionarUsuarioInterno();
	}

	selecionarUsuarioInterno() {
		this.applicationService.get<any>('UsuarioInternoLogado').subscribe(result => {
			this.isUnidadeCadastradoraManaus = result.isUnidadeCadastradoraManaus;

			if (!this.isUnidadeCadastradoraManaus) {
				this.idUnidadeCadastradoraParametro = result.idUnidadeCadastradora;
				setTimeout(() => { this.unidadeCadastradoraDropList.listItems(); }, 0);
			} else {
				this.unidadeCadastradoraDropList.listItems();
			}
		});
	}

	selecionarUnidadeCadastradora() {
		this.usuarioInternoDropList.reset();

		if (!this.idUnidadeCadastradora) {
			this.usuarioInternoDropList.listItems();
			this.lista = null;
			return;
		}

		this.listarParametroAnalista();
	}

	listarParametroAnalista() {
		const parametros = {
			size: 2147483647,
			IdUnidadeCadastradora: this.idUnidadeCadastradora
		};

		this.applicationService.get<any>('ParametroAnalistaGrid', parametros).subscribe(result => {
			this.lista = result.items;
		});
	}

	atualizarStatusProtocolo(item, isAtivo) {
		const parametros = {
			IdParametroAnalista: item.idParametroAnalista,
			IsStatusAtivoProtocolo: isAtivo
		};

		this.atualizarParametroAnalista(parametros);
	}

	atualizarStatusAgendamento(item, isAtivo) {
		const parametros = {
			IdParametroAnalista: item.idParametroAnalista,
			IsStatusAtivoAgendamento: isAtivo
		};

		this.atualizarParametroAnalista(parametros);
	}

	adicionarParametroAnalista() {
		if (!this.idUnidadeCadastradora || !this.idUsuarioInterno) { return; }

		const parametros = {
			IdUnidadeCadastradora: this.idUnidadeCadastradora,
			IdUsuarioInterno: this.idUsuarioInterno
		};

		this.applicationService.post('ParametroAnalista', parametros).subscribe(result => {
			this.listarParametroAnalista();
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}

	atualizarParametroAnalista(parametros) {
		this.applicationService.put('ParametroAnalista', parametros).subscribe(result => {
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}

	excluirParametroAnalista(idParametroAnalista) {
		this.applicationService.delete('ParametroAnalista', idParametroAnalista).subscribe(result => {
			this.listarParametroAnalista();
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}

	abrirModalServicos(item) {
		this.idParametroAnalista = item.idParametroAnalista;
		this.appModalServicos.abrir(item.parametroAnalistaServico);
	}

	confirmarModalServicos(servicos) {
		if (servicos && servicos.length > 0) {
			for (let i = 0; i < servicos.length; i++) {
				servicos[i].idParametroAnalista = this.idParametroAnalista;
			}
		} else {
			servicos = new Array<any>();
			servicos.push({ idParametroAnalista: this.idParametroAnalista });
		}

		this.applicationService.post('ParametroAnalistaServico', servicos).subscribe(result => {
			this.idParametroAnalista = null;
			this.listarParametroAnalista();
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}
}
