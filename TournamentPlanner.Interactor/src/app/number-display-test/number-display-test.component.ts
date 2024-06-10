import { Component, signal } from '@angular/core';
import { RegularNumberComponent } from '../regular-number/regular-number.component';
import { SevenSegmentNumberComponent } from '../seven-segment-number/seven-segment-number.component';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-number-display-test',
  standalone: true,
  imports: [RegularNumberComponent, SevenSegmentNumberComponent, ReactiveFormsModule,CommonModule],
  templateUrl: './number-display-test.component.html',
  styleUrl: './number-display-test.component.scss',
})
export class NumberDisplayTestComponent {

  numberInput = new FormControl(0);
}
