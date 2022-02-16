import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'protocolo' })
export class ProtocoloPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		if (!value) { return value; }
		return value.toString().padStart(6, '0');
	}
}
