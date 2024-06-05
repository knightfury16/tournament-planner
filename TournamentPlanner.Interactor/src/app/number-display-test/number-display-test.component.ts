import { Component } from '@angular/core';
import { RegularNumberComponent } from '../regular-number/regular-number.component';

@Component({
  selector: 'app-number-display-test',
  standalone: true,
  imports: [RegularNumberComponent],
  templateUrl: './number-display-test.component.html',
  styleUrl: './number-display-test.component.scss'
})
export class NumberDisplayTestComponent {

}
