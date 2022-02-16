import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { UsuarioPapelComponent } from './usuario-papel.component';
import { UsuarioPapelGridComponent } from './grid/grid.component';
import { ViewsComponentsModule } from '../../views-components/views-components.module';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
		ViewsComponentsModule,
	],
	declarations: [
		UsuarioPapelComponent,
		UsuarioPapelGridComponent,
	],
	exports: [
		UsuarioPapelComponent,
		UsuarioPapelGridComponent,
	]
})
export class UsuarioPapelModule { }
