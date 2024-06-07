import {
  Component,
  Input,
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
  private _numberOfDigit: WritableSignal<number> = signal<number>(0);
  private _number = signal(0);
  public _digits: number[] = [];

  @Input() set number(value: number) {
    this._number.set(value);
    this.computeAndSetDigits();
  }
  @Input() set numberOfDigit(value: number) {
    this._numberOfDigit.set(value);
  }

  private computeAndSetDigits() {
    const numberOfDigits: number = this._numberOfDigit();
    this._digits = [];

    for (let i = 0; i < numberOfDigits ; i++) {
      this._digits[i] = this.getDigit(i);
    }
  }

  private getDigit = (position: number): number => {
    const divisor = Math.pow(10, position);
    return Math.floor(this._number() / divisor) % 10;
  };
}
