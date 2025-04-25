<template>
  <div class="bg-base-200 border border-base-300 p-4 rounded-box gap-3">
    <form @submit.prevent="submitForm">
      <fieldset class="fieldset w-full bg-base-200 border-base-300 p-4 rounded-box gap-3">
        <div v-if="showSuccess" role="alert" class="alert alert-success alert-outline">
          <span>E-mail with the link has been sent!</span>
        </div>

        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>

        <label class="fieldset-label bold">E-mail</label>
        <input v-model="email" class="input w-full" />
        <span v-if="emailValidation" class="text-red-500">{{ emailValidation }}</span>

        <div>
          <button type="submit" class="btn btn-success mt-4 w-max float-right" :disabled="inProgress">Change e-mail</button>
        </div>
      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { mapStores } from 'pinia'
import { HTTP } from '@/utils/http'
import { ERRORS } from '@/utils/errors'
import { useUserStore } from '@/stores/user'
import { useProfileStore } from '@/stores/profile'

export default {
  data() {
    return {
      email: '',
      inProgress: false,
      showSuccess: false,

      error: '',
      emailValidation: '',
    }
  },
  async mounted() {
    let userStore = useUserStore()
    let user = await userStore.get()

    this.email = user.email.value
  },
  methods: {
    async submitForm() {
      this.inProgress = true

      HTTP.post('/api/account/change-email', {
        email: this.email,
      })
        .then(async (response) => {
          this.error = ''
          this.emailValidation = ''
          this.showSuccess = true
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.emailValidation = ''
          this.showSuccess = false

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
