import { Injectable, computed, signal } from '@angular/core';
import { Offering } from './shopping.service';

@Injectable({
  providedIn: 'root',
})
export class ShoppingCartService {
  public shoppingCart = signal<Offering[]>([]);

  public addToCart(offering: Offering) {
    this.shoppingCart.update((cart) => {
      //* check if the item already exists in the cart
      const itemIndex = cart.findIndex((item) => item.name === offering.name);

      //*if already in cart then just add the price of the item to the existing cart item
      //* item already exists, update its price
      if (itemIndex !== -1) {
        let updatedCart = [...cart];
        const updatedPrice = parseFloat(
          (updatedCart[itemIndex].priceEur + offering.priceEur).toFixed(2)
        );

        //* update the price
        updatedCart[itemIndex] = {
          ...updatedCart[itemIndex],
          priceEur: updatedPrice,
        };

        return updatedCart;
      }

      //* if item dont exists on the cart just add the item to the cart
      cart = [...cart, { ...offering }];

      //* return the updated cart
      return cart;
    });
  }

  public totalPrice = computed(() => {
    let totalPrice = 0;

    this.shoppingCart().forEach((item) => {
      totalPrice += item.priceEur;
    });

    return parseFloat(totalPrice.toFixed(2));
  });

}
