import { Component, ViewChild } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';

@Component({
	selector: 'app-modal-consultar-insumo',
	templateUrl: './modal-consultar-insumo.component.html',
})

export class ModalConsultarInsumoComponent {	
	isDisplay: boolean = false;
	servico = 'LEInsumo';
	parametros: any = {};
	formPai: any;
	codigoProduto: any;
	idLe: any;
	path: string;
	somenteLeitura: boolean = false;
	servicoSalvarAnalise = "LEAnaliseInsumo";

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard : AuthGuard,

	) { }

	@ViewChild('appModalConsultarInsumo') appModalConsultarInsumo;
	@ViewChild('appModalConsultarInsumoBackground') appModalConsultarInsumoBackground;
	@ViewChild('formularioB') formulario;

	@ViewChild('codigoNCM1') codigoNCM1;
	@ViewChild('codigoNCM2') codigoNCM2;
	@ViewChild('codigoInsumo1') codigoInsumo1;
	@ViewChild('insumoDesc') insumoDesc;
	@ViewChild('coefTec') coefTec;

	public visualizar(formPai, item) {
		this.path = "Visualizar";
		this.formPai = formPai;
		this.somenteLeitura = true;
		this.parametros.codigoInsumo = item.codigoInsumo.toString();
		if(formPai != null && formPai.model != null){
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}
		if (item != null){
			this.parametros.tipoInsumo = item.tipoInsumo;
			this.parametros.idLeInsumo = item.idLeInsumo;
			if(item.tipoInsumo == 1) {
				this.parametros.codigoNCM1 = item.idCodigoNCM;
				this.parametros.idInsumo = item.idCodigoDetalhe;
				this.parametros.codigoDetalhe = item.codigoDetalhe;
			}
			else if (item.tipoInsumo > 1){
				this.parametros.codigoNCM2 = item.idCodigoNCM;
				this.parametros.descricaoInsumo = item.descricaoInsumo;
			}
			this.parametros.codigoNCM = item.codigoNCMFormatado;
			this.parametros.codigoUnidadeMedida = item.codigoUnidadeMedida;
			this.parametros.descricaoUnidadeMedida = item.descricaoUnidadeMedida;
			this.parametros.valorCoeficienteTecnico = item.valorCoeficienteTecnico;
			this.parametros.codigoPartNumber = item.codigoPartNumber;
			this.parametros.descricaoEspecTecnica = item.descricaoEspecTecnica;
		}
		else{
			this.parametros = {};
		}

		this.carregarAnalise(this.parametros.idLeInsumo);
		this.appModalConsultarInsumo.nativeElement.style.display = 'block';
		this.appModalConsultarInsumoBackground.nativeElement.style.display = 'block';
	}
	carregarAnalise(id) {
		this.applicationService.get(this.servicoSalvarAnalise, {id:id}).subscribe((result: any) => {
			this.parametros.situacaoInsumo = result.situacaoInsumo.toString();
			this.parametros.descricaoErro = result.ultimoInsumoErro != null ? result.ultimoInsumoErro.descricaoErro : null;
			
			if (result.mensagemErro != null) {
				this.modal.alerta(result.mensagemErro, "Informação", "");
				return;
			}
			else if (result.mensagem != null){
				this.cleanFields();
				this.formPai.buscar();
				this.modal.resposta(result.mensagem, "Informação", "");
				this.appModalConsultarInsumo.nativeElement.style.display = 'none';
				this.appModalConsultarInsumoBackground.nativeElement.style.display = 'none';
			}
		});
	}
	public fechar() {
		this.cleanFields();
		this.appModalConsultarInsumo.nativeElement.style.display = 'none';
		this.appModalConsultarInsumoBackground.nativeElement.style.display = 'none';
	}
	
	cleanFields() {
		if(this.codigoNCM1 != null)
			this.codigoNCM1.clear();
		if(this.codigoNCM2 != null)
			this.codigoNCM2.clear();
		if(this.codigoInsumo1 != null)
			this.codigoInsumo1.clear();
		this.parametros = {};
	}

    numericOnly(event): boolean { // restrict e,+,-,E characters in  input type number
        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode == 101 || charCode == 69 || charCode == 45 || charCode == 43) {
            return false;
        }
        return true;
	}
	
	public selecionaNCM(event){
		if(event != null){
			let cod = event.text.split("|")[0].trim();
			this.parametros.codigoNCM = cod;
			if(this.codigoInsumo1 != null)
				this.codigoInsumo1.clear();
			this.applicationService.get("ViewNcm", event.id).subscribe( (result : any) => {
				if(result != null){
					this.parametros.codigoUnidadeMedida = result.idNcmUnidadeMedida;
					this.parametros.descricaoUnidadeMedida = result.descricaoUnidadeMedida;
				}
			});
		}
	}

	public selecionaInsumo(event){
		if(event != null){
			let cod = event.text.split("|")[0].trim();
			let desc = event.text.split("|")[1].trim();
			this.parametros.codigoDetalhe = cod;
			this.parametros.descricaoInsumo = desc;
		}
	}
	
	onBlurValidar(){

		if(!(this.isNumber(this.coefTec.nativeElement.value)) &&
					this.coefTec.nativeElement.value){

			this.modal.alerta("Valor inválido", 'Informação');
			this.coefTec.nativeElement.value = null;
			this.parametros.valorCoeficienteTecnico = null;
			return false;
		} else {
			let val = 9999999.99999999;
			if(this.coefTec.nativeElement.value != null && this.coefTec.nativeElement.value != ""){
				let valorAtual = parseFloat(this.replaceVirgula(this.coefTec.nativeElement.value));

				if(valorAtual < val)
				{
					this.parametros.valorCoeficienteTecnico = valorAtual;
				}
				else{
					this.modal.alerta("Valor inválido", 'Informação');
					this.coefTec.nativeElement.value = null;
					this.parametros.valorCoeficienteTecnico = null;
				}
				
			}
			return true;
		}
	}

	isNumber(n: string) {
		let value = this.replaceVirgula(n);
		return !isNaN(parseFloat(value)) && isFinite(parseFloat(value));
	 // $.isNumeric('123'); // true
	 // $.isNumeric('asdasd123'); // false
	}

	replaceVirgula(value): string{
		return value.replace(',','.');
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
