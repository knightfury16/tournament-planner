import { Component, Input, computed, signal } from '@angular/core';

@Component({
  selector: 'app-seven-segment-digit',
  standalone: true,
  imports: [],
  templateUrl: './seven-segment-digit.component.html',
  styleUrl: './seven-segment-digit.component.scss',
})
export class SevenSegmentDigitComponent {
  _digit = signal(7);

  @Input() set digit(value: number) {
    this._digit.set(value);
  }

  // Segment a
  a = computed(() => [0, 2, 3, 5, 6, 7, 8, 9].includes(this._digit()));

  // Segment b
  b = computed(() => [0, 1, 2, 3, 4, 7, 8, 9].includes(this._digit()));

  // Segment c
  c = computed(() => [0, 1, 3, 4, 5, 6, 7, 8, 9].includes(this._digit()));

  // Segment d
  d = computed(() => [0, 2, 3, 5, 6, 8, 9].includes(this._digit()));

  // Segment e
  e = computed(() => [0, 2, 6, 8].includes(this._digit()));

  // Segment f
  f = computed(() => [0, 4, 5, 6, 8, 9].includes(this._digit()));

  // Segment g
  g = computed(() => [2, 3, 4, 5, 6, 8, 9].includes(this._digit()));
}
