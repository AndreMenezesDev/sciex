import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterRecintoAlfandegasComponent } from './manter-recinto-alfandega.component';
import { ManterTipoEmbalagemGridComponent } from './grid/grid.component';
import { FormularioManteRecintoAlfandegaComponent } from './formulario/formulario-manter-recinto-alfandega.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterRecintoAlfandegasComponent,
        ManterTipoEmbalagemGridComponent,
        FormularioManteRecintoAlfandegaComponent        
	],
})
export class ManterRecintoAlfandegaModule { }
