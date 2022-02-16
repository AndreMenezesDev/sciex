import { Component, ViewChild, Pipe, PipeTransform, Input, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router, Route } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterParidadeCambialParam } from '../../../view-model/manterParidadeCambialParam';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';

@Pipe({
    name: 'customDateFormat',
})

@Component({
	selector: 'app-manter-paridade-cambial-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterParidadeCambialFormularioComponent implements OnInit {
	path: string;
	parametros: any = {};
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servicoParidadeCambial = 'ParidadeCambial';
	servicoParidadeCambialGrid = 'ParidadeCambial';
	habilitarCampoCodigo: boolean;
    model: manterParidadeCambialParam = new manterParidadeCambialParam();
	@ViewChild('formulario') formulario;
	@ViewChild('appModalAjuda') appModalAjuda;
	@ViewChild('dataOrigem') dataOrigem;
	@ViewChild('dataParidade') dataParidade;

    @Input()
    tipoGeracao: number = 0;

	constructor(
		private route: ActivatedRoute,
		private route1: Router,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private modal: ModalService,
		private validationService: ValidationService
	) {
        this.tipoGeracao = 0;
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
        this.verificarRota();
        this.parametros.listaParidadeCambialAdd = [];
        this.parametros.listaParidadeCambialRemover = [];        
	}

    ngOnInit() {
        this.parametros.dias = "";
    }

    transformDate(value: string) {
        var datePipe = new DatePipe("pt-BR");        
        value = datePipe.transform(value, 'dd/MM/yyyy');
        return value;
    }

	public verificarRota() {
		this.habilitarCampoCodigo = false;
		this.tituloPanel = 'Formulário';

		if (this.path == 'visualizar') {
			this.desabilitarTela();
			this.tituloPanel = 'Registros';
		}

		if (this.path == 'editar' || this.path == 'visualizar') {
			this.selecionar(this.route.snapshot.params['id']);
			this.habilitarCampoCodigo = true;
		}

		if (this.path == 'editar') {
			this.titulo = 'Alterar';
		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}


	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterParidadeCambialParam>(this.servicoParidadeCambial, id).subscribe(result => {
			this.model = result;
		});
	}

	public validarData() {			
		var date = new Date();
		var year  = date.getFullYear();
		var month = (date.getMonth() + 1).toString();		
		month.toString().length == 1 ? month = "0"+month : month = month+1;
		var day   = date.getDate().toString();  
		var dataAtual = year +'-'+ month +'-'+ day;
		this.dataParidade.nativeElement.setCustomValidity('');
		var dataCorreta = new Date(dataAtual);	
		
		let dat = new Date();
		let dataHoje = dat.toLocaleDateString('en-CA');

		if(this.parametros.dataParidadeInicio < dataHoje){
			this.modal.alerta('Data da Paridade deve ser maior ou igual a data Corrente', 'Danger', '');
			return false;
		} else{
			return true;
		}
	}

	public salvar(tipoGeracao) {		
			
		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }
        
        this.parametros.tipoGeracao = tipoGeracao;
        this.parametros.adicionaParidade = true;

		if(this.validarData()){
			this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
                if (isConfirmado) {
                    this.parametros.dataParidade = this.parametros.dataParidadeInicio;
					this.salvarRegistro();
					
				}
			});
		}	
		//this.parametros.TipoGeracao = 2;
		//this.parametros.indSobrepor = 0;
		
	}

    private salvarRegistro() {        
        this.applicationService.put<manterParidadeCambialParam>(this.servicoParidadeCambial, this.parametros).subscribe(result => {

			if (this.path != "editar")
				localStorage.clear();

			this.parametros.dataParidadeProxima = null;

            if (result.paridadeCambialVM != null)
                this.parametros.listaParidadeCambialAdd.push(result.paridadeCambialVM);
            
            if (result.indRetorno == 1) {
                
                if (result.mensagem != "" )
                    this.modal.alerta(result.mensagem, "Erro");
				else
					this.modal.alerta(this.msg.NAO_EXISTE_ARQUIVO_PARIDADE_CAMBIAL_CADASTRADA_DATA + " "
						+ this.transformDate(this.parametros.dataOrigem), "Erro");
			}
			else if (result.indRetorno == 0 && result.dias == 1) {
				this.route1.navigate(["/paridade-cambial"]); 
                //this.modal.alerta(this.msg.NAO_EXISTE_PARIDADE_CAMBIAL_CADASTRADA_DATA + " "
                //    + this.transformDate(this.parametros.dataOrigem), "Erro");
            }
            else if (result.indRetorno == 3) {
                this.parametros.dataParidadeProxima = result.dataParidadeProxima;
                this.parametros.indSobrepor = 1;                
                this.confirmarSubstituir();               
			} 
			else if(result.indRetorno == 4){
				this.modal.alerta(this.msg.PARIDADE_JA_CADASTRADA, " ", " ");
				return false;
			}          
			else {
				
                this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/paridade-cambial");
                this.model = result;
            }
		});

    }

    confirmarSubstituir() {
        this.modal.confirmacao("Paridade já cadastrada para a data informada " +
            this.transformDate(this.parametros.dataParidadeProxima)
            + ". Deseja substituir?", '','')
            .subscribe(isConfirmado => {
                if (isConfirmado) {                    
                    this.parametros.adicionaParidade = true;
					this.salvarRegistro();
                }
                else
                {
					this.parametros.adicionaParidade = false;					
                    this.salvarRegistro();
                }
            });
	}

	ajuda() {
		this.appModalAjuda.abrir();
	}

}
