import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterUnidadeReceitaFederalComponent } from './manter-unidade-receita-federal.component';
import { ManterUnidadeReceitaFederalFormularioComponent } from './formulario/formulario.component';
import { ManterUnidadeReceitaFederalGridComponent } from './grid/grid.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ManterUnidadeReceitaFederalComponent,
        ManterUnidadeReceitaFederalFormularioComponent,        
        ManterUnidadeReceitaFederalGridComponent            
	],
})
export class ManterUnidadeReceitaFederalModule { }
