import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import Home from './views/Home.vue';
import Settings from './views/Settings.vue';

// Define routes
const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/about',
    name: 'About',
    component: Settings,
  },
];

// Create and configure the router
const router = createRouter({
  history: createWebHistory(),
  routes, // Short for `routes: routes`
});

export default router;