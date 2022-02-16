import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterNCMExcecaoComponent } from './manter-ncm-excecao.component';
import { ManterNCMExcecaoFormularioComponent } from './formulario/formulario.component';
import { ManterNCMExcecaoGridComponent } from './grid/grid.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
		ManterNCMExcecaoComponent,
		ManterNCMExcecaoFormularioComponent,        
		ManterNCMExcecaoGridComponent
	],
})
export class ManterNCMExcecaoModule { }
