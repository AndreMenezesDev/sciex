import { NgModule, LOCALE_ID } from '@angular/core';

// Importação de módulos nativos devem ser organizados por ordem alfabética
import { AppRoutingModule } from '../route/app-routing.module';
import { BootstrapModalModule } from 'angularx-bootstrap-modal';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

// Importação de componentes customizados devem ser organizados por ordem alfabética
import { CalendarModule } from 'angular-calendar';
import { ComponentsModule } from './components/components.module';
import { DirectivesModule } from './directives/directives.module';
import { InterceptorsModule } from './interceptors/interceptors.module';
import { MaskModule } from 'soft-angular-mask'; // https://github.com/renatosistemasvc/soft-angular-mask
import { PipesModule } from './pipes/pipes.module';
import { ServicesModule } from './services/services.module';
import { ToastrModule } from 'toastr-ng2';
import { PopoverModule } from 'ngx-popover';

@NgModule({
	// Imports devem ser organizados por ordem alfabética
	imports: [
		AppRoutingModule,
		BootstrapModalModule,
		BrowserModule,
		CalendarModule.forRoot(),
		CommonModule,
		ComponentsModule,
		DirectivesModule,
		FormsModule,
		HttpClientModule,
		InterceptorsModule,
		MaskModule,
		PipesModule,
		PopoverModule,
		ServicesModule,
        ToastrModule.forRoot(),     
	],
	// Exports devem ser organizados por ordem alfabética
	exports: [
		BootstrapModalModule,
		CalendarModule,
		ComponentsModule,
		DirectivesModule,
		MaskModule,
		PipesModule,
        PopoverModule       
	],
	providers: [
		{
			provide: LOCALE_ID,
			useValue: navigator.language,
		}
	]
})
export class SharedModule { }
