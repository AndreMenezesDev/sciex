import { Component, ViewChild } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { MessagesService } from '../../../shared/services/messages.service';

@Component({
	selector: 'app-modal-editar-detalhe-insumo',
	templateUrl: './modal-editar-detalhe-insumo.component.html',
})

export class ModalEditarDetalheInsumoComponent {	
	idPEDetalheInsumo: number;
	parametros: any = {};
	servico = 'PEDetalheInsumo';
	servicoCorrigirDetalhe = 'CorrigirPEDetalheInsumo';

	// #region campos Anexo
	@ViewChild('arquivo') arquivo;
	ocultarInputAnexo = false;
	limiteArquivo = 10485760; // 10MB
	temArquivo = false;
	filetype: string;
	filesize: number;
	types = ['application/x-zip-compressed','application/pdf'];
	// #endregion

	@ViewChild('modalidade') modalidade;
	@ViewChild('tipoExportacao') tipoExportacao;

	formPai: any;
	isDisplay: boolean = false;
	codigoProduto: any;
	idLe: any;
	path: string;
	somenteLeitura: boolean = false;
	servicoSalvarAnalise = "";
	campoAlterado: boolean = false;
	isQuadroNacional: boolean;
	titulo: string;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard : AuthGuard,
		private msg: MessagesService
	) { }

	@ViewChild('appModalEditarDetalhe') appModalEditarDetalhe;
	@ViewChild('appModalEditarDetalheBackground') appModalEditarDetalheBackground;


	@ViewChild('formularioB') formulario;

	@ViewChild('codigoNCM1') codigoNCM1;
	@ViewChild('codigoNCM2') codigoNCM2;
	@ViewChild('codigoInsumo1') codigoInsumo1;
	@ViewChild('insumoDesc') insumoDesc;
	@ViewChild('coefTec') coefTec;

	confirmaFechar(){
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.fechar();
				}
			});
	}
	public fechar() {
		this.limparDados();
		this.appModalEditarDetalhe.nativeElement.style.display = 'none';
		this.appModalEditarDetalheBackground.nativeElement.style.display = 'none';
	}

	public abrir(formPai, idPEDetalheInsumo,dados){
		this.isQuadroNacional = dados.isQuadroNacional;
		this.idPEDetalheInsumo = idPEDetalheInsumo;
		this.limparDados();
		this.formPai = formPai;
		this.parametros = dados;
		if (!this.parametros.isCorrecao){
			this.titulo = "Alterar";
		}else{
			this.titulo = "Corrigir";
		}
		this.parametros.idPEProduto = formPai.idPEProduto;
		this.parametros.idPEInsumo = formPai.idPEInsumo;
		this.parametros.quantidade = parseFloat(dados.quantidade).toFixed(5);
		this.parametros.valorUnitario = parseFloat(dados.valorUnitario).toFixed(7);
		if (!this.isQuadroNacional){
			this.parametros.valorFrete = parseFloat(dados.valorFrete).toFixed(7);
		}
		

		this.appModalEditarDetalhe.nativeElement.style.display = 'block';
		this.appModalEditarDetalheBackground.nativeElement.style.display = 'block';
	}
	limparDados() {
		this.parametros = {};
		this.limparAnexo();
	}

	onBlurQtTotal(event){
		if(event.target.value !== ''){
	
			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.quantidade = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:5});
				this.campoAlterado = true;
			}
		}

	}

	onBlurVlrUnitario(event){
		if(event.target.value !== ''){
	
			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.valorUnitario = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:2});
				this.campoAlterado = true;
			}
		}

	}

	onBlurVlrUnitarioFOB(event){
		if(event.target.value !== ''){
	
			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.valorUnitarioFOB = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:2});
				this.campoAlterado = true;
			}
		}

	}

	confirmaSalvar(){
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.validarCampos();
				}
			});
	}

	public validarCampos(){

		if (this.parametros.quantidade == null || this.parametros.quantidade == undefined || this.parametros.quantidade == ''){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace("$","Quantidade"));
			return;
		}

		if (this.parametros.valorUnitario == null || this.parametros.valorUnitario == undefined){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace("$","Valor Unitário"));
			return;
		}

		this.salvar();
	}

	formatarParaConverter(value): string{
		let valor = value.replace('.','');
		valor = valor.replace(',','.');
		return valor;
	}

	formatarSemAlteracaoQtTotal(event){
		if(event !== ''){
			event = Number(event);
			this.parametros.quantidade = event.toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:5});
		}
	}

	formatarSemAlteracaoVlrUnitario(event){
		if(event !== ''){
			event = Number(event);
			this.parametros.valorUnitario = event.toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:2});

		}
	}
	public salvar(){

		this.parametros.quantidade = Number(this.parametros.quantidade);
		this.parametros.valorUnitario = Number(this.parametros.valorUnitario);

		if (!this.isQuadroNacional)
			this.parametros.valorFrete = Number(this.parametros.valorFrete);

		this.parametros.idPEDetalheInsumo = this.idPEDetalheInsumo;	
		this.parametros.isQuadroNacional = this.isQuadroNacional;	

		if(!this.parametros.isCorrecao) {
			this.applicationService.put(this.servico, this.parametros).subscribe((result: any) => {
				if (result == 1) {
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
					.subscribe(()=>{
						this.campoAlterado = false;
						this.formPai.ngOnInit();
	
						this.appModalEditarDetalhe.nativeElement.style.display = 'none';
						this.appModalEditarDetalheBackground.nativeElement.style.display = 'none';
					});
				}else if (result == 2){				
					this.modal.alerta("A quantidade total excede a quantidade máxima permitida", "Informação", "");
					return;
				}
				else if (result == 3){				
					this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
					return;
				}
			});
		}else{
			this.applicationService.put(this.servicoCorrigirDetalhe, this.parametros).subscribe((retorno: any) => {
				if (retorno.resultado) {
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
					.subscribe(()=>{
						this.campoAlterado = false;
						this.formPai.idPEInsumo = retorno.idNovoInsumo;
						this.formPai.ngOnInit();
	
						this.appModalEditarDetalhe.nativeElement.style.display = 'none';
						this.appModalEditarDetalheBackground.nativeElement.style.display = 'none';
					});
				}else{	
					this.modal.alerta(retorno.mensagem, "Informação", "");			
					return;
				}
			});
		}
	}

	// #region MetodosAnexo
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
							this.ocultarInputAnexo = true;
							this.parametros.nomeAnexo = file.name;
							this.parametros.anexo = (reader.result as string).split(',')[1];
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
				this.modal.alerta(this.msg.FAVOR_SELECIONAR_UM_ARQUIVO_NO_FORMATO_ZIP,'Atenção');
				this.limparAnexo();
			}

		}else{
			this.temArquivo = false;
			this.parametros.nomeAnexo = null;
			this.parametros.anexo = null;
		}

	}

	limparAnexo() {
		this.temArquivo = false;
		if (this.arquivo != undefined)
			this.arquivo.nativeElement.value = '';
		this.ocultarInputAnexo = false;
		this.parametros.anexo = null;
		this.parametros.nomeAnexo = null;
	}
	// #endregion

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
		this.appModalEditarDetalhe.nativeElement.style.display = 'block';
		this.appModalEditarDetalheBackground.nativeElement.style.display = 'block';
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
				this.appModalEditarDetalhe.nativeElement.style.display = 'none';
				this.appModalEditarDetalheBackground.nativeElement.style.display = 'none';
			}
		});
	}
	
	
	cleanFields() {
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
