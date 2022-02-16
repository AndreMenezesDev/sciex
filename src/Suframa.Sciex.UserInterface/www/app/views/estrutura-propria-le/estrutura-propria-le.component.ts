import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { EnumPerfil } from '../../shared/enums/EnumPerfil';
import { ValidationService } from '../../shared/services/validation.service';



@Component({
	selector: 'app-estrutura-propria-le.component',
	templateUrl: './estrutura-propria-le.component.html'
})


export class EstruturaPropriaLEComponent implements OnInit {
	parametros: any = {};
	servicoEstruturaPropria = "EstruturaPropriaLEArquivo";
	model: any;
	somenteLeitura: boolean = false;

	filetype: string;
	filesize: number;
	inscricaoCadastral: string;
	razaoSocial: string;

	@ViewChild('inputArquivo') inputArquivo;
	@ViewChild('optionchecked') optionchecked;
	@ViewChild('btnReset') btnReset;
	@ViewChild('appUploadFile') appUploadFile;
	@ViewChild('appModalEstruturaPropriaLE') appModalEstruturaPropriaLE;

	@ViewChild('formulario') formulario;
	@ViewChild('codigoProduto') codigoProduto;
	@ViewChild('codigoTipoProduto') codigoTipoProduto;
	@ViewChild('codigoNCM') codigoNCM;
	@ViewChild('unidadeMedida') unidadeMedida;
	@ViewChild('modeloProduto') modeloProduto;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private validationService: ValidationService,
		private router: Router
	) {
		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	}

	ngOnInit(): void {
		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
			if (result && result.perfis.includes(EnumPerfil.pessoaJuridica)) {
				this.inscricaoCadastral = result.usuInscricaoCadastral;
				this.razaoSocial = result.usuNomeEmpresaOuLogado;
				this.parametros.razaoSocial = this.inscricaoCadastral + " | " + this.razaoSocial;
				this.parametros.inscricaoCadastral = this.inscricaoCadastral;
			}

		});
	}

	validarIncluir() {
		if (this.codigoProduto.valorInput.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Produto", "Informação");
		}
		else if (this.codigoTipoProduto.valorInput.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Tipo do Produto", "Informação");
		} else if (this.codigoNCM.valorInput.nativeElement.value.trim() == "" ) {
			this.modal.alerta("Preencha o campo NCM", "Informação");
		}
		else if (this.unidadeMedida.valorInput.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Unidade de Medida", "Informação");
		}
		else if (this.modeloProduto.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Modelo do Produto", "Informação");
		}
	}

	
	produtoModificado(event){
		this.parametros.idCodigoProdutoSuframa = event;
		this.codigoTipoProduto.clear();
		this.parametros.idCodigoTipoProduto = null;
		this.codigoNCM.clear();
		this.parametros.idCodigoNCM = null;
		this.unidadeMedida.clear();
		this.parametros.idCodigoUnidadeMedida = null;

	}
	tipoProdutoModificado(event){
		this.parametros.idCodigoTipoProduto = event;
		this.codigoNCM.clear();
		this.parametros.idCodigoNCM = null;
		this.unidadeMedida.clear();
		this.parametros.idCodigoUnidadeMedida = null;

	}
	
	ncmProdutoModificado(event){
		this.parametros.idCodigoNCM = event;
		this.unidadeMedida.clear();
		this.parametros.idCodigoUnidadeMedida = null;
	}

	enviarArquivo(event) {
		this.validarIncluir();

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		//validação se existe arquivo
        if (this.appUploadFile.isFiles())
        {
			let reader = new FileReader();

			reader.onload = function (e) {
				var dataURL = reader.result;
			}
			 
			let file = this.appUploadFile.uploader.queue[0]._file;

			reader.onload = () => {
				this.parametros.Arquivo = (reader.result as String).split(',')[1];
				this.parametros.Nomearquivo = file.name;
				this.applicationService.put<string>(this.servicoEstruturaPropria, this.parametros).subscribe(result => {
					if (parseInt(result) > 0) {
						this.modal.resposta(this.msg.ESTRUTURA_PROPRIA_SUCESSO + result, "", "");
						this.limpar();
					} else
						if (result != "" && result != null && result != undefined) {
							this.modal.alerta(result, "", "");
						}
				});
			};
			reader.readAsDataURL(file);
        } else
        {
			this.modal.alerta(this.msg.ESTRUTURA_PROPRIA_FALTA_ARQUIVO, "", "");
			return
		}
	}

	limpar() {
		this.parametros = {};
		this.parametros.inscricaoCadastral = this.inscricaoCadastral;
		this.parametros.razaoSocial = this.inscricaoCadastral + " | " + this.razaoSocial;
		this.codigoProduto.clear();
		this.codigoTipoProduto.clear();
		this.codigoNCM.clear();
		this.unidadeMedida.clear();
		this.appUploadFile.removerTodos();
	}

	public abrir() {
		this.appModalEstruturaPropriaLE.abrir();
	}
}
