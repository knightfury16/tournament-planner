import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-regular-digit',
  standalone: true,
  imports: [],
  templateUrl: './regular-digit.component.html',
  styleUrl: './regular-digit.component.scss'
})
export class RegularDigitComponent {
  _digit = 7;

  @Input() set  digit(value: number){
    this._digit = value;
  }

}
 