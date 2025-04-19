import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignInView from '../views/SignInView.vue'
import RegisterView from '../views/RegisterView.vue'
import ConfirmRegistrationView from '@/views/ConfirmRegistrationView.vue'
import ResetPasswordView from '@/views/ResetPasswordView.vue'
import ConfirmPasswordResetView from '@/views/ConfirmPasswordResetView.vue'

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
    {
      path: '/reset-password',
      component: ResetPasswordView,
    },
    {
      path: '/confirm-password-reset',
      component: ConfirmPasswordResetView,
    },
  ],
})

export default router
