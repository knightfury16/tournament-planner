import { Component, signal } from '@angular/core';
import { RegularNumberComponent } from '../regular-number/regular-number.component';
import { SevenSegmentNumberComponent } from '../seven-segment-number/seven-segment-number.component';

@Component({
  selector: 'app-number-display-test',
  standalone: true,
  imports: [RegularNumberComponent, SevenSegmentNumberComponent],
  templateUrl: './number-display-test.component.html',
  styleUrl: './number-display-test.component.scss',
})
export class NumberDisplayTestComponent {
  _number = signal(0);
  private intervalId: any;

  //update the number base on time  ngOnInit() {
  ngOnInit() {
    this.intervalId = setInterval(() => {
      this._number.update((n) => n + 1);
    }, 1000);
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
