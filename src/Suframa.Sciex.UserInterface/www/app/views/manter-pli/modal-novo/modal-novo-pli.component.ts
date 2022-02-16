import { Component, ViewChild } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';


@Component({
	selector: 'app-modal-novo-pli',
	templateUrl: './modal-novo-pli.component.html',
})

export class ModalNovoPliComponent {	
	isDisplay: boolean = false;
	cpfDigitado: string = '';
	servicoPli = 'Pli';
	parametros: any = {};
	model: manterPliVM = new manterPliVM();
	tipoPli: string;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard : AuthGuard,

	) { }

	@ViewChild('appModalNovoPli') appModalNovoPli;
	@ViewChild('appModalNovoPliBackground') appModalNovoPliBackground;
	@ViewChild('formularioB') formulario;
	@ViewChild('cpf') cpf;

	public abrir() {
		this.tipoPli = "NORMAL";
		this.cpf.nativeElement.value = "";
		this.cpf.nativeElement.setCustomValidity('');
		this.appModalNovoPli.nativeElement.style.display = 'block';
		this.appModalNovoPliBackground.nativeElement.style.display = 'block';
	}
	
	// isSetorTrue() {
	// 	this.isDisplay = true;
	// }

	// isSetorFalse($event) {
	// 	let selectedOptions = $event.target['options'];
	// 	let selectedIndex = selectedOptions.selectedIndex;
	// 	let selectElementText = selectedOptions[selectedIndex].text;
	// 	if (selectElementText == 'INDUSTRIALIZAÇÃO-OUTROS') {
	// 		this.isDisplay = true;
	// 	} else {
	// 		this.isDisplay = false;
	// 	}
	// }

	public fechar() {
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.model = new manterPliVM();
					this.appModalNovoPli.nativeElement.style.display = 'none';
					this.appModalNovoPliBackground.nativeElement.style.display = 'none';
				}
			});
    }

    numericOnly(event): boolean { // restrict e,+,-,E characters in  input type number
        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode == 101 || charCode == 69 || charCode == 45 || charCode == 43) {
            return false;
        }
        return true;
    }
	
	public salvar() {
		if (this.validaCPF(this.removerCaractere(this.cpfDigitado)) == false) {
			this.cpf.nativeElement.setCustomValidity('CPF Inválido');
		} else
		{
			this.model.numCPFRepLegalSISCO = this.removerCaractere(this.cpfDigitado);
		}

		if (!this.validationService.form('formularioB')) { return; }
		if (!this.formulario.valid) { return; }

		this.applicationService.put<manterPliVM>(this.servicoPli, this.model).subscribe(result => {			
			if (result.mensagem != null) {
				this.modal.alerta(result.mensagem, "Informação", "");
				return;
			}			
			this.appModalNovoPli.nativeElement.style.display = 'none';
			this.appModalNovoPliBackground.nativeElement.style.display = 'none';

			if (this.model.tipoDocumento == 1) {
				if (this.model.codigoPliAplicao == 0 || this.model.codigoPliAplicao == 2 || this.model.codigoPliAplicao == 3) {
					this.authguard.active = true;
					localStorage.setItem('manter-pli', JSON.stringify(result));
					this.router.navigate([`/manter-pli/${result.idPLI}/cadastrarcomercializacao`]);
				}
				if (this.model.codigoPliAplicao == 1) {
					this.authguard.active = true;
					localStorage.setItem('manter-pli', JSON.stringify(result));
					this.router.navigate(['manter-pli/cadastrar']);
				}
            }
			else if (this.model.tipoDocumento == 2) {

				if (this.model.codigoPliAplicao == 0 || this.model.codigoPliAplicao == 2 || this.model.codigoPliAplicao == 3) {
					this.authguard.active = true;
					localStorage.setItem('manter-pli', JSON.stringify(result));
					this.router.navigate([`/manter-pli/${result.idPLI}/cadastrarcomercializacaosubstitutivo`]);
				}
				if (this.model.codigoPliAplicao == 1) {
					this.authguard.active = true;
					localStorage.setItem('manter-pli', JSON.stringify(result));
					this.router.navigate([`/manter-pli/${result.idPLI}/cadastrarsubstitutivo`]);
				}
			}
			else if (this.model.tipoDocumento == 3) { //Retificador

				if (this.model.codigoPliAplicao == 0 || this.model.codigoPliAplicao == 2 || this.model.codigoPliAplicao == 3) {
					this.authguard.active = true;
					localStorage.setItem('manter-pli', JSON.stringify(result));
					this.router.navigate([`/manter-pli/${result.idPLI}/cadastrarretificadoracomercializacao`]);
				}
				if (this.model.codigoPliAplicao == 1) {
					this.authguard.active = true;
					localStorage.setItem('manter-pli', JSON.stringify(result));
					this.router.navigate([`/manter-pli/${result.idPLI}/cadastrarretificadora`]);
				}
            }

		});
	}
	
	public validaCPF(cpfDigitado) {
		var Soma;
		var Resto;
		var i;
		Soma = 0;

		this.cpf.nativeElement.setCustomValidity('');

		//Invalida CPF menor que 11 dígitos
		if (this.cpf.nativeElement.value.length < 11) { return false; }

		//Invalida CPF com dígitos semelhantes
		if ((cpfDigitado == "00000000000") ||
			  cpfDigitado == "11111111111" ||
			  cpfDigitado == "22222222222" ||
			  cpfDigitado == "33333333333" ||
			  cpfDigitado == "44444444444" ||
			  cpfDigitado == "55555555555" ||
			  cpfDigitado == "66666666666" ||
			  cpfDigitado == "77777777777" ||
			  cpfDigitado == "88888888888" ||
			  cpfDigitado == "99999999999") { return false; }

		for (i = 1; i <= 9; i++) Soma = Soma + parseInt(cpfDigitado.substring(i - 1, i)) * (11 - i);
		Resto = (Soma * 10) % 11;

		if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(cpfDigitado.substring(9, 10))) { return false; }

		Soma = 0;
		for (i = 1; i <= 10; i++) Soma = Soma + parseInt(cpfDigitado.substring(i - 1, i)) * (12 - i);
		Resto = (Soma * 10) % 11;

		if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(cpfDigitado.substring(10, 11))) { return false; }

		if (this.cpf.nativeElement.value.length == 0) { return false }

		return true;
	}

	removerHint() {
		if (this.cpf.nativeElement.value.length > 0) {
			this.cpf.nativeElement.setCustomValidity('');
		}
	}

	removerCaractere(documento) {
		var nomeDocumento = "";
		for (var i = 0; i < documento.length; i++) {
			if (documento[i] != "." && documento[i] != "-" && documento[i] != "/") {
				nomeDocumento = nomeDocumento + documento[i];
			}
		}
		return nomeDocumento;
	}

}
