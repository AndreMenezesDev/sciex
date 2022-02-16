import { Component, OnInit, AfterViewChecked, Input } from '@angular/core';
import { resumoVM } from '../../../view-model/ResumoVM';

@Component({
	selector: 'app-destaca-campo',
	templateUrl: './destaca-campo.component.html',
})
export class DestacaCampoComponent implements OnInit, AfterViewChecked {
	@Input() campoTela: string;
	@Input() listaErros: resumoVM[] = new Array<resumoVM>();

	erro: resumoVM;
	isDisabled = false;

	ngOnInit(): void {
		this.validaErro();
	}

	ngAfterViewChecked() {
		if (this.listaErros && this.listaErros.length != 0 && !this.isDisabled) {
			// Procura o campo com aquele ID para desabilitar
			const campo = document.getElementById(this.campoTela.charAt(0).toLowerCase() + this.campoTela.slice(1));

			if (!campo) {
				// Pesquisa se há radios buttons com aquele name
				// e se houver ele desabilita
				const radios = document.getElementsByName(this.campoTela.charAt(0).toLowerCase() + this.campoTela.slice(1));

				if (!radios) { return; }

				for (let i = 0; i < radios.length; i++) {
					this.desabilitaCampo(radios[i]);
				}

				return;
			}

			this.desabilitaCampo(campo);
		}
	}

	desabilitaCampo(campo) {
		if (campo.children.length > 0 && campo.nodeName != 'FIELDSET') {
			campo.children[0].setAttribute('disabled', 'disabled');

			return;
		}

		campo.setAttribute('disabled', 'disabled');
	}

	validaErro() {
		if (!this.listaErros) { return; }

		this.erro = this.listaErros.find(x => x.campoTela == this.campoTela);

		if (this.erro) {
			this.isDisabled = true;
		}
	}
}
