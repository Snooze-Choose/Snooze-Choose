/// <reference types="vite/types/importMeta.d.ts" />

import Keycloak from 'keycloak-js'

const keycloak: Keycloak = new Keycloak({
  url: import.meta.env.services__keycloak__http__0,
  realm: 'adminfrontend',
  clientId: 'adminfrontend'
})

export default keycloak
