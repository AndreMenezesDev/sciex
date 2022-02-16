import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterSetorArmazenamentoComponent } from './manter-setor-armazenamento.component';
import { SetorArmazenamentoGridComponent } from './grid/grid.component';
import { FormularioSetorArmazenamentoComponent } from './formulario/formulario.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterSetorArmazenamentoComponent,
        SetorArmazenamentoGridComponent,
        FormularioSetorArmazenamentoComponent        
	],
})
export class ManterSetorArmazenamentoModule { }
