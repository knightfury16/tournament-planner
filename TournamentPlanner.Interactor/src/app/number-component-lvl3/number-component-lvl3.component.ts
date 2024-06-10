import { Component, Input, computed, signal } from '@angular/core';

@Component({
  selector: 'app-number-component-lvl3',
  standalone: true,
  imports: [],
  templateUrl: './number-component-lvl3.component.html',
  styleUrl: './number-component-lvl3.component.scss',
})
export class NumberComponentLvl3Component {
  _number = signal<number>(0);
  _digits = signal<unknown[]>([0, 0, 0, 0]);

  _numberOfdigits = computed(() => this._digits().length);

  @Input() set nubmerOfDigit(value: number) {
    let digits = [];

    for (let i = 0; i < value; i++) {
      digits.push(0);
    }
    this._digits.set(digits);
  }

  @Input() set number(value: number) {
    this._number.set(value);
  }
}
