import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterTipoEmabalagemComponent } from './manter-tipo-embalagem.component';
import { ManterTipoEmbalagemGridComponent } from './grid/grid.component';
import { ManteTipoEmbalagemFormularioComponent } from './formulario/formulario-tipo-embalagem.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterTipoEmabalagemComponent,
        ManterTipoEmbalagemGridComponent,
        ManteTipoEmbalagemFormularioComponent        
	],
})
export class ManterTipoEmbalagemModule { }
