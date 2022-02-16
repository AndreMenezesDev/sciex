import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterAladiComponent } from './manter-aladi.component';
import { ManterAladiFormularioComponent } from './formulario/formulario.component';
import { ManterAladiGridComponent } from './grid/grid.component';
import { ChartsModule } from 'ng2-charts';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
		ChartsModule
    ],
    declarations: [
        ManterAladiComponent,
        ManterAladiFormularioComponent,        
		ManterAladiGridComponent,		
	],
})
export class ManterAladiModule { }
