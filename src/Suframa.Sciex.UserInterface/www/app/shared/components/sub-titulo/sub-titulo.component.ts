import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'app-sub-titulo',
	template: '<small><ng-content></ng-content></small>',
})
export class SubTituloComponent {
	constructor() { }
}
