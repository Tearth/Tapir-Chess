import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/auth/HomeView.vue'
import SignInView from '../views/auth/SignInView.vue'
import RegisterView from '../views/auth/RegisterView.vue'
import ConfirmRegistrationView from '@/views/auth/ConfirmRegistrationView.vue'
import ResetPasswordView from '@/views/auth/ResetPasswordView.vue'
import ConfirmPasswordResetView from '@/views/auth/ConfirmPasswordResetView.vue'
import SettingsView from '@/views/account/settings/SettingsView.vue'
import EditProfileView from '@/views/account/settings/EditProfileView.vue'
import ChangeUsernameView from '@/views/account/settings/ChangeUsernameView.vue'
import ChangeEmailView from '@/views/account/settings/ChangeEmailView.vue'
import ConfirmEmailChangeView from '@/views/account/settings/ConfirmEmailChangeView.vue'
import ChangePasswordView from '@/views/account/settings/ChangePasswordView.vue'

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
    {
      path: '/account/settings',
      component: SettingsView,
      children: [
        {
          path: 'profile',
          component: EditProfileView,
        },
        {
          path: 'username',
          component: ChangeUsernameView,
        },
        {
          path: 'email',
          component: ChangeEmailView,
        },
        {
          path: 'email/confirm',
          component: ConfirmEmailChangeView,
        },
        {
          path: 'password',
          component: ChangePasswordView,
        },
      ],
    },
  ],
})

export default router
