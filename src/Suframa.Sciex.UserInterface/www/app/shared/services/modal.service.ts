import { Injectable } from '@angular/core';

import { DialogService } from 'angularx-bootstrap-modal';

import { ModalAlertaComponent } from '../components/modal/modal-alerta/modal-alerta.component';

import { ModalConfirmacaoComponent } from '../components/modal/modal-confirmacao/modal-confirmacao.component';
import { ModalConfirmacaoVisualizacaoComponent } from "../components/modal/modal-confirmacao-visualizacao/modal-confirmacao-visualizacao.component";
import { ModalPromptComponent } from '../components/modal/modal-prompt/modal-prompt.component';
import { ModalPromptVisualizacaoComponent } from "../components/modal/modal-prompt-visualizacao/modal-prompt-visualizacao.component";
import { ModalRespostaComponent } from "../components/modal/modal-resposta/modal-resposta.component";

@Injectable()
export class ModalService {
	constructor(private dialogService: DialogService) { }

	alerta(mensagem: string, titulo?: string, caminhoURL? : string) {
		return this.dialogService.addDialog(ModalAlertaComponent, {
			title: titulo,
			message: mensagem,
			caminhoURL: caminhoURL
		});
	}

	confirmacao(mensagem: string, titulo: string, caminho: string) {
		return this.dialogService.addDialog(ModalConfirmacaoComponent, {
			title: titulo,
			message: mensagem,
			caminho: caminho
		});
	}



	confirmacaoVisualizacao(mensagem: string, titulo: string) {
		return this.dialogService.addDialog(ModalConfirmacaoVisualizacaoComponent, {
			title: titulo,
			message: mensagem
		});
    }

    resposta(mensagem: string, titulo, caminhoURL: string) {       

        return this.dialogService.addDialog(ModalRespostaComponent, {
            title: titulo,
            message: mensagem,
            caminho: caminhoURL
        });
    }

	prompt(titulo: string, mensagem: string, tituloPrompt: string) {
		return this.dialogService.addDialog(ModalPromptComponent, {
			title: titulo,
			message: mensagem,
			titlePrompt: tituloPrompt
		});
	}

	promptVisualizacao(titulo: string, mensagem: string, tituloPrompt: string) {
		return this.dialogService.addDialog(ModalPromptVisualizacaoComponent, {
			title: titulo,
			message: mensagem,
			titlePrompt: tituloPrompt
		});
	}


}
