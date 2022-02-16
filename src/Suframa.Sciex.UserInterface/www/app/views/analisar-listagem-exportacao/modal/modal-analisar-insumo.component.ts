import { Component, ViewChild } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { AnalisarLEInsumoFormularioComponent } from '../formulario/formularioInsumo.component';
import { EnumTipoInsumoAlteracao } from '../../../shared/enums/EnumTipoInsumoAlteracao';


@Component({
	selector: 'app-modal-analisar-insumo',
	templateUrl: './modal-analisar-insumo.component.html',
})

export class ModalAnalisarInsumoComponent {
	isDisplay: boolean = false;
	servico = 'LEInsumo';
	parametros: any = {};
	model1: any = {};
	model2: any = {};
	formPai: any;
	codigoProduto: any;
	idLe: any;
	path: string;
	somenteLeitura: boolean = false;
	servicoSalvarAnalise = "LEAnaliseInsumo";
	isAlteracao: boolean;
	buscarInsumosOriginalAlterado: string = "LEAnaliseInsumoGrid";
	tipoInsumoAlteracao: any;
	idLeInsumo: any;
	exibirOriginalEAlterado: boolean;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard: AuthGuard,

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
		if (formPai != null && formPai.model != null) {
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}
		this.parametros = {};
		this.appModalNovoInsumo.nativeElement.style.display = 'block';
		this.appModalNovoInsumoBackground.nativeElement.style.display = 'block';
	}

	public salvarAnalise() {

		if (this.parametros.situacaoInsumo == 0 || this.parametros.situacaoInsumo == null || this.parametros.situacaoInsumo == undefined) {
			this.modal.alerta("Informe a situação", 'Informação');
			return;
		}

		if (this.parametros.situacaoInsumo == '2' && (this.parametros.descricaoErro == null || this.parametros.descricaoErro.length == 0)) {
			this.modal.alerta("Informe mensagem erro", 'Informação');
			return;
		}

		this.parametros.isAlteracao = this.isAlteracao;
		this.parametros.tipoInsumoAlteracao = this.tipoInsumoAlteracao;
		this.applicationService.put(this.servicoSalvarAnalise, this.parametros).subscribe((result: any) => {
			if (result.mensagemErro != null) {
				this.modal.alerta(result.mensagemErro, "Informação", "");
				return;
			}
			else if (result.mensagem != null) {
				this.cleanFields();
				this.formPai.buscarInsumos();
				this.modal.resposta(result.mensagem, "Informação", "");
				this.appModalNovoInsumo.nativeElement.style.display = 'none';
				this.appModalNovoInsumoBackground.nativeElement.style.display = 'none';
			}
		});
	}

	carregarInsumosOriginalEAlterado(id) {
		this.applicationService.get(this.buscarInsumosOriginalAlterado, { id: id }).subscribe((result: any) => {
			this.model1 = this.parametros = result.insumoOriginal;
			this.parseDados(this.model1,result.insumoOriginal);
			this.parametros.isAlteracao = this.isAlteracao;
			this.parametros.tipoInsumoAlteracao = this.tipoInsumoAlteracao;

			if (result.insumoAlterado != null){
				this.model2 = result.insumoAlterado;
				this.parseDados(this.model2,result.insumoAlterado);
			}
			

			this.carregarAnalise(this.idLeInsumo);
		});
	}

	parseDados(model, fonte) {
		model.codigoInsumo = fonte.codigoInsumo.toString();

		if (fonte != null) {
			model.tipoInsumo = fonte.tipoInsumo;
			model.idLeInsumo = fonte.idLeInsumo;
			if (fonte.tipoInsumo == 'P') {
				model.codigoNCM1 = fonte.idCodigoNCM;
				model.idInsumo = fonte.idCodigoDetalhe;
				model.codigoDetalhe = fonte.codigoDetalhe;
			}
			else if (fonte.tipoInsumo != 'P') {
				model.codigoNCM2 = fonte.idCodigoNCM;
				model.descricaoInsumo = fonte.descricaoInsumo;
			}
			model.codigoNCM = fonte.codigoNCMFormatado;
			model.codigoUnidadeMedida = fonte.codigoUnidadeMedida;
			model.descricaoUnidadeMedida = fonte.descricaoUnidadeMedida;
			model.valorCoeficienteTecnico = fonte.valorCoeficienteTecnico;
			model.codigoPartNumber = fonte.codigoPartNumber;
			model.descricaoEspecTecnica = fonte.descricaoEspecTecnica;
		}
	}
	public analisar(formPai, item, isAlteracao) {
		this.path = "Analisar";
		this.formPai = formPai;
		this.somenteLeitura = true;
		this.isAlteracao = isAlteracao;
		this.exibirOriginalEAlterado = (this.isAlteracao && item.existeCopia) || item.situacaoInsumo == null ? true : false;
		this.idLeInsumo = item.idLeInsumo;
		this.tipoInsumoAlteracao = item.tipoInsumoAlteracao; //conforme enumTipoInsumoAlteracao.ts
		if (formPai != null && formPai.model != null) {
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}

		if (this.exibirOriginalEAlterado) {
			this.carregarInsumosOriginalEAlterado(item.idLeInsumo);
		} else {
			this.parametros.codigoInsumo = item.codigoInsumo.toString();

			if (formPai != null && formPai.model != null) {
				this.codigoProduto = formPai.model.codigoProdutoSuframa;
				this.idLe = formPai.model.idLe;
			}
			if (item != null) {
				this.parametros.tipoInsumo = item.tipoInsumo;
				this.parametros.idLeInsumo = item.idLeInsumo;
				if (item.tipoInsumo == 'P') {
					this.parametros.codigoNCM1 = item.idCodigoNCM;
					this.parametros.idInsumo = item.idCodigoDetalhe;
					this.parametros.codigoDetalhe = item.codigoDetalhe;
				}
				else if (item.tipoInsumo != 'P') {
					this.parametros.codigoNCM2 = item.idCodigoNCM;
					this.parametros.descricaoInsumo = item.descricaoInsumo;
				}
				this.parametros.codigoNCM = item.codigoNCMFormatado;
				this.parametros.codigoUnidadeMedida = item.codigoUnidadeMedida;
				this.parametros.descricaoUnidadeMedida = item.descricaoUnidadeMedida;
				this.parametros.valorCoeficienteTecnico = item.valorCoeficienteTecnico;
				this.parametros.codigoPartNumber = item.codigoPartNumber;
				this.parametros.descricaoEspecTecnica = item.descricaoEspecTecnica;
				this.carregarAnalise(this.idLeInsumo);
			}
			else {
				this.parametros = {};
			}
		}

		

		this.appModalNovoInsumo.nativeElement.style.display = 'block';
		this.appModalNovoInsumoBackground.nativeElement.style.display = 'block';
	}
	carregarAnalise(id) {
		this.applicationService.get(this.servicoSalvarAnalise, id).subscribe((result: any) => {
			this.parametros.situacaoInsumo = result.situacaoInsumo != null ? result.situacaoInsumo.toString() : '0';
			this.parametros.descricaoErro = result.ultimoInsumoErro != null ? result.ultimoInsumoErro.descricaoErro : null;

			if (result.mensagemErro != null) {
				this.modal.alerta(result.mensagemErro, "Informação", "");
				return;
			}
			else if (result.mensagem != null) {
				this.cleanFields();
				this.formPai.buscar();
				this.modal.resposta(result.mensagem, "Informação", "");
				this.appModalNovoInsumo.nativeElement.style.display = 'none';
				this.appModalNovoInsumoBackground.nativeElement.style.display = 'none';
			}
		});
	}

	public alterar(formPai, item) {
		this.path = "Alterar";
		this.formPai = formPai;
		this.somenteLeitura = false;
		if (formPai != null && formPai.model != null) {
			this.codigoProduto = formPai.model.codigoProdutoSuframa;
			this.idLe = formPai.model.idLe;
		}
		if (item != null) {
			this.parametros.tipoInsumo = item.tipoInsumo;
			this.parametros.idLeInsumo = item.idLeInsumo;
			if (item.tipoInsumo == 1) {
				this.parametros.codigoNCM1 = item.idCodigoNCM;
				this.parametros.idInsumo = item.idCodigoDetalhe;
				this.parametros.codigoDetalhe = item.codigoDetalhe;
				this.parametros.descricaoInsumo = item.descricaoInsumo;
			}
			else if (item.tipoInsumo > 1) {
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

	public fechar() {
		if (this.path == "Visualizar") {
			this.cleanFields();
			this.appModalNovoInsumo.nativeElement.style.display = 'none';
			this.appModalNovoInsumoBackground.nativeElement.style.display = 'none';
		}
		else {
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
		if (this.codigoNCM1 != null)
			this.codigoNCM1.clear();
		if (this.codigoNCM2 != null)
			this.codigoNCM2.clear();
		if (this.codigoInsumo1 != null)
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

	public selecionaNCM(event) {
		if (event != null) {
			let cod = event.text.split("|")[0].trim();
			this.parametros.codigoNCM = cod;
			if (this.codigoInsumo1 != null)
				this.codigoInsumo1.clear();
			this.applicationService.get("ViewNcm", event.id).subscribe((result: any) => {
				if (result != null) {
					this.parametros.codigoUnidadeMedida = result.idNcmUnidadeMedida;
					this.parametros.descricaoUnidadeMedida = result.descricaoUnidadeMedida;
				}
			});
		}
	}

	public selecionaInsumo(event) {
		if (event != null) {
			let cod = event.text.split("|")[0].trim();
			let desc = event.text.split("|")[1].trim();
			this.parametros.codigoDetalhe = cod;
			this.parametros.descricaoInsumo = desc;
		}
	}

	onBlurValidar() {

		if (!(this.isNumber(this.coefTec.nativeElement.value)) &&
			this.coefTec.nativeElement.value) {

			this.modal.alerta("Valor inválido", 'Informação');
			this.coefTec.nativeElement.value = null;
			this.parametros.valorCoeficienteTecnico = null;
			return false;
		} else {
			let val = 9999999.99999999;
			if (this.coefTec.nativeElement.value != null && this.coefTec.nativeElement.value != "") {
				let valorAtual = parseFloat(this.replaceVirgula(this.coefTec.nativeElement.value));

				if (valorAtual < val) {
					this.parametros.valorCoeficienteTecnico = valorAtual;
				}
				else {
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

	replaceVirgula(value): string {
		return value.replace(',', '.');
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
			else if (result.mensagem != null) {
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
		}*/
		else if (this.parametros.descricaoEspecTecnica == null || this.parametros.descricaoEspecTecnica == "") {
			this.modal.alerta("Preencha o campo Especificação Técnica", "Informação");
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
