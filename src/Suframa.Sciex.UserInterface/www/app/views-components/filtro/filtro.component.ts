import { Component, OnInit, EventEmitter, ViewChild, Input, Output } from '@angular/core';
import { FiltroCadastroPessoaJuridicaVM } from '../../view-model/FiltroCadastroPessoaJuridicaVM';
import { ApplicationService } from '../../shared/services/application.service';
import { ExtractNumberService } from '../../shared/services/extract-number.service';
import { ValidationService } from '../../shared/services/validation.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ModalService } from '../../shared/services/modal.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../shared/services/authentication.service';

@Component({
    selector: 'app-filtro',
    templateUrl: './filtro.component.html',
    inputs: ['model', 'isVisualizar', 'listaErros'],
    outputs: ['filtroChange']
})
export class FiltroComponent {
    model: FiltroCadastroPessoaJuridicaVM = new FiltroCadastroPessoaJuridicaVM();
    listaErros: any;
    isVisualizar: boolean = false;

    servicoFiltroCadastro: string = "FiltroCadastroPessoaJuridica";
    consultarSituacaoUsuario: string = "ConsultarSituacaoUsuario";

    cnpjAntigo: string;

	@ViewChild('naturezaJuridicaDropList') naturezaJuridicaDropList;

    filtroChange: EventEmitter<any> = new EventEmitter<any>();

    constructor(
        private applicationService: ApplicationService,
        private validation: ValidationService,
        private toastrService: ToastrService,
        private msg: MessagesService,
        private modal: ModalService,
        private extractNumber: ExtractNumberService) {
    }

	public buscarCnpj() {
		if (!this.model || !this.model.cnpj) { return; }

		if (!this.validarCNPJ()) { return; }

		this.cnpjAntigo = this.model.cnpj;
		this.model = new FiltroCadastroPessoaJuridicaVM();

        // Verificar se tem bloqueio
        this.applicationService.get<boolean>(this.consultarSituacaoUsuario, { cpfCnpj: this.cnpjAntigo })
            .subscribe(result => {
                if (result) {
                    // Consultar informações do CNPJ
                    this.applicationService.get<FiltroCadastroPessoaJuridicaVM>(this.servicoFiltroCadastro, { cnpj: this.cnpjAntigo })
                        .subscribe(result => {
                            if (!result)
                                this.model.cnpj = this.cnpjAntigo;
                            else
                                this.model = result;

                            this.filtroChange.emit(this.model);
                        });
                }
                else {
                    this.modal.alerta(this.msg.USUARIO_POSSUI_PENDENCIAS);
                }
            });
    }

	validarCNPJ() {
		this.model.cnpj = this.extractNumber.extractNumbers(this.model.cnpj);

		return this.validation.cnpj(this.model.cnpj);
	}

	onChangeNatureza() {
		if (this.naturezaJuridicaDropList && this.naturezaJuridicaDropList.getSelectedItem()) {
			this.model.idNaturezaGrupo = this.naturezaJuridicaDropList.getSelectedItem().idNaturezaGrupo;
			this.model.isQuadroSocietario = this.naturezaJuridicaDropList.getSelectedItem().isQuadroSocietario;
			this.model.naturezaQualificacao = this.naturezaJuridicaDropList.getSelectedItem().naturezaQualificacao;
		}

		this.filtroChange.emit(this.model);
	}

	isValidSalvar() {
		if (!this.validation.form('formularioFiltro')) { return false; }
		if (!this.validarCNPJ()) { return false; }

		return true;
	}
}
