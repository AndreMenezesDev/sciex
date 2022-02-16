import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterFundamentoLegalFormularioComponent } from './formulario/formulario.component';
import { ManterFundamentoLegalGridComponent } from './grid/grid.component';
import { ManterFundamentoLegalComponent } from './manter-fundamento-legal.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterFundamentoLegalFormularioComponent,        
		ManterFundamentoLegalGridComponent,
		ManterFundamentoLegalComponent
		
	],
})
export class ManterFundamentoLegalModule { }
