<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <form @submit.prevent="submitForm">
      <fieldset v-if="!formSent" class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Register</legend>
        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>
        <label class="fieldset-label">Username</label>
        <input v-model="username" class="input" />
        <span v-if="usernameValidation" class="text-red-500">{{ usernameValidation }}</span>

        <label class="fieldset-label">E-Mail</label>
        <input v-model="email" class="input" />
        <span v-if="emailValidation" class="text-red-500">{{ emailValidation }}</span>

        <label class="fieldset-label">Password</label>
        <input v-model="password" type="password" class="input" />
        <span v-if="passwordValidation" class="text-red-500">{{ passwordValidation }}</span>

        <label class="fieldset-label">Confirm password</label>
        <input v-model="confirmPassword" type="password" class="input" />
        <span v-if="confirmPasswordValidation" class="text-red-500">{{ confirmPasswordValidation }}</span>

        <button type="submit" class="btn btn-neutral mt-4" :disabled="inProgress">Register</button>
      </fieldset>
      <fieldset v-else class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Register</legend>

        <div role="alert" class="alert alert-success alert-outline">
          <span>Thank you for joining us!</span>
        </div>
        <div class="text-sm pt-3">Please check your email to confirm the account and follow the link to complete your registration.</div>
      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { HTTP } from '@/utils/http'
import { ERRORS } from '@/utils/errors'

export default {
  data() {
    return {
      username: '',
      email: '',
      password: '',
      confirmPassword: '',
      rememberMe: false,
      inProgress: false,
      formSent: false,

      error: '',
      usernameValidation: '',
      emailValidation: '',
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

      HTTP.post('/api/auth/register', {
        username: this.username,
        email: this.email,
        password: this.password,
      })
        .then((response) => {
          this.formSent = true
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.usernameValidation = ''
          this.emailValidation = ''
          this.passwordValidation = ''
          this.confirmPasswordValidation = ''

          // Validation errors
          if (response.errors != null) {
            if (response.errors.username != null) {
              this.usernameValidation = ERRORS.get(response.errors.username[0])
            }

            if (response.errors.email != null) {
              this.emailValidation = ERRORS.get(response.errors.email[0])
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
