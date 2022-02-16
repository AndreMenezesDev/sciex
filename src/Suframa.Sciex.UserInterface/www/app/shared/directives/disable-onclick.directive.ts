import { Directive, EventEmitter, HostListener } from '@angular/core';

@Directive({ selector: '[disableOnClick]' })
export class DisableOnClickDirective {
	disabled = 'disabled';
	time = 2000;

	public click = new EventEmitter();

	@HostListener('click', ['$event']) onClick(element) {
		// element.target.setAttribute(this.disabled, this.disabled);

		element.target.classList.add(this.disabled);

		setTimeout(() => {
			// element.target.removeAttribute(this.disabled)
			element.target.classList.remove(this.disabled);
		}, this.time);
	}
}
