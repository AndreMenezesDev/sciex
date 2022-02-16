import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule, Title } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';
import { BootstrapModalModule } from 'angularx-bootstrap-modal';
import { CabecalhoGovernoFederalComponent } from "./layout/cabecalho-governo-federal/cabecalho-governo-federal.component";
import { LayoutModule } from './layout/layout.module';
import { LoadingComponent } from './views-components/loading/loading.component';
import { AuthGuard } from '../app/shared/guards/auth-guard.service';


@NgModule(
	{
		declarations: [
			AppComponent,
			CabecalhoGovernoFederalComponent,
			LoadingComponent,			

		],
		imports: [
			BootstrapModalModule,
			BrowserAnimationsModule,
			BrowserModule,
			FormsModule,
			LayoutModule,
			
		],		
	bootstrap: [AppComponent],
		providers: [AuthGuard,
			{ provide: LOCALE_ID, useValue: "pt-BR" },
			{ provide: LocationStrategy, useClass: HashLocationStrategy }
		],

	})


export class AppModule { }
