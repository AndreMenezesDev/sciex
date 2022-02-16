import { Component, ViewChild } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ManterLEInsumoFormularioComponent } from '../formulario/formularioInsumo.component';


@Component({
	selector: 'app-modal-novo-insumo',
	templateUrl: './modal-novo-insumo.component.html',
})

export class ModalNovoInsumoComponent {	
	isDisplay: boolean = false;
	servico = 'LEInsumo';
	servicoAlterar = 'LEAlterarInsumo';
	parametros: any = {};
	formPai: any;
	codigoProduto: any;
	idLe: any;
	path: string;
	somenteLeitura: boolean = false;
	valorCodigoNCM: any;
	valorCodigoDetalheMercadoria: any;
	valorCodigoProdutoSuframa: any;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard : AuthGuard,

	) { }

	@ViewChild('appModalNovoInsumo') appModalNovoInsumo;
	@ViewChild('appModalNovoInsumoBackground') appModalNovoInsumoBackground;
	@ViewChild('formularioB') formulario;

	@ViewChild('codigoNCM1') codigoNCM1;
	@ViewChild('codigoNCM2') codigoNCM2;
	@ViewChild('codigoInsumo1') codigoInsumo1;
	@ViewChild('insumoDesc') insumoDesc;
	@ViewChild('coefTec') coefTec;

	public cadastrar(formPai) {
		this.path = "Cadastrar";
		this.formPai = formPai;
		this.somenteLeitura = false;
		if(formPai != null && formPai.model != null){
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}
		this.parametros = {};
		this.appModalNovoInsumo.nativeElement.style.display = 'block';
		this.appModalNovoInsumoBackground.nativeElement.style.display = 'block';
	}
	
	public visualizar(formPai, item) {
		this.path = "Visualizar";
		this.formPai = formPai;
		this.somenteLeitura = true;
		if(formPai != null && formPai.model != null){
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}
		if (item != null){
			this.parametros.tipoInsumo = item.tipoInsumo;
			this.parametros.idLeInsumo = item.idLeInsumo;
			if(item.tipoInsumo == 'P') {
				this.parametros.codigoNCM1 = item.idCodigoNCM;
				this.parametros.idInsumo = item.idCodigoDetalhe;
				this.parametros.codigoDetalhe = item.codigoDetalhe;

				//nova configuração para carregar insumo tipo P da view prj insumo padrao
				this.valorCodigoNCM = item.codigoNCM;
				this.valorCodigoDetalheMercadoria = item.codigoDetalhe;
				this.valorCodigoProdutoSuframa = item.idCodigoNCM;
				//
			}
			else if (item.tipoInsumo != 'P'){
				this.parametros.codigoNCM2 = item.idCodigoNCM;
				this.parametros.descricaoInsumo = item.descricaoInsumo;
			}
			this.parametros.codigoNCM = item.codigoNCMFormatado;
			this.parametros.codigoUnidadeMedida = item.codigoUnidadeMedida;
			this.parametros.descricaoUnidadeMedida = item.descricaoUnidadeMedida;
			this.parametros.valorCoeficienteTecnico = item.valorCoeficienteTecnico;
			this.parametros.codigoPartNumber = item.codigoPartNumber;
			this.parametros.descricaoEspecTecnica = item.descricaoEspecTecnica;
			this.parametros.descricaoInsumo = item.descricaoInsumo || '-';
		}
		else
			this.parametros = {};
		this.appModalNovoInsumo.nativeElement.style.display = 'block';
		this.appModalNovoInsumoBackground.nativeElement.style.display = 'block';
	}

	public alterar(formPai, item){
		this.path = "Alterar";
		this.formPai = formPai;
		this.somenteLeitura = false;
		if(formPai != null && formPai.model != null){
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}
		if (item != null){
			this.parametros.tipoInsumo = item.tipoInsumo;
			this.parametros.idLeInsumo = item.idLeInsumo;
			if(item.tipoInsumo == 'P') {
				this.parametros.codigoNCM1 = item.idCodigoNCM;
				this.parametros.idInsumo = item.idCodigoDetalhe;
				this.parametros.codigoDetalhe = item.codigoDetalhe;
				this.parametros.descricaoInsumo = item.descricaoInsumo;

				//nova configuração para carregar insumo tipo P da view prj insumo padrao
				this.valorCodigoNCM = item.codigoNCM;
				this.valorCodigoDetalheMercadoria = item.codigoDetalhe;
				this.valorCodigoProdutoSuframa = item.idCodigoNCM;
				//
			}
			else if (item.tipoInsumo != 'P'){
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
		else
			this.parametros = {};
		this.appModalNovoInsumo.nativeElement.style.display = 'block';
		this.appModalNovoInsumoBackground.nativeElement.style.display = 'block';
	}

	public alterarBloqueado(formPai, item){
		this.path = "Alterar";
		this.formPai = formPai;
		this.somenteLeitura = false;
		this.parametros.idLeInsumo = item.idLeInsumo;

		this.applicationService.put(this.servicoAlterar, this.parametros).subscribe((result: any) => {
			if(result != null){
				if (result.mensagemErro != null) {
					this.modal.alerta(result.mensagemErro, "Informação", "");
					return;
				}
				else if (result.mensagem != null){
					if(this.codigoNCM1 != undefined)
						this.codigoNCM1.clear();
					if(this.codigoNCM2 != undefined)
						this.codigoNCM2.clear();
					if(this.codigoInsumo1 != undefined)
						this.codigoInsumo1.clear();

					this.formPai.buscar();
					this.parametros.idLeInsumo = result.idLeInsumo;

					if(formPai != null && formPai.model != null){
						this.codigoProduto = formPai.model.codigoProdutoSuframa;
						this.idLe = formPai.model.idLe;
					}
					if (item != null){
						this.parametros.tipoInsumo = item.tipoInsumo;
						if(item.tipoInsumo == 'P') {
							this.parametros.tipoInsumo = item.tipoInsumo;
							this.parametros.codigoNCM1 = item.idCodigoNCM;
							this.parametros.idInsumo = item.idCodigoDetalhe || result.idCodigoDetalhe || 0;
							this.parametros.codigoDetalhe = item.codigoDetalhe;
							this.parametros.descricaoInsumo = item.descricaoInsumo;

							//nova configuração para carregar insumo tipo P da view prj insumo padrao
							this.valorCodigoNCM = item.codigoNCM;
							this.valorCodigoDetalheMercadoria = item.codigoDetalhe;
							this.valorCodigoProdutoSuframa = item.idCodigoNCM;
							//
						}
						else if (item.tipoInsumo != 'P'){
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
					else
						this.parametros = {};

					this.appModalNovoInsumo.nativeElement.style.display = 'block';
					this.appModalNovoInsumoBackground.nativeElement.style.display = 'block';
				}
				
			}
		});

	}

	public fechar() {
		if(this.path == "Visualizar"){
			this.cleanFields();
			this.appModalNovoInsumo.nativeElement.style.display = 'none';
			this.appModalNovoInsumoBackground.nativeElement.style.display = 'none';
		}
		else{
			this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.cleanFields();
					this.appModalNovoInsumo.nativeElement.style.display = 'none';
					this.appModalNovoInsumoBackground.nativeElement.style.display = 'none';
				}
			});
		}
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

	public selecionaNCMViewPadrao(event){
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

	public salvar() {

		this.validarIncluir();

		if (!this.formulario.valid) { return; }
		//if (!this.validationService.form('formularioB')) { return; }
		this.parametros.codigoProduto = this.codigoProduto;
		this.parametros.idLe = this.idLe;
		this.parametros.path = this.path;
		
		this.applicationService.put(this.servico, this.parametros).subscribe((result: any) => {			
			if (result.mensagemErro != null) {
				this.modal.alerta(result.mensagemErro, "Informação", "");
				return;
			}
			else if (result.mensagem != null){
				this.cleanFields();
				this.formPai.buscar();
				this.modal.resposta(result.mensagem, "Informação", "");
				this.appModalNovoInsumo.nativeElement.style.display = 'none';
				this.appModalNovoInsumoBackground.nativeElement.style.display = 'none';
			}
		});
	}

	validarIncluir() {
		if (this.parametros.tipoInsumo == null) {
			this.modal.alerta("Preencha o campo Tipo Insumo", "Informação");
		}
		else if (this.parametros.codigoNCM1 == null && this.parametros.codigoNCM2 == null) {
			this.modal.alerta("Preencha o campo NCM", "Informação");
		} else if (this.codigoInsumo1 != null && this.parametros.idInsumo == null || this.insumoDesc != null && (this.parametros.descricaoInsumo == null || this.parametros.descricaoInsumo == "")) {
			this.modal.alerta("Preencha o campo Insumo", "Informação");
		}
		else if (this.parametros.valorCoeficienteTecnico == null || this.parametros.valorCoeficienteTecnico == "") {
			this.modal.alerta("Preencha o campo Coeficiente Técnico", "Informação");
		}
		/*else if (this.parametros.codigoPartNumber == null || this.parametros.codigoPartNumber == "") {
			this.modal.alerta("Preencha o campo Part Number", "Informação");
		}
		else if (this.parametros.descricaoEspecTecnica == null || this.parametros.descricaoEspecTecnica == "") {
			this.modal.alerta("Preencha o campo Especificação Técnica", "Informação");
		}*/
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
