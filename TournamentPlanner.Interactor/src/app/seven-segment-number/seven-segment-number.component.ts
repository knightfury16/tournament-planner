import {
  Component,
  Input,
  Signal,
  WritableSignal,
  computed,
  effect,
  signal,
} from '@angular/core';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-seven-segment-number',
  standalone: true,
  imports: [SevenSegmentDigitComponent, CommonModule],
  templateUrl: './seven-segment-number.component.html',
  styleUrl: './seven-segment-number.component.scss',
})
export class SevenSegmentNumberComponent {
  _number = signal(0);
  _digits = signal<unknown[]>([0, 0, 0, 0]);

  @Input() set number(value: number) {
    this._number.set(value);
  }

  @Input() set numberOfDigit(value: number) {
    let newDigits: unknown[] = [];
    for (let i = 0; i < value; i++) {
      newDigits.push(0);
    }

    // Set the signal
    this._digits.set(newDigits);
  }

  _numberOfDigits = computed(() => this._digits().length);

  constructor() {
    // This is just for demonstration purposes. Sometime, you
    // need debug output whenever a signal changes. This is how
    // you implement that.
    effect(() => console.log(this._digits()));
  }

  public getDigit(index: number): Signal<number> {
    return computed(() => {
      // We have to compute the digit on the given number.
      let digit = this._number() / Math.pow(10, index);
      if (index > 0 && digit < 1) {
        // We display a zero only if we were asked for the
        // first digit. This is necessary to avoid printing "0001"
        // when the value is 1.
        return -1;
      }
      return Math.floor(digit) % 10;
    });
  }
  // private computeAndSetDigits() {
  //   const numberOfDigits: number = this._numberOfDigit();
  //   this._digits = [];

  //   for (let i = 0; i < numberOfDigits; i++) {
  //     this._digits[i] = this.getDigit(i);
  //   }
  //   this._digits.reverse();
  // }

  // private getDigit = (position: number): number => {
  //   const divisor = Math.pow(10, position);
  //   return Math.floor(this._number() / divisor) % 10;
  // };
}
