<script setup lang="ts">
import router from '@/router'
import { Avatar, AvatarFallback, AvatarImage } from './ui/avatar'

import { Button } from './ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from './ui/dropdown-menu'
import keycloak from '@/keycloak'

const navigateTo = (path: string) => {
  router.push(`/settings/${path}`)
}

const logout = () => {
  keycloak.logout({ redirectUri: `${window.location.origin}/` })
}
</script>

<template>
  <DropdownMenu>
    <DropdownMenuTrigger as-child>
      <Button variant="ghost" class="relative h-8 w-8 rounded-full">
        <Avatar class="h-8 w-8">
          <AvatarImage src="/avatars/01.png" alt="@shadcn" />
          <AvatarFallback>SC</AvatarFallback>
        </Avatar>
      </Button>
    </DropdownMenuTrigger>
    <DropdownMenuContent class="w-56" align="end">
      <DropdownMenuLabel class="font-normal flex">
        <div class="flex flex-col space-y-1">
          <p class="text-sm font-medium leading-none">shadcn</p>
          <p class="text-xs leading-none text-muted-foreground">m@example.com</p>
        </div>
      </DropdownMenuLabel>
      <DropdownMenuSeparator />
      <DropdownMenuGroup>
        <DropdownMenuItem @click="navigateTo('profile')"> Profile </DropdownMenuItem>
        <DropdownMenuItem @click="navigateTo('orders')"> Meine Bestellungen </DropdownMenuItem>
        <DropdownMenuItem @click="navigateTo('appearance')"> Appearance </DropdownMenuItem>
      </DropdownMenuGroup>
      <DropdownMenuSeparator />
      <DropdownMenuItem @click="logout"> Log out </DropdownMenuItem>
    </DropdownMenuContent>
  </DropdownMenu>
</template>
