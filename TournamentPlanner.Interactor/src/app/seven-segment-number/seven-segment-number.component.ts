import { Component, Input, computed, signal } from '@angular/core';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';

@Component({
  selector: 'app-seven-segment-number',
  standalone: true,
  imports: [SevenSegmentDigitComponent],
  templateUrl: './seven-segment-number.component.html',
  styleUrl: './seven-segment-number.component.scss',
})
export class SevenSegmentNumberComponent {
  _number = signal(0);
  @Input() set number(value: number) {
    this._number.set(value);
  }

  private getDigit = (position: number): number => {
    if (this._number() / Math.pow(10, position) < 1) {
      return -1;
    }
    return Math.floor(this._number() / Math.pow(10, position)) % 10;
  };

  first = computed(() => this._number() % 10);
  second = computed(() => this.getDigit(1));
  third = computed(() => this.getDigit(2));
  fourth = computed(() => this.getDigit(3));
}
