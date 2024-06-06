import {
  Component,
  Input,
  WritableSignal,
  computed,
  effect,
  signal,
} from '@angular/core';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';

@Component({
  selector: 'app-seven-segment-number',
  standalone: true,
  imports: [SevenSegmentDigitComponent],
  templateUrl: './seven-segment-number.component.html',
  styleUrl: './seven-segment-number.component.scss',
})
export class SevenSegmentNumberComponent {
  private _numberOfDigit: WritableSignal<number> = signal<number>(0);
  private _number = signal(0);
  _computedNumber: WritableSignal<number[]> = signal<number[]>([]);

  @Input() set number(value: number) {
    this._number.set(value);
  }
  @Input() set numberOfDigit(value: number) {
    this._numberOfDigit.set(value);
  }

  constructor() {
    effect(() => {
      this.computeAndSetDigits();
    });
  }

private computeAndSetDigits() {
  const numberOfDigits: number = this._numberOfDigit();
  const number: number = this._number();
  const digits: number[] = [];

  for (let i = numberOfDigits - 1; i >= 0; i--) {
    digits[i] = this.getDigit(i);
  }

  this._computedNumber.set(digits);
}

  private getDigit = (position: number): number => {
    const divisor = Math.pow(10, position);
    return Math.floor(this._number() / divisor) % 10;
  };

}
