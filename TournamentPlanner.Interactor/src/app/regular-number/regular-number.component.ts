import { Component } from '@angular/core';
import { RegularDigitComponent } from '../regular-digit/regular-digit.component';

@Component({
  selector: 'app-regular-number',
  standalone: true,
  imports: [RegularDigitComponent],
  templateUrl: './regular-number.component.html',
  styleUrl: './regular-number.component.scss'
})
export class RegularNumberComponent {

}
