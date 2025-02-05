import { defineStore } from 'pinia'

interface CartItem {
  id: string
  name: string
  price: number
  quantity: number
  imageUrl: string
}

export const useCartStore = defineStore('cart', {
  state: () => ({
    items: [] as CartItem[]
  }),
  getters: {
    cartTotal: (state) => state.items.reduce((total, item) => total + item.price * item.quantity, 0)
  },
  actions: {
    addProductToCart(product: CartItem) {
      const existingItem = this.items.find((item) => item.id === product.id)
      if (existingItem) {
        existingItem.quantity += product.quantity // Menge addieren
      } else {
        this.items.push({ ...product }) // Neues Produkt mit der Menge hinzufügen
      }
      console.log('Aktueller Warenkorb:', this.items)
    },
    updateQuantity(productId: string, quantity: number) {
      const item = this.items.find((item) => item.id === productId)
      if (item) {
        item.quantity = quantity
      }
    },
    removeProductFromCart(productId: string) {
      this.items = this.items.filter((item) => item.id !== productId)
    },
    clearCart() {
      this.items = []
    }
  },
  persist: true // Persistenz aktivieren
})
