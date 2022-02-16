import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterNaladiComponent } from './manter-naladi.component';
import { ManterNaladiFormularioComponent } from './formulario/formulario.component';
import { ManterNaladiGridComponent } from './grid/grid.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterNaladiComponent,
		ManterNaladiFormularioComponent,        
        ManterNaladiGridComponent            
	],

})
export class ManterNaladiModule { }
