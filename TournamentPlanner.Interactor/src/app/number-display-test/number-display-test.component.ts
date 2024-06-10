import { Component, signal } from '@angular/core';
import { RegularNumberComponent } from '../regular-number/regular-number.component';
import { SevenSegmentNumberComponent } from '../seven-segment-number/seven-segment-number.component';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NumberComponentLvl3Component } from '../number-component-lvl3/number-component-lvl3.component';

@Component({
  selector: 'app-number-display-test',
  standalone: true,
  imports: [RegularNumberComponent, SevenSegmentNumberComponent, ReactiveFormsModule,CommonModule, NumberComponentLvl3Component],
  templateUrl: './number-display-test.component.html',
  styleUrl: './number-display-test.component.scss',
})
export class NumberDisplayTestComponent {

  digitInput = new FormControl(0);
  numberInput = new FormControl(0);
  precisionInput = new FormControl(0);
}
