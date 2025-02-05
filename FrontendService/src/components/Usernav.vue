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

const isAuthenticated = keycloak.authenticated

let firstName = ''
let lastName = ''
let email = ''

if (isAuthenticated) {
  if (keycloak.tokenParsed) {
    const userInfo = keycloak.tokenParsed
    firstName = userInfo.given_name
    lastName = userInfo.family_name
    email = userInfo.email
  }
}

const getInitials = (firstName: string, lastName: string) => {
  const firstInitial = firstName ? firstName.charAt(0).toUpperCase() : ''
  const lastInitial = lastName ? lastName.charAt(0).toUpperCase() : ''
  return `${firstInitial}${lastInitial}`
}
</script>

<template>
  <DropdownMenu>
    <DropdownMenuTrigger as-child>
      <Button variant="ghost" class="relative h-8 w-8 rounded-full">
        <Avatar class="h-8 w-8">
          <AvatarFallback v-if="isAuthenticated">{{
            getInitials(firstName, lastName)
          }}</AvatarFallback>
          <AvatarFallback v-else>?</AvatarFallback>
        </Avatar>
      </Button>
    </DropdownMenuTrigger>

    <DropdownMenuContent class="w-56" align="end">
      <DropdownMenuLabel class="font-normal flex">
        <div class="flex flex-col space-y-1">
          <p v-if="isAuthenticated" class="text-sm font-medium leading-none">
            Hallo {{ firstName }}
          </p>
          <p v-if="isAuthenticated" class="text-xs leading-none text-muted-foreground">
            {{ email }}
          </p>
        </div>
      </DropdownMenuLabel>

      <DropdownMenuSeparator v-if="isAuthenticated" />

      <DropdownMenuGroup v-if="isAuthenticated">
        <DropdownMenuItem @click="navigateTo('profile')">Profile</DropdownMenuItem>
        <DropdownMenuItem @click="navigateTo('orders')">Meine Bestellungen</DropdownMenuItem>
        <DropdownMenuItem @click="navigateTo('appearance')">Appearance</DropdownMenuItem>
      </DropdownMenuGroup>

      <DropdownMenuSeparator v-if="isAuthenticated" />

      <DropdownMenuItem v-if="isAuthenticated" @click="logout">Log out</DropdownMenuItem>
      <DropdownMenuItem v-else @click="keycloak.login()">Login</DropdownMenuItem>
    </DropdownMenuContent>
  </DropdownMenu>
</template>
