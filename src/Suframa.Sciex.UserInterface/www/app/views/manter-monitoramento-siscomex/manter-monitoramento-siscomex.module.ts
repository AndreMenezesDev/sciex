import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterMonitoramentoSiscomexGrid } from './grid/grid.component';
import { ManterMonitoramentoSiscomex } from './manter-monitoramento-siscomex.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
    ],
    declarations: [
		ManterMonitoramentoSiscomexGrid,
		ManterMonitoramentoSiscomex,
	],
})
export class ManterMonitoramentoSiscomexModule { }
