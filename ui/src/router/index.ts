import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/auth/HomeView.vue'
import SignInView from '../views/auth/SignInView.vue'
import RegisterView from '../views/auth/RegisterView.vue'
import ConfirmRegistrationView from '@/views/auth/ConfirmRegistrationView.vue'
import ResetPasswordView from '@/views/auth/ResetPasswordView.vue'
import ConfirmPasswordResetView from '@/views/auth/ConfirmPasswordResetView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
    },
    {
      path: '/auth/signin',
      component: SignInView,
    },
    {
      path: '/auth/register',
      component: RegisterView,
    },
    {
      path: '/auth/confirm-registration',
      component: ConfirmRegistrationView,
    },
    {
      path: '/auth/reset-password',
      component: ResetPasswordView,
    },
    {
      path: '/auth/confirm-password-reset',
      component: ConfirmPasswordResetView,
    },
  ],
})

export default router
