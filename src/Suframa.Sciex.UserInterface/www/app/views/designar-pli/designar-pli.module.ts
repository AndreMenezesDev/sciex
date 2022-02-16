import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { DesignarPliComponent } from './designar-pli.component';
import { DesignarPliGridComponent } from './grid/grid.component';
import { ModalDesignarAnalistaComponent } from './modal/modal-designar-analista.component';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
    ],
    declarations: [
      DesignarPliComponent,
      DesignarPliGridComponent,
      ModalDesignarAnalistaComponent
	],
})
export class DesignarPliModule { }
