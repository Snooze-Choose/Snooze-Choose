import './assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia'; // Pinia importieren
import App from './App.vue';
import router from './router';

const app = createApp(App);

// Pinia erstellen und initialisieren
const pinia = createPinia();
app.use(pinia); // Pinia einbinden

// Router verwenden
app.use(router);

// App mounten
app.mount('#app');
