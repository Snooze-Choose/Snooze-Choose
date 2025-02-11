import { defineStore } from 'pinia';

interface Order {
  id: string;
  addressId: string;
  paymentMethodId: string;
  totalPrice: number;
  items: Array<{ id: string; name: string; quantity: number; price: number }>;
}

export const useOrderStore = defineStore('order', {
  state: () => ({
    orders: [] as Order[],
  }),
  actions: {
    addOrder(order: Order) {
      this.orders.push(order);
    }
  }
});
