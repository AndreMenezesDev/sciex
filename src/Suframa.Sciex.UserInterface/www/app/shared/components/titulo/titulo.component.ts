import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'app-titulo',
	template: '<h1 class="m-b-xs text-black"><ng-content></ng-content></h1>',
})
export class TituloComponent {
	constructor() { }
}
