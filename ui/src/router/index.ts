import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignInView from '../views/SignInView.vue'
import RegisterView from '../views/RegisterView.vue'
import ConfirmRegistrationView from '@/views/ConfirmRegistrationView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
    },
    {
      path: '/signin',
      component: SignInView,
    },
    {
      path: '/register',
      component: RegisterView,
    },
    {
      path: '/confirm-registration',
      component: ConfirmRegistrationView,
    },
  ],
})

export default router
