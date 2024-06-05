import { Component, Input, computed, input, signal } from '@angular/core';
import { RegularDigitComponent } from '../regular-digit/regular-digit.component';

@Component({
  selector: 'app-regular-number',
  standalone: true,
  imports: [RegularDigitComponent],
  templateUrl: './regular-number.component.html',
  styleUrl: './regular-number.component.scss'
})
export class RegularNumberComponent {
  _number = signal(0);
  @Input() set number(value: number) {
    this._number.set(value);
  }

  private getDigit = (position: number):number => {
    if(this._number() / Math.pow(10, position) < 1){
      return -1
    }
    return Math.floor(this._number() / Math.pow(10, position)) % 10;
  };

  first = computed(() => this._number() % 10);
  second = computed(() => this.getDigit(1));
  third = computed(() => this.getDigit(2));
  fourth = computed(() => this.getDigit(3));
}
