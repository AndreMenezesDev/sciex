import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../../app.component';

@Component({
	selector: 'app-rodape-menu',
	templateUrl: 'rodape-menu.component.html',
})

export class RodapeMenuComponent {
	constructor(
		private appSciex: AppComponent
	) { }

	onLogout() {
		alert('oi');
		this.appSciex.onLogout();
	}

}
