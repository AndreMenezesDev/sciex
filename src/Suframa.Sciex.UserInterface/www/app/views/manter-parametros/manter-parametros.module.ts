import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterParametrosComponent } from './manter-parametros.component';
import { ManterParametrosFormularioComponent } from './formulario/formulario.component';
import { ManterParametrosGridComponent } from './grid/grid.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterParametrosComponent,
        ManterParametrosFormularioComponent,        
        ManterParametrosGridComponent            
	], 
})
export class ManterParametrosModule { }
