import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here

import { AppRoutingModule } from '../route/app-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ViewsModule } from '../views/views.module';
import { ViewsComponentsModule } from "../views-components/views-components.module";

import { RodapeGovernoFederalComponent } from './rodape-governo-federal/rodape-governo-federal.component';
import { RodapeMenuComponent } from './rodape-menu/rodape-menu.component';
import { MenuComponent } from './menu/menu.component';
import { CabecalhoSuframaComponent } from './cabecalho-suframa/cabecalho-suframa.component';
import { LayoutComponent } from './layout.component';
import { LoginComponent } from '../views/login/login.component';

@NgModule({
	imports: [
		AppRoutingModule,
		CommonModule,
		FormsModule,
		SharedModule,
		ViewsModule,
		ViewsComponentsModule
	],
	declarations: [
		CabecalhoSuframaComponent,
		LayoutComponent,
		LoginComponent,
		MenuComponent,
		RodapeGovernoFederalComponent,
		RodapeMenuComponent,
	],
	exports: [
		LayoutComponent,
	]
})
export class LayoutModule { }
