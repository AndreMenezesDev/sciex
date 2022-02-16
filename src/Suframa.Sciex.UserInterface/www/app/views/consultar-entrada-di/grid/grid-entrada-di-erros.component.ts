import { Component, EventEmitter, Injectable, Input, Output } from "@angular/core";

@Component({
	selector: 'app-consultar-entrada-di-erros-grid',
	templateUrl: './grid-entrada-di-erros.component.html'
})

@Injectable()
export class ConsultarEntradaDiErrosGridComponent {

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
    
}