import { CommonModule } from '@angular/common';
import { Component, Input, Signal, computed, signal } from '@angular/core';
import { SevenSegmentDigitComponent } from '../seven-segment-digit/seven-segment-digit.component';

@Component({
  selector: 'app-number-component-lvl3',
  standalone: true,
  imports: [CommonModule, SevenSegmentDigitComponent],
  templateUrl: './number-component-lvl3.component.html',
  styleUrl: './number-component-lvl3.component.scss',
})
export class NumberComponentLvl3Component {
  //*
  _number = signal<number>(0);

  //* basically im keeping this array to loop through by *ngFor nothing else
  _digits = signal<unknown[]>([0, 0, 0, 0]);

  _numberOfdigits = computed(() => this._digits().length);

  _precision = signal(0);

  @Input() set nubmerOfDigit(value: number) {
    let digits = [];

    for (let i = 0; i < value; i++) {
      digits.push(0);
    }
    this._digits.set(digits);
  }

  @Input() set number(value: number) {
    this._number.set(value);
    this._precision.set(this.precisionOfNumber());
  }

  @Input() set precision(value: number) {
    this._precision.set(value);
  }

  //compute the digit given the index or position of the number
  public calculateDigit(index: number) {
    let offsetNumber = Math.floor(this._number() / Math.pow(10, index)); // 1234 -> 123 for index 1, 0 base index
    if (offsetNumber == 0) {
      return -1;
    }
    let lastDigitOfTheOffsetNumber = offsetNumber % 10; // 123 -> 3
    return lastDigitOfTheOffsetNumber;
  }

  //* To handle decimal digit dynamically
  public calculateDigitV2(index: number) {
    let numberString = this._number().toString();

    //if number is decimal
    if (numberString.indexOf('.') !== -1) {
      //remove the decimal point
      numberString = numberString.replace('.', '');
    }
    // Pad the string with leading zeros to match the total number of digits
    numberString = numberString.padStart(this._numberOfdigits(), '0');

    // if (index == 0 && numberString == '0') {
    //   return -1;
    // }

    // Check if the index is within the length of the string
    if (index >= numberString.length) {
      return -1;
    }

    // Get the digit at the specified index
    //* to display in right to left fashion, reversing the numberString
    const digit = numberString[numberString.length - this._numberOfdigits() + index];

    // Convert the digit back to a number and return
    return parseInt(digit, 10);
  }

  public precisionOfNumber() {
    let numberString = this._number().toString();
    if (numberString.indexOf('.') == -1) {
      return 0;
    }
    let precision = numberString.split('.')[1].length;
    return precision;
  }

  public calculateDecimal(index: number): boolean {
    if (
      this._numberOfdigits() - index - 1 > 0 &&
      this._numberOfdigits() - index - 1 === this._precision()
    )
      return true;
    return false;
  }
}
