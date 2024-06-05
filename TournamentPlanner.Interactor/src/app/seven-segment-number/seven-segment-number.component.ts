import { Component } from '@angular/core';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';

@Component({
  selector: 'app-seven-segment-number',
  standalone: true,
  imports: [SevenSegmentDigitComponent],
  templateUrl: './seven-segment-number.component.html',
  styleUrl: './seven-segment-number.component.scss'
})
export class SevenSegmentNumberComponent {

}
