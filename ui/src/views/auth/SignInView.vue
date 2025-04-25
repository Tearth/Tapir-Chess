<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <form @submit.prevent="submitForm">
      <fieldset class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Sign in</legend>
        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>
        <label class="fieldset-label">Username or e-mail address</label>
        <input v-model="username" class="input" />
        <span v-if="usernameValidation" class="text-red-500">{{ usernameValidation }}</span>

        <label class="fieldset-label">Password</label>
        <input v-model="password" type="password" class="input" />
        <span v-if="passwordValidation" class="text-red-500">{{ passwordValidation }}</span>

        <div class="grid grid-cols-2 grid-rows-1 pt-1">
          <div>
            <label class="fieldset-label">
              <input v-model="rememberMe" type="checkbox" class="checkbox" />
              Remember me
            </label>
          </div>
          <div class="text-right pt-1">
            <RouterLink to="/auth/reset-password" class="link link-neutral">Forgot password?</RouterLink>
          </div>
        </div>

        <button type="submit" class="btn btn-neutral mt-4" :disabled="inProgress">Sign in</button>

        <div class="text-center pt-2">
          Don't have an account?
          <RouterLink to="/auth/register" class="link link-neutral">Join us!</RouterLink>
        </div>
      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { HTTP } from '@/utils/http'
import { ERRORS } from '@/utils/errors'
import { useUserStore } from '@/stores/user'
import { useProfileStore } from '@/stores/profile'

export default {
  data() {
    return {
      username: '',
      password: '',
      rememberMe: false,
      inProgress: false,

      error: '',
      usernameValidation: '',
      passwordValidation: '',
    }
  },
  methods: {
    async submitForm() {
      this.inProgress = true

      HTTP.post('/api/auth/signin', {
        username: this.username,
        password: this.password,
        rememberMe: this.rememberMe,
      })
        .then((response) => {
          useUserStore().fetch()
          useProfileStore().fetch()
          router.push('/')
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.usernameValidation = ''
          this.passwordValidation = ''

          // Validation errors
          if (response.errors != null) {
            if (response.errors.username != null) {
              this.usernameValidation = ERRORS.get(response.errors.username[0])
            }

            if (response.errors.password != null) {
              this.passwordValidation = ERRORS.get(response.errors.password[0])
            }
          } else if (response.detail != null) {
            this.error = ERRORS.get(response.detail)
          }
        })
        .finally(() => {
          this.inProgress = false
        })
    },
  },
}
</script>
