import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterCancelamentoLiComponent } from './manter-cancelamento-li.component';
import { ManterCancelamentoLiGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ManterCancelamentoLiComponent,
		ManterCancelamentoLiGridComponent
	],
})
export class ManterCancelamentoLiModule { }
