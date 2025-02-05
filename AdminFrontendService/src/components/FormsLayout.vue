<script setup lang="ts">
import Separator from './ui/separator/Separator.vue'
import SidebarNav from './SidebarNav.vue'
</script>

<template>
  <div class="">
    <div class="space-y-0.5">
      <h2 class="text-2xl font-bold tracking-tight">Shop Einstellungen</h2>
      <p class="text-muted-foreground">
        Hier kannst du deine Bestellungen einsehen oder deine Produkte im Shop verwalten.
      </p>
    </div>
    <Separator class="my-6" />
    <div class="flex flex-col space-y-8 lg:flex-row lg:space-x-12 lg:space-y-0">
      <aside class="-mx-4 lg:w-1/5">
        <SidebarNav />
      </aside>
      <div class="flex-1 lg:max-w-2xl">
        <div class="space-y-6">
          <router-view />
        </div>
      </div>
    </div>
  </div>
</template>
