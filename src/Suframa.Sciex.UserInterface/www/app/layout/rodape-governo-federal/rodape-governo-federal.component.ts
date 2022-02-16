// http://barra.governoeletronico.gov.br
import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
	selector: 'app-rodape-governo-federal',
	template: '<footer><div id="footer-brasil"></div></footer>',
	styles: ['#footer-brasil {   background: none repeat scroll 0% 0% #00420c;   padding: 1em 0px;   max-width: 100%; }',
		'footer {  position: absolute;  right: 0;  bottom: 0;  left: 0;  padding: 0rem;  background-color: #efefef;  text-align: center;}'],
	encapsulation: ViewEncapsulation.None // https://stackoverflow.com/questions/40116678/angular2-styleurls-not-loading-external-styles
})

export class RodapeGovernoFederalComponent {
	constructor() { }
}
