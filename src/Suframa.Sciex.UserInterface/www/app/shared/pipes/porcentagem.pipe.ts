import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'porcentagem' })
export class PorcentagemPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		if (!value) { return value; }
		return value.toString().replace('.', ',') + '%';
	}
}
