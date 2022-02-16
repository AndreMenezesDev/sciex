import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterGupoBeneficioGridComponent } from './grid/grid-manter-grupo-beneficio.component';
import { ManterGrupoBeneficioComponent } from './manter-grupo-beneficio.component';
import { CadastrarNCMBeneficioComponent } from './formulario/formulario-cadastrar-ncm-beneficio.component';
import { CadastrarNCMBeneficioGridComponent } from './grid/grid-ncm-beneficio.component';
import { CadastrarBeneficioComponent } from './formulario/formulario-cadastrar-beneficio.component';
import { AlterarBeneficioComponent } from './formulario/formulario-alterar-beneficio.component'
import { ModalAmparoLegalBeneficio } from './modal/modal-amparo-legal-beneficio.component';
import { ModalJustificativaStatusGrupoBeneficioComponent } from './modal-justificativa-status/modal-justificativa-status.component';
import { ModalHistoricoBeneficioComponent } from './modal-historico/modal-historico-beneficio.component';
import { BeneficioHistoricoGridComponent } from './grid-historico/grid-historico-beneficio.component';
import { ModalEmpresaPDIComponent } from './modal-empresa-pdi/modal-empresa-pdi.component';
import { EmpresaPDIGridComponent } from './grid/grid-empresa-pdi.component';

@NgModule({
    imports: [

      CommonModule,
      FormsModule,
      RouterModule,
      SharedModule,
    ],
    declarations: [

      ManterGupoBeneficioGridComponent,
      ManterGrupoBeneficioComponent,
      CadastrarBeneficioComponent,
      ModalAmparoLegalBeneficio,
      AlterarBeneficioComponent,
      CadastrarNCMBeneficioComponent,
      CadastrarNCMBeneficioGridComponent,
      ModalJustificativaStatusGrupoBeneficioComponent,
      ModalHistoricoBeneficioComponent,
      BeneficioHistoricoGridComponent,
      ModalEmpresaPDIComponent,
      EmpresaPDIGridComponent
	],
})
export class ManterGrupoBeneficioModule { }
