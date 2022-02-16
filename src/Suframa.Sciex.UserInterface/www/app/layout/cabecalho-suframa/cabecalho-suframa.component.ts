import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';

@Component({
	selector: 'app-cabecalho-suframa',
	templateUrl: './cabecalho-suframa.component.html',
	encapsulation: ViewEncapsulation.None
})
export class CabecalhoSuframaComponent implements OnInit {
	versao: string;

	@Input() authenticated: boolean;

	constructor() { }

	ngOnInit() {
        this.versao = '1.0.0';
    }
	
}
