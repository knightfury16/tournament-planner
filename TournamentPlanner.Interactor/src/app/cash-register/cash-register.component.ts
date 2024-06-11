import { Component } from '@angular/core';
import { Offering, ShoppingService } from '../shopping.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cash-register',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cash-register.component.html',
  styleUrl: './cash-register.component.scss'
})
export class CashRegisterComponent {

  _offering: Offering[];

  constructor(public shoppingService: ShoppingService) {
    this._offering = this.shoppingService.offerings;
  }



}
 