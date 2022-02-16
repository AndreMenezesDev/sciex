import { Component, EventEmitter, Injectable, Input, OnInit, Output } from "@angular/core";
import { ErrorDIProcessamentoVM } from "../../../view-model/ErrorDIProcessamentoVM";

@Component({
	selector: 'app-consultar-entrada-di-processado-grid',
	templateUrl: './grid-entrada-di-processado.component.html'
})

@Injectable()
export class ConsultarEntradaDIProcessadoGridComponent implements OnInit {

	

    constructor(
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	listaTelaErros: ErrorDIProcessamentoVM = new ErrorDIProcessamentoVM();

	ngOnInit() {
		localStorage.removeItem("TelaErrosProcessamento");
	}

	changeSize($event) {		
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	public DadosErrorProcessamento(item: any = {}) {

		this.listaTelaErros.identificador = item.identificador;
		this.listaTelaErros.cnpjEmpresa = item.cnpj;
		this.listaTelaErros.nomeEmpresa = item.nomeEmpresa;
		this.listaTelaErros.numeroDI = item.numero;
		this.listaTelaErros.dataHoraValidacao = item.dataValidacao;
		this.listaTelaErros.qtdErros = item.qtdErros;

		localStorage.setItem("TelaErrosProcessamento", JSON.stringify(this.listaTelaErros));
		
	}
}
