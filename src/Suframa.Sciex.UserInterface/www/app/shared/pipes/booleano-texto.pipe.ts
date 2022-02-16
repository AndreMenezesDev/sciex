import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'booleanoTexto' })
export class BooleanoTextoPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		return value ? 'Sim' : 'Não';
	}
}
