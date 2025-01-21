// src/keycloak.ts
import Keycloak from 'keycloak-js'

// Define the Keycloak configuration
const keycloakConfig = {
  url: import.meta.env.services__keycloak__http__0, // Keycloak server URL
  realm: 'frontend', // Your realm
  clientId: 'vue' // Your client ID
}

let keycloak: Keycloak.KeycloakInstance

export function initializeKeycloak(): Promise<any> {
  return new Promise((resolve, reject) => {
    keycloak = new Keycloak(keycloakConfig)

    keycloak
      .init({ onLoad: 'login-required', checkLoginIframe: false })
      .then((authenticated) => {
        if (authenticated) {
          console.log('Authenticated')
          resolve(keycloak)
        } else {
          reject('Authentication failed')
        }
      })
      .catch((error) => {
        reject(error)
      })
  })
}

export function getKeycloak() {
  return keycloak
}

export function logout() {
  keycloak.logout()
}
