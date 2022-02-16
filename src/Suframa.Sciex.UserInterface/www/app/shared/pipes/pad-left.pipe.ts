import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'padLeft' })
export class PadLeftPipe implements PipeTransform {
	transform(value: any, number: number, char: any): string {
		return value.toString().padStart(number, char);
	}
}
