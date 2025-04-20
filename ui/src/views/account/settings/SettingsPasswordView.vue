<template>
  <div class="bg-base-200 border border-base-300 p-4 rounded-box gap-3">
    <form @submit.prevent="submitForm">
      <fieldset class="fieldset w-full bg-base-200 border-base-300 p-4 rounded-box gap-3">
        <div v-if="showSuccess" role="alert" class="alert alert-success alert-outline">
          <span>Password has been changed!</span>
        </div>
        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>

        <label class="fieldset-label">Old password</label>
        <input v-model="oldPassword" type="password" class="input w-full" />
        <span v-if="oldPasswordValidation" class="text-red-500">{{ oldPasswordValidation }}</span>

        <label class="fieldset-label">New password</label>
        <input v-model="newPassword" type="password" class="input w-full" />
        <span v-if="newPasswordValidation" class="text-red-500">{{ newPasswordValidation }}</span>

        <label class="fieldset-label">Confirm new password</label>
        <input v-model="confirmNewPassword" type="password" class="input w-full" />
        <span v-if="confirmNewPasswordValidation" class="text-red-500">{{ confirmNewPasswordValidation }}</span>

        <div>
          <button type="submit" class="btn btn-success mt-4 w-max float-right" :disabled="inProgress">Change password</button>
        </div>
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
      oldPassword: '',
      newPassword: '',
      confirmNewPassword: '',
      inProgress: false,
      showSuccess: false,

      error: '',
      oldPasswordValidation: '',
      newPasswordValidation: '',
      confirmNewPasswordValidation: '',
    }
  },
  methods: {
    async submitForm() {
      if (this.newPassword != this.confirmNewPassword) {
        this.confirmNewPasswordValidation = 'Passwords are not the same.'
        return
      }

      this.inProgress = true

      HTTP.post('/api/account/change-password', {
        oldPassword: this.oldPassword,
        newPassword: this.newPassword,
      })
        .then((response) => {
          this.error = ''
          this.oldPasswordValidation = ''
          this.newPasswordValidation = ''
          this.confirmNewPasswordValidation = ''
          this.showSuccess = true

          this.oldPassword = ''
          this.newPassword = ''
          this.confirmNewPassword = ''
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.oldPasswordValidation = ''
          this.newPasswordValidation = ''
          this.confirmNewPasswordValidation = ''
          this.showSuccess = false

          // Validation errors
          if (response.errors != null) {
            if (response.errors.oldPassword != null) {
              this.oldPasswordValidation = ERRORS.get(response.errors.oldPassword[0])
            }

            if (response.errors.newPassword != null) {
              this.newPasswordValidation = ERRORS.get(response.errors.newPassword[0])
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
