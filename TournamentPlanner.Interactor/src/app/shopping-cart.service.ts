import { Injectable, signal } from '@angular/core';
import { Offering } from './shopping.service';

@Injectable({
  providedIn: 'root',
})
export class ShoppingCartService {

  public shoppingCart = signal<Offering[]>([]);

  public addToCart(offering: Offering){
    this.shoppingCart.update(cart => {

      //* check if the item already exists in the cart
      const itemIndex = cart.findIndex(item => item.name === offering.name);


      //*if already in cart then just add the price of the item to the existing cart item
      //* item already exists, update its price
      if(itemIndex !== -1){
        let updatedCart = [...cart];

        //* update the price
        updatedCart[itemIndex] = {
          ...updatedCart[itemIndex],
          priceEur: updatedCart[itemIndex].priceEur + offering.priceEur
        };

        return updatedCart;
      }

      //* if item dont exists on the cart just add the item to the cart
      //! test if spread is necessary, that is if i change value of this offering later, will it impact in the cart
      cart = [...cart, offering];
      offering.priceEur = 100;

      //* return the updated cart
      return cart;

    });
  }

}
