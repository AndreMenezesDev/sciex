import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import {Location} from '@angular/common';

declare var $: any;

@Component({
	selector: 'app-manter-plano-formulario-plano-comprovacao',
	templateUrl: './formularioPlanoComprovacao.component.html'
})

export class ManterPlanoFormularioPlanoComprovacaoComponent implements OnInit {
	path: string;
	desabilitado: boolean;
	servico = "PlanoExportacao";
	servicoProduto = "PEProduto"
	servicoAnexo = "PlanoExportacaoAnexo"
	somenteLeitura: boolean;
	model: any = {};
	modelProduto: any = {};
	codigo1: string;
	isCancelarVisible: boolean;
	parametros: any = {};
	grid: any = { sort: {} };
	listaProdutos: any;
	listaMercadorias: any;
	descricaoTipoPli: string;
	idPEProdutoOpen: number;
	CNPJ: string;
	cpfDigitado: string;
	idPE : number;

	@ViewChild('arquivo') arquivo;
	ocultarInputAnexo = false;
	limiteArquivo = 10485760; // 10MB
	temArquivo = false;
	filetype: string;
	filesize: number;
	types = ['application/x-zip-compressed', 'application/zip' ,'application/pdf'];

	@ViewChild('produto') produto;
	@ViewChild('formulario') formulario;
	@ViewChild('codigo') codigo;
	@ViewChild('cpf') cpf;
	validar: boolean;
	titulo: string;
	isCorrecaoComprovacao: boolean;
	tipoExportacao: any;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private modal: ModalService,
		private router: Router,
		private validationService: ValidationService,
		private Location: Location,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idPE = this.route.snapshot.params['id'];		
	}

	ngOnInit() {
		this.verificarRota();

		$(document).ready(function () {
			$("#collapse-init").click(function () {
				$('.accordion-toggle').collapse('hide');
				$('.panel-collapse').collapse('hide');
			});
		});
	}

	public verificarRota() {
		this.somenteLeitura = false;
		this.isCancelarVisible = true;
		this.temArquivo = false;
		this.ocultarInputAnexo = false;

		this.desabilitarTela();

		this.isCancelarVisible = false;

		if (this.path == 'cadastrar') {
			this.titulo = "Cadastrar";
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.isCorrecaoComprovacao = false;
		}
		if (this.path == 'cadastrarcomprovacao') {
			this.titulo = "Cadastrar";
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.isCorrecaoComprovacao = false;
		}
		else if (this.path == 'correcao') {
			this.titulo = "Corrigir";
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.isCorrecaoComprovacao = false;
		}
		else if (this.path == 'correcaoComprovacao') {
			this.titulo = "Corrigir";
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.isCorrecaoComprovacao = true;
		}
		else if (this.path == 'validar-produto') {
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.isCorrecaoComprovacao = false;
		}
		else if (this.path == 'validar-insumo') {
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.isCorrecaoComprovacao = false;
		}
		else if (this.path == 'visualizarcomprovacao') {
			this.selecionar(this.route.snapshot.params['id']);
			this.somenteLeitura = this.parametros.somenteLeitura = true;
			this.isCorrecaoComprovacao = true;
		}
		
	}	

	public onBlurEvent() {
		if (this.codigo.nativeElement.value != undefined)
			if (this.codigo.nativeElement.value.length != undefined)
				if (this.codigo.nativeElement.value.length > 0) {

					this.codigo1 = this.codigo.nativeElement.value;
					this.codigo1 = ("000000" + this.codigo1).slice(-6);
				}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.listaProdutos = null;
		this.applicationService.get(this.servico, id).subscribe((result: any) => {

			this.model = result;
			this.tipoExportacao = result.tipoExportacao;
			this.model.nomeAnexo= this.model.listaAnexos[0].nomeArquivo;
			if(result.listaAnexos.length > 0 && result.listaAnexos[0].anexo != "") {
				this.temArquivo = true;
				this.ocultarInputAnexo = true;
			}

			this.listaProdutos = result.listaPEProdutos;
		});
	}

	downloadAnexo() {
		try {
			const hashPDF = this.model.listaAnexos[0].anexo;
			const linkSource = 'data:' + 'application/pdf' + ';base64,' + hashPDF;
			const downloadLink = document.createElement('a');
			const fileName = this.model.listaAnexos[0].nomeArquivo;

			document.body.appendChild(downloadLink);

			downloadLink.href = linkSource;
			downloadLink.download = fileName;

			downloadLink.target = '_self';

			downloadLink.click();
		} catch (err) {
			this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
		}

	}

	adicionarProduto() {

		this.produto.valorInput.nativeElement.setCustomValidity('');

		if (this.parametros.idLEProduto == undefined || this.parametros.idLEProduto == null) {
			this.produto.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		}

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.parametros.idPlanoExportacao = this.model.idPlanoExportacao;

		this.applicationService.put(this.servicoProduto, this.parametros).subscribe((result: any) => {

			if (result.mensagemErro != "" && result.mensagemErro != null) {
				this.modal.alerta(this.msg.PRODUTO_JA_ADICIONADO, "Informação", "");
			} else {
				this.applicationService.get(this.servico, this.idPE).subscribe((result: any) => {
					this.listaProdutos = result.listaPEProdutos;

					//Limpar componente select 2
					this.produto.clearInput = true;
					let element: HTMLElement = document.getElementById('valorInput') as HTMLElement;
					element.click();
					this.produto.clearInput = false;
				});
			}
		});
	}

	public excluirProduto(item) {
		this.applicationService.delete(this.servicoProduto, item.idPEProduto).subscribe(result => {
			const index: number = this.listaProdutos.indexOf(item);
			if (index !== -1)
				this.listaProdutos.splice(index, 1);

			this.selecionar(this.idPE);
		}, error => {
			const index: number = this.listaProdutos.indexOf(item);
			if (index !== -1)
				this.listaProdutos.splice(index, 1);
		});			
	}

	confirmarExclusaoProduto(item) {
		this.modal.confirmacao("Todas os dados relacionados ao produto também serão removidos. Confirma a operação?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.excluirProduto(item);
				}
			});
	}

	abrirListaMercadorias(idPEProduto) {
		this.idPEProdutoOpen = idPEProduto;
		this.parametros.modelProduto = this.modelProduto;
		this.parametros.idPEProdutoOpen = idPEProduto;


		if ($("#" + idPEProduto).hasClass('fa-plus-square-o')) {
			$(".a").removeClass('fa-minus-square-o');
			$(".a").addClass('fa-plus-square-o');
			$("#" + idPEProduto).removeClass('fa-plus-square-o');
			$("#" + idPEProduto).addClass('fa-minus-square-o');
		} else {
			$(".a").removeClass('fa-minus-square-o');
			$(".a").addClass('fa-plus-square-o');
			$("#" + idPEProduto).removeClass('fa-minus-square-o');
			$("#" + idPEProduto).addClass('fa-plus-square-o');
		}

		$('.accordion-toggle').collapse('hide');
		$('.panel-collapse').collapse('hide');
	}

	voltar(){
		this.Location.back();
	}

	limparAnexo() {
		this.temArquivo = false;
		if (this.arquivo != undefined)
			this.arquivo.nativeElement.value = '';
		this.ocultarInputAnexo = false;
		this.model.listaAnexos[0].anexo = null;
		//this.modelPli.nomeAnexo = null;
	}
	
	onFileChange(event) {

		let reader = new FileReader();

		if (event.target.files && event.target.files.length > 0) {
			let file = event.target.files[0];

			reader.readAsDataURL(file);
			this.filetype = file.type;
			this.filesize = file.size;


			if(this.types.includes(this.filetype)) {
				if(file.name.length <= 50){
					if(this.filesize < this.limiteArquivo){
						this.temArquivo = true;
	
						reader.onload = () => {
							

							if (this.model.listaAnexos.length > 0){
								this.model.listaAnexos[0].anexo = (reader.result as string).split(',')[1];
							}else{
								(this.model.listaAnexos as any[]).push({
									anexo: (reader.result as string).split(',')[1], 
									idPlanoExportacao: this.model.idPlanoExportacao
								});
							}
							this.model.listaAnexos[0].nomeArquivo= file.name;

							this.applicationService.put(this.servicoAnexo, this.model).subscribe((result: any) => {

								if (result == null) {
									this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "")
									.subscribe(()=>{
										this.ocultarInputAnexo = false;
									});;
								} else {
									this.modal.resposta(this.msg.ALTERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
									.subscribe(()=>{
										this.ocultarInputAnexo = true;
									});
								}
							});
						};
					}else{
					this.modal.alerta(this.msg.ANEXO_ACIMA_DO_LIMITE.replace('($)','10'),'Atenção');
					this.limparAnexo();
					}
				} else {
					this.modal.alerta("O nome do arquivo ultrapassou o limite de 50 caracteres",'Atenção');
					this.limparAnexo();
				}
			} else {
				this.modal.alerta('Favor selecionar um arquivo no formato ZIP. ou PDF.','Atenção');
				this.limparAnexo();
			}

		}else{
			this.temArquivo = false;
			//this.modelPli.nomeAnexo = null;
			//this.modelPli.anexo = null;
		}

	}

	abrirPropriedadeProduto(item){
		if(this.tipoExportacao != 'CO'){
			this.router.navigate([`/manter-plano-exportacao/${item.idPEProduto}/propriedadeprodutocomprovacao`])
		}else{
			this.router.navigate([`/manter-plano-exportacao/${item.idPEProduto}/propriedadeprodutocomprovacaocorrecao`])
		}
	}
}
