<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <form @submit.prevent="submitForm">
      <fieldset v-if="!formSent" class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Confirm password reset</legend>
        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>

        <label class="fieldset-label">New password</label>
        <input v-model="password" type="password" class="input" />
        <span v-if="passwordValidation" class="text-red-500">{{ passwordValidation }}</span>

        <label class="fieldset-label">Confirm new password</label>
        <input v-model="confirmPassword" type="password" class="input" />
        <span v-if="confirmPasswordValidation" class="text-red-500">{{ confirmPasswordValidation }}</span>

        <button type="submit" class="btn btn-neutral mt-4" :disabled="inProgress">Reset password</button>
      </fieldset>
      <fieldset v-else class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Confirm password reset</legend>

        <div role="alert" class="alert alert-success alert-outline">
          <span>Your password has been reset!</span>
        </div>
        <div class="text-sm pt-3">You can now <RouterLink to="/signin" class="link link-neutral">sign in</RouterLink> using new credentials.</div>
      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { useRouter, useRoute } from 'vue-router'
import { HTTP } from '@/utils/http'
import { ERRORS } from '@/utils/errors'

export default {
  data() {
    return {
      password: '',
      confirmPassword: '',
      inProgress: false,
      formSent: false,

      error: '',
      passwordValidation: '',
      confirmPasswordValidation: '',
    }
  },
  methods: {
    async submitForm() {
      if (this.password != this.confirmPassword) {
        this.confirmPasswordValidation = 'Passwords are not the same.'
        return
      }

      this.inProgress = true

      let urlParams = new URLSearchParams(window.location.search)
      let userId = urlParams.get('userId')
      let token = urlParams.get('token')

      HTTP.post('/api/auth/reset-password/confirm', {
        userId: userId,
        token: token,
        password: this.password,
      })
        .then((response) => {
          this.formSent = true
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.passwordValidation = ''
          this.confirmPasswordValidation = ''

          // Validation errors
          if (response.errors != null) {
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
