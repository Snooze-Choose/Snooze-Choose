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

// Pinia Store für Warenkorb importieren
import { useCartStore } from '@/store/cart'

// Zugriff auf den Warenkorb-Store
const cartStore = useCartStore()
</script>

<template>
  <NavigationMenu>
    <NavigationMenuList>
      <!-- Link: Alle Produkte -->
      <NavigationMenuItem>
        <NavigationMenuLink href="/" :class="navigationMenuTriggerStyle()">
          Alle Produkte
        </NavigationMenuLink>
      </NavigationMenuItem>

      <!-- Dropdown: Kategorien -->
      <NavigationMenuItem>
        <NavigationMenuTrigger>Unsere Kategorien</NavigationMenuTrigger>
        <NavigationMenuContent>
          <ul class="grid gap-3 p-6 md:w-[400px] lg:w-[500px] lg:grid-cols-[minmax(0,.75fr)_minmax(0,1fr)]">
            <li>
              <NavigationMenuLink as-child>
                <a
                  href="/mode"
                  class="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"
                >
                  <div class="text-sm font-medium leading-none">Mode</div>
                  <p class="line-clamp-2 text-sm leading-snug text-muted-foreground">
                    Hier finden Sie alle unsere Klamotten.
                  </p>
                </a>
              </NavigationMenuLink>
            </li>
            <li>
              <NavigationMenuLink as-child>
                <a
                  href="/haushalt"
                  class="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"
                >
                  <div class="text-sm font-medium leading-none">Haushalt</div>
                  <p class="line-clamp-2 text-sm leading-snug text-muted-foreground">
                    Nützliche Dinge für den Haushalt.
                  </p>
                </a>
              </NavigationMenuLink>
            </li>
            <li>
              <NavigationMenuLink as-child>
                <a
                  href="/outdoor"
                  class="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"
                >
                  <div class="text-sm font-medium leading-none">Outdoor</div>
                  <p class="line-clamp-2 text-sm leading-snug text-muted-foreground">
                    Für den Outdoor Verrückten.
                  </p>
                </a>
              </NavigationMenuLink>
            </li>
          </ul>
        </NavigationMenuContent>
      </NavigationMenuItem>

      <!-- Link: Sale -->
      <NavigationMenuItem>
        <NavigationMenuLink href="/sale" :class="navigationMenuTriggerStyle()">
          Sale
        </NavigationMenuLink>
      </NavigationMenuItem>

      <!-- Link: Warenkorb -->
      <NavigationMenuItem>
        <NavigationMenuLink href="/warenkorb" :class="navigationMenuTriggerStyle()">
          Warenkorb ({{ cartStore.items.length }})
        </NavigationMenuLink>
      </NavigationMenuItem>
    </NavigationMenuList>
  </NavigationMenu>
</template>
