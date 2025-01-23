import { defineStore } from "pinia";

interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
  imageUrl: string;
}

export const useCartStore = defineStore("cart", {
  state: () => ({
    items: [] as CartItem[],
  }),
  getters: {
    cartTotal: (state) =>
      state.items.reduce((total, item) => total + item.price * item.quantity, 0),
  },
  actions: {
    addProductToCart(product: CartItem) {
      const existingItem = this.items.find((item) => item.id === product.id);
      if (existingItem) {
        existingItem.quantity++;
      } else {
        this.items.push({ ...product, quantity: 1 });
      }
    },
    updateQuantity(productId: number, quantity: number) {
      const item = this.items.find((item) => item.id === productId);
      if (item) {
        item.quantity = quantity;
      }
    },
    removeProductFromCart(productId: number) {
      this.items = this.items.filter((item) => item.id !== productId);
    },
  },
});
