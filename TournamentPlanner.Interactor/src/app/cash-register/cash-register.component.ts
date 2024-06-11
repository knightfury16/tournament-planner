import { Component } from '@angular/core';
import { Offering, ShoppingService } from '../shopping.service';
import { CommonModule } from '@angular/common';
import { ShoppingCartService } from '../shopping-cart.service';
import { NumberComponentLvl3Component } from '../number-component-lvl3/number-component-lvl3.component';

@Component({
  selector: 'app-cash-register',
  standalone: true,
  imports: [CommonModule, NumberComponentLvl3Component],
  templateUrl: './cash-register.component.html',
  styleUrl: './cash-register.component.scss'
})
export class CashRegisterComponent {

  _offering: Offering[];

  constructor(public shoppingService: ShoppingService, public cart: ShoppingCartService) {
    this._offering = this.shoppingService.offerings;
  }


  public getTotalPriceDigit(): number{
    let numberString = this.cart.totalPrice().toString();
    if(numberString.indexOf('.') == -1){
      return numberString.length;
    }
    return numberString.length - 1;
    
  }
}
 