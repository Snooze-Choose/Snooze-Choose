<script setup lang="ts">
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
  navigationMenuTriggerStyle,
} from '@/components/ui/navigation-menu'

import { useCartStore } from '@/store/cart'
import { computed } from 'vue'

const cartStore = useCartStore()

const cartItemCount = computed(() =>
  cartStore.items.reduce((total, item) => total + item.quantity, 0) 
)
</script>

<template>
  <NavigationMenu>
    <NavigationMenuList>
      <NavigationMenuItem>
        <NavigationMenuLink href="/" :class="navigationMenuTriggerStyle()">
          Alle Produkte
        </NavigationMenuLink>
      </NavigationMenuItem>

      <NavigationMenuItem>
        <NavigationMenuLink href="/sale" :class="navigationMenuTriggerStyle()">
          Sale
        </NavigationMenuLink>
      </NavigationMenuItem>

      <NavigationMenuItem>
        <NavigationMenuLink href="/warenkorb" :class="navigationMenuTriggerStyle()">
          Warenkorb ({{ cartItemCount }})
        </NavigationMenuLink>
      </NavigationMenuItem>
    </NavigationMenuList>
  </NavigationMenu>
</template>
