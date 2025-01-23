<template>
  <div>
    <h1>Warenkorb</h1>
    <Table v-if="cartItems.length">
      <TableHead>
        <TableRow>
          <TableCell>Produkt</TableCell>
          <TableCell>Preis</TableCell>
          <TableCell>Menge</TableCell>
          <TableCell>Aktionen</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        <TableRow v-for="item in cartItems" :key="item.id">
          <TableCell>{{ item.name }}</TableCell>
          <TableCell>{{ item.price.toFixed(2) }} €</TableCell>
          <TableCell>
            <Input
              type="number"
              v-model.number="item.quantity"
              @change="updateQuantity(item.id, item.quantity)"
              min="1"
            />
          </TableCell>
          <TableCell>
            <Button @click="removeFromCart(item.id)">Entfernen</Button>
          </TableCell>
        </TableRow>
      </TableBody>
    </Table>
    <div v-else>
      <p>Dein Warenkorb ist leer.</p>
    </div>
    <div class="cart-summary">
      <p>Gesamtsumme: {{ cartTotal.toFixed(2) }} €</p>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useCartStore } from "@/store/cart";

export default defineComponent({
  setup() {
    const cartStore = useCartStore();
    const updateQuantity = (productId: number, quantity: number) => {
      cartStore.updateQuantity(productId, quantity);
    };
    const removeFromCart = (productId: number) => {
      cartStore.removeProductFromCart(productId);
    };

    return {
      cartItems: cartStore.items,
      cartTotal: cartStore.cartTotal,
      updateQuantity,
      removeFromCart,
    };
  },
});
</script>
