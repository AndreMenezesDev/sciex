import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'due' })
export class DuePipe implements PipeTransform {
	transform(value: any, args?: any): any {
		if (!value) { return value; } //52214/001.5
		return value.replace(/^(\d{5})(\d{3})(\d{1})/, '$1/$2-$3');
	}
}
