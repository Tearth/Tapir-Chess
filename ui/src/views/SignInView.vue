<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <form @submit.prevent="submitForm">
      <fieldset class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Sign in</legend>
        <div role="alert" class="alert alert-error alert-outline" v-if="error">
          <span>{{ error }}</span>
        </div>
        <label class="fieldset-label">Username</label>
        <input v-model="username" class="input" />
        <span class="text-red-500" v-if="username_validation">{{ username_validation }}</span>

        <label class="fieldset-label">Password</label>
        <input type="password" class="input" v-model="password" />
        <span class="text-red-500" v-if="password_validation">{{ password_validation }}</span>

        <div class="grid grid-cols-2 grid-rows-1 pt-1">
          <div>
            <label class="fieldset-label">
              <input type="checkbox" v-model="rememberMe" class="checkbox" />
              Remember me
            </label>
          </div>
          <div class="text-right pt-1">
            <RouterLink to="/reset-password" class="link link-neutral">Forgot password?</RouterLink>
          </div>
        </div>

        <button type="submit" class="btn btn-neutral mt-4" :disabled="inProgress">Sign in</button>

        <div class="text-center pt-2">
          Don't have an account?
          <RouterLink to="/register" class="link link-neutral">Join us!</RouterLink>
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

export default {
  data() {
    return {
      username: null,
      password: null,
      rememberMe: false,
      inProgress: false,

      error: '',
      username_validation: '',
      password_validation: '',
    }
  },
  methods: {
    async submitForm() {
      this.inProgress = true

      HTTP.post('/api/auth/signin', {
        Username: this.username,
        Password: this.password,
        RememberMe: this.rememberMe,
      })
        .then((response) => {
          useUserStore().fetch()
          router.push('/')
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.username_validation = ''
          this.password_validation = ''

          // Validation errors
          if (response.errors != null) {
            if (response.errors.Username != null) {
              this.username_validation = ERRORS.get(response.errors.Username[0])
            }

            if (response.errors.Password != null) {
              this.password_validation = ERRORS.get(response.errors.Password[0])
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
