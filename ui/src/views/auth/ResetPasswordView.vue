<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <form @submit.prevent="submitForm">
      <fieldset v-if="!formSent" class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Reset password</legend>
        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>

        <label class="fieldset-label">E-Mail</label>
        <input v-model="email" class="input" />
        <span v-if="emailValidation" class="text-red-500">{{ emailValidation }}</span>

        <button type="submit" class="btn btn-neutral mt-4" :disabled="inProgress">Send e-mail</button>
      </fieldset>
      <fieldset v-else class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Reset password</legend>

        <div role="alert" class="alert alert-success alert-outline">
          <span>E-mail with the link has been sent!</span>
        </div>
        <div class="text-sm pt-3">Please follow the instructions in a received message to reset your password.</div>
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
      email: '',
      inProgress: false,
      formSent: false,

      error: '',
      emailValidation: '',
    }
  },
  methods: {
    async submitForm() {
      this.inProgress = true

      HTTP.post('/api/auth/reset-password', {
        email: this.email,
      })
        .then((response) => {
          this.formSent = true
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.emailValidation = ''

          // Validation errors
          if (response.errors != null) {
            if (response.errors.email != null) {
              this.emailValidation = ERRORS.get(response.errors.email[0])
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
