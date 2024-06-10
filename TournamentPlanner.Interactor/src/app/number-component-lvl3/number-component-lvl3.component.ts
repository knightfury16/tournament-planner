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

  //compute the digit given the index or position of the number
  public calculateDigit(index: number): Signal<number> {
    return computed(() => {
      let offsetNumber = Math.floor(this._number() / Math.pow(10, index)); // 1234 -> 123 for index 1, 0 base index
      if(offsetNumber == 0){
        return -1;
      }
      let lastDigitOfTheOffsetNumber = offsetNumber % 10; // 123 -> 3
      return lastDigitOfTheOffsetNumber;
    });
  }
}
